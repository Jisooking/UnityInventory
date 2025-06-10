using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "InventoryManagerData", menuName = "ScriptableObjects/Inventory Manager")]
public class InventoryManagerData : ScriptableObject
{
    public List<ItemData> allItems = new List<ItemData>(); // 모든 아이템 데이터 리스트
}