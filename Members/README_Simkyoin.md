# 🧠 Unity 협업 프로젝트 - 개인 기술 리드미 (저장 시스템 & 퀘스트 시스템 중심)

---

## 📌 역할 및 담당 시스템

- **저장 시스템 (Save System)** 전반 설계 및 구현
- **퀘스트 시스템** 구조 설계, ScriptableObject 기반 확장 구현
- 협업을 위한 **코드 컨벤션 정리 및 반영**
- **StageManager / SkillManager / BuildingManager / QuestManager** 저장 연동

---

## ⚙️ 기술 스택 및 주요 구현

- **Unity C#**
- **ScriptableObject 기반 테이블 구조 (QuestTable, SkillTable 등)**
- **JsonUtility 기반 직렬화 저장 시스템**
- **매니저 단위 상태 추출 및 복원 방식**
- **EventChannelSO 기반 이벤트 통신 구조**

---

## ✅ 주요 기술적 의사결정

### 1. ScriptableObject 기반 데이터 관리
- **사용 이유**: 에셋 기반 데이터 관리를 통한 확장성과 직관성 확보
- **결과**: 퀘스트, 스킬 등 다양한 게임 데이터를 깔끔하게 분리 가능

### 2. 매니저 단위 Json 직렬화 저장 방식
- **사용 이유**: 각 시스템이 자신의 상태만 관리하도록 하여 책임 분리
- **결과**: 확장성과 유지보수성이 우수한 저장 구조 완성

### 3. 이벤트 채널 기반 시스템 통신
- **사용 이유**: 시스템 간 직접 참조를 줄이고 느슨한 결합 유지
- **결과**: 매니저, UI, 게임 로직 간 의존도 최소화

---

## ⚠ 트러블슈팅 사례

### 퀘스트 테이블 로드 실패
- **문제**: `QuestManager`가 `QuestTable`을 참조하지 못함
- **원인**: ScriptableObject가 Resources 폴더 외부에 있거나 초기화 타이밍 문제
- **해결**: `Coroutine`을 통한 지연 초기화 (`DelayedInitialize()`), 경로 수정

---

## 🔐 저장 연동된 주요 매니저

| 매니저 | 연동 방식 |
|--------|-----------|
| `QuestManager` | `GetAllProgress()` / `ApplyLoadedProgress()` |
| `SkillManager` | `GetUnlockedSkillIds()` / `ApplyUnlockedSkillIds()` |
| `BuildingManager` | `GetAllBuildingData()` / `RebuildFromData()` |
| `PlayerController` | 위치 |
| `GoldManager` | `Gold` 프로퍼티 |
| `StageManager` | `MaxStage` 프로퍼티 |

---

## ✨ 핵심 설계 철학

> 데이터 중심 설계 + 단방향 통신 구조 + 매니저 책임 분리
