using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "NewStatData", menuName = "ScriptableObjects/Stat Data")]
public class StatusData : ScriptableObject
{
    public string statusName; 
    public Sprite statusIcon;
    public int statusValue;   
}