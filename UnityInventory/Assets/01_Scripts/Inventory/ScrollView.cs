using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Unity.VisualScripting;

public class ScrollView : MonoBehaviour
{
    public InventoryManagerData inventoryManagerData;
    public GameObject slotPrefab; // 프리팹
    public Transform content;     // ScrollView의 Content

    [System.Serializable]
    public class InventorySlot
    {
        public ItemData itemData;
        public int quantity;
    }
    
    public List<InventorySlot> playerInventory = new List<InventorySlot>();
    
    private void Start()
    {
        InitializePlayerInventory();
        GenerateAndDisplaySlots();
    }
    
    private void InitializePlayerInventory()    // 아이템 인벤토리 초기화 함수
    {
        if (!inventoryManagerData || inventoryManagerData.allItems.Count <= 0) return;
        
        playerInventory.Clear();

        foreach (var item in inventoryManagerData.allItems)
        {
            AddItemToPlayerInventory(item, 1, false); 
        }
    }

    private void AddItemToPlayerInventory(ItemData itemToAdd, int quantity,bool updateUI = true) // 아이템 
    {
        if (!itemToAdd || quantity <= 0) return;

        // 이미 인벤토리에 같은 아이템이 있는지 확인하여 수량만 업데이트
        foreach (var slot in playerInventory)
        {
            if (slot.itemData != itemToAdd) continue;
            slot.quantity += quantity;
            if (updateUI) UpdateInventoryDisplay(); // UI 업데이트
            return;
        }

        // 새 아이템 슬롯 추가
        playerInventory.Add(new InventorySlot { itemData = itemToAdd, quantity = quantity });
        if (updateUI) UpdateInventoryDisplay(); // UI 업데이트
    }

    public void AddItem(ItemData itemToAdd, int quantity)
    {
        AddItemToPlayerInventory(itemToAdd, quantity);
    }
    
    // 플레이어 인벤토리에서 아이템을 제거하는 함수 (옵션)
    public void RemoveItemFromPlayerInventory(ItemData itemToRemove, int quantity)
    {
        for (var i = playerInventory.Count - 1; i >= 0; i--)
        {
            if (playerInventory[i].itemData != itemToRemove) continue;
            playerInventory[i].quantity -= quantity;
            if (playerInventory[i].quantity <= 0)
            {
                playerInventory.RemoveAt(i);
            }
            UpdateInventoryDisplay();
            return;
        }
    }

    // 현재 플레이어 인벤토리 데이터를 기반으로 UI 슬롯을 생성하고 표시
    private void GenerateAndDisplaySlots()
    {
        // 기존 슬롯 제거 (UI 갱신 시 매번 초기화)
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }
        
        // 플레이어 인벤토리에 있는 아이템 수만큼 슬롯 생성 및 데이터 할당
        foreach (var slotData in playerInventory)
        {
            var slotGo = Instantiate(slotPrefab, content);
            var slotUI = slotGo.GetComponent<ItemSlots>();
            if (!slotUI) continue;
            slotUI.SetItem(slotData.itemData, slotData.quantity);

            // 슬롯 클릭 이벤트 리스너 추가 (옵션)
            var slotButton = slotGo.GetComponent<Button>();
            if (!slotButton)
            {
                slotButton = slotGo.AddComponent<Button>(); // 버튼 컴포넌트가 없으면 추가
            }
            slotButton.onClick.AddListener(slotUI.OnSlotClicked);
        }
    }
    
    public void UpdateInventoryDisplay()
    {
        GenerateAndDisplaySlots();
    }
}