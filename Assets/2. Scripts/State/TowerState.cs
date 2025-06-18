using System;
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
        private readonly int Attack = Animator.StringToHash("Attack");
        private float attackTimer = 0;
        private float attackSpd;
        private bool _attackDone;
        private Coroutine attackCoroutine;
        public AttackState(float attackSpd, float attackRange)
        {
            this.attackSpd = attackSpd;
        }

        public void OnEnter(TowerController owner)
        {
            attackCoroutine = owner.StartCoroutine(DoAttack(owner));
        }

        public void OnUpdate(TowerController owner)
        {
            
            if (owner.FireWeaponTransform != null)
            {
                var targetPos = owner.Target.Collider.transform.position;
                targetPos.y = owner.FireWeaponTransform.position.y;
                owner.FireWeaponTransform.LookAt(targetPos);
            }
        }

        private IEnumerator DoAttack(TowerController owner)
        {
            while (owner.Target != null && !owner.Target.IsDead)
            {
                attackSpd = owner.StatManager.GetValue(StatType.AttackSpd);
                yield return new WaitForSeconds(1f / attackSpd);
                owner.Attack();
            }
        }

        public void OnFixedUpdate(TowerController owner)
        {
        }

        public void OnExit(TowerController entity)
        {
            if (attackCoroutine != null)
                entity.StopCoroutine(attackCoroutine);
        }

        public TowerState CheckTransition(TowerController owner)
        {
            if (owner.Target == null || owner.Target.IsDead || !owner.IsTargetInAttackRange())
                return TowerState.Idle;

            return TowerState.Attack;
        }
    }

    public class BuildState : IState<TowerController, TowerState>
    {
        public event Action<Vector3Int> OnTryPlace;


        public void OnEnter(TowerController owner)
        {
            OnTryPlace += BuildingPlacer.Instance.CompleteBuildingTower;
        }

        public void OnUpdate(TowerController owner)
        {
            var (canBuild, cell) = BuildingPlacer.Instance.GetGhostBuildInfo();
            if (Input.GetMouseButtonDown(0) && canBuild)
            {
                OnTryPlace?.Invoke(cell);
            }
        }

        public void OnFixedUpdate(TowerController owner)
        {
        }

        public void OnExit(TowerController entity)
        {
            OnTryPlace -= BuildingPlacer.Instance.CompleteBuildingTower;
        }

        public TowerState CheckTransition(TowerController owner)
        {
            return owner.IsPlaced ? TowerState.Idle : TowerState.Build;
        }
    }
}