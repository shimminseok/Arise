# Arise - 3D 타워 디펜스

## 📘 UI 시스템 요약
본 프로젝트에서 직접 제작한 주요 UI 스크립트 목록입니다.  
각 스크립트는 모듈화와 이벤트 기반 연동을 중점으로 설계되었습니다.

---

### 🎮 `UIManager.cs`
- 전체 UI 루트 관리 및 씬 전환 시 초기화 처리  
- 공통 UI 열기/닫기 기능 제공 (`Open<T>()`, `Close<T>()`)  
- 스탯 UI 연결 (`ConnectStatUI`) 및 BGM 관리 등 전역 UI 관리

---

### 🧾 퀘스트 UI 관련
- `QuestUIController.cs`: 퀘스트 패널 열기/닫기 제어  
- `QuestOpenButton.cs`, `QuestCloseButton.cs`: UI 버튼 입력 → 이벤트 채널 호출  
- 이벤트 채널을 통해 UI 흐름을 분리하여 구성

---

### 🏹 `StageInfoUI.cs`
- 현재 스테이지/웨이브 정보 표시  
- 남은 몬스터 수, 웨이브 카운트다운 UI 실시간 업데이트

---

### 🧠 스킬 UI 관련
- `SkillUIBinder.cs`: 스킬 쿨타임 UI 등록 및 연동  
- `SkillCooldownIndicator.cs`: 스킬 쿨타임 오버레이 및 남은 시간 표시

---

### 🏰 터렛 UI 관련
- `TurretPanelToggleButton.cs`: 터렛 패널 열기/닫기 애니메이션 및 버튼 텍스트 갱신  
- `TurretDescriptionPanel.cs`: 선택한 터렛의 상세 설명창 표시/숨김 제어

---

### 🔧 `UITowerUpgrade.cs`
- 설치된 타워 선택 시 업그레이드/삭제 UI 표시  
- 업그레이드 비용 표시 및 골드 차감, 최대레벨 처리 포함

---

### 💰 `UIGoldText.cs`
- 현재 골드 실시간 텍스트 표시  
- `GoldManager.OnGoldChanged` 이벤트를 구독하여 자동 갱신

---

### 🧱 `UIPlayerStatPanel.cs`
- 플레이어 및 무기 스탯(공격력, 연사속도, 이동속도) 실시간 표시  
- `StatManager.OnStatChanged`를 통해 값 변경 시 즉시 반영

---

### ⚙ 설정 UI 관련
- `SettingsPanel.cs`: BGM, 효과음, 마우스 감도 조절 UI 구성  
- `SettingsToggleButton.cs`: 설정창 열기/닫기 버튼 제어  
- 추후 저장 연동을 고려한 값 저장 구조 설계됨

---

> 🎯 **모든 UI는 `ScriptableObject 기반 이벤트 채널`과 `매니저 기반 구조`를 통해  
> 느슨한 연결(Decoupling)과 재사용성을 고려하여 설계되었습니다.**



## 👥 팀원별 모듈 문서

| 이름 | 담당 기능 | 문서 링크 |
|------|-----------|-----------|
| 박상민 | 스킬 / 스탯 / 버프 시스템 | [박상민_README.md](./Members/LeeREADME.md) |
| 심교인 | 보스 스킬, 풀링 시스템     | [심교인_README.md](./Members/ParkREADME.md) |
| 심민석 | 타워 설치, FSM, ObjectPooling,Stat     | [심민석_README.md](./Members/README_Shimminseok.md) |
| 전인우 | 보스 스킬, 풀링 시스템     | [전인우_README.md](./Members/ParkREADME.md) |

[⬅ 메인 리드미로 돌아가기](../README.md)
