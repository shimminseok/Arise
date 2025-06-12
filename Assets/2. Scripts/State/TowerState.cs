namespace TowerStates
{
    public class IdleState : IState<TowerController, TowerState>
    {
        public void OnEnter(TowerController owner)
        {
        }

        public void OnUpdate(TowerController owner)
        {
        }

        public void OnFixedUpdate(TowerController owner)
        {
        }

        public void OnExit(TowerController entity)
        {
        }

        public TowerState CheckTransition(TowerController owner)
        {
            return TowerState.Idle;
        }
    }

    public class AttackState : IState<TowerController, TowerState>
    {
        public void OnEnter(TowerController owner)
        {
        }

        public void OnUpdate(TowerController owner)
        {
        }

        public void OnFixedUpdate(TowerController owner)
        {
        }

        public void OnExit(TowerController entity)
        {
        }

        public TowerState CheckTransition(TowerController owner)
        {
            return TowerState.Attack;
        }
    }
}