using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//해당 스킬과 부딪히면 OnTriggerEnter로 감지
//현재까지는 안쓰는 코드상태
//BossController.FindTarget()에서 적 오브젝트를 찾아 실행중이라서 안쓰게 됨
public class BossSkillTrigger : MonoBehaviour
{
    BossSkillController bossSkillController;

    private IDamageable _target;

    public void SetTarget(BossSkillController owner)
    {
        bossSkillController = owner;
        _target = bossSkillController.Target;
    }

    private void OnTriggerEnter(Collider other)
    {

    }
}