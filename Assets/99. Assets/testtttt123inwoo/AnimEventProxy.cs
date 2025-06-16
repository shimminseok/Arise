using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEventProxy : MonoBehaviour
{
    public GameObject target;

    BossController bossController;
    void Awake()
    {
               bossController = target.GetComponent<BossController>(); 
    }
    public void CallExternal(int num)
    {
        bossController?.FindTarget();
        bossController?.FireSkill(num);
        bossController.istest = false;
        //        owner.FindTarget();
        // owner.FireSkill();
        // owner.istest = false;
    }
}
