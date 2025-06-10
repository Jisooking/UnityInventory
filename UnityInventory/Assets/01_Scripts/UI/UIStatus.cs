using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic; 
using TMPro; 

public class UIStatus : MonoBehaviour
{
   public GameObject statusSlotPrefab;
   public Transform slotParent;
   
   private List<GameObject> statusSlots = new List<GameObject>(); // 생성된 슬롯을 관리하기 위한 리스트
   
    // UI를 초기화하고 플레이어 스탯을 표시하는 메서드
    public void InitializeUI()
    {
        var character = GameManager.Instance.Player;
        // 이전에 생성된 슬롯이 있다면 모두 제거합니다.

        if (character == null)
        {
            return;
        }
        
        // 2. 플레이어의 각 StatusData를 사용하여 슬롯 생성 및 할당
        AddStatusSlot(character.Attack);
        AddStatusSlot(character.Defense);
        AddStatusSlot(character.Hp);
        AddStatusSlot(character.Critical);
    }

    private void AddStatusSlot(StatusData statusData)
    {
        if (!statusSlotPrefab)
        {
            return;
        }

        if (!statusData)    
        {
            return; 
        }

        GameObject newSlotGO = Instantiate(statusSlotPrefab, slotParent);
        StatusSlot slotUI = newSlotGO.GetComponent<StatusSlot>();

        if (slotUI)
        {
            slotUI.SetData(statusData);
            statusSlots.Add(newSlotGO); // 생성된 슬롯을 리스트에 추가하여 관리
        }
        else
        {
            Destroy(newSlotGO); // 컴포넌트가 없으면 생성된 GameObject 삭제
        }
    }
}
