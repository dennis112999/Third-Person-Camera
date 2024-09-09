using UnityEngine;

namespace TPC
{
    public class PlayerStateReload : PlayerFSMState
    {
        public float ReloadTime = 3.0f;
        private float _reloadDt = 0.0f;

        public int PreviousState;

        public PlayerStateReload(Player player) : base(player)
        {
            _stateType = PlayerFSMStateType.RELOAD;
        }

        #region State

        public override void Enter()
        {
            _player.playerAnimator.SetTrigger("Reload");
            _reloadDt = 0.0f;
        }

        public override void Exit()
        {
            // Calculate how many bullets can be added to the magazine
            int bulletsNeededToFillMagazine = _player.maxAmunitionBeforeReload - _player.bulletsInMagazine;

            if (_player.totalAmunitionCount >= bulletsNeededToFillMagazine)
            {
                _player.bulletsInMagazine += bulletsNeededToFillMagazine;
                _player.totalAmunitionCount -= bulletsNeededToFillMagazine;
            }
            else if (_player.totalAmunitionCount > 0)
            {
                _player.bulletsInMagazine += _player.totalAmunitionCount;
                _player.totalAmunitionCount = 0;
            }
        }

        public override void Update()
        {
            _reloadDt += Time.deltaTime;
            if (_reloadDt >= ReloadTime)
            {
                _player.playerFSM.SetCurrentState(PlayerFSMStateType.MOVEMENT);
            }
        }

        public override void FixedUpdate() { }

        #endregion State
    }
}
