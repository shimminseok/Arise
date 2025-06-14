using Cinemachine;
using UnityEngine;

namespace TowerStates
{
    public class IdleState : IState<TowerController, TowerState>
    {
        public void OnEnter(TowerController owner)
        {
        }

        public void OnUpdate(TowerController owner)
        {
            if (owner.IsPlaced)
                owner.FindTarget();
        }

        public void OnFixedUpdate(TowerController owner)
        {
        }

        public void OnExit(TowerController entity)
        {
        }

        public TowerState CheckTransition(TowerController owner)
        {
            var canAttack =
                owner.IsPlaced &&
                owner.Target != null &&
                !owner.Target.IsDead &&
                owner.IsTargetInAttackRange();

            return canAttack ? TowerState.Attack : TowerState.Idle;
        }
    }

    public class AttackState : IState<TowerController, TowerState>
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

        public void OnEnter(TowerController owner)
        {
        }

        public void OnUpdate(TowerController owner)
        {
            if (owner.FireTransformRoot != null)
            {
                var targetPos = owner.Target.Collider.transform.position;
                targetPos.y = owner.FireTransformRoot.position.y;
                owner.FireTransformRoot.LookAt(targetPos);
            }

            attackTimer += Time.deltaTime;
            if (attackTimer >= attackSpd)
            {
                owner.Attack();
                attackTimer = 0;
            }
        }

        public void OnFixedUpdate(TowerController owner)
        {
        }

        public void OnExit(TowerController entity)
        {
        }

        public TowerState CheckTransition(TowerController owner)
        {
            if (owner.Target == null || owner.Target.IsDead || !owner.IsTargetInAttackRange())
                return TowerState.Idle;

            return TowerState.Attack;
        }
    }
}