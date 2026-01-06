using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    private GameObject[] Inventory;
    private Transform[] ItemIcons;
    private TextMeshProUGUI[] ItemNames;
    void Awake()
    {
        Inventory = new GameObject[4];
        ItemIcons = new Transform[4];
        ItemNames = new TextMeshProUGUI[4];
        for(int i = 0; i < 4; i++)
        {
            ItemIcons[i] = transform.GetChild(i);
            ItemNames[i] = ItemIcons[i].GetComponentInChildren<TextMeshProUGUI>();
        }
    }
    void Update()
    {
    }
    public void InsertItem(int index, GameObject Item)
    {
        ItemNames[index].text = Item.name;
        Inventory[index] = Item;
    }
    public GameObject ThrowOutItem(int index)
    {
        if(Inventory[index] != null){
            GameObject ReturnValue = Inventory[index];
            Inventory[index] = null;
            ItemNames[index].text = "";
            return ReturnValue;
        }
        else{
            return null;
        }
    }
}
