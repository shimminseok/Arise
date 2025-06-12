using UnityEngine;

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
            
            return EnemyState.Attack;
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