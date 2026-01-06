using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

public class ItemPickUpScript : MonoBehaviour
{
    public LayerMask PM;
    public GameObject ItemDisplay;
    public InventoryManager inventoryManager;
    private Collider BeforeDetect;
    private GameObject[] Inventory;
    private CustomInputs Inputs;
    private RaycastHit hit;
    
    void Awake()
    {
        Inventory = new GameObject[4];
        Inputs = new CustomInputs();
        Inputs.Player.Interactive.performed += ctx => OnInteractive();
        Inputs.Player.ThrowOut.performed += ctx => OnThrowOut();
        Inputs.Player.Inventory.performed += ctx =>{
            int equipItemNumber = (int)ctx.ReadValue<float>();
            inventoryManager.SelectInventoryChanged(equipItemNumber-1);
        };
    }
    void OnEnable() => Inputs.Enable();
    void OnDisable() => Inputs.Disable();
    void OnInteractive(){//E눌러서 템먹는 함수
        Debug.Log("try Interactive");
        if(hit.collider != null){
            OnThrowOut();//아템 버리기
            hit.collider.gameObject.SetActive(false);//inv 매니저에서 이미 있는 아이템 가져오기
            inventoryManager.InsertItem(hit.collider.gameObject);//아이템 인벤토리에 넣기
        }
        return;
    }
    void OnThrowOut()//Q 눌러서 템 버리는 함수
    {
        GameObject Item = inventoryManager.ThrowOutItem();//inv 매니저에서 이미 있는 아이템 가져오기
        if(Item != null){
            Item.transform.position = transform.position + transform.forward*0.5f;
            Item.transform.rotation = Quaternion.Euler(0,0,0);
            Item.SetActive(true);
        }
    }
    void Update()
    {   
        Debug.DrawRay(transform.position,transform.forward * 3f,Color.red);
        if(Physics.Raycast(transform.position,transform.forward,out hit,3f,PM))
        {
            Debug.Log($"닿은 아이템 이름 {hit.collider.name}");
            if(hit.collider != BeforeDetect && BeforeDetect != null){
                OffGuide();
            }
            hit.collider.GetComponent<Outline>().enabled = true;
            hit.collider.transform.GetChild(hit.collider.transform.childCount-1).gameObject.SetActive(true);
            BeforeDetect = hit.collider;
        }
        else if(BeforeDetect != null){OffGuide();}
        if(inventoryManager.GetHand() != null){
            inventoryManager.GetHand().transform.position = ItemDisplay.transform.position;
            inventoryManager.GetHand().transform.rotation = transform.rotation;
        }
    }
    void OffGuide()
    {
        BeforeDetect.GetComponent<Outline>().enabled = false;
        BeforeDetect.transform.GetChild(BeforeDetect.transform.childCount-1).gameObject.SetActive(false);
    }
}
