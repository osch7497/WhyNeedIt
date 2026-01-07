using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;

public class DoorInteractScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public LayerMask PM;
    private Collider BeforeDetect;
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
                PadLockScript PDS = hit.collider.GetComponent<PadLockScript>();
                Outline DO = PDS.Door.GetComponent<Outline>();
                if(InventoryManager.holdingItem != null && PDS != null && InventoryManager.holdingItem.itemName == PDS.RequireKey.itemName){
                    hit.collider.transform.SetParent(transform.parent.parent);
                    hit.collider.GetComponent<Rigidbody>().isKinematic = false;
                    if(hit.collider.transform.Find("Padlock_collider") != null)
                        hit.collider.transform.Find("Padlock_collider").localPosition = Vector3.zero;
                    PDS.Door.tag = "Door";
                    DO.OutlineColor = new Color(1,1,1);
                    hit.collider.transform.tag = "Untagged";
                    Destroy(hit.collider.gameObject,2.5f);
                }
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(Physics.Raycast(transform.position,transform.forward,out hit, 3f, PM)){
            AimPointScript.Door = true;
            // Debug.Log($"닿은 아이템 이름 {hit.collider.name}");
            if(hit.collider != BeforeDetect && BeforeDetect != null){
                OffGuide();
            }
            if(hit.collider.CompareTag("PadLock")){ 
                PadLockScript PDS = hit.collider.GetComponent<PadLockScript>();
                Outline DO = hit.collider.GetComponent<Outline>();
                if(InventoryManager.holdingItem != null && InventoryManager.holdingItem.itemName == PDS.RequireKey.itemName)
                    DO.OutlineColor = new Color(1,1,1);
                else
                    DO.OutlineColor = new Color(1,0.35f,0.35f);
            }
            else if (hit.collider.CompareTag("LockedDoor")){
                hit.collider.GetComponent<Outline>().OutlineColor = new Color(1,0.35f,0.35f);
            }
            hit.collider.GetComponent<Outline>().enabled = true;
            BeforeDetect = hit.collider;
        }
        else if(BeforeDetect != null){
            OffGuide();
        }
    }
    void OffGuide()
    {
        BeforeDetect.GetComponent<Outline>().enabled = false;
    }
}
