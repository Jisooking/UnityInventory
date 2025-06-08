using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager Instance { get; set; } // 싱글톤

    [SerializeField] private Canvas UIMainMenuCanvas;
    [SerializeField] private Canvas UIStatusCanvas;
    [SerializeField] private Canvas UIInventoryCanvas;
    [SerializeField] private Button statusButton;
    [SerializeField] private Button inventoryButton;

    public Canvas CurrentActiveCanvas { get; private set; }

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
    }

    private void Start()
    {
        if (statusButton)
        {
            statusButton.onClick.AddListener(OpenStatusUI);
        }
        else
        {
            Debug.Log("연결된 버튼이 없습니다.");
        }
        if (inventoryButton)
        {
            inventoryButton.onClick.AddListener(OpenInventoryUI);
        }
        else
        {
            Debug.Log("연결된 버튼이 없습니다.");
        }
        OpenMainMenuUI();
    }

    private void CloseAllCanvases()
    {
        UIMainMenuCanvas.gameObject.SetActive(false);
        UIStatusCanvas.gameObject.SetActive(false);
        UIInventoryCanvas.gameObject.SetActive(false);
    }

    public void OpenMainMenuUI()
    {
        CloseAllCanvases();
        UIMainMenuCanvas.gameObject.SetActive(true);
        CurrentActiveCanvas = UIMainMenuCanvas;
    }

    public void OpenStatusUI()
    {
        CloseAllCanvases();
        UIStatusCanvas.gameObject.SetActive(true);
        CurrentActiveCanvas = UIStatusCanvas;
    }

    public void OpenInventoryUI()
    {
        CloseAllCanvases();
        UIInventoryCanvas.gameObject.SetActive(true);
        CurrentActiveCanvas = UIInventoryCanvas;
    }

    public void GoBackToMainMenu()
    {
        OpenMainMenuUI();
    }
}