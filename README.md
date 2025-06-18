# Arise - 3D 타워 디펜스

본 프로젝트는 FSM, 오브젝트 풀링, 스킬/스탯, 타워 설치, 보스 스킬, 퀘스트, 세이브/로드 등 다양한 게임 시스템을 모듈화하고 확장성 있는 구조로 개발하였습니다.

## 🧑 플레이어
<details>
<summary>플레이어</summary>
<div markdown="1">

- WASD로 캐릭터를 이동할 수 있습니다.
- Shift를 누른 상태로 이동시 달릴 수 있습니다.
- 적이 근접하면 자동으로 공격을 합니다.
- Z,X,C로 스킬을 사용할 수 있습니다.
- 스테이지가 클리어되면 패시브 스킬을 선택해서 캐릭터를 강화할 수 있습니다.
![Movie_018](https://github.com/user-attachments/assets/3d2085b9-90b7-474c-9f99-39869aa28f9b)
![Movie_019](https://github.com/user-attachments/assets/385a86f7-3535-4a61-a449-bd691b82d9bc)
![Movie_020](https://github.com/user-attachments/assets/2276a4b5-9eeb-4135-b7de-b3ec43297b9b)

</div>
</details>

---

## 🎥 인트로씬
<details>
## <summary>인트로씬</summary>
<div markdown="1">

- 인트로씬입니다.
- 씨네머신을 사용하여 역동적인 카메라 이동을 구현했습니다.
![Movie_025](https://github.com/user-attachments/assets/1a5c904d-0a9b-4b33-ba67-088b9c685359)

</div>
</details>

---


## 🧬 저장/ 불러오기기
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
 ## 🏰 타워 시스템
<details>
<summary>타워 설치</summary>
<div markdown="1">

- 오른쪽 화살표를 눌러 타워 설치 모드로 진입하여 타워를 설치할 수 있습니다.
- 설치 가능한 구역이면 초록색, 불가능한 구역이면 빨간색으로 표시됩니다.
- 설치된 타워를 클릭하여 업그레이드, 제거가 가능합니다.
![Movie_006](https://github.com/user-attachments/assets/ccafee1c-af4b-49cd-a38b-5cf4f96f08f1)

</div>
</details>

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

---
✅ 결론

이 프로젝트는 다양한 핵심 시스템들을 제네릭, 인터페이스, 싱글톤, Table 기반 구조로 설계함으로써 재사용성과 유지보수성을 높였습니다. 특히, FSM과 스탯 시스템은 실무에서도 그대로 활용 가능한 구조로 구현되어 포트폴리오에서 강력한 어필이 가능합니다.
 사용 기술 스택
- Unity 2022.3.17f1 (LTS)
- URP (Universal Render Pipeline)
- Git 기반 형상 관리


## 👥 팀원별 모듈 문서

| 이름 | 담당 기능 | 문서 링크 |
|------|-----------|-----------|
| 김경민 | 타워 설치, FSM           | [김경민_README.md](./Members/README_rudals4469.md) |
| 박상민 | 스킬 / 스탯 / 버프 시스템 | [박상민_README.md](./Members/LeeREADME.md) |
| 심교인 | 저장 시스템 & 퀘스트 시스템    | [심교인_README.md](./Members/README_Simkyoin.md) |
| 심민석 | 타워 설치, FSM, ObjectPooling,Stat     | [심민석_README.md](./Members/README_Shimminseok.md) |
| 전인우 | 보스 스킬, 풀링 시스템     | [전인우_README.md](./Members/README_InwooJeon.md) |
