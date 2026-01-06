using TMPro;
using UnityEngine;

public class ItemScript : MonoBehaviour
{
    public Item Item;
    public GameObject Guide;
    private Rigidbody rb;
    void Awake()
    {
        GameObject newGuide = Instantiate(Guide);
        TextMeshProUGUI GuideText = newGuide.GetComponentInChildren<TextMeshProUGUI>();
        newGuide.transform.SetParent(transform);
        newGuide.transform.localPosition = new Vector3(0,0,0);
        GuideText.text = $"{Item.itemName} 줍기";
        Debug.Log($"변경된 내용:{GuideText.text}");
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        if(transform.position.y < -100)
        {
            transform.position = GameObject.FindWithTag("Player").transform.position;
            rb.linearVelocity = new Vector2(0f,0f);
        }
    }
}
