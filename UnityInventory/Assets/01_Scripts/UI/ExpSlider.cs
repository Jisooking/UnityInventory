using UnityEngine;
using UnityEngine.UI; // UI.Text를 사용한다면 필요 (TextMeshProUGUI는 필요 없음)
using TMPro; // TextMeshPro를 사용한다면 필요

public class ExperienceBar : MonoBehaviour
{
    public Slider expSlider; // 유니티 에디터에서 슬라이더를 연결해줄 변수
    public TextMeshProUGUI levelText; // 유니티 에디터에서 레벨 텍스트를 연결해줄 변수 (TextMeshProUGUI)
    // public Text levelText; // 만약 기존 UI.Text를 사용한다면 이 주석을 해제하고 위 TMPro 라인을 주석처리

    [Header("Experience Settings")]
    public float currentExp = 0;
    public float maxExp = 100; // 현재 레벨의 최대 경험치
    public float expPerTick = 5; // 한 번에 획득할 경험치 양
    public float tickInterval = 1.0f; // 경험치를 획득할 시간 간격 (초)

    [Header("Level Settings")]
    public int currentLevel = 1; // 현재 레벨

    void Start()
    {
        expSlider.maxValue = maxExp;
        expSlider.value = currentExp;
        UpdateLevelText(); // 게임 시작 시 초기 레벨 텍스트 설정

        InvokeRepeating("AddExperienceTick", 0f, tickInterval);
    }

    void AddExperienceTick()
    {
        AddExperience(expPerTick);
    }

    public void AddExperience(float amount)
    {
        currentExp += amount;

        if (currentExp >= maxExp)
        {
            // 레벨업 처리:
            // 남은 경험치 계산 (선택 사항, 초과된 경험치를 다음 레벨로 이월할 경우)
            // float overflowExp = currentExp - maxExp;

            currentLevel++; // 레벨 증가
            currentExp = 0; // 경험치 0으로 초기화 (또는 overflowExp로 설정)
            maxExp = CalculateNextMaxExp(currentLevel); // 다음 레벨의 최대 경험치 계산

            Debug.Log($"레벨업! 현재 레벨: {currentLevel}, 다음 레벨까지 필요 경험치: {maxExp}");
            UpdateLevelText(); // 레벨 텍스트 업데이트
            LevelUpEffects(); // 레벨업 시 시각/청각 효과 등을 여기에 추가
        }

        UpdateExpBar();
    }

    void UpdateExpBar()
    {
        expSlider.value = currentExp;
        // Debug.Log($"Current Exp: {currentExp}/{maxExp}"); // 필요시 주석 해제
    }

    // 레벨 텍스트를 업데이트하는 함수
    void UpdateLevelText()
    {
        if (levelText != null)
        {
            levelText.text = "Lv " + currentLevel.ToString(); // "Lv.1", "Lv.2" 등으로 설정
            // Debug.Log("Level Text Updated: " + levelText.text); // 디버그용
        }
        else
        {
            Debug.LogError("레벨 텍스트(TextMeshProUGUI)가 연결되지 않았습니다!");
        }
    }

    // 다음 레벨까지 필요한 최대 경험치를 계산하는 함수 (예시)
    float CalculateNextMaxExp(int level)
    {
        // 레벨이 올라갈수록 필요 경험치가 증가하는 로직
        // 예시: 1레벨 100, 2레벨 150, 3레벨 200...
        return 100 + (level - 1) * 50;
    }

    // 레벨업 시 호출될 효과 (선택 사항)
    void LevelUpEffects()
    {
        // 파티클 시스템 재생, 사운드 재생, 애니메이션 등
        Debug.Log("레벨업 효과 재생!");
    }

    // (기존 레벨업 함수는 위 AddExperience 내에 통합되었습니다. 필요시 분리하여 사용)
    // public void LevelUp(float newMaxExp)
    // {
    //     // 이 함수는 더 이상 필요 없을 수 있습니다.
    // }
}