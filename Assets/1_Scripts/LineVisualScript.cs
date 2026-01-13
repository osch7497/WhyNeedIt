using System.Collections.Generic;
using System;
using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.Events;
using System.Reflection;
using System.Collections;
using TMPro;



[Serializable]
public class Line{
    public string Content;
    public float StartTime;
    public float Duration;
    public ComponentType EventType;
    public GameObject EventComponent;
}
public enum ComponentType
{
    Null,
    Item,
    Light,
    GameObject

}
[Serializable]
public struct TriggerBoxInfo{
    public Line line;
    public GameObject TriggerBox;
}
public class LineVisualScript : MonoBehaviour
{
    [Header("Script Setting")]
    [SerializeField] private List<TriggerBoxInfo> scripts;
    [SerializeField] private GameObject textPrefab;
    [SerializeField] private Transform textUIGroup;
    IEnumerator CoPrintLine(Line line){
        yield return new WaitForSeconds(line.StartTime-0.1f);
        Transform newLine = Instantiate(textPrefab).transform;
        TextMeshProUGUI newText = newLine.GetComponent<TextMeshProUGUI>();
        newLine.transform.parent = textUIGroup;
        newText.text = line.Content;
        for(int i = 1; i <= 10; i++){
            newText.color = new Color(newText.color.r,newText.color.g,newText.color.b,newText.color.a+0.1f);
            yield return new WaitForSeconds(0.01f);
        }
        yield return new WaitForSeconds(line.Duration-0.1f);
        for(int i = 0; i < 10; i++){
            newText.color = new Color(newText.color.r,newText.color.g,newText.color.b,newText.color.a-0.1f);
            yield return new WaitForSeconds(0.01f);
        }
        switch (line.EventType)
        {
            case ComponentType.Light:
                Light[] com = line.EventComponent.GetComponentsInChildren<Light>();
                foreach(Light l in com)
                    l.enabled = !l.enabled;
                break;
            case ComponentType.Item:
                if(line.EventComponent.layer == 0)
                    line.EventComponent.layer = 6;
                else
                    line.EventComponent.layer = 0;

                break;
            case ComponentType.GameObject:
                line.EventComponent.SetActive(!line.EventComponent.activeSelf);
                break;
            default:
            break;
        }
        Destroy(newLine.gameObject);
    }

    public void Awake()
    {
        foreach(TriggerBoxInfo TBX in scripts){
            TriggerBoxScript newTriggerBox = TBX.TriggerBox.AddComponent<TriggerBoxScript>();
            newTriggerBox.line = TBX.line;
            newTriggerBox.ScriptUI = this;
        }
    }
    public void PrintLine(Line line){
        IEnumerator co =  CoPrintLine(line);
        StartCoroutine(co);
    }
}
