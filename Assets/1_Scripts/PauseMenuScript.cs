using Unity.VisualScripting;
using UnityEngine;

public class PauseMenuScript : MonoBehaviour
{
    private CustomInputs Inputs;
    [SerializeField]private Canvas PauseUI;
    
    void OnEnable() => Inputs.Enable();
    void OnDisable() => Inputs.Disable();
    void Awake()
    {
        Inputs = new CustomInputs();
        Inputs.Player.Pause.performed += ctx => OnPause();
    }
    void Update()
    {
        
    }
    public void OnPause(){
        Debug.Log("Test");
        if(Time.timeScale == 0f){
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            PauseUI.enabled = false;
        }
        else{
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.Confined;
            PauseUI.enabled = true;
        }
    }
    
}
