본 프로젝트는 FSM, 오브젝트 풀링, 스킬/스탯, 타워 설치, 보스 스킬, 퀘스트, 세이브/로드 등 다양한 게임 시스템을 모듈화하고 확장성 있는 구조로 개발하였습니다.

## 🧠 FSM (Finite State Machine)

- 제네릭 기반 상태 머신 (StateMachine<T, TState>)으로 구현
- IState<TOwner, TState> 인터페이스를 통해 상태별 책임 분리
- 사용 대상:
  - 플레이어: Idle, Move, Run, Attack
  - 몬스터: Idle, Move, Attack, Die
  - 보스: Idle, Move, Attack, Skill, Die
  - 타워: Build, Idle, Attack
- 특징:
  - SOLID 원칙 기반 (특히 OCP, SRP)
  - 상태별 코루틴, 애니메이션, FSM 전이 로직 독립 구현

---

## ♻️ Object Pooling
- ObjectPoolManager를 중심으로 설계된 재사용 가능한 오브젝트 풀링 시스템
- IPoolObject 인터페이스로 풀링 대상 오브젝트의 초기화 / 반환 처리 통일
- 에디터 상에서 "오브젝트 등록" 버튼으로 자동 할당 지원 (ObjectPoolManagerEditor.cs)
- 자동 풀 생성 / 반환 딜레이 / 런타임 동적 생성 모두 지원

---

## 🧬 Skill / Stat / Buff 시스템
🔹 액티브 스킬 (SkillManager.cs)
- 쿨타임 관리, 스킬 인스턴스 생성, 트랜스폼 기반 실행 구조
- SkillTable 기반으로 스킬 데이터 조회 및 실행

🔹 패시브 스킬 (PassiveSkillManager.cs)
- 랜덤 3종 선택 → 선택한 스킬은 StatusEffectManager를 통해 적용
- 골드 획득 / 이동속도 증가 / 공격 관련 스탯 강화 등 다양한 효과 지원

🔹 스탯 시스템 (StatManager.cs)
- CalculatedStat, ResourceStat 구조 분리
- 버프, 장비, 회복, 소모를 세부적으로 분리 (StatModifierType 기반)
- 실시간 동기화 및 최대 체력 반영 처리 포함

🔹 상태이상 (StatusEffectManager.cs)
- 버프/디버프/회복 등 타이머 기반 효과 관리
- 중첩 제한, 우선순위 처리 등 확장 가능

---

## 🏰 타워 설치 시스템

- BuildingPlacer와 GridManager를 기반으로 타워 배치
- BuildingGhost로 실시간 프리뷰 및 설치 가능 여부 표시
- GridCell 단위로 충돌 감지 및 겹침 방지 처리
- 설치 완료 시 타워 FSM 상태 Build -> Idle 전이
- 업그레이드 / 파괴 / 저장 가능 (TowerController.cs)

---

## 💥 보스 스킬 시스템
- BossSkillController는 IPoolObject를 구현해 풀링 가능
- SetTarget()으로 대상 지정 후 FireBossSkill() 코루틴 동작
- 디버깅용 트리거 (BossSkillTrigger)는 구조 상 비활성 처리됨 (로직 대체됨)

## 💾 세이브 / 로드 시스템
- SaveManager를 통해 JSON 기반 저장/불러오기 지원
- 저장 대상:
  - 퀘스트 진행도 (QuestProgress)
  - 스킬 해금 여부 (UnlockedSkills)
  - 배치된 타워 정보 (BuildingSaveData)
  - 플레이어 위치, 골드, 스테이지 정보
- S, L 키로 수동 저장/로드 테스트 가능

---

## 📜 퀘스트 시스템
- QuestManager에서 전체 퀘스트 초기화 및 진행도 관리
- QuestData (SO) + QuestProgress (Data) 구조
- 조건 달성 시 자동으로 완료 처리 (UpdateProgress)
- 보상 수령 (ClaimReward) 시 골드 획득
- UI 연동: QuestPanelUI 자동 갱신
- 세이브/로드 연동도 포함

