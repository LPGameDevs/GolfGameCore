
using System;
using GameCore.Cards;
using GameCore.Exceptions;
using GameCore.Players;

namespace GameCore
{
    public class GameManager
    {
        private bool _isLastRound = false;
        public bool IsLastRound => _isLastRound;

        private bool _isGameOver = false;
        public bool IsGameOver => _isGameOver;

        private IStateMachine _stateMachine;
        private PlayerManager _playerManager;

        private GameState _gameState;

        #region GameManagement

        public void StartNewGame(GameState gameState)
        {
            _gameState = gameState;

            // Any services the need refreshing between games should go here.
            // Services that done need refreshing can go in the singleton constructor.
            _stateMachine = new GolfStateMachine();
            _playerManager = new PlayerManager();

            // Concepts:
            // - Initialise is when a new class is created, basic setup.
            // - Refresh is when a class is reset to values provided by the game state.
            // @todo Is there a difference between these two concepts?

            // @todo Should these be singletons? Why?
            DeckManager.Instance.Initialize(_gameState);
            TurnManager.Instance.Initialize();

            _playerManager.Initialize(_gameState);
            _stateMachine.Initialize();

            _stateMachine.StartNewGame();

            // @todo this happens on server.
            // DeckManager.Instance.TurnOverDeck();

            StartNewMatch();
        }

        public void RejoinGame(GameState gameState)
        {
            // @todo Similar to starting a new game, but game state is already in progress.
        }

        public void StartNewMatch()
        {
            _isLastRound = false;
            _stateMachine.StartNewMatch();
        }

        public void StartNewTurn()
        {
            _stateMachine.StartNewTurn();
        }

        public void StartLastRound()
        {
            _isLastRound = true;
            PlayerEvents.CompleteTurn();
        }

        public void GameOver()
        {
            _isGameOver = true;

            StopPlaying();
        }

        /**
         * This method is called when the player stops engaging in an active game.
         */
        public void StopPlaying()
        {
            _stateMachine.StopPlaying();
        }

        public void UpdateGameFromState(GameState gameState)
        {
            DeckManager.Instance.Refresh();
        }

        #endregion

        #region PlayerActions

        public Player[] GetPlayers()
        {
            if (_playerManager.GetPlayers().Length == 0)
            {
                throw new NoPlayersException();
            }
            return _playerManager.GetPlayers();
        }

        public Player GetPlayer(PlayerId id)
        {
            return _playerManager.GetPlayer(id);
        }

        public bool DrawCard(DeckType deck = DeckType.Draw)
        {
            if (GetCurrentState() != nameof(DrawCard))
            {
                return false;
            }

            var card = DeckManager.Instance.DrawCard(deck);

            var currentPlayer = TurnManager.Instance.GetCurrentTurn();
            var player = GetPlayer(currentPlayer);
            player.HoldCard(card);

            Console.WriteLine($"DrawCard: {card}");

            return true;
        }

        public bool PlaceCard(Card card)
        {
            if (GetCurrentState() != nameof(DiscardCard))
            {
                return false;
            }

            var currentPlayer = TurnManager.Instance.GetCurrentTurn();
            if (card.Player != currentPlayer)
            {
                return false;
            }


            var player = GetPlayer(currentPlayer);
            player.PlaceCard(card.Index);

            Console.WriteLine($"PlaceCard: {card}");
            return true;

        }

        public bool DiscardCard()
        {
            if (GetCurrentState() != nameof(DiscardCard))
            {
                return false;
            }

            var currentPlayer = TurnManager.Instance.GetCurrentTurn();
            var player = GetPlayer(currentPlayer);
            Console.WriteLine($"DiscardCard: {player.GetHoldingCard()}");


            player.DiscardCard();
            return true;
        }

        public CardDto[] GetCards()
        {
            return _gameState.deck.Cards;
        }

        public CardDto[] GetPlayerCards(PlayerId id)
        {
            int playerId = (int) id - 1;
            string key = playerId.ToString();
            return _gameState.hands[key];
        }

        #endregion


        public string GetCurrentState()
        {
            return _stateMachine.GetCurrentState();
        }


        #region Singleton

        private static GameManager _instance;

        private GameManager()
        {

        }

        public static GameManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameManager();
                }

                return _instance;
            }
        }

        #endregion

    }
}
