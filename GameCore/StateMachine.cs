using System;

namespace GameCore
{
    public abstract class StateMachine : IStateMachine
    {
        public static event Action<string,string> OnTransitionState;
        public static event Action OnTurnFinished;

        protected IState _currentState = null;

        public string GetCurrentState()
        {
            return _currentState.GetType().Name;
        }

        protected void TransitionTo(IState nextState)
        {
            var previousState = _currentState;
            nextState.Enter();
            _currentState = nextState;

            if (previousState != null)
            {
                OnTransitionState?.Invoke(previousState.GetType().Name, _currentState.GetType().Name);
            }

        }

        public abstract void StartNewGame();

        public abstract void StartNewMatch();

        public abstract void StartNewTurn();

        public abstract void Initialize();

        public abstract void StopPlaying();
    }

    public interface IStateMachine
    {
        string GetCurrentState();
        void StartNewGame();
        void StartNewMatch();
        void StartNewTurn();

        void Initialize();
        void StopPlaying();
    }

    public interface IState
    {
        void Enter();

        void Exit();
    }
}
