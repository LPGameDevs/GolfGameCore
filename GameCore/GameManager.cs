
using System;
using GameCore.Players;

namespace GameCore
{
    public class GameManager
    {
        // Track the current match.
        // Track the current player.

        private bool _isLastRound = false;
        private bool _isGameOver = false;
        public bool IsLastRound => _isLastRound;

        public bool IsGameOver => _isGameOver;

        private readonly IStateMachine _stateMachine;
        private readonly PlayerManager _playerManager;

        private GameState _gameState;

        public void StartNewGame(GameState gameState)
        {
            _gameState = gameState;
            StartListening();
            TurnManager.Instance.Refresh();
            _playerManager.Refresh();
            _stateMachine.StartNewGame();
            DeckManager.Instance.TurnOverDeck();

            StartNewMatch();
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

        private void StartLastRound()
        {
            _isLastRound = true;
        }

        public string GetCurrentState()
        {
            return _stateMachine.GetCurrentState();
        }

        public Player[] GetPlayers()
        {
            if (_playerManager.GetPlayers().Length == 0)
            {
                throw new NoPlayersException();
            }
            return _playerManager.GetPlayers();
        }

        public void StartListening()
        {
            _stateMachine.StartListening();
            PlayerEvents.OnPlayerCallLastRound += StartLastRound;
        }

        public void StopListening()
        {
            _stateMachine.StopListening();
            PlayerEvents.OnPlayerCallLastRound -= StartLastRound;
        }

        #region Singleton

        private static GameManager _instance;

        private GameManager()
        {
            IStateMachine stateMachine = new GolfStateMachine();
            _stateMachine = stateMachine;
            _playerManager = new PlayerManager();
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

        public void GameOver()
        {
            _isGameOver = true;
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

            PlayerEvents.DrawCard();

            Console.WriteLine($"DrawCard: {card}");

            return true;
        }

        public void PlaceCard(Card card)
        {
            if (GetCurrentState() != nameof(DiscardCard))
            {
                return;
            }

            var currentPlayer = TurnManager.Instance.GetCurrentTurn();
            if (card.Player != currentPlayer)
            {
                return;
            }


            var player = GetPlayer(currentPlayer);
            player.PlaceCard(card.Index);

            Console.WriteLine($"PlaceCard: {card}");

        }

        public void DiscardCard()
        {
            if (GetCurrentState() != nameof(DiscardCard))
            {
                return;
            }

            var currentPlayer = TurnManager.Instance.GetCurrentTurn();
            var player = GetPlayer(currentPlayer);
            Console.WriteLine($"DiscardCard: {player.GetHoldingCard()}");


            player.DiscardCard();

            PlayerEvents.DiscardCard();
        }

        public CardDto[] GetCards()
        {
            return _gameState.deck;
        }
    }

    public class NoPlayersException : Exception
    {
    }

    public class Card
    {
        public int Index { get; private set; }
        public CardDto CardData { get; set; }
        public PlayerId Player { get; set; }

        public Card(CardDto cardData, int index, PlayerId player)
        {
            CardData = cardData;
            Index = index;
            Player = player;
        }
    }
}
