# Arise - 3D 타워 디펜스

FSM 기반 플레이어 이동 및 공격, 무기 데이터, 액티브 스킬과 패시브 스킬, UI 애니메이션을 구현했습니다.

## 🧠 FSM (Finite State Machine) 기반 플레이어 이동 및 공격 시스템

- 제네릭 기반 상태 머신 (StateMachine<T, TState>)으로 플리에어 FSM 구현
- IState<TOwner, TState> 인터페이스를 통해 상태별 책임 분리 : PlayerStates
  - 플레이어: Idle, Move, Run, Attack
- PlayerController에서 상태에 대한 로직을 구현

---

## ♻️ 무기 시스템
- WeaponController: 플레이어(또는 캐릭터)에 무기를 장착·교체하고, 해당 무기의 스탯과 이펙트를 관리
- WeaponSO: 무기별 데이터를 담는 ScriptableObject(이름: WeaponData)로, ID·Prefab·스탯 리스트·스킬 ID 리스트를 제공
- 무기 장착 & 교체
  - EquipWeapon(int newWeaponID) 호출 시 TableManager에서 해당 ID의 WeaponSO를 조회
  - 기존 장착 무기 인스턴스 파괴 후 새 Prefab 인스턴스 생성
  - StatManager 초기화

---

## 🧬 Skill 시스템
🔹 액티브 스킬 (SkillManager.cs)
- 쿨타임 관리, 스킬 인스턴스 생성, 트랜스폼 기반 실행 구조
- SkillTable 기반으로 스킬 데이터 조회 및 실행
- Area Skill
  - 프리팹 Instantiate
  - IAreaShape에 따라 Collider 생성
  - SkillAreaTrigger 부착
- Projectile Skill
  - ProjectileMover로 이동 제어

🔹 패시브 스킬 (PassiveSkillManager.cs)
- 랜덤 3종 선택 → 선택한 스킬은 StatusEffectManager를 통해 적용
- 골드 획득 / 이동속도 증가 / 공격 관련 스탯 강화 등 다양한 효과 지원

---

## 🏰 UI 애니메이션 시스템
- PopupUIClickScaleTweenHandler
  - 팝업 등 UI 활성화 시 작은 바운스 효과로 등장
  - startScale에서 endScale(기본 1.1배)까지 튕긴 뒤, 최종적으로 1배 크기로 고정

- TouchToScreenAnimator
  - 계속해서 반복되는 호흡감(펄스) 애니메이션
  - minScale ↔ maxScale 사이를 scaleSpeed 속도로 무한 반복

- UIClickScaleTweenHandler
  - 클릭(터치) 시작 시 지정한 targetScale으로 부드럽게 확대
  - 클릭 해제 시 원래 크기로 빠르게 복귀
  - ProgressTweener를 사용해 easeOutCurve 커브로 커스터마이징 가능
