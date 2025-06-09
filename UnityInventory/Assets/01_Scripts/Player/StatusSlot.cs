using UnityEngine;
using UnityEngine.UI;
using TMPro; // TextMeshPro를 사용한다면

public class UIStatusSlot : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI statusNameText; // TextMeshProUGUI 대신 Text 사용 시 Text로 변경
    [SerializeField] private TextMeshProUGUI statusValueText; // TextMeshProUGUI 대신 Text 사용 시 Text로 변경

    // 이 슬롯이 표시할 StatusData 참조 (외부에서 주입될 것입니다)
    private StatusData _currentStatusData;

    /// <summary>
    /// 이 슬롯의 UI를 지정된 StatusData와 현재 값으로 업데이트합니다.
    /// </summary>
    /// <param name="data">표시할 StatusData ScriptableObject.</param>
    /// <param name="currentValue">플레이어의 실제 능력치 값.</param>
    public void SetSlotData(StatusData data, int currentValue)
    {
        _currentStatusData = data;

        // 아이콘 설정
        if (iconImage != null)
        {
            if (data.statusIcon != null)
            {
                iconImage.sprite = data.statusIcon;
                iconImage.enabled = true; // 아이콘이 있으면 보이게 함
            }
            else
            {
                iconImage.enabled = false; // 아이콘이 없으면 숨김
            }
        }

        // 능력치 이름 설정
        if (statusNameText != null)
        {
            statusNameText.text = data.statusName;
        }

        // 능력치 값 설정
        if (statusValueText != null)
        {
            statusValueText.text = currentValue.ToString();
        }

        // Debug.Log($"SetSlotData: {data.statusName}, Value: {currentValue}"); // 디버그용
    }

    /// <summary>
    /// 슬롯을 초기화하여 내용을 비웁니다 (재사용 시 유용).
    /// </summary>
    public void ClearSlot()
    {
        _currentStatusData = null;
        if (iconImage != null) iconImage.sprite = null;
        if (statusNameText != null) statusNameText.text = string.Empty;
        if (statusValueText != null) statusValueText.text = string.Empty;
    }
}