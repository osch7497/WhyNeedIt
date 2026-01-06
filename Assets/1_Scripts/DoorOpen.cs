using UnityEngine;
using DG.Tweening;

public class DoorOpen : MonoBehaviour
{ 
    public Transform playerCamera;
    public Animator FrontDoorAnimator;
    public Animator BackDoorAnimator;

    void Start()
    {
        FrontDoorAnimator.SetBool("isOpen", true);
    }
    
    void Update()
    {
        RaycastHit hit;
        
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (Physics.Raycast(transform.position, playerCamera.forward, out hit))
            {
                if (hit.transform.CompareTag("FrontDoor"))
                {
                    FrontDoorOpening(hit.collider.gameObject);
                }
                else if (hit.transform.CompareTag("BackDoor"))
                {
                    BackDoorOpening(hit.collider.gameObject);
                }
            }
        }
    }

    void FrontDoorOpening(GameObject door)
    {
        FrontDoorAnimator.SetBool("isOpen", true);
        GetComponent<FrontDoorInfo>().isOpen = true;
    }

    void BackDoorOpening(GameObject door)
    {
        BackDoorAnimator.SetBool("isOpen", true);
    }
}
