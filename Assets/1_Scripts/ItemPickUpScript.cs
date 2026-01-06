using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class ItemPickUpScript : MonoBehaviour
{
    public LayerMask PM;
    public MeshFilter ItemDisplay;
    public InventoryManager inventoryManager;
    private Collider BeforeDetect;
    private GameObject[] Inventory;
    private CustomInputs Inputs;
    private RaycastHit hit;
    private int equipItemNumber;
    
    void Awake()
    {
        equipItemNumber = 0;
        Inventory = new GameObject[4];
        Inputs = new CustomInputs();
        Inputs.Player.Interactive.performed += ctx => OnInteractive();
        Inputs.Player.ThrowOut.performed += ctx => OnThrowOut();
    }
    void OnEnable() => Inputs.Enable();
    void OnDisable() => Inputs.Disable();
    void OnInteractive()
    {
        Debug.Log("try Interactive");
        if(hit.collider != null){
            OnThrowOut();
            hit.collider.gameObject.SetActive(false);
            ItemDisplay.mesh = hit.collider.GetComponent<MeshFilter>().mesh;
            inventoryManager.InsertItem(equipItemNumber,hit.collider.gameObject);
        }
        return;
    }
    void OnThrowOut()
    {
        GameObject Item = inventoryManager.ThrowOutItem(equipItemNumber);
        if(Item != null){
            Item.transform.position = transform.position + transform.forward*1.5f;
            Item.transform.rotation = Quaternion.Euler(0,0,0);
            ItemDisplay.mesh = null;
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
            hit.collider.transform.GetChild(0).gameObject.SetActive(true);
            BeforeDetect = hit.collider;
        }
        else if(BeforeDetect != null){OffGuide();}
    }
    void OffGuide()
    {
        BeforeDetect.GetComponent<Outline>().enabled = false;
        BeforeDetect.transform.GetChild(0).gameObject.SetActive(false);
    }
}
