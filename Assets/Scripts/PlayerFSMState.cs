using FSM;

namespace TPC
{
    /// <summary>
    /// Represents a state in the Player's finite state machine (FSM).
    /// </summary>
    public class PlayerFSMState : State
    {
        /// <summary>
        /// Property to expose the state type.
        /// </summary>
        public PlayerFSMStateType StateType { get { return _stateType; } }
        protected PlayerFSMStateType _stateType;

        protected Player _player = null;

        /// <summary>
        /// Constructor that accepts only the player reference and derives the state machine from the player.
        /// </summary>
        /// <param name="player">The player instance that owns this state.</param>
        public PlayerFSMState(Player player) : base(player.playerFSM)
        {
            _player = player;
            _stateMachine = _player.playerFSM;
        }

        #region Base Class - State

        public override void Enter()
        {
            base.Enter();
        }
        public override void Exit()
        {
            base.Exit();
        }
        public override void Update()
        {
            base.Update();
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        #endregion Base Class - State
    }
}
