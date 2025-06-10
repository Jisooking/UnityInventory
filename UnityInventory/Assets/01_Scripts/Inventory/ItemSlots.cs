using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlots : MonoBehaviour
{
    public Image itemIcon;
    public TextMeshProUGUI itemQuantityText;
    private ItemData currentItemData;

    public void SetItem(ItemData data, int quantity = 1)
    {
        currentItemData = data;

        if (data)
        {
            itemIcon.sprite = data.itemIcon;
            itemIcon.enabled = true;

            if (itemQuantityText)
            {
                itemQuantityText.text = quantity.ToString();
                itemQuantityText.enabled = quantity > 1;
            }
        }
        else
        {
            itemIcon.sprite = null; 
            itemIcon.enabled = false;

            if (!itemQuantityText) return;
            itemQuantityText.text = ""; 
            itemQuantityText.enabled = false;
        }
    }
    public void SetData(ItemData data)
    {
        if (!data) return;
        if (!itemIcon) return;
        itemIcon.sprite = data.itemIcon; // StatusData의 Sprite를 Image 컴포넌트의 sprite 속성에 할당
        itemIcon.enabled = (data.itemIcon); // 아이콘이 없으면 Image 컴포넌트 비활성화
    }
    
    public void OnSlotClicked()
    {
        if (currentItemData)
        {
            Debug.Log($"슬롯 클릭: {currentItemData.itemName}");
        }
        else
        {
            Debug.Log("빈 슬롯 클릭");
        }
    }
}
