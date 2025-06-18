using UnityEngine;
using UnityEngine.AI;

namespace BossStates
{
    public enum BossState
    {
        Idle,
        Move,
        Attack,
        Die,
        Skill
    }

    public class IdleState : IState<BossController, BossState>
    {
        public void OnEnter(BossController owner)
        {
                                  Debug.LogWarning("Boss IdleState.OnEnter@@@@@@@@@");
        }

        public void OnUpdate(BossController owner)
        {
                                 Debug.LogWarning("Boss IdleState.OnUpdate@@@@@@@@@");
        }

        public void OnFixedUpdate(BossController owner)
        {
        }

        public void OnExit(BossController entity)
        {
        }

        public BossState CheckTransition(BossController owner)
        {
            if (owner.istest)
            {
                 owner.istest = false;
                return BossState.Skill; 
            }
            if (owner.IsDead)
                    return BossState.Die;
                else if (owner.Target != null && !owner.Target.IsDead)
                    return BossState.Move;
            
            return BossState.Idle;
        }
    }

    public class MoveState : IState<BossController, BossState>
    {
        public void OnEnter(BossController owner)
        {
              Debug.LogWarning("Boss MoveState.OnEnter@@@@@@@@@");
            owner.Agent.isStopped = false;
        }

        public void OnUpdate(BossController owner)
        {
                                             Debug.LogWarning("Boss MoveState.OnUpdate@@@@@@@@@");
            owner.Movement();
        }

        public void OnFixedUpdate(BossController owner)
        {
        }

        public void OnExit(BossController entity)
        {
        }

        public BossState CheckTransition(BossController owner)
        {
                            Debug.LogWarning($"{owner.IsDead},{owner.Target},{owner.Target.IsDead},{owner.IsTargetInAttackRange()}");
            if (owner.istest)
            {
                return BossState.Idle;
            }
            if (owner.IsDead)
                    return BossState.Die;
            if (owner.Target != null && !owner.Target.IsDead)
            {
                return owner.IsTargetInAttackRange() ? BossState.Attack : BossState.Move;
            }
            
            return BossState.Idle;
        }
    }

    public class AttackState : IState<BossController, BossState>
    {
        private float attackTimer = 0;
        private readonly float attackSpd;
        private readonly float attackRange;

        public AttackState(float attackSpd, float attackRange)
        {
            attackTimer = attackSpd;
            this.attackSpd = attackSpd;
            this.attackRange = attackRange;
        }

        public void OnEnter(BossController owner)
        {
                                                          Debug.LogWarning("Boss ATTackState.OnEnter@@@@@@@@@");
            owner.Agent.ResetPath();
            owner.Agent.isStopped = true;
            owner.Agent.velocity = Vector3.zero;
            int order = BossManager.Instance.GetArrivalOrder();
            owner.Agent.avoidancePriority = Mathf.Clamp(order, 0, 99);
        }

        public void OnUpdate(BossController owner)
        {
                                             Debug.LogWarning("Boss AttackState.OnUpdate@@@@@@@@@");
            attackTimer += Time.deltaTime;
            if (attackTimer >= attackSpd)
            {
                owner.Attack();
                attackTimer = 0;
            }
        }

        public void OnFixedUpdate(BossController owner)
        {
        }

        public void OnExit(BossController entity)
        {
        }

        public BossState CheckTransition(BossController owner)
        {
            if (owner.IsDead)
                return BossState.Die;
            if (owner.Target == null || owner.Target.IsDead)
            {
                return BossState.Idle;
            }

            return owner.IsTargetInAttackRange() ? BossState.Attack : BossState.Move;
        }
    }

    public class DeadState : IState<BossController, BossState>
    {
        public void OnEnter(BossController owner)
        {
                                                          Debug.LogWarning("Boss DeadState.OnEnter@@@@@@@@@");
        }

        public void OnUpdate(BossController owner)
        {
                                             Debug.LogWarning("Boss DeadState.OnUpdate@@@@@@@@@");
        }

        public void OnFixedUpdate(BossController owner)
        {
        }

        public void OnExit(BossController owner)
        {
        }

        public BossState CheckTransition(BossController owner)
        {
            return BossState.Die;
        }
    }

    public class SkillState : IState<BossController, BossState>
    {
        string[] triggers = { "IsDispel","IsEarthquake" };


        public void OnEnter(BossController owner)
        {
                        Debug.LogWarning("Boss SkillState.OnEnter@@@@@@@@@@");
            string randomTrigger = triggers[Random.Range(0, triggers.Length)];
            owner.animator.SetTrigger(randomTrigger);
        }

        public void OnUpdate(BossController owner)
        {
                                 Debug.LogWarning("Boss SkillState.OnUpdate@@@@@@@@@");
        }

        public void OnFixedUpdate(BossController owner)
        {

        }

        public void OnExit(BossController owner)
        {

        }

        public BossState CheckTransition(BossController owner)
        {

            return BossState.Idle;

        }

    }

}