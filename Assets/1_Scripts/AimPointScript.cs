using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public class AimPointScript : MonoBehaviour
{
    public static bool Item = false;
    public static bool Door = false;
    [SerializeField] private GameObject Aimpoint;
    
    void Update()
    {
        Aimpoint.SetActive(Item || Door);
    }
}
