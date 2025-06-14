using System.Collections;
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
        private bool _attackDone;

        public AttackState(float attackSpd, float attackRange)
        {
            this.attackSpd = attackSpd;
        }

        public void OnEnter(TowerController owner)
        {
            _attackDone = false;
            owner.StartCoroutine(DoAttack(owner));
        }

        public void OnUpdate(TowerController owner)
        {
            if (owner.FireTransformRoot != null)
            {
                var targetPos = owner.Target.Collider.transform.position;
                targetPos.y = owner.FireTransformRoot.position.y;
                owner.FireTransformRoot.LookAt(targetPos);
            }
        }

        private IEnumerator DoAttack(TowerController owner)
        {
            yield return new WaitForSeconds(1f / attackSpd);
            owner.Attack();
            _attackDone = true;
        }

        public void OnFixedUpdate(TowerController owner)
        {
        }

        public void OnExit(TowerController entity)
        {
            _attackDone = false;
        }

        public TowerState CheckTransition(TowerController owner)
        {
            if (owner.Target == null || owner.Target.IsDead || !owner.IsTargetInAttackRange())
                return TowerState.Idle;

            return TowerState.Attack;
        }
    }
}