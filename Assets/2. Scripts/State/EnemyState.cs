using UnityEngine;
using UnityEngine.AI;

namespace EnemyStates
{
    public enum EnemyState
    {
        Idle,
        Move,
        Attack,
        Die
    }

    public class IdleState : IState<EnemyController, EnemyState>
    {
        public void OnEnter(EnemyController owner)
        {
        }

        public void OnUpdate(EnemyController owner)
        {
        }

        public void OnFixedUpdate(EnemyController owner)
        {
        }

        public void OnExit(EnemyController entity)
        {
        }

        public EnemyState CheckTransition(EnemyController owner)
        {
            if (!owner.Target.IsDead)
                return EnemyState.Move;

            return EnemyState.Idle;
        }
    }

    public class MoveState : IState<EnemyController, EnemyState>
    {
        public void OnEnter(EnemyController owner)
        {
            owner.Agent.isStopped = false;
        }

        public void OnUpdate(EnemyController owner)
        {
            owner.Movement();
        }

        public void OnFixedUpdate(EnemyController owner)
        {
        }

        public void OnExit(EnemyController entity)
        {
        }

        public EnemyState CheckTransition(EnemyController owner)
        {
            if (owner.Target != null && !owner.Target.IsDead)
            {
                return owner.IsTargetInAttackRange() && owner.Agent.remainingDistance < 0.5f ? EnemyState.Attack : EnemyState.Move;
            }

            return EnemyState.Idle;
        }
    }

    public class AttackState : IState<EnemyController, EnemyState>
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

        public void OnEnter(EnemyController owner)
        {
            owner.Agent.ResetPath();
            owner.Agent.isStopped = true;
            owner.Agent.velocity = Vector3.zero;
            int order = EnemyManager.Instance.GetArrivalOrder();
            owner.Agent.avoidancePriority = Mathf.Clamp(order, 0, 99);
        }

        public void OnUpdate(EnemyController owner)
        {
            attackTimer += Time.deltaTime;
            if (attackTimer >= attackSpd)
            {
                owner.Attack();
                attackTimer = 0;
            }
        }

        public void OnFixedUpdate(EnemyController owner)
        {
        }

        public void OnExit(EnemyController entity)
        {
        }

        public EnemyState CheckTransition(EnemyController owner)
        {
            if (owner.Target == null || owner.Target.IsDead)
            {
                return EnemyState.Idle;
            }

            return owner.IsTargetInAttackRange() ? EnemyState.Attack : EnemyState.Move;
        }
    }

    public class DeadState : IState<EnemyController, EnemyState>
    {
        public void OnEnter(EnemyController owner)
        {
        }

        public void OnUpdate(EnemyController owner)
        {
        }

        public void OnFixedUpdate(EnemyController owner)
        {
        }

        public void OnExit(EnemyController owner)
        {
        }

        public EnemyState CheckTransition(EnemyController owner)
        {
            return EnemyState.Die;
        }
    }
}