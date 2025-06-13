using System.Reflection.Emit;
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
        private readonly int isMoving = Animator.StringToHash("IsMove");
        private readonly int attack = Animator.StringToHash("Attack");
        public void OnEnter(EnemyController owner)
        {
            owner.Animator.SetBool(isMoving, false);
            owner.Animator.ResetTrigger(attack);
        }

        public void OnUpdate(EnemyController owner)
        {
            Debug.Log("Update Idle State");
        }

        public void OnFixedUpdate(EnemyController owner)
        {
        }

        public void OnExit(EnemyController entity)
        {
        }

        public EnemyState CheckTransition(EnemyController owner)
        {
            if (owner.IsDead)
                return EnemyState.Die;

            if (owner.Target != null && !owner.Target.IsDead)
                return EnemyState.Move;
            

            return EnemyState.Idle;
        }
    }

    public class MoveState : IState<EnemyController, EnemyState>
    {
        private readonly int isMoving = Animator.StringToHash("IsMove");
        private readonly int attack = Animator.StringToHash("Attack");
        public void OnEnter(EnemyController owner)
        {
            owner.Animator.SetBool(isMoving, true);
            owner.Animator.ResetTrigger(attack);
            owner.Agent.isStopped = false;
        }

        public void OnUpdate(EnemyController owner)
        {
            owner.Movement();
        }

        public void OnFixedUpdate(EnemyController owner)
        {
        }

        public void OnExit(EnemyController owner)
        {
            owner.Animator.SetBool(isMoving, false);
        }

        public EnemyState CheckTransition(EnemyController owner)
        {
            if (owner.IsDead)
                return EnemyState.Die;
            if (owner.Target != null && !owner.Target.IsDead)
                return owner.IsTargetInAttackRange() ? EnemyState.Attack : EnemyState.Move;
            
            return EnemyState.Idle;
        }
    }

    public class AttackState : IState<EnemyController, EnemyState>
    {
        private float attackTimer = 0;
        private readonly float attackSpd;
        private readonly float attackRange;
        private readonly int isMoving = Animator.StringToHash("IsMove");
        private readonly int attack = Animator.StringToHash("Attack");
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
            var order = EnemyManager.Instance.GetArrivalOrder();
            owner.Agent.avoidancePriority = Mathf.Clamp(order, 0, 99);
            owner.Animator.SetBool(isMoving, false);
        }

        public void OnUpdate(EnemyController owner)
        {
            Debug.Log("Update Attack State");
            attackTimer += Time.deltaTime;
            if (attackTimer >= attackSpd)
            {
                owner.Animator.SetTrigger(attack);
                owner.Attack();
                attackTimer = 0;
            }
        }

        public void OnFixedUpdate(EnemyController owner)
        {
        }

        public void OnExit(EnemyController owner)
        {
            owner.Animator.SetBool(isMoving, false);
            owner.Animator.ResetTrigger(attack);

        }

        public EnemyState CheckTransition(EnemyController owner)
        {
            if (owner.Target == null)
                return EnemyState.Idle;
            else if (owner.IsDead)
                return EnemyState.Die;

            return owner.IsTargetInAttackRange() ? EnemyState.Attack : EnemyState.Move;
            
            
        }
    }

    public class DeadState : IState<EnemyController, EnemyState>
    {
        private readonly int die = Animator.StringToHash("Die");

        public void OnEnter(EnemyController owner)
        {
            owner.Animator.SetTrigger(die);
        }

        public void OnUpdate(EnemyController owner)
        {
            Debug.Log("Update Dead State");

        }

        public void OnFixedUpdate(EnemyController owner)
        {
        }

        public void OnExit(EnemyController owner)
        {
            owner.Animator.ResetTrigger(die);
        }

        public EnemyState CheckTransition(EnemyController owner)
        {
            if (!owner.IsDead)
            {
                return EnemyState.Idle;
            }
            return EnemyState.Die;
        }
    }
}