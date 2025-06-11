using System;
using System.Collections.Generic;
using UnityEngine;

public class Character
{        
    public string Name { get; private set; }
    public int Level { get; private set; }
    public int Experience { get; private set; }
    public StatusData Attack {get; private set;}
    public StatusData Defense {get; private set;}
    public StatusData Hp {get; private set;}
    public StatusData Critical {get; private set;}
    public int Gold {get; private set;}
    
    public ItemData equippedWeapon;
    public ItemData equippedArmor;
    public ItemData equippedHelmet;
    public ItemData equippedAccessory;

    private int _baseAttack;
    private int _baseDefense;
    private int _baseHealth;
    private int _baseCritical;
        
    private int currentHp;
    public int CurrentHp
    {
        get { return currentHp; }
        set { currentHp = Mathf.Clamp(value, 0, GetHpValue()); } // Max HP를 넘지 않도록 Clamp
    }
    public Character(string name = "New Character", int level = 1, int gold = 0)
    {
        Name = name;
        Level = level;
        Experience = 0;
        Gold = gold;
        
        Attack = ScriptableObject.CreateInstance<StatusData>();
        Attack.statusName = "Attack";

        Defense = ScriptableObject.CreateInstance<StatusData>();
        Defense.statusName = "Defense";

        Hp = ScriptableObject.CreateInstance<StatusData>();
        Hp.statusName = "Hp";

        Critical = ScriptableObject.CreateInstance<StatusData>();
        Critical.statusName = "Critical";
        
        _baseAttack = 0; 
        _baseDefense = 0;
        _baseHealth = 0;
        _baseCritical = 0;
        
        CalculateTotalStats();
        CurrentHp = GetHpValue();
    }
    
    public void SetBaseStats(int baseAtk, int baseDef, int baseHp, int baseCrit)
    {
        _baseAttack = baseAtk;
        _baseDefense = baseDef;
        _baseHealth = baseHp;
        _baseCritical = baseCrit;
        
        CalculateTotalStats(); 
    }
    
    public List<StatusData> GetAllStatuses()
    {
        return new List<StatusData> { Attack, Defense, Hp, Critical };
    }
    
    public void CalculateTotalStats()
    {
        int currentCalculatedAttack = _baseAttack;
        int currentCalculatedDefense = _baseDefense;
        int currentCalculatedHealth = _baseHealth;
        int currentCalculatedCritical = _baseCritical; 

        if (equippedWeapon)
        {
            currentCalculatedAttack += equippedWeapon.attackBonus;
            currentCalculatedDefense += equippedWeapon.defenseBonus;
            currentCalculatedHealth += equippedWeapon.healthBonus;
        }
        
        if (equippedArmor)
        {
            currentCalculatedAttack += equippedArmor.attackBonus;
            currentCalculatedDefense += equippedArmor.defenseBonus;
            currentCalculatedHealth += equippedArmor.healthBonus;
        }
        
        if (equippedHelmet)
        {
            currentCalculatedAttack += equippedHelmet.attackBonus; 
            currentCalculatedDefense += equippedHelmet.defenseBonus;
            currentCalculatedHealth += equippedHelmet.healthBonus;
        }
        
        if (equippedAccessory)
        {
            currentCalculatedAttack += equippedAccessory.attackBonus;
            currentCalculatedDefense += equippedAccessory.defenseBonus;
            currentCalculatedHealth += equippedAccessory.healthBonus;
        }
        
        Attack.statusValue = currentCalculatedAttack;
        Defense.statusValue = currentCalculatedDefense;
        Hp.statusValue = currentCalculatedHealth;
        Critical.statusValue = currentCalculatedCritical;
        
        if (CurrentHp > Hp.statusValue)
        {
            CurrentHp = Hp.statusValue;
        }
    }
     public void EquipItem(ItemData itemToEquip)
    {
        if (!itemToEquip) return;

        ItemData previousItem = null;

        switch (itemToEquip.itemType)
        {
            case ItemType.Weapon:
                previousItem = equippedWeapon;
                equippedWeapon = itemToEquip;
                break;
            case ItemType.Armor:
                previousItem = equippedArmor;
                equippedArmor = itemToEquip;
                break;
            case ItemType.Helmet:
                previousItem = equippedHelmet;
                equippedHelmet = itemToEquip;
                break;
            case ItemType.Accessory:
                previousItem = equippedAccessory;
                equippedAccessory = itemToEquip;
                break;
            default:
                return;
        }
        
        if (previousItem)
        {
            Debug.Log($"{previousItem.itemName} 장비 해제됨.");
        }

        Debug.Log($"{itemToEquip.itemName} 장착 완료!");
        CalculateTotalStats();
        CurrentHp = GetHpValue();
    }

    public void UnequipItem(ItemType itemTypeToUnequip)
    {
        ItemData unequippedItem = null;
        switch (itemTypeToUnequip)
        {
            case ItemType.Weapon:
                unequippedItem = equippedWeapon;
                equippedWeapon = null;
                break;
            case ItemType.Armor:
                unequippedItem = equippedArmor;
                equippedArmor = null;
                break;
            case ItemType.Helmet:
                unequippedItem = equippedHelmet;
                equippedHelmet = null;
                break;
            case ItemType.Accessory:
                unequippedItem = equippedAccessory;
                equippedAccessory = null;
                break;
            default:
                return;
        }

        if (unequippedItem)
        {
            Debug.Log($"{unequippedItem.itemName} 장비 해제됨.");
        }

        CalculateTotalStats();
        CurrentHp = Mathf.Min(CurrentHp, GetHpValue());

    }
    public void SetAttackStatus(StatusData status)
    {
        Attack = status;
    }
    public void SetDefenseStatus(StatusData status)
    {
        Defense = status;
    }

    public void SetHpStatus(StatusData status)
    {
        Hp = status;
    }

    public void SetCriticalStatus(StatusData status)
    {
        Critical = status;
    }

    public void SetGold(int gold)
    {
        Gold = gold;
    }
    public int GetAttackValue()
    {
        return Attack ? Attack.statusValue : 0;
    }

    public int GetDefenseValue()
    {
        return Defense ? Defense.statusValue : 0;
    }

    public int GetHpValue()
    {
        return Hp ? Hp.statusValue : 0;
    }

    public int GetCriticalValue()
    {
        return Critical? Critical.statusValue : 0;
    }
    public void LevelUp()
    {
        Level++;
    }
    
}
