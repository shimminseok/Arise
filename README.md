# Arise - 3D 타워 디펜스

Arise는 3D 타워 디펜스에 **RPG 요소(플레이어 성장, 스킬 선택, 퀘스트, 저장 시스템 등)**를 결합한 프로젝트입니다.
기존 디펜스 게임에서 보기 힘든 플레이어 조작과 전투 시스템을 도입하여, 단순한 배치 전략을 넘어 직접 성장하고 전투에 참여하는 몰입감 있는 경험을 제공합니다.
Unity 2022.3.17f1 + URP 환경에서 개발되었으며, Git을 기반으로 팀 협업을 진행했습니다.

---

## 🧑 플레이어
<details>
<summary>플레이어</summary>

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
<summary>인트로씬</summary>

- 인트로씬입니다.
- 씨네머신을 사용하여 역동적인 카메라 이동을 구현했습니다.
![Movie_025](https://github.com/user-attachments/assets/1a5c904d-0a9b-4b33-ba67-088b9c685359)

</div>
</details>

---


## 🧬💾 세이브 / 로드 시스템
<details>
<summary>저장</summary>
- 플레이도중 S키를 눌러 게임의 상태를 저장합니다.
- 플레이어 위치, 타워 배치, 스테이지 정보등이 저장됩니다.

 ![Movie_029](https://github.com/user-attachments/assets/fce5c15e-5dfa-4e77-9509-e88271f3e286)
</div>
</details>
<details>
<summary>불러오기</summary>
- 플레이 도중 L키를 눌러 이전에 저장한 게임 상태를 불러옵니다.
- 플레이어 위치, 타워 배치, 스테이지 정보등이 불러와집니다. 

 ![Movie_031](https://github.com/user-attachments/assets/a51f5c4e-3907-495a-b2a9-a1b5eedd2bcd)
</div>
</details>

---
 ## 🏰 타워 시스템
<details>
<summary>타워 설치</summary>

- 오른쪽 화살표를 눌러 타워 설치 모드로 진입하여 타워를 설치할 수 있습니다.
- 설치 가능한 구역이면 초록색, 불가능한 구역이면 빨간색으로 표시됩니다.
- 설치된 타워를 클릭하여 업그레이드, 제거가 가능합니다.
![Movie_006](https://github.com/user-attachments/assets/ccafee1c-af4b-49cd-a38b-5cf4f96f08f1)

</div>
</details>

---

## 📜 퀘스트 시스템
<details>
<summary>퀘스트</summary>
 - 퀘스트 시스템입니다.
 - 다양한 퀘스트를 통해 게임의 목표성을 부여합니다.

 ![Movie_032](https://github.com/user-attachments/assets/550534e3-8968-41ef-a568-efa05da150f6)

</div>
</details>

---
##  🗿보스 스킬 패턴
<details>
<summary>보스</summary>
 -보스의 스킬 시전 모습입니다.
 -타워를 부시거나 디버프를 걸 수 있습니다.
 
![Image](https://github.com/user-attachments/assets/c4298b75-27a3-4583-893f-d059a06f6785)

![Image](https://github.com/user-attachments/assets/bca58867-9a85-4a39-92d7-da4a94a78693)

</div>
</details>

---

## 🔩 사용 기술 스택
- Unity 2022.3.17f1 (LTS)
- URP (Universal Render Pipeline)
- Git 기반 형상 관리


## 👥 팀원별 모듈 문서

| 이름 | 담당 기능 | 문서 링크 |
|------|-----------|-----------|
| 김경민 | UI / 튜토리얼        | [김경민_README.md](./Members/README_rudals4469.md) |
| 박상민 | 스킬 / 스탯 / 버프 시스템 | [박상민_README.md](./Members/README_Sangmin1008.md) |
| 심교인 | 저장 시스템 & 퀘스트 시스템    | [심교인_README.md](./Members/README_Simkyoin.md) |
| 심민석 | 타워 설치, FSM, ObjectPooling,Stat     | [심민석_README.md](./Members/README_Shimminseok.md) |
| 전인우 | 보스 스킬, 풀링 시스템     | [전인우_README.md](./Members/README_InwooJeon.md) |
