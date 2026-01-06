using TMPro;
using UnityEngine;

public class SetUITextScript : MonoBehaviour
{
    TextMeshProUGUI GuideText;
    GameObject Player;
    void Start()
    {   
        transform.localPosition = Vector3.zero;
        Player = GameObject.FindWithTag("MainCamera");
    }
    void Update() {
        Transform target = GameObject.FindWithTag("MainCamera").transform; 
        transform.rotation = Player.transform.rotation;    
        transform.Rotate(0f,180f,0f);
    }
}
