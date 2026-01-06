using UnityEngine;

public class ItemScript : MonoBehaviour
{
    public Items Item;
    public GameObject Guide;
    void Awake()
    {
        GameObject newGuide = Instantiate(Guide);
        newGuide.transform.SetParent(transform);
    }
}
