using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class UIHoverEffect : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    private bool isEntering;
    private float targetScale;
    void Awake(){
        targetScale = 1f;
    }
    public void OnPointerEnter(PointerEventData pointerEventData){
        Debug.Log($"Enter Pointer {gameObject.name} target - {targetScale}");
        targetScale = 1.2f;
    }
    public void OnPointerExit(PointerEventData pointerEventData) {
        Debug.Log($"Exit Pointer {gameObject.name} target - {targetScale}");
        targetScale = 1f;
    }
    void FixedUpdate()
    {
        transform.localScale = new Vector3(
            transform.localScale.x + (targetScale-transform.localScale.x)*Time.fixedDeltaTime,
            transform.localScale.y + (targetScale-transform.localScale.y)*Time.fixedDeltaTime,
            transform.localScale.z + (targetScale-transform.localScale.z)*Time.fixedDeltaTime
        );
    }
}
