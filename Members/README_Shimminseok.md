# Arise - 3D 타워 디펜스(심민석)

이 문서는 Arise 프로젝트 내에서 담당한 주요 시스템들의 내부 구조와 설계 철학을 중심으로 정리된 개인 기술 리드미입니다. 특히 타워 설치 시스템, Table 기반 구조, 오브젝트 풀링, 몬스터 관리, FSM(Finite State Machine) 시스템에 중점을 두고 설명합니다.

## 🧠 FSM (Finite State Machine)

### 구조
- 제네릭 기반 상태 머신 (StateMachine<T, TState>)으로 구현
- IState<TOwner, TState> 인터페이스를 통해 상태별 책임 분리
- 사용 대상:
  - 플레이어: Idle, Move, Run, Attack
  - 몬스터: Idle, Move, Attack, Die
  - 보스: Idle, Move, Attack, Skill, Die
  - 타워: Build, Idle, Attack
- 특징:
  - 각 상태는 CheckTransition()을 통해 상태 전이를 명확히 분리
  - 상태 간 의존성 없이 독립 구성 가능 (OCP 원칙 실현)
  - 코루틴 공격 루프, 애니메이션 트리거 제어, 이동 로직 등 상태별 책임 분리 철저

---

## ♻️ Object Pooling
### 구성 요소
- ObjectPoolManager: 메인 관리 클래스
- IPoolObject: 풀링 오브젝트 인터페이스. Initialize(), ReturnObject() 필수 구현
- ObjectPoolManagerEditor: 에디터 상에서 오브젝트 자동 등록 기능 제공

### 동작 방식
- GetObject(string key) 호출 시:
  - 존재하는 풀에서 SetActive(true)로 꺼내 사용
  - 없을 경우 자동 Instantiate 및 풀에 등록
- ReturnObject() 호출 시:
  - IPoolObject 구현체의 OnReturnToPool() 호출
  - 이후 SetActive(false) 및 풀에 반환

- 풀 키는 프리팹 이름을 기준으로 관리됨
---

## 🏰 타워 설치 시스템

### 구조도
![Image](https://github.com/user-attachments/assets/9543f5b6-9ec6-4c07-b7e6-159dbdc9f723)

### 핵심 기능
- 실시간 배치 미리보기: BuildingGhost는 SetMaterialColor와 SetPosition으로 설치 가능 여부 및 위치를 실시간 반영합니다.
- 그리드 충돌 검사: GridManager.CanPlaceBuilding()에서 GridCell을 기반으로 설치 가능 여부를 판단.
- 설치 완료 시 타워 FSM 상태 Build -> Idle 전이
- 배치 처리: BuildingPlacer에서 설치 확정 시, PlaceBuilding()을 통해 실제 위치에 오브젝트를 정렬 및 GridCell에 등록합니다.

---
## 👹 몬스터 시스템
### 핵심 구성
- 데이터: MonsterSO (스탯 및 외형 정의), MonsterTable (CSV 테이블 기반 정보)
- 로직: EnemyController (몬스터 AI), EnemyManager (스폰 및 관리)
### 특징
- 풀링 시스템과 통합되어 ObjectPoolManager로부터 몬스터 생성
- 몬스터는 FSM 기반으로 상태 전이 (Idle, Move, Attack, Die)
- EnemyManager는 웨이브/전체 리스트 관리 및 사망 시 Remove() 처리
- IsTargetInAttackRange()로 플레이어나 타워와의 교전 여부 체크
- StatManager, StatusEffectManager와 연동되어 스탯 변화/디버프 반영

---
## 🧬 스탯 시스템
### 구성 요소
- StatManager: 체력, 마나, 공격력 등 기본 스탯을 관리하며 실시간 업데이트 가능
- StatBase: 스탯의 공통 인터페이스 역할, CalculatedStat, ResourceStat 등 세분화
- StatusEffectManager: 디버프/버프 관리
- StatusEffect: 지속시간, 타겟 스탯, 효과 타입 등을 기반으로 스탯 변화 적용
- BuffFactory: 상태이상 오브젝트를 동적으로 생성 및 관리
### 특징
- 실시간 스탯 변화 (ex. 지속 데미지, 회복, 공격력 증가 등)에 최적화
- FSM이나 전투 시스템과 쉽게 연동 가능
- 상태이상도 스탯의 일부로 통합되어 버프/디버프의 직관적 설계 가능
---

## ✅ 결론
위 시스템들은 모두 SOLID 원칙과 모듈화를 고려하여 설계되었으며, 특히 실제 게임 플레이와 유지보수에서 다음과 같은 강점을 가집니다:
- 타워/몬스터/보스 등의 개별 FSM이 명확한 책임 분리를 가지며 재사용성/확장성이 높음
- 오브젝트 풀링/테이블 기반 구조로 런타임 성능 및 유연성 확보
- 스탯/상태이상/버프 시스템과의 연동을 통해 다양한 전투/강화 시나리오 대응 가능

[⬅ 메인 리드미로 돌아가기](../README.md)
