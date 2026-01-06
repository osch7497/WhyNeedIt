using UnityEngine;

public class DoorInteractScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public LayerMask PM;
    public Collider BeforeDetect;
    RaycastHit hit;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position,transform.forward * 3f,Color.red);
        if(Physics.Raycast(transform.position,transform.forward,out hit, 3f, PM)){
            Debug.Log($"닿은 아이템 이름 {hit.collider.name}");
            if(hit.collider != BeforeDetect && BeforeDetect != null){
                OffGuide();
            }
            hit.collider.GetComponent<Outline>().enabled = true;
            BeforeDetect = hit.collider;
        }
        else if(BeforeDetect != null){OffGuide();}
    }
    void OffGuide()
    {
        BeforeDetect.GetComponent<Outline>().enabled = false;
    }
}
