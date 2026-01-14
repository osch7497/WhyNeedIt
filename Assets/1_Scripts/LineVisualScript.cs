using System.Collections.Generic;
using System;
using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.Events;
using System.Reflection;
using System.Collections;
using TMPro;
using UnityEngine.UI;



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
    public GameObject TriggerBox;
    public List<Line> line;
}
public class LineVisualScript : MonoBehaviour
{
    [Header("Script Setting")]
    [SerializeField] private List<TriggerBoxInfo> scripts;
    [SerializeField] private TextMeshProUGUI ScriptDisplay;
    IEnumerator CoPrintLine(List<Line> line){
        foreach(Line l in line){
            yield return new WaitForSeconds(l.StartTime-0.1f);
            ScriptDisplay.text = l.Content;
            for(int i = 1; i <= 10; i++){
                ScriptDisplay.color = new Color(ScriptDisplay.color.r,ScriptDisplay.color.g,ScriptDisplay.color.b,ScriptDisplay.color.a+0.1f);
                yield return new WaitForSeconds(0.01f);
            }
            yield return new WaitForSeconds(l.Duration-0.1f);
            for(int i = 0; i < 10; i++){
                ScriptDisplay.color = new Color(ScriptDisplay.color.r,ScriptDisplay.color.g,ScriptDisplay.color.b,ScriptDisplay.color.a-0.1f);
                yield return new WaitForSeconds(0.01f);
            }
            switch (l.EventType)
            {
                case ComponentType.Light:
                    Light[] com = l.EventComponent.GetComponentsInChildren<Light>();
                    foreach(Light light in com)
                        light.enabled = !light.enabled;
                    break;
                case ComponentType.Item:
                    if(l.EventComponent.layer == 0)
                        l.EventComponent.layer = 6;
                    else
                        l.EventComponent.layer = 0;

                    break;
                case ComponentType.GameObject:
                    l.EventComponent.SetActive(!l.EventComponent.activeSelf);
                    break;
                default:
                break;
            }
        }
    }

    public void Awake()
    {
        foreach(TriggerBoxInfo TBX in scripts){
            if(TBX.TriggerBox.GetComponent<TriggerBoxScript>() == null){
                TriggerBoxScript newTriggerBox = TBX.TriggerBox.AddComponent<TriggerBoxScript>();
                newTriggerBox.line = TBX.line;
                newTriggerBox.ScriptUI = this;
            }
        }
    }
    public void PrintLine(List<Line> line){
        IEnumerator co = CoPrintLine(line);
        StartCoroutine(co);
    }
}
