using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI; // List를 사용하기 위해 추가

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    public Character playerCharacter;

    [Header("Base Status")]
    public int baseAttack = 10;
    public int baseDefense = 5;
    public int baseHealth = 100;
    public int baseCritical = 5;

    [Header("Status Icon Asset")]
    public Sprite attackIcon;
    public Sprite defenseIcon;
    public Sprite hpIcon;
    public Sprite criticalIcon;

    [Header("Show UI")]
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI goldText;

    [Header("Equip UI")]
    public Image equippedWeaponIcon;
    public Image equippedArmorIcon;
    public Image equippedHelmetIcon;
    public Image equippedAccessoryIcon;
    
    public UIStatus uiStatus;
    [SerializeField]
    private ScrollView scrollview;
    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // Character 인스턴스 생성
        playerCharacter = new Character("Toothless", 1, 1000);

        // Character의 StatusData 인스턴스에 아이콘 할당
        // 이 아이콘들은 PlayerManager 인스펙터에서 할당
        playerCharacter.Attack.statusIcon = attackIcon;
        playerCharacter.Defense.statusIcon = defenseIcon;
        playerCharacter.Hp.statusIcon = hpIcon;
        playerCharacter.Critical.statusIcon = criticalIcon;

        // Character의 기본 능력치 설정
        playerCharacter.SetBaseStats(baseAttack, baseDefense, baseHealth, baseCritical);
    }

    void Start()
    {
        UpdatePlayerBasicUI();
        UpdateEquippedGearUI();

        if (uiStatus)
        {
            uiStatus.InitializeUI();
        }
    }

    public void UpdatePlayerBasicUI()
    {
        if (playerCharacter == null) return;
        if (nameText) nameText.text = playerCharacter.Name;
        if (levelText) levelText.text = $"Lv. {playerCharacter.Level}";
        if (goldText) goldText.text = $"골드: {playerCharacter.Gold}";
    }

    public void UpdateEquippedGearUI()
    {
        SetEquippedIcon(equippedWeaponIcon, playerCharacter.equippedWeapon);
        SetEquippedIcon(equippedArmorIcon, playerCharacter.equippedArmor);
        SetEquippedIcon(equippedHelmetIcon, playerCharacter.equippedHelmet);
        SetEquippedIcon(equippedAccessoryIcon, playerCharacter.equippedAccessory);
    }

    private void SetEquippedIcon(Image iconImage, ItemData item)
    {
        if (!iconImage) return;

        if (item && item.itemIcon)
        {
            iconImage.sprite = item.itemIcon;
            iconImage.enabled = true;
        }
        else
        {
            iconImage.sprite = null;
            iconImage.enabled = false;
        }
    }

    public void RequestEquipItem(ItemData itemToEquip)
    {
        playerCharacter.EquipItem(itemToEquip);
        UpdatePlayerBasicUI(); 
        UpdateEquippedGearUI(); 
        uiStatus?.UpdateStatusUI(); 
        scrollview.UpdateInventoryDisplay(); 
    }

    public void RequestUnequipItem(ItemType itemTypeToUnequip)
    {
        playerCharacter.UnequipItem(itemTypeToUnequip);
        UpdatePlayerBasicUI();
        UpdateEquippedGearUI();
        uiStatus?.UpdateStatusUI();
        scrollview.UpdateInventoryDisplay();
    }
}