using UnityEngine;

public class DoorSound : MonoBehaviour
{
    public void DoorPlaySound()
    {
        AudioManager.instance.PlaySFX("DoorClose", transform.position, 2f);
    }
}
