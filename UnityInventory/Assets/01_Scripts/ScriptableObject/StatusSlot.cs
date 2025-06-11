using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatusSlot : MonoBehaviour
{
    public TextMeshProUGUI statusNameText;
    public Image statusIconImage;
    public TextMeshProUGUI statusValueText;
    
    public void SetData(StatusData data)
    {
        if (data)
        {
            if (statusIconImage)
            {
                statusIconImage.sprite = data.statusIcon; // StatusData의 Sprite를 Image 컴포넌트의 sprite 속성에 할당
                statusIconImage.enabled = (data.statusIcon); // 아이콘이 없으면 Image 컴포넌트 비활성화
            }
            if (statusValueText)
            {
                statusValueText.text = data.statusValue.ToString();
            }

            if (statusNameText)
            {
                statusNameText.text = data.statusName;
            }
        }
        else // 데이터가 null일 경우 슬롯 비활성화 또는 초기화
        {
            if (statusIconImage)
            {
                statusIconImage.sprite = null;
                statusIconImage.enabled = false;
            }
            if (statusValueText)
            {
                statusValueText.text = "";
            }
            if (statusNameText)
            {
                statusNameText.text = "";
            }
        }
    }
}