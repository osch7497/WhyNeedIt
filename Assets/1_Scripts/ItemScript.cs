using TMPro;
using UnityEngine;

public class ItemScript : MonoBehaviour
{
    public Item Item;
    private Rigidbody rb;
    void Awake()
    {
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
