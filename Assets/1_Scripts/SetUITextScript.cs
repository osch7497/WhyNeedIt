using System;
using TMPro;
using UnityEngine;

public class SetUITextScript : MonoBehaviour
{
    TextMeshProUGUI GuideText;
    void Start()
    {   
        GuideText = GetComponentInChildren<TextMeshProUGUI>();
        GuideText.text = $"{transform.parent.name}를 줍기";
        Debug.Log($"변경된 내용:{GuideText.text}");
    }
}
