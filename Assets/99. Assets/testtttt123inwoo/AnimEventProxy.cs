using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BossSkillName
{
    EarthQuake,
    Dispel
}

public class AnimEventProxy : MonoBehaviour
{
    public GameObject target;

    BossController bossController;
    void Awake()
    {
        bossController = target.GetComponent<BossController>();
    }
    public void CallExternal(BossSkillName bossSkillName)
    {
        bossController?.FindTargetByEnum(bossSkillName);
        bossController?.FireSkill(bossSkillName);
        bossController.istest = false;
        //        owner.FindTarget();
        // owner.FireSkill();
        // owner.istest = false;
    }
}
