namespace TPC
{
    public class PlayerStateDamage : PlayerFSMState
    {
        public PlayerStateDamage(Player player) : base(player)
        {
            _stateType = PlayerFSMStateType.TAKE_DAMAGE;
        }

        #region State

        public override void Enter() { }
        public override void Exit() { }
        public override void Update() { }
        public override void FixedUpdate() { }

        #endregion State
    }
}
