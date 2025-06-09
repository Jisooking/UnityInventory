using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic; // List 사용을 위해
using TMPro; // TextMeshPro 사용 시

public class UIStatus : MonoBehaviour
{
    [Header("UI References (Basic Player Info)")]
    // 기존 플레이어 정보 UI 요소들을 여기에 연결합니다.
    // 예를 들어, 플레이어 이름, 레벨, 경험치 바 등
    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private TextMeshProUGUI playerLevelText;
    [SerializeField] private Slider playerExpSlider;
    [SerializeField] private TextMeshProUGUI playerExpAmountText;
    [SerializeField] private Image characterImage;
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI characterDescriptionText;


    [Header("Status Slots Setup")]
    [SerializeField] private Transform statusSlotsParent; // Grid Layout Group이 있는 Content 오브젝트
    [SerializeField] private UIStatusSlot statusSlotPrefab; // StatusSlot 프리팹

    [Header("Stat Data Assets")]
    // Inspector에서 이 리스트에 모든 StatusData ScriptableObject 에셋들을 연결합니다.
    [SerializeField] private List<StatusData> allStatusDataAssets;

    // 현재 생성된 슬롯들을 관리하기 위한 리스트
    private List<UIStatusSlot> activeStatusSlots = new List<UIStatusSlot>();

    // (가정) 플레이어의 실제 능력치 데이터를 저장하는 클래스 (임시용)
    // 실제 게임에서는 PlayerManager나 CharacterStats 컴포넌트 등에서 이 데이터를 관리할 것입니다.
    public class PlayerCurrentStats
    {
        public string Name = "Chad";
        public int Level = 10;
        public int CurrentExp = 9;
        public int MaxExp = 12;
        public int Coins = 20000;
        public Sprite CharacterSprite; // 실제 캐릭터 스프라이트

        // Key: StatusData의 statusName, Value: 현재 캐릭터의 해당 능력치 값
        public Dictionary<string, int> CurrentStatusValues = new Dictionary<string, int>()
        {
            { "Attack", 35 },
            { "Defense", 40 },
            { "Hp", 100 },
            { "Critical", 25 },
            // 여기에 다른 능력치들을 추가
        };

        public string Description = "고집의 노예가 된지 1년짜리 되는 마법사입니다. 오늘도 환상에만 남아서 시간을 .... 내키고 있네요.";
    }

    private PlayerCurrentStats _currentPlayerStats = new PlayerCurrentStats(); // 임시 플레이어 스탯 인스턴스

    void Start()
    {
        // 예시: 캐릭터 스프라이트 할당 (실제로는 게임 시작 시 또는 캐릭터 로딩 시 할당)
        // _currentPlayerStats.CharacterSprite = Resources.Load<Sprite>("Sprites/YourCharacterSprite");

        RefreshUI(); // UI 초기화 및 업데이트
    }

    /// <summary>
    /// 전체 스테이터스 UI를 새로고침합니다.
    /// </summary>
    public void RefreshUI()
    {
        UpdatePlayerBasicInfo(); // 기본 정보 업데이트
        GenerateStatusSlots();   // 능력치 슬롯 업데이트
    }

    /// <summary>
    /// 플레이어의 기본 정보 UI를 업데이트합니다.
    /// </summary>
    private void UpdatePlayerBasicInfo()
    {
        if (playerNameText != null) playerNameText.text = _currentPlayerStats.Name;
        if (playerLevelText != null) playerLevelText.text = $"LV {_currentPlayerStats.Level}";

        if (playerExpSlider != null)
        {
            playerExpSlider.minValue = 0;
            playerExpSlider.maxValue = _currentPlayerStats.MaxExp;
            playerExpSlider.value = _currentPlayerStats.CurrentExp;
        }
        if (playerExpAmountText != null) playerExpAmountText.text = $"{_currentPlayerStats.CurrentExp}/{_currentPlayerStats.MaxExp}";

        if (characterImage != null && _currentPlayerStats.CharacterSprite != null) characterImage.sprite = _currentPlayerStats.CharacterSprite;
        if (coinText != null) coinText.text = _currentPlayerStats.Coins.ToString("N0"); // 천단위 구분자 표시
        if (characterDescriptionText != null) characterDescriptionText.text = _currentPlayerStats.Description;
    }

    /// <summary>
    /// StatusData를 기반으로 능력치 슬롯들을 동적으로 생성하고 업데이트합니다.
    /// </summary>
    private void GenerateStatusSlots()
    {
        // 기존에 생성된 모든 슬롯 제거 (UI 갱신 시 재사용 또는 파괴 후 재생성)
        foreach (var slot in activeStatusSlots)
        {
            Destroy(slot.gameObject); // 오브젝트 파괴
        }
        activeStatusSlots.Clear(); // 리스트 비우기

        // allStatusDataAssets 리스트에 있는 각 StatusData에 대해 슬롯 생성
        foreach (StatusData statusData in allStatusDataAssets)
        {
            // StatusSlot 프리팹 인스턴스 생성
            UIStatusSlot newSlot = Instantiate(statusSlotPrefab, statusSlotsParent);
            newSlot.name = $"StatusSlot_{statusData.statusName}"; // 생성된 오브젝트의 이름 설정

            // 플레이어의 현재 능력치 값 가져오기
            int statValue = 0;
            // _currentPlayerStats.CurrentStatusValues 딕셔너리에서 해당 능력치 이름의 값을 찾습니다.
            if (_currentPlayerStats.CurrentStatusValues.TryGetValue(statusData.statusName, out int value))
            {
                statValue = value;
            }

            // UIStatusSlot 스크립트의 SetSlotData 함수를 호출하여 UI 업데이트
            newSlot.SetSlotData(statusData, statValue);
            activeStatusSlots.Add(newSlot); // 생성된 슬롯 리스트에 추가
        }
    }

    /// <summary>
    /// 외부에서 플레이어의 모든 스탯 데이터를 업데이트할 때 호출합니다.
    /// </summary>
    /// <param name="newStats">새로운 플레이어 스탯 데이터.</param>
    public void UpdatePlayerStats(PlayerCurrentStats newStats)
    {
        _currentPlayerStats = newStats; // 플레이어 스탯 데이터 갱신
        RefreshUI(); // UI 새로고침
    }

    // 테스트용: 인스펙터에서 캐릭터 스프라이트를 할당할 수 있도록 합니다.
    public void SetCharacterSprite(Sprite sprite)
    {
        _currentPlayerStats.CharacterSprite = sprite;
        // 만약 캐릭터 이미지가 Panel 안에 있다면, 해당 Panel의 Image 컴포넌트를 업데이트해야 합니다.
        if (characterImage != null) characterImage.sprite = _currentPlayerStats.CharacterSprite;
    }
}