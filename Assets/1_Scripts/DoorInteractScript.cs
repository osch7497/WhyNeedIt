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
            Animator doorSystem = hit.collider.GetComponent<Animator>();
            doorSystem.SetBool("isOpen",!doorSystem.GetBool("isOpen"));
        }
    }
    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position,transform.forward * 3f,Color.red);
        if(Physics.Raycast(transform.position,transform.forward,out hit, 3f, PM)){
            AimPointScript.Door = true;
            Debug.Log($"닿은 아이템 이름 {hit.collider.name}");
            if(hit.collider != BeforeDetect && BeforeDetect != null){
                OffGuide();
            }
            hit.collider.GetComponent<Outline>().enabled = true;
            BeforeDetect = hit.collider;
        }
        else if(BeforeDetect != null){
            OffGuide();
            AimPointScript.Door = false;
        }
    }
    void OffGuide()
    {
        BeforeDetect.GetComponent<Outline>().enabled = false;
    }
}
