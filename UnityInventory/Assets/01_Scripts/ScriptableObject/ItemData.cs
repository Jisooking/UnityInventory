using UnityEngine;

[CreateAssetMenu(fileName = "NewItemData", menuName = "ScriptableObjects/Item Data")]
public class ItemData : ScriptableObject
{
    public string itemName; 
    public Sprite itemIcon; 
    public int itemValue;   
    public ItemType itemType;
    
    [Header("Item Effects")]
    public int attackBonus;
    public int defenseBonus;
    public int healthBonus;
    public int critBonus;
}

public enum ItemType
{
    Weapon,
    Armor,
    Helmet,
    Accessory
}