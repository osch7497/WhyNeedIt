using UnityEngine;

public class UseItem : MonoBehaviour
{
    CustomInputs inputs;
    void Awake()
    {
        inputs = new CustomInputs();
        inputs.Player.ItemUse.performed += ctx => OnActivate();
    }
    void OnDisable() => inputs.Disable();
    void OnEnable() => inputs.Enable();
    void OnActivate(){
        Debug.Log("testActivate");
        if(InventoryManager.holdingItem != null && InventoryManager.holdingItem.itemName == "손전등"){
            Debug.Log("FlashDetect");
            Light FlashLight = InventoryManager.holdingObject.GetComponentInChildren<Light>();
            FlashLight.enabled = !FlashLight.enabled;
        }
    }
}
