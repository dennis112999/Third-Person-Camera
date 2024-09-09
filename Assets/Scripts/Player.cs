using UnityEngine;

namespace TPC
{
    public enum PlayerFSMStateType
    {
        MOVEMENT = 0,
        CROUCH,
        ATTACK,
        RELOAD,
        TAKE_DAMAGE,
        DEAD,
    }

    public class Player : MonoBehaviour
    {
        public PlayerMovement playerMovement;
        public Animator playerAnimator;
        public PlayerFSM playerFSM = null;

        [Header("Attack")]
        public int maxAmunitionBeforeReload = 40;
        public int totalAmunitionCount = 100;
        [HideInInspector]
        public int bulletsInMagazine = 40;
        public float roundsPerSecond = 10;

        #region MonoBehaviour

        private void Start()
        {
            Initialize();
        }

        private void Update()
        {
            playerFSM.Update();
        }

        private void FixedUpdate()
        {
            playerFSM.FixedUpdate();
        }

        #endregion MonoBehaviour

        private void Initialize()
        {
            playerFSM = new PlayerFSM();

            // create the FSM.
            playerFSM.Add(new PlayerStateMovement(this));
            playerFSM.Add(new PlayerStateAttack(this));
            playerFSM.Add(new PlayerStateReload(this));
            playerFSM.Add(new PlayerStateDamage(this));
            playerFSM.Add(new PlayerStateDead(this));
            playerFSM.Add(new PlayerStateCrouch(this));

            playerFSM.SetCurrentState(PlayerFSMStateType.MOVEMENT);
        }
    }

}