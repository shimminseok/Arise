using System.Collections;
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
            owner.Agent.velocity = Vector3.zero;
            owner.Animator.SetBool(isMoving, false);
            owner.Animator.ResetTrigger(attack);

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
        private float _attackTimer = 0;
        private readonly float _attackSpd;
        private bool _attackDone;

        private readonly int attackType = Animator.StringToHash("AttackType");
        private readonly int isMoving = Animator.StringToHash("IsMove");
        private readonly int attack = Animator.StringToHash("Attack");

        private Coroutine attackCoroutine;
        
        public AttackState(float attackSpd, float attackRange)
        {
            _attackTimer = attackSpd;
            this._attackSpd = attackSpd;
        }

        public void OnEnter(EnemyController owner)
        {
            owner.StopMovement();
            _attackDone = false;
            var order = EnemyManager.Instance.GetArrivalOrder();
            owner.Agent.avoidancePriority = Mathf.Clamp(order, 0, 99);
            owner.Animator.SetBool(isMoving, false);
            attackCoroutine = owner.StartCoroutine(DoAttack(owner));
        }

        public void OnUpdate(EnemyController owner)
        {
            // _attackTimer += Time.deltaTime;
            // if (_attackTimer >= _attackSpd)
            // {
            //     int randomAttack = Random.Range(0, 3);
            //     owner.Animator.SetTrigger(attack);
            //     owner.Animator.SetInteger(attackType, randomAttack);
            //     owner.Attack();
            //     _attackTimer = 0;
            // }
        }

        private IEnumerator DoAttack(EnemyController owner)
        {
            while (true)
            {
                yield return new WaitForSeconds(1f / _attackSpd);
                int randomAttack = Random.Range(0, 3);
                owner.Animator.SetTrigger(attack);
                owner.Animator.SetInteger(attackType, randomAttack);
                owner.Attack();
            }

        }

        public void OnFixedUpdate(EnemyController owner)
        {
        }

        public void OnExit(EnemyController owner)
        {
            owner.Animator.SetBool(isMoving, false);
            owner.Animator.ResetTrigger(attack);
            _attackDone = false;
            if (attackCoroutine != null)
                owner.StopCoroutine(attackCoroutine);
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
            owner.StopMovement();
        }

        public void OnUpdate(EnemyController owner)
        {
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