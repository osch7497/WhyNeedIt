using TMPro;
using UnityEngine;

public class ItemScript : MonoBehaviour
{
    public Item Item;
    public GameObject Guide;
    void Awake()
    {
        GameObject newGuide = Instantiate(Guide);
        TextMeshProUGUI GuideText = newGuide.GetComponentInChildren<TextMeshProUGUI>();
        
        newGuide.transform.SetParent(transform);
        GuideText.text = $"{Item.itemName} 줍기";
        Debug.Log($"변경된 내용:{GuideText.text}");
    }
}
