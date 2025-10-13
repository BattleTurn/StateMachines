using System;
using System.Collections.Generic;

using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace BattleTurn.StateMachines
{
    [Serializable]
    public class StateMachine
    {
        [SerializeField]
        [Tooltip("Should the state machine recycle states? If true, exited states will be stored and reused when switching back to them.")]
        private bool _recycleStates = false;
#if ODIN_INSPECTOR
        [ReadOnly]
        [ShowInInspector]
#endif
        private IState _currentState;

#if ODIN_INSPECTOR
        [ReadOnly]
        [ShowInInspector]
#endif
        private Dictionary<Type, IState> _recycledStates = new();

        public StateMachine(bool recycleStates = false)
        {
            _recycleStates = recycleStates;
        }

        public bool TryGetCurrentState<T>(out T state) where T : IState
        {
            if (_currentState is not T)
            {
                state = default;
                return false;
            }
            state = (T)_currentState;
            return true;
        }

        // Switch to another state
        public void ChangeState<T>() where T : IState
        {
            if (_currentState != null)
                OnExitCurrentState();

            if (_recycledStates.ContainsKey(typeof(T)))
            {
                _currentState = _recycledStates[typeof(T)];
                _recycledStates.Remove(typeof(T));
            }
            _currentState.OnEnter();
        }

        public void ChangeState(IState newState)
        {
            if (_currentState != null)
                OnExitCurrentState();

            if (!RecycleState(newState))
                _currentState = newState;
            _currentState.OnEnter();
        }

        private bool RecycleState(IState state)
        {
            if (_recycledStates.ContainsKey(state.GetType()))
            {
                _currentState = _recycledStates[state.GetType()];
                _recycledStates.Remove(state.GetType());
                return true;
            }
            return false;
        }

        private void OnExitCurrentState()
        {
            _currentState.OnExit();
            if (_recycleStates)
                _recycledStates[_currentState.GetType()] = _currentState;
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
