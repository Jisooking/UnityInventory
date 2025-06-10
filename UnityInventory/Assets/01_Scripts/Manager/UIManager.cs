using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; } 
    
    private Stack<System.Action> backActionStack = new Stack<System.Action>();

    [Header("UI Canvases (Panels)")]    // UI 판넬 연결
    [SerializeField] private GameObject UIMainMenuPanel;
    [SerializeField] private GameObject UIStatusPanel;
    [SerializeField] private GameObject UIInventoryPanel;

    private UIStatus uiStatus;
    
    [Header("Buttons for Opening Panels")]  // UI 오픈 버튼
    [SerializeField] private Button statusOpenButton; 
    [SerializeField] private Button inventoryOpenButton; 

    [Header("Global Back Button")]  // 뒤로가기 버튼
    [SerializeField] private Button globalBackButton;
    
    [Header("Gold Text")]
    [SerializeField] private TextMeshProUGUI goldText;
    
    void Awake()
    {
        if (Instance && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }

        if (UIStatusPanel)
        {
            uiStatus = UIStatusPanel.GetComponent<UIStatus>();
            if (uiStatus == null)
            {
                return;
            }
        }
    }

    void Start()
    {
        InitializeUI(); // 
        OpenMainMenuUI(); // 게임 시작 시 메인 메뉴 열기
    }

    private void Update()
    {
        goldText.text = ($"Gold : {GameManager.Instance.playerGoldStatus.ToString()}");
    }

    private void InitializeUI()
    {
        CloseAllPanels();
        
        if (statusOpenButton)
            statusOpenButton.onClick.AddListener(OpenStatusUI);
        if (inventoryOpenButton)
            inventoryOpenButton.onClick.AddListener(OpenInventoryUI);
        
        if (globalBackButton)
        {
            globalBackButton.onClick.AddListener(OnGlobalBackButtonClicked);
            globalBackButton.gameObject.SetActive(false); 
        }
    }

    private void CloseAllPanels() // 모든 UI 닫기
    {
        UIMainMenuPanel?.SetActive(false);
        UIStatusPanel?.SetActive(false);
        UIInventoryPanel?.SetActive(false);
    }
    
    private void ActivatePanel(GameObject panelToActivate, GameObject panelToDeactivate, System.Action onBackAction)
    {
        if (panelToDeactivate != null && panelToDeactivate.activeSelf)
        {
            backActionStack.Push(() => {    // 스택에 푸쉬
                panelToActivate.SetActive(false); // 현재 패널 닫기
                panelToDeactivate.SetActive(true); // 이전 패널 열기
                UpdateGlobalBackButtonVisibility(); // 스택 변화에 따라 버튼 가시성 업데이트
            });
        }
        
        CloseAllPanels(); // 모든 패널을 일단 닫고 시작
        panelToActivate.SetActive(true); // 원하는 패널 활성화
        
        UpdateGlobalBackButtonVisibility();
    }


    private void OpenMainMenuUI()
    {
        CloseAllPanels();   // 모든 UI 닫기
        UIMainMenuPanel.SetActive(true);
        backActionStack.Clear(); // 메인 메뉴로 돌아오면 스택 초기화
        UpdateGlobalBackButtonVisibility();
    }

    private void OpenStatusUI()
    {
        GameObject previousPanel = GetActivePanel();    // 판넬을 스택에 푸쉬
        ActivatePanel(UIStatusPanel, previousPanel, () => { /* 이전 판넬로 돌아가는 동작 */ });
        
        if (uiStatus)
        {
            uiStatus.InitializeUI();
        }
    }

    private void OpenInventoryUI()
    {
        GameObject previousPanel = GetActivePanel();
        ActivatePanel(UIInventoryPanel, previousPanel, () => { /* 이전 판넬로 돌아가는 동작 */ });
    }
    
    private GameObject GetActivePanel() // 현재 활성화된 판넬을 반환하는 동작
    {
        if (UIMainMenuPanel.activeSelf) return UIMainMenuPanel;
        if (UIStatusPanel.activeSelf) return UIStatusPanel;
        if (UIInventoryPanel.activeSelf) return UIInventoryPanel;
        return null;
    }

    // 뒤로가기 버튼 클릭 시 호출될 함수
    private void OnGlobalBackButtonClicked()
    {
        if (backActionStack.Count > 0)
        {
            System.Action previousAction = backActionStack.Pop(); // 스택에서 가장 최근 동작을 꺼냄
            previousAction?.Invoke(); // 그 동작을 실행 (이전 판넬 활성화)
            UpdateGlobalBackButtonVisibility(); // 스택이 변경되었으므로 버튼 가시성 업데이트
        }
        else
        {
            // 스택이 비어있다는 것은 메인 메뉴에 있다는 의미
            OpenMainMenuUI();
        }
    }

    // 스택 상태에 따라 뒤로가기 버튼 활성화/비활성화
    private void UpdateGlobalBackButtonVisibility()
    {
        if (globalBackButton)
        {
            // 메인 메뉴에서는 뒤로가기 버튼이 필요 없으므로 스택이 비어있을 때 비활성화
            globalBackButton.gameObject.SetActive(backActionStack.Count > 0);
        }
    }

    private void OnDestroy()    // 버튼 사라질 때 리스너도 제거
    {
        if (statusOpenButton)
            statusOpenButton.onClick.RemoveListener(OpenStatusUI);
        if (inventoryOpenButton)
            inventoryOpenButton.onClick.RemoveListener(OpenInventoryUI);
        if (globalBackButton)
            globalBackButton.onClick.RemoveListener(OnGlobalBackButtonClicked);
    }
}