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
    private int currentQuantity;
    public GameObject equippedIndicator;

    private void Awake()
    {
        if (equippedIndicator)
        {
            equippedIndicator.SetActive(false);
        }
    }
    
    public void SetItem(ItemData data, int quantity = 1)
    {
        currentItemData = data;
        currentQuantity = quantity;

        if (data)
        {
            itemIcon.sprite = data.itemIcon;
            itemIcon.enabled = (data.itemIcon);

            if (itemQuantityText)
            {
                itemQuantityText.text = quantity.ToString();
                itemQuantityText.enabled = quantity > 1;
            }
            
            UpdateEquippedIndicator();
        }
        else
        {
            itemIcon.sprite = null; 
            itemIcon.enabled = false;

            if (!itemQuantityText) return;
            itemQuantityText.text = ""; 
            itemQuantityText.enabled = false;
            if (equippedIndicator)
            {
                equippedIndicator.SetActive(false);
            }
        }
    }
    
    public void OnSlotClicked()
    {
        if(!currentItemData)
        {
            Debug.Log("빈 슬롯 클릭");
            return;
        }
        if (!PlayerManager.Instance)
        {
            Debug.LogError("PlayerManager.Instance를 찾을 수 없습니다.");
            return;
        }
        switch (currentItemData.itemType)
        {
            case ItemType.Weapon:
            case ItemType.Armor: 
            case ItemType.Helmet:
            case ItemType.Accessory:
                HandleEquipmentClick();
                break;
            default:
                Debug.Log($"알 수 없는 아이템 타입: {currentItemData.itemType}");
                break;
        }
    }
    private void HandleEquipmentClick()
    {
        Character character = PlayerManager.Instance.playerCharacter;
        bool isCurrentlyEquipped = false;

        switch (currentItemData.itemType)
        {
            case ItemType.Weapon:
                isCurrentlyEquipped = (character.equippedWeapon == currentItemData);
                break;
            case ItemType.Armor:
                isCurrentlyEquipped = (character.equippedArmor == currentItemData);
                break;
            case ItemType.Helmet:
                isCurrentlyEquipped = (character.equippedHelmet == currentItemData);
                break;
            case ItemType.Accessory:
                isCurrentlyEquipped = (character.equippedAccessory == currentItemData);
                break;
        }

        if (isCurrentlyEquipped)
        {
            PlayerManager.Instance.RequestUnequipItem(currentItemData.itemType);
        }
        else
        {
            PlayerManager.Instance.RequestEquipItem(currentItemData);
        }
    }

    public void UpdateEquippedIndicator()
    {
        if (!equippedIndicator || !PlayerManager.Instance || !currentItemData)
        {
            if (equippedIndicator) equippedIndicator.SetActive(false);
            return;
        }

        Character character = PlayerManager.Instance.playerCharacter;
        if (character == null)
        {
            if (equippedIndicator) equippedIndicator.SetActive(false);
            return;
        }

        bool isEquipped = false;
        switch (currentItemData.itemType)
        {
            case ItemType.Weapon:
                isEquipped = (character.equippedWeapon == currentItemData);
                break;
            case ItemType.Armor:
                isEquipped = (character.equippedArmor == currentItemData);
                break;
            case ItemType.Helmet:
                isEquipped = (character.equippedHelmet == currentItemData);
                break;
            case ItemType.Accessory:
                isEquipped = (character.equippedAccessory == currentItemData);
                break;
            default:
                isEquipped = false;
                break;
        }

        equippedIndicator.SetActive(isEquipped);
    }
}
