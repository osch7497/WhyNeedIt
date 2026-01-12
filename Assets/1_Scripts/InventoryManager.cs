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
    [SerializeField]private TextMeshProUGUI ItemNameUI;
    public static Item holdingItem;
    public static GameObject holdingObject;
    private int SelectedIndex;//Selected Inventory
    void Awake()
    {
        //기본적인 배열들 초기화
        Inventory = new GameObject[4];
        ItemIconBG = new RectTransform[4];
        ItemIcons = new Image[4];
        ItemNames = new TextMeshProUGUI[4];
        SelectedIndex = 0;
        for(int i = 0; i < 4; i++){
            ItemIconBG[i] = transform.GetChild(i).GetComponentInChildren<RectTransform>();
            ItemIcons[i] = transform.GetChild(i).GetChild(0).GetComponentInChildren<Image>();
            ItemNames[i] = ItemIconBG[i].GetComponentInChildren<TextMeshProUGUI>();
        }
        //처음 선택된 0번 인벤토리만 확대시킴
        ItemIconBG[0].localScale = new Vector3(1.2f,1.2f,1.2f);
        holdingItem = null;
        holdingObject = null;
    }
    void Update(){
        if(holdingItem != null)
            ItemNameUI.text = holdingItem.itemName;
        else
            ItemNameUI.text = "";
    }
    public GameObject InsertItem(GameObject Item){//인벤에 아이템 넣기 메서트
        int itemindex = SelectedIndex;
        bool isFull = true;
        GameObject ReturnValue = null;
        if(Inventory[SelectedIndex] != null){
            for(int i = 0; i < 4; i++){
                if(Inventory[i] == null){
                    isFull = false;
                    itemindex = i;
                    break;
                }
            }
        }
        if(isFull)
            ReturnValue = Inventory[SelectedIndex];//현재 사용중(이었던)아이템 미리 저장
        Debug.Log($"insertItemIndex:{itemindex}");//넣은 아이템 번호 로그에 넣기
        Item item = Item.GetComponent<ItemScript>().Item;//ScriptableObject가져오기
        ItemIcons[itemindex].enabled = true;
        ItemNames[itemindex].text = item.itemName;    //이름
        ItemIcons[itemindex].sprite = item.itemImage; //이미지
        Inventory[itemindex] = Item;          
        if(itemindex == SelectedIndex){
            Inventory[itemindex].SetActive(true);         //아이템 보이게 하기(팔에 보이게 하기 위해서)
            holdingItem = Inventory[itemindex].GetComponent<ItemScript>().Item;        //오브젝트
            holdingObject = Inventory[itemindex];
        }
        return ReturnValue;

    }
    public GameObject GetHand(){
        return Inventory[SelectedIndex];//팔에 보일 아이템을 가져갈때 쓸 메서드, 현재 아이템 반환
    }
    public GameObject ThrowOutItem(){
        ItemIcons[SelectedIndex].sprite = null;//인벤토리 스프라이트를 비움
        ItemIcons[SelectedIndex].enabled = false;
        holdingItem = null;
        holdingObject = null;
        if(Inventory[SelectedIndex] != null){
            GameObject ReturnValue = Inventory[SelectedIndex];//현재 사용중(이었던)아이템 미리 저장
            Inventory[SelectedIndex] = null;//인벤토리 비움
            ItemNames[SelectedIndex].text = "";//텍스트 없앰
            return ReturnValue;//위에 저장한 아이템 리턴
        }
        else{
            return null;
        }
    }
    public void SelectInventoryChanged(int index)//인벤토리 번호 업데이트 메소드
    {
        ItemIconBG[SelectedIndex].localScale = new Vector3(1f,1f,1f);//선택되었던 아이템 1로 크기 지정
        if(Inventory[SelectedIndex] != null)
            Inventory[SelectedIndex].SetActive(false);//선택되었던 아이템칸에 아이템이 있으면 안보이게 하기
        SelectedIndex = index;//인벤토리 번호 업데이트
        if(Inventory[SelectedIndex] != null){
            Inventory[SelectedIndex].SetActive(true);//현재 아이템칸에 템 있으면 아이템 보이게하기
            holdingItem = Inventory[SelectedIndex].GetComponent<ItemScript>().Item;
            holdingObject = Inventory[SelectedIndex];
        }
        else{
            holdingItem = null;
            holdingObject = null;
        }
        ItemIconBG[index].localScale = new Vector3(1.2f,1.2f,1.2f);//선택된 아이템 번호 1.2로 크기지정
    }
}
