using UnityEngine;

namespace TPC
{
    public class PlayerStateAttack : PlayerFSMState
    {
        private float _elapsedTime;  // Fixed the spelling error

        // Factory pattern placeholder for future use
        public GameObject AttackGameObject { get; set; } = null;

        // Attack ID and corresponding attack name
        private int _attackID = 0;
        private string _attackName;

        /// <summary>
        /// Property for Attack ID that also updates the attack name
        /// </summary>
        public int AttackId
        {
            get => _attackID;
            set
            {
                _attackID = value;
                _attackName = "Attack" + (_attackID + 1).ToString();
            }
        }

        // Constructor to initialize the player state as ATTACK
        public PlayerStateAttack(Player player) : base(player)
        {
            _stateType = PlayerFSMStateType.ATTACK;
        }

        // Called when entering the attack state
        public override void Enter()
        {
            _player.playerAnimator.SetBool(_attackName, true);
            _elapsedTime = 0.0f;
        }

        // Called when exiting the attack state
        public override void Exit()
        {
            _player.playerAnimator.SetBool(_attackName, false);
        }

        // Update method to handle player input and attack logic
        public override void Update()
        {
            // If the player needs to reload, transition to the RELOAD state
            if (_player.bulletsInMagazine == 0 && _player.totalAmunitionCount > 0)
            {
                _player.playerFSM.SetCurrentState(PlayerFSMStateType.RELOAD);
                return;
            }

            // If no ammo is left, transition to the MOVEMENT state
            if (_player.totalAmunitionCount == 0)
            {
                _player.playerFSM.SetCurrentState(PlayerFSMStateType.MOVEMENT);
                return;
            }

            // Handle the attack logic when the "Fire1" button is pressed
            if (Input.GetButton("Fire1"))
            {
                // Set attack animation
                _player.playerAnimator.SetBool(_attackName, true);

                // Fire the weapon if enough time has passed since the last shot
                if (_elapsedTime == 0.0f)
                {
                    Fire();
                }

                // Update the elapsed time for the next shot
                _elapsedTime += Time.deltaTime;

                // Reset elapsed time after firing at the correct rounds per second rate
                if (_elapsedTime >= 1.0f / _player.roundsPerSecond)
                {
                    _elapsedTime = 0.0f;
                }
            }
            else
            {
                // If not firing, reset the attack animation and transition to movement
                _elapsedTime = 0.0f;
                _player.playerAnimator.SetBool(_attackName, false);
                _player.playerFSM.SetCurrentState(PlayerFSMStateType.MOVEMENT);
            }
        }

        // Method to handle firing logic
        private void Fire()
        {
            // Deduct one bullet from the magazine when firing
            _player.bulletsInMagazine -= 1;

            // TODO: Implement further firing effects like spawning projectiles or sounds
        }
    }
}