using NavKeypad;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;

public class DoorInteractScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public LayerMask PM;//문 감지 마스크임 헷갈리지 마세요.
    [SerializeField]private TextMeshProUGUI InteractiveUI;
    private Collider BeforeDetect;
    private Outline BeforeOutline;
    RaycastHit hit;
    CustomInputs inputs;
    void Awake()
    {
        inputs = new CustomInputs();
        inputs.Player.Interactive.performed += ctx => OnInteract();
    }
    void OnEnable() => inputs.Enable();
    void OnDisable() => inputs.Disable();

    void OnInteract(){
        Debug.Log("Try Interactive");
        if(hit.collider != null){
            Debug.Log($"Door Child Count : {hit.collider.transform.childCount}");
            if(hit.collider.CompareTag("Door")){
                Animator doorSystem = hit.collider.GetComponent<Animator>();
                doorSystem.SetBool("isOpen",!doorSystem.GetBool("isOpen"));
            }
            else if (hit.collider.CompareTag("LockedDoor")){
                Debug.Log("Try Open But it is locked");
            }
            else if(hit.collider.CompareTag("PadLock")){
                PadLockScript padlockscript = hit.collider.GetComponent<PadLockScript>();
                Outline doorOutline = padlockscript.Door.GetComponent<Outline>();
                if(InventoryManager.holdingItem != null && padlockscript != null && InventoryManager.holdingItem.itemName == padlockscript.RequireKey.itemName){
                    hit.collider.transform.SetParent(transform.parent.parent);
                    hit.collider.GetComponent<Rigidbody>().isKinematic = false;
                    if(hit.collider.transform.Find("Padlock_collider") != null)
                        hit.collider.transform.Find("Padlock_collider").localPosition = Vector3.zero;
                    padlockscript.Door.tag = "Door";
                    doorOutline.OutlineColor = new Color(1,1,1);
                    hit.collider.transform.tag = "Untagged";
                    AudioManager.instance.PlaySFX("KeyUnlock", hit.collider.transform.position);
                    Destroy(hit.collider.gameObject,2.5f);
                }
            }
            else if (hit.collider.CompareTag("KeyPadButton")){
                KeypadButton button = hit.collider.GetComponent<KeypadButton>();
                button.PressButton();
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(Physics.Raycast(transform.position,transform.forward,out hit, 3f, PM)){
            AimPointScript.Door = true;
            // Debug.Log($"닿은 아이템 이름 {hit.collider.name}");
            if(hit.collider != BeforeDetect && BeforeDetect != null && BeforeOutline != null){
                OffGuide();
            }

            if(hit.collider.CompareTag("PadLock")){ 
                
                PadLockScript padlockscript = hit.collider.GetComponent<PadLockScript>();
                InteractiveUI.rectTransform.parent.gameObject.SetActive(true);
                InteractiveUI.text = $"자물쇠 잠금 해제({padlockscript.RequireKey.itemName} 필요)";
                Outline doorOutline = hit.collider.GetComponent<Outline>();
                if(InventoryManager.holdingItem != null && InventoryManager.holdingItem.itemName == padlockscript.RequireKey.itemName){
                    doorOutline.OutlineColor = new Color(1,1,1);
                    InteractiveUI.color = Color.white;
                }
                else{
                    doorOutline.OutlineColor = new Color(1,0.35f,0.35f);
                    InteractiveUI.color = Color.gray3;
                }
            }
            else if (hit.collider.CompareTag("LockedDoor")){
                InteractiveUI.rectTransform.parent.gameObject.SetActive(true);
                InteractiveUI.color = Color.gray3;
                InteractiveUI.text = $"잠긴 문";
                hit.collider.GetComponent<Outline>().OutlineColor = new Color(1,0.35f,0.35f);
            }
            else if (hit.collider.CompareTag("KeyPadButton")){
                InteractiveUI.rectTransform.parent.gameObject.SetActive(true);
                InteractiveUI.color = Color.white;
                string value = hit.collider.GetComponent<KeypadButton>().value;
                if(value == "enter")
                    InteractiveUI.text = $"비밀번호 입력 종료";
                else
                    InteractiveUI.text = $"{hit.collider.GetComponent<KeypadButton>().value} 입력";
            }
            else if (hit.collider.CompareTag("Door")){
                InteractiveUI.rectTransform.parent.gameObject.SetActive(true);
                InteractiveUI.color = Color.white;
                InteractiveUI.text = $"문 열기";
            }

            if(hit.collider.GetComponent<Outline>()){
                BeforeOutline = hit.collider.GetComponent<Outline>();
                BeforeOutline.enabled = true;}
            else{
                InteractiveUI.color = Color.white;
            }
            BeforeDetect = hit.collider;
            
        }
        else{
            if(BeforeDetect != null&&BeforeOutline != null){
                OffGuide();
                BeforeDetect = null;
            }
            InteractiveUI.color = Color.white;
        }
    }
    void OffGuide()
    {
        InteractiveUI.rectTransform.parent.gameObject.SetActive(false);
        BeforeOutline.enabled = false;
    }
}
