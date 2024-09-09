using UnityEngine;

namespace TPC
{
    public class PlayerStateMovement : PlayerFSMState
    {
        public PlayerStateMovement(Player player) : base(player)
        {
            _stateType = PlayerFSMStateType.MOVEMENT;
        }

        #region State

        public override void Enter()
        {
            base.Enter();
        }

        public override void Update()
        {
            base.Update();

            _player.playerMovement.Move();

            if (Input.GetButton("Fire1")) // the left CTRL or the left mouse button.
            {
                PlayerStateAttack attackState = (PlayerStateAttack)_player.playerFSM.GetState(PlayerFSMStateType.ATTACK);
                attackState.AttackId = 0;
                _player.playerFSM.SetCurrentState(PlayerFSMStateType.ATTACK);
            }
            else if (Input.GetButton("Fire2")) // the left ALT key
            {
                PlayerStateAttack attackState = (PlayerStateAttack)_player.playerFSM.GetState(PlayerFSMStateType.ATTACK);
                attackState.AttackId = 1;
                _player.playerFSM.SetCurrentState(PlayerFSMStateType.ATTACK);
            }
            else if (Input.GetButton("Fire3")) // the left SHIFT key
            {
                PlayerStateAttack attackState = (PlayerStateAttack)_player.playerFSM.GetState(PlayerFSMStateType.ATTACK);
                attackState.AttackId = 2;
                _player.playerFSM.SetCurrentState(PlayerFSMStateType.ATTACK);
            }
            else if (Input.GetButton("Crouch")) // The TAB key.
            {
                _player.playerFSM.SetCurrentState(PlayerFSMStateType.CROUCH);
            }

        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        #endregion
    }
}
