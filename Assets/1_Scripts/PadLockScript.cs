using UnityEngine;

public class PadLockScript : MonoBehaviour
{
    public Item RequireKey;
    public GameObject Door;
    void Start()
    {
        if(RequireKey == null)
            Debug.LogError($"{transform.parent.name}에 키가 할당되지 않았습니다.");
    }
    
}
