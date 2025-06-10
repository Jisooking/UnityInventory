using UnityEngine;

[CreateAssetMenu(fileName = "NewItemData", menuName = "ScriptableObjects/Item Data")]
public class ItemData : ScriptableObject
{
    public string itemName; 
    public Sprite itemIcon; 
    public int itemValue;   
    public ItemType itemType;
}

public enum ItemType
{
    Weapon,
    Armor,
    Helmet,
    Accessory
}