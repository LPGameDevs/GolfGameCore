
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

        public void StartNewGame()
        {
            _playerManager.Refresh();
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

            StartListening();
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
    }

    public class NoPlayersException : Exception
    {
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
