using UnityEngine;
using UnityEngine.UI;

public class ScrollView : MonoBehaviour
{
    public GameObject slotPrefab; // 프리팹
    public Transform content;     // ScrollView의 Content

    public int slotCount = 30;

    void Start()
    {
        GenerateSlots();
    }

    void GenerateSlots()
    {
        for (int i = 0; i < slotCount; i++)
        {
            Instantiate(slotPrefab, content);
        }
    }
}