using UnityEngine;

public class ItemPickUpScript : MonoBehaviour
{
    public LayerMask PM;
    private Collider BeforeDetect;
    void Update()
    {   
        RaycastHit hit;
        Debug.DrawRay(transform.position,transform.forward * 15f,Color.red);
        if(Physics.Raycast(transform.position,transform.forward,out hit,15f,PM))
        {
            Debug.Log($"닿은 아이템 이름 {hit.collider.name}");
            hit.collider.GetComponent<Outline>().enabled = true;
            hit.collider.transform.Find("InterectiveGuide").gameObject.SetActive(true);
            BeforeDetect = hit.collider;
        }else if(BeforeDetect != null)
        {
            BeforeDetect.GetComponent<Outline>().enabled = false;
            BeforeDetect.transform.Find("InterectiveGuide").gameObject.SetActive(false);
        }
    }
}
