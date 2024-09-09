using FSM;
using TRC;

namespace TPC
{
    public class PlayerFSM : StateMachine
    {
        public PlayerFSM() : base()
        {
        }

        public void Add(PlayerFSMState state)
        {
            _states.Add((int)state.StateType, state);
        }

        public PlayerFSMState GetState(PlayerFSMStateType key)
        {
            return (PlayerFSMState)GetState((int)key);
        }

        public void SetCurrentState(PlayerFSMStateType stateKey)
        {
            State state = _states[(int)stateKey];
            if (state != null)
            {
                SetCurrentState(state);
            }
        }
    }
}
