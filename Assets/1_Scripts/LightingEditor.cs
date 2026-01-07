using UnityEngine;
using UnityEngine.Rendering;

public class LightingEditor : MonoBehaviour
{

    public float LightingValue = 1f;
    public Volume GlobalVolume;
    void Update(){
        GlobalVolume.weight = GlobalVolume.weight + (LightingValue-GlobalVolume.weight)*Time.deltaTime*5;
    }
}
