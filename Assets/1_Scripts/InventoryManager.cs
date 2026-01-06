using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    private GameObject[] Inventory;
    private Image[] ItemIcons;
    private RectTransform[] ItemIconBG;
    private TextMeshProUGUI[] ItemNames;
    private int SIN;
    void Awake()
    {
        Inventory = new GameObject[4];
        ItemIconBG = new RectTransform[4];
        ItemIcons = new Image[4];
        ItemNames = new TextMeshProUGUI[4];
        SIN = 0;
        for(int i = 0; i < 4; i++){
            ItemIconBG[i] = transform.GetChild(i).GetComponentInChildren<RectTransform>();
            ItemIcons[i] = transform.GetChild(i).GetChild(0).GetComponentInChildren<Image>();
            ItemNames[i] = ItemIconBG[i].GetComponentInChildren<TextMeshProUGUI>();
        }
        ItemIconBG[0].localScale = new Vector3(1.2f,1.2f,1.2f);
    }
    void Update(){
        
    }
    public void InsertItem(GameObject Item){
        Debug.Log($"insertItemIndex:{SIN}");
        Item item = Item.GetComponent<ItemScript>().Item;
        ItemNames[SIN].text = item.itemName;
        ItemIcons[SIN].sprite = item.itemImage;
        Inventory[SIN] = Item;
        Inventory[SIN].SetActive(true);
    }
    public GameObject GetHand(){
        return Inventory[SIN];
    }
    public GameObject ThrowOutItem(){
        ItemIcons[SIN].sprite = null;
        if(Inventory[SIN] != null){
            GameObject ReturnValue = Inventory[SIN];
            Inventory[SIN] = null;
            ItemNames[SIN].text = "";
            return ReturnValue;
        }
        else{
            return null;
        }
    }
    public Mesh getItemMesh()
    {
        if(Inventory[SIN] == null){return null;}
        return Inventory[SIN].GetComponent<MeshFilter>().mesh;
    }
    public void SelectInventoryChanged(int index)
    {
        ItemIconBG[SIN].localScale = new Vector3(1f,1f,1f);
        if(Inventory[SIN] != null)
            Inventory[SIN].SetActive(false);
        SIN = index;
        if(Inventory[SIN] != null)
            Inventory[SIN].SetActive(true);
        ItemIconBG[index].localScale = new Vector3(1.2f,1.2f,1.2f);
    }
}
