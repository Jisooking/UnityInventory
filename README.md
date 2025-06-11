# UnityInventory
# 🧭 UnityInventory 프로젝트

Unity 기반 인벤토리 및 캐릭터 상태 보기 UI 구현 프로젝트입니다.  
**UGUI**를 활용해 메인 메뉴, 상태창(Status), 인벤토리(Inventory)를 전환하며 캐릭터 정보를 확인하고 아이템을 장착/해제할 수 있습니다.

---

## ✅ 구현 기능

### 1. 메인 화면 구성
- 아이디, 레벨, 골드 표시
- `Status` 버튼 → 상태 보기 UI 전환
- `Inventory` 버튼 → 인벤토리 UI 전환

---

### 2. Status 보기
- `Status`, `Inventory` 버튼 숨김
- 캐릭터 스탯 표시
- `뒤로가기` 버튼 → 메인 메뉴로 복귀

---

### 3. Inventory 보기
- `Status`, `Inventory` 버튼 숨김
- 스크롤 가능한 인벤토리 슬롯 표시
- 장착 아이템은 별도 표시
- `뒤로가기` 버튼 → 메인 메뉴로 복귀

---


### STEP 1. UI 구성하기
- `UIMainMenu`, `UIStatus`, `UIInventory` → 각각 `Canvas`로 제작
- 필요한 Sprite 이미지는 자유롭게 사용

---

### STEP 2. 스크립트 만들기
- 필수 스크립트:
  - `GameManager`, `UIManager`, `UIMainMenu`, `UIStatus`, `UIInventory`, `Character`
- 각 UI 스크립트에는 해당 UI의 하위 구성요소 연결
- `Character` 클래스에 플레이어 정보 및 생성자 작성

---

### STEP 3. UI 간 전환 기능
- `UIManager`를 싱글톤으로 구성
- 버튼 클릭 시:
  - 메인 메뉴 <→ 상태창
  - 메인 메뉴 <→ 인벤토리
- `OpenMainMenu`, `OpenStatus`, `OpenInventory` 메서드 구현
- 각 UI의 버튼에 `AddListener()`로 연결

---

### STEP 4. 캐릭터 정보 세팅
- `Character` 클래스 필드를 `{ get; private set; }`로 구성
- `GameManager`의 `SetData()`에서 플레이어 생성 및 데이터 세팅
- 각 UI에서 캐릭터 정보를 표시

---

### STEP 5. UISlot 동적 생성
- `UIInventory`에 ScrollView 구성
- `UISlot` 프리팹 생성 및 동적 인스턴스화
- 슬롯 리스트를 구성하고 `SetItem()`으로 정보 전달

---

### STEP 6. Item 데이터 구성
- `ItemData` 클래스에 아이템 정보 정의 (공격력, 방어력, 체력 보너스 등)
- `Character` 클래스에 인벤토리 및 장착 시스템 추가
- `GameManager`에서 아이템 생성 및 플레이어 인벤토리에 추가

---

### STEP 7. 아이템 장착 기능
- `UIInventory`에서 슬롯 클릭 시 아이템 장착
- 장착 상태는 슬롯 UI에 반영됨

---

### STEP 8. Status에 장착 정보 반영
- `Character` 클래스에서 장착 아이템 스탯 계산
- `UIStatus`에서 해당 스탯이 실시간 반영됨

---

## 🛠 Troubleshooting

### ❓ UIStatus가 처음 켜질 때 슬롯이 생성되지 않음

#### 현상  
- `UIStatus` 오브젝트를 처음 `SetActive(true)` 했을 때, `StatusSlot`들이 생성되지 않음  
- 두 번째부터는 정상 동작

#### 원인  
- `UIStatus.OnEnable()`에서 `InitializeUI()`를 호출할 때, `PlayerManager.Instance.playerCharacter`가 아직 초기화되지 않아 `null` 상태였음  
- 이로 인해 `InitializeUI()`가 조기 `return`됨 → 슬롯 미생성

#### 해결 방법  
- `Coroutine`을 사용해 `playerCharacter`가 준비될 때까지 기다린 후 `InitializeUI()`를 호출하도록 수정함
```
private void OnEnable()
{
    StartCoroutine(WaitForPlayerCharacter());
}

private IEnumerator WaitForPlayerCharacter()
{
    yield return new WaitUntil(() => PlayerManager.Instance && PlayerManager.Instance.playerCharacter != null);
    InitializeUI();
}

private void OnEnable()
{
    Debug.Log("UIStatus OnEnable called");
    StartCoroutine(WaitForPlayerCharacter());
}

private IEnumerator WaitForPlayerCharacter()
{
    Debug.Log("Waiting for playerCharacter...");
    yield return new WaitUntil(() => PlayerManager.Instance && PlayerManager.Instance.playerCharacter != null);
    Debug.Log("PlayerCharacter found, initializing UI");
    InitializeUI();
}
```

❓ 인벤토리 장착 장비의 능력치가 Status에 반영되지 않음

현상
인벤토리에서 아이템을 장착해도 Status UI 상의 공격력, 방어력, 체력 등이 변하지 않음

장비가 실제로는 Character.Equip()에 의해 장착되었지만, UI에는 반영되지 않음

원인
UIStatus나 UIInventory 등의 스크립트에서 필수 GameObject/프리팹/Transform 참조가 인스펙터에 연결되지 않음

특히 StatusSlotPrefab, slotParent, Text 필드 등은 코드상 문제가 없더라도 Unity 인스펙터에서 연결되지 않으면 null로 처리되어 UI 갱신이 되지 않음

해결 방법
관련 GameObject 및 프리팹을 반드시 인스펙터에 할당

필드가 null일 경우 Debug.LogWarning() 등으로 경고 로그 출력도 추가하면 디버깅에 도움됨

점검 체크리스트
 UIStatus > statusSlotPrefab 연결 여부

 UIStatus > slotParent 연결 여부

 StatusSlot 내 TextMeshProUGUI 필드들 연결 여부

 UIManager에 UIStatus, UIInventory 인스턴스가 모두 연결되었는지 확인

디버깅 로그
```
if (!statusSlotPrefab)
{
    Debug.LogWarning("StatusSlotPrefab is not assigned in the Inspector!");
    return;
}
```
💡 TODO
 아이템 Tooltip 기능 추가

 장비별 상세 UI (무기/방어구별 세부 능력치)

 저장/불러오기 기능 (PlayerPrefs 또는 Json)

📁 구조
Assets/
├── Scripts/
│   ├── Manager
│   │   ├── GameManager.cs
│   │   ├── PlayerManager.cs
│   │   └── UIManager.cs
│   ├── UI/
│   │   ├── UIMainMenu.cs
│   │   ├── UIStatus.cs
│   │   ├── UIInventory.cs
│   │   └── StatusSlot.cs
│   ├── ScriptableObject/
│   │   ├── ItemData.cs
│   │   └── StatusData.cs
├── Prefabs/
│   ├── ItemSlot.prefab
└── └── StatusSlot.prefab

🧑‍💻 개발 환경
Unity 2022.3.17

TextMeshPro 사용


