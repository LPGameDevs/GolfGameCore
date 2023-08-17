using GameCore.States;

namespace GameCore
{
    public class GolfStateMachine : StateMachine
    {
        public GolfStateMachine()
        {
            _currentState = new Waiting();
        }

        public override void StartNewGame()
        {
            // Nothing to do.
        }

        public override void StartNewMatch()
        {
            IState nextState = new ViewCards();
            TransitionTo(nextState);
        }

        private void OnExitViewCards()
        {
            IState nextState = new Waiting();
            TransitionTo(nextState);
        }

        // @todo Something needs to start a new turn.
        public override void StartNewTurn()
        {
            IState nextState = new DrawCard();
            TransitionTo(nextState);
        }

        private void OnExitDrawCard()
        {
            IState nextState = new DiscardCard();
            TransitionTo(nextState);
        }

        private void OnExitDiscardCard()
        {
            IState nextState = new Waiting();
            if (!GameManager.Instance.IsLastRound)
            {
                nextState = new CompleteTurn();
            }
            TransitionTo(nextState);
        }

        private void OnExitCompleteTurn()
        {
            // No state transitions on complete turn.
        }

        public override void Initialize()
        {
            ViewCards.OnExit += OnExitViewCards;
            DrawCard.OnExit += OnExitDrawCard;
            DiscardCard.OnExit += OnExitDiscardCard;
            CompleteTurn.OnExit += OnExitCompleteTurn;
        }

        public override void StopPlaying()
        {
            ViewCards.OnExit -= OnExitViewCards;
            DrawCard.OnExit -= OnExitDrawCard;
            DiscardCard.OnExit -= OnExitDiscardCard;
            CompleteTurn.OnExit -= OnExitCompleteTurn;
        }


    }
}
