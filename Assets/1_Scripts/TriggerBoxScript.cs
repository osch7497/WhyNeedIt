using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerBoxScript : MonoBehaviour
{
    // 외부에서 구독할 수 있는 이벤트 선언
    [Header("***Need to drag In ScriptUI***")]
    public LineVisualScript ScriptUI;
    public List<Line> line;
    private void OnTriggerEnter(Collider other)
    {
        ScriptUI.PrintLine(line);
        Destroy(gameObject);
    }
}