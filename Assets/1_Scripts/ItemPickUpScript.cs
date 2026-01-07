using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
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
    private LightingEditor LE;
    
    void Awake()
    {
        Inventory = new GameObject[4];
        Inputs = new CustomInputs();
        LE = GameObject.FindWithTag("Light").GetComponent<LightingEditor>();
        Inputs.Player.Interactive.performed += ctx => OnInteractive();//템 먹는 함수 E키랑 연결
        Inputs.Player.ThrowOut.performed += ctx => OnThrowOut();//템 버리는 함수 Q키랑 연결
        Inputs.Player.Inventory.performed += ctx =>{//익명함수랑 연결해서 인벤토리 번호 변경시 inventorymanager한테 선택된 번호 넘기기.
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
            AimPointScript.Item = true;
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
        Debug.DrawRay(transform.position,transform.forward * 3f,Color.red);//디버그용 빨간 레이저 쏘기
        if(Physics.Raycast(transform.position,transform.forward,out hit,3f,PM))//레이캐스팅
        {
            Debug.Log($"닿은 아이템 이름 {hit.collider.name}");//닿은 아이템 이름 로그에 남기기
            if(hit.collider != BeforeDetect && BeforeDetect != null){//콜라이더가 바뀌면(다른 아이템을 가리켰으면)
                OffGuide();
            }
            hit.collider.GetComponent<Outline>().enabled = true;
            hit.collider.transform.GetChild(hit.collider.transform.childCount-1).gameObject.SetActive(true);
            BeforeDetect = hit.collider;
        }
        else if(BeforeDetect != null){
            OffGuide();
            AimPointScript.Item = false;
        }
        GameObject HandleItem = inventoryManager.GetHand();
        if(HandleItem != null){//만일 손에 들 아이템이 있다면
            Item Hitem = HandleItem.GetComponent<ItemScript>().Item;
            //Debug.Log($"rotation = {transform.rotation * HandleItem.GetComponent<ItemScript>().Item.HandleRotation}");
            HandleItem.transform.position = ItemDisplay.transform.position;//손 위치로 아이템 이동
            HandleItem.transform.rotation = transform.rotation * Hitem.HandleRotation;//방향 카메라와 동일하게 고정시킴
            LE.LightingValue = Hitem.LightingValue;
        }
        else{
            LE.LightingValue = 0.9f;
        }
    }
    void OffGuide()//UI 종료 메서드
    {
        BeforeDetect.GetComponent<Outline>().enabled = false;//아웃라인 끄기
        BeforeDetect.transform.GetChild(BeforeDetect.transform.childCount-1).gameObject.SetActive(false);//가이드 ui 끄기
    }
}
