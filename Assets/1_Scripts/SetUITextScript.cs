using TMPro;
using UnityEngine;

public class SetUITextScript : MonoBehaviour
{
    TextMeshProUGUI GuideText;
    GameObject Player;
    void Start()
    {   
        Player = GameObject.FindWithTag("MainCamera");
        transform.localPosition = Vector3.zero;
        GuideText = GetComponentInChildren<TextMeshProUGUI>();
        GuideText.text = $"{transform.parent.name}를 줍기";
        Debug.Log($"변경된 내용:{GuideText.text}");
    }
    void Update() {
        Transform target = GameObject.FindWithTag("MainCamera").transform; 
        transform.rotation = Player.transform.rotation;    
        transform.Rotate(0f,180f,0f);
    }
}
