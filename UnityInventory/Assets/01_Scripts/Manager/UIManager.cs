using UnityEngine;
using UnityEngine.Events; // UnityEvent는 필요에 따라 남겨두거나 제거 가능
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro; // Stack<T> 사용을 위해 필요

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; } // 싱글톤 (set은 private이 더 안전)

    // 전역 뒤로가기 스택: 각 UI 전환 시 이전 상태로 돌아가는 동작을 저장합니다.
    private Stack<System.Action> backActionStack = new Stack<System.Action>();

    [Header("UI Canvases (Panels)")]
    [SerializeField] private GameObject UIMainMenuPanel; // Canvas 대신 GameObject로 관리하여 더 유연하게
    [SerializeField] private GameObject UIStatusPanel;
    [SerializeField] private GameObject UIInventoryPanel;

    [Header("Buttons for Opening Panels")]
    [SerializeField] private Button statusOpenButton; // 메인 메뉴에서 스탯 열기 버튼
    [SerializeField] private Button inventoryOpenButton; // 메인 메뉴에서 인벤토리 열기 버튼

    [Header("Global Back Button")]
    [SerializeField] private Button globalBackButton; // 씬에 하나만 존재하고 모든 뒤로가기 동작을 담당할 버튼
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환 시에도 유지하고 싶다면 사용
        }
    }

    void Start()
    {
        InitializeUI();
        OpenMainMenuUI(); // 게임 시작 시 메인 메뉴 열기
    }

    private void InitializeUI()
    {
        // 모든 패널을 비활성화 상태로 시작
        CloseAllPanels();

        // 버튼 리스너 등록
        if (statusOpenButton != null)
            statusOpenButton.onClick.AddListener(OpenStatusUI);
        if (inventoryOpenButton != null)
            inventoryOpenButton.onClick.AddListener(OpenInventoryUI);
        
        // 전역 뒤로가기 버튼 리스너 등록
        if (globalBackButton != null)
        {
            globalBackButton.onClick.AddListener(OnGlobalBackButtonClicked);
            globalBackButton.gameObject.SetActive(false); // 초기에는 비활성화
        }
    }

    private void CloseAllPanels()
    {
        // 모든 UI 패널 GameObject를 비활성화합니다.
        UIMainMenuPanel?.SetActive(false);
        UIStatusPanel?.SetActive(false);
        UIInventoryPanel?.SetActive(false);
        // 새로운 패널이 추가될 때마다 여기에 추가
    }

    // --- UI 패널 열고 닫는 메서드 ---

    // 제네릭하게 패널을 활성화하고, 이전 패널을 스택에 푸시하는 메서드
    private void ActivatePanel(GameObject panelToActivate, GameObject panelToDeactivate, System.Action onBackAction)
    {
        // 현재 활성화된 패널이 있다면, 그 패널을 닫는 동작을 스택에 푸시합니다.
        // 그리고 그 패널을 비활성화합니다.
        if (panelToDeactivate != null && panelToDeactivate.activeSelf)
        {
            // 현재 패널을 닫고, 이전 패널을 활성화하는 동작을 스택에 추가
            // UIMainMenuPanel로 돌아오는 동작을 스택에 푸시
            // 만약 MainMenuPanel -> StatusPanel -> InventoryPanel 순서로 열었다면,
            // Inventory에서 뒤로가기 누르면 Status로, Status에서 뒤로가기 누르면 MainMenu로 가야 합니다.
            // 따라서 'panelToDeactivate'를 다시 활성화하는 동작을 푸시해야 합니다.
            backActionStack.Push(() => {
                panelToActivate.SetActive(false); // 현재 패널 닫기
                panelToDeactivate.SetActive(true); // 이전 패널 열기
                UpdateGlobalBackButtonVisibility(); // 스택 변화에 따라 버튼 가시성 업데이트
            });
        }
        
        CloseAllPanels(); // 모든 패널을 일단 닫고 시작
        panelToActivate.SetActive(true); // 원하는 패널 활성화
        
        UpdateGlobalBackButtonVisibility(); // 스택이 변경되었으므로 버튼 가시성 업데이트
        Debug.Log($"Opened: {panelToActivate.name}, Stack Size: {backActionStack.Count}");
    }


    public void OpenMainMenuUI()
    {
        // 메인 메뉴는 가장 기본이 되는 화면이므로, 스택을 비우고 시작합니다.
        // 메인 메뉴에서 뒤로가기 버튼은 비활성화됩니다.
        CloseAllPanels();
        UIMainMenuPanel.SetActive(true);
        backActionStack.Clear(); // 메인 메뉴로 돌아오면 스택을 비웁니다.
        UpdateGlobalBackButtonVisibility();
        Debug.Log("Opened: Main Menu, Stack Cleared");
    }

    public void OpenStatusUI()
    {
        // 스탯 메뉴를 열기 전에 현재 활성화된 패널이 무엇인지 확인하고 그 패널을 스택에 푸시합니다.
        GameObject previousPanel = GetActivePanel();
        ActivatePanel(UIStatusPanel, previousPanel, () => { /* 이전 패널로 돌아가는 동작 */ });
        Debug.Log("OpenStatusUI 호출됨");
    }
    
    public void OpenInventoryUI()
    {
        GameObject previousPanel = GetActivePanel();
        ActivatePanel(UIInventoryPanel, previousPanel, () => { /* 이전 패널로 돌아가는 동작 */ });
        Debug.Log("OpenInventoryUI 호출됨");
    }

    // 현재 활성화된 패널 GameObject를 반환하는 헬퍼 함수
    private GameObject GetActivePanel()
    {
        if (UIMainMenuPanel.activeSelf) return UIMainMenuPanel;
        if (UIStatusPanel.activeSelf) return UIStatusPanel;
        if (UIInventoryPanel.activeSelf) return UIInventoryPanel;
        return null;
    }

    // --- 뒤로가기 스택 관리 및 전역 뒤로가기 버튼 동작 ---

    // 전역 뒤로가기 버튼 클릭 시 호출될 함수
    public void OnGlobalBackButtonClicked()
    {
        if (backActionStack.Count > 0)
        {
            System.Action previousAction = backActionStack.Pop(); // 스택에서 가장 최근 동작을 꺼냄
            previousAction?.Invoke(); // 그 동작을 실행 (이전 패널 활성화)
            UpdateGlobalBackButtonVisibility(); // 스택이 변경되었으므로 버튼 가시성 업데이트
            Debug.Log("Global Back Button Clicked. Stack Size: " + backActionStack.Count);
        }
        else
        {
            // 스택이 비어있다는 것은 메인 메뉴에 있거나, 더 이상 뒤로 갈 곳이 없다는 의미.
            // 여기서는 메인 메뉴를 다시 활성화하거나, 게임 종료 팝업 등을 띄울 수 있습니다.
            OpenMainMenuUI(); // 스택이 비면 그냥 메인 메뉴로 (선택적)
            Debug.Log("No more screens to go back to. Returning to Main Menu.");
        }
    }

    // 스택 상태에 따라 전역 뒤로가기 버튼 활성화/비활성화
    private void UpdateGlobalBackButtonVisibility()
    {
        if (globalBackButton != null)
        {
            // 메인 메뉴에서는 뒤로가기 버튼이 필요 없으므로 스택이 비어있을 때 비활성화
            globalBackButton.gameObject.SetActive(backActionStack.Count > 0);
        }
    }
    
    // --- OnDestroy 시 리스너 제거 (메모리 누수 방지) ---
    void OnDestroy()
    {
        if (statusOpenButton != null)
            statusOpenButton.onClick.RemoveListener(OpenStatusUI);
        if (inventoryOpenButton != null)
            inventoryOpenButton.onClick.RemoveListener(OpenInventoryUI);
        if (globalBackButton != null)
            globalBackButton.onClick.RemoveListener(OnGlobalBackButtonClicked);
    }
}