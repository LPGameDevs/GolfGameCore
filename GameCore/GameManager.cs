
using System;

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

        IStateMachine _stateMachine;
        Player[] _players = Array.Empty<Player>();

        public void SetStateMachine(IStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }
        public void SetPlayers(Player[] players)
        {
            _players = players;
        }

        public void StartNewGame()
        {
            _players = Array.Empty<Player>();
            _stateMachine.StartNewGame();
            StartNewMatch();
        }

        public void StartNewMatch()
        {
            _isLastRound = false;
            _stateMachine.StartNewMatch();
        }

        public void StartNewTurn()
        {
            if (!TurnManager.Instance.IsCurrentPlayerTurn)
            {
                return;
            }
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
            if (_players.Length == 0)
            {
                throw new NoPlayersException();
            }
            return _players;
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
    }

    public class NoPlayersException : Exception
    {
    }

    public class Player
    {
        public int Points() => 0;
        public Card[] Cards { get; }

        public Player()
        {
            Cards = new[]
            {
                new Card(1),
                new Card(7),
                new Card(3),
                new Card(4)
            };
        }
    }

    public class Card
    {
        public int Number { get; private set; }

        public Card(int number)
        {
            Number = number;
        }
    }
}
