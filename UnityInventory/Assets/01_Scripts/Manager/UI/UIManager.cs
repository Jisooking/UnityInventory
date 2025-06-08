using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager Instance { get; set; } // 싱글톤

    [SerializeField]
    private Canvas UIMainMenuCanvas; 
    [SerializeField]
    private Canvas UIStatusCanvas;   
    [SerializeField]
    private Canvas UIInventoryCanvas;

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
        Button statusButton = UIMainMenuCanvas.transform.GetChild(0).GetComponent<Button>();
        Button inventoryButton = UIStatusCanvas.transform.GetChild(1).GetComponent<Button>();
        statusButton.onClick.AddListener(OpenStatusUI);
        inventoryButton.onClick.AddListener(OpenInventoryUI);
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