using UnityEngine;

namespace TPC
{
    public class PlayerStateCrouch : PlayerFSMState
    {
        public PlayerStateCrouch(Player player) : base(player)
        {
            _stateType = PlayerFSMStateType.CROUCH;
        }

        #region State

        public override void Enter()
        {
            _player.playerAnimator.SetBool("Crouch", true);
        }
        public override void Exit()
        {
            _player.playerAnimator.SetBool("Crouch", false);
        }
        public override void Update()
        {
            if (Input.GetButton("Crouch")) return;

            _player.playerFSM.SetCurrentState(PlayerFSMStateType.MOVEMENT);
        }

        public override void FixedUpdate() { }

        #endregion
    }

}