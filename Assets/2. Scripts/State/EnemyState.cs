using UnityEngine;

namespace EnemyStates
{
    public class IdleState : IState<EnemyController, EnemyState>
    {
        public void OnEnter(EnemyController owner)
        {
        }

        public void OnUpdate(EnemyController owner)
        {
            owner.FindTarget();
        }

        public void OnFixedUpdate(EnemyController owner)
        {
        }

        public void OnExit(EnemyController entity)
        {
        }

        public EnemyState CheckTransition(EnemyController owner)
        {
            return EnemyState.Idle;
        }
    }

    public class MoveState : IState<EnemyController, EnemyState>
    {
        public void OnEnter(EnemyController owner)
        {
            
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
            return EnemyState.Move;
        }
    }

    public class AttackState : IState<EnemyController, EnemyState>
    {
        private float attackTimer;
        private readonly float attackDelay;
        private readonly float attackRange;

        public AttackState(float atkSpd, float atkRange)
        {
            attackDelay = atkSpd;
            attackRange = atkRange;
        }

        public void OnEnter(EnemyController owner)
        {
        }

        public void OnUpdate(EnemyController owner)
        {
            attackTimer += Time.deltaTime;
            if (attackTimer >= attackDelay)
            {
                attackTimer = 0f;
                owner.Attack();
            }
        }

        public void OnFixedUpdate(EnemyController owner)
        {
        }

        public void OnExit(EnemyController entity)
        {
            attackTimer = 0f;
        }

        public EnemyState CheckTransition(EnemyController owner)
        {
            
            return EnemyState.Idle;
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
            return EnemyState.Idle;
        }
    }
}