using System.Collections.Generic;

namespace FSM
{
    public class StateMachine
    {
        protected Dictionary<int, State> _states;
        protected State _currentState;

        public StateMachine()
        {
            _states = new Dictionary<int, State>();
        }

        public void Add(int key, State state)
        {
            _states.Add(key, state);
        }

        public State GetState(int key)
        {
            return _states[key];
        }

        public void SetCurrentState(State state)
        {
            if (_currentState != null)
            {
                _currentState.Exit();
            }

            _currentState = state;

            if (_currentState != null)
            {
                _currentState.Enter();
            }
        }

        public void Update()
        {
            if (_currentState != null)
            {
                _currentState.Update();
            }
        }

        public void FixedUpdate()
        {
            if (_currentState != null)
            {
                _currentState.FixedUpdate();
            }
        }
    }
}
