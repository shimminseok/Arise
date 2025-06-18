using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerStates
{
    public class IdleState : IState<PlayerController, PlayerState>
    {
        public void OnEnter(PlayerController owner)
        {
        }

        public void OnUpdate(PlayerController owner)
        {
        }

        public void OnFixedUpdate(PlayerController owner)
        {
        }

        public void OnExit(PlayerController owner)
        {
        }

        public PlayerState CheckTransition(PlayerController owner)
        {
            // if (owner.AttackTriggered)

            if (owner.MoveInput.sqrMagnitude > 0.01f)
            {
                return owner.IsRunning ? PlayerState.Run : PlayerState.Move;
            }

            if (owner.IsTargetExists) return PlayerState.Attack;
            return PlayerState.Idle;
        }
    }

    public class MoveState : IState<PlayerController, PlayerState>
    {
        public void OnEnter(PlayerController owner)
        {
            owner.PlayerAnimation.Animator.SetBool(owner.PlayerAnimation.AnimationData.WalkParameterHash, true);
        }

        public void OnUpdate(PlayerController owner)
        {
            owner.Movement();
        }

        public void OnFixedUpdate(PlayerController owner)
        {
        }

        public void OnExit(PlayerController owner)
        {
            owner.PlayerAnimation.Animator.SetBool(owner.PlayerAnimation.AnimationData.WalkParameterHash, false);
        }

        public PlayerState CheckTransition(PlayerController owner)
        {
            if (owner.AttackTriggered)
                return PlayerState.Attack;

            if (owner.MoveInput.sqrMagnitude < 0.01f)
                return PlayerState.Idle;

            if (!owner.IsRunning)
                return PlayerState.Move;

            return PlayerState.Run;
        }
    }

    public class AttackState : IState<PlayerController, PlayerState>
    {
        private readonly float _atkSpd;
        private readonly float _atkRange;
        private bool _attackDone;

        public AttackState(float atkSpd, float atkRange)
        {
            this._atkSpd = atkSpd;
            this._atkRange = atkRange;
        }

        public void OnEnter(PlayerController owner)
        {
            _attackDone = false;
            owner.PlayerAnimation.Animator.SetBool(owner.PlayerAnimation.AnimationData.AttackParameterHash, true);
            owner.StartCoroutine(DoAttack(owner));
        }

        private IEnumerator DoAttack(PlayerController owner)
        {
            yield return new WaitForSeconds(1f / _atkSpd);
            owner.AttackAllTargets();
            _attackDone = true;
        }

        public void OnUpdate(PlayerController owner)
        {
        }

        public void OnFixedUpdate(PlayerController owner)
        {
        }

        public void OnExit(PlayerController owner)
        {
            owner.AttackTriggered = false;
            owner.PlayerAnimation.Animator.SetBool(owner.PlayerAnimation.AnimationData.AttackParameterHash, false);
        }

        public PlayerState CheckTransition(PlayerController owner)
        {
            if (!_attackDone)
                return PlayerState.Attack;

            return PlayerState.Idle;
        }
    }

    public class RunState : IState<PlayerController, PlayerState>
    {
        public void OnEnter(PlayerController owner)
        {
            owner.PlayerAnimation.Animator.SetBool(owner.PlayerAnimation.AnimationData.WalkParameterHash, true);
            owner.PlayerAnimation.Animator.SetBool(owner.PlayerAnimation.AnimationData.RunParameterHash, true);

            QuestManager.Instance.UpdateProgress(QuestType.DashCount, 1);
        }

        public void OnUpdate(PlayerController owner)
        {
            owner.Movement();
        }

        public void OnFixedUpdate(PlayerController owner)
        {
        }

        public void OnExit(PlayerController owner)
        {
            owner.PlayerAnimation.Animator.SetBool(owner.PlayerAnimation.AnimationData.RunParameterHash, false);
        }

        public PlayerState CheckTransition(PlayerController owner)
        {
            if (owner.AttackTriggered)
                return PlayerState.Attack;

            if (owner.MoveInput.sqrMagnitude < 0.01f)
                return PlayerState.Idle;

            if (!owner.IsRunning)
                return PlayerState.Move;

            return PlayerState.Run;
        }
    }
}