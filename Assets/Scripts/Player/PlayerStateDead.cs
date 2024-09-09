namespace TPC
{
    public class PlayerStateDead : PlayerFSMState
    {
        public PlayerStateDead(Player player) : base(player)
        {
            _stateType = PlayerFSMStateType.DEAD;
        }

        #region State

        public override void Enter()
        {
            _player.playerAnimator.SetTrigger("Die");
        }

        public override void Exit() { }
        public override void Update() { }
        public override void FixedUpdate() { }

        #endregion State
    }

}