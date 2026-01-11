using UnityEngine;
using UnityEngine.UI;

public class AimPointScript : MonoBehaviour
{
    public static bool Item = false;
    public static bool Door = false;
    [SerializeField] private RectTransform Aimpoint;
    
    void Update()
    {
        Aimpoint.localScale = (Item || Door)?new Vector3(1,1,1):new Vector3(0.75f,0.75f,0.75f);
    }
}
