using System;
using System.Collections.Generic;

namespace BattleTurn.StateMachines
{
    public class StateMachine
    {
        private IState _currentState;
        private Dictionary<Type, IState> _states = new();

        // Register a state
        public void AddState(IState state)
        {
            _states[state.GetType()] = state;
        }

        // Switch to another state
        public void ChangeState<T>() where T : IState
        {
            if (_currentState != null)
                _currentState.OnExit();

            _currentState = _states[typeof(T)];
            _currentState.OnEnter();
        }

        public void ChangeState(IState newState)
        {
            if (_currentState != null)
                _currentState.OnExit();

            _states[newState.GetType()] = newState;
            _currentState = newState;
            _currentState.OnEnter();
        }

        // Update loop
        public void Tick()
        {
            _currentState?.Tick();
        }

        // Physics loop
        public void FixedTick()
        {
            _currentState?.FixedTick();
        }
    }
}
