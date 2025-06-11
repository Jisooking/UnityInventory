using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic; 
using TMPro; 

public class UIStatus : MonoBehaviour
{
   public GameObject statusSlotPrefab;
   public Transform slotParent;
   
   private List<GameObject> statusSlots = new List<GameObject>(); // 생성된 슬롯을 관리하기 위한 리스트
   
   private void OnEnable()
   {
       StartCoroutine(WaitForPlayerCharacter());
   }
   
   private IEnumerator WaitForPlayerCharacter()
   {
       yield return new WaitUntil(() => PlayerManager.Instance && PlayerManager.Instance.playerCharacter != null);

       InitializeUI();
   }
   
    // UI를 초기화하고 플레이어 스탯을 표시하는 메서드
    public void InitializeUI()
    {
        ClearStatusSlots();
                
        var character = PlayerManager.Instance.playerCharacter;

        foreach (var status in character.GetAllStatuses())
        {
            AddStatusSlot(status);
        }
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
    public void UpdateStatusUI()
    {
        InitializeUI();
    }
    
    private void ClearStatusSlots()
    {
        foreach (GameObject slotGO in statusSlots)
        {
            if (slotGO)
            {
                Destroy(slotGO);
            }
        }
        statusSlots.Clear();
    }
}
