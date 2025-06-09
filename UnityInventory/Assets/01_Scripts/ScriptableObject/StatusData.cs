using UnityEngine;

[CreateAssetMenu(fileName = "NewStatData", menuName = "ScriptableObjects/Stat Data")]
public class StatusData : ScriptableObject
{
    public string statusName; // 능력치 이름 (예: "공격력", "방어력")
    public Sprite statusIcon; // 능력치 아이콘 이미지
    public int statusValue;   // 기본 능력치 값 (나중에 캐릭터 스탯과 연동하여 최종값 계산)
    
    // public string description;
}