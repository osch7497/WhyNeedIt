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
        //기본적인 배열들 초기화
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
        //처음 선택된 0번 인벤토리만 확대시킴
        ItemIconBG[0].localScale = new Vector3(1.2f,1.2f,1.2f);
    }
    void Update(){
        
    }
    public void InsertItem(GameObject Item){//인벤에 아이템 넣기 메서트
        Debug.Log($"insertItemIndex:{SIN}");//넣은 아이템 번호 로그에 넣기
        Item item = Item.GetComponent<ItemScript>().Item;//ScriptableObject가져오기
        ItemIcons[SIN].enabled = true;
        ItemNames[SIN].text = item.itemName;    //이름
        ItemIcons[SIN].sprite = item.itemImage; //이미지
        Inventory[SIN] = Item;                  //오브젝트
        Inventory[SIN].SetActive(true);         //아이템 보이게 하기(팔에 보이게 하기 위해서)
    }
    public GameObject GetHand(){
        return Inventory[SIN];//팔에 보일 아이템을 가져갈때 쓸 메서드, 현재 아이템 반환
    }
    public GameObject ThrowOutItem(){
        ItemIcons[SIN].sprite = null;//인벤토리 스프라이트를 비움
        ItemIcons[SIN].enabled = false;
        if(Inventory[SIN] != null){
            GameObject ReturnValue = Inventory[SIN];//현재 사용중(이었던)아이템 미리 저장
            Inventory[SIN] = null;//인벤토리 비움
            ItemNames[SIN].text = "";//텍스트 없앰
            return ReturnValue;//위에 저장한 아이템 리턴
        }
        else{
            return null;
        }
    }
    public void SelectInventoryChanged(int index)//인벤토리 번호 업데이트 메소드
    {
        ItemIconBG[SIN].localScale = new Vector3(1f,1f,1f);//선택되었던 아이템 1로 크기 지정
        if(Inventory[SIN] != null)
            Inventory[SIN].SetActive(false);//선택되었던 아이템칸에 아이템이 있으면 안보이게 하기
        SIN = index;//인벤토리 번호 업데이트
        if(Inventory[SIN] != null)
            Inventory[SIN].SetActive(true);//현재 아이템칸에 템 있으면 아이템 보이게하기
        ItemIconBG[index].localScale = new Vector3(1.2f,1.2f,1.2f);//선택된 아이템 번호 1.2로 크기지정
    }
}
