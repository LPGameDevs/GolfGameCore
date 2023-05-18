using System;
using GameCore.Players;

namespace GameCore
{
    public enum PlayerId
    {
        Player1 = 1,
        Player2 = 2,
        Player3 = 3,
        Player4 = 4,
        NoPlayer = 0
    }

    /**
     * Turn Manager for progressing current players turn.
     *
     * Also moves to the next player in the correct order to advance the game.
     */
    public class TurnManager
    {
        public static event Action<PlayerId,PlayerId> OnTransitionPlayerTurn;

        private Player _currentPlayer;
        private PlayerId _currentPlayerName = PlayerId.NoPlayer;

        // This should probably be private...
        public void NextPlayerStartTurn()
        {
            PlayerId previousPlayer = _currentPlayerName;
            Player[] players = GameManager.Instance.GetPlayers();

            if (_currentPlayerName == PlayerId.NoPlayer)
            {
                _currentPlayer = players[0];
                _currentPlayerName = _currentPlayer.Id;
            }
            else
            {
                for (int i = 0; i < players.Length; i++)
                {
                    if (players[i].Id == _currentPlayerName)
                    {
                        _currentPlayer = players[(i + 1) % players.Length];
                        _currentPlayerName = _currentPlayer.Id;
                        break;
                    }
                }
            }

            OnTransitionPlayerTurn?.Invoke(previousPlayer, _currentPlayerName);
            GameManager.Instance.StartNewTurn();
        }

        public void TakeTurn()
        {
            _currentPlayer.StartTurn();

            while (_currentPlayer.CanGo)
            {
                _currentPlayer.TakeTurn();
            }

            _currentPlayer.EndTurn();
        }

        public PlayerId GetCurrentTurn()
        {
            return _currentPlayerName;
        }

        public void Refresh()
        {
            _currentPlayer = null;
            _currentPlayerName = PlayerId.NoPlayer;
        }

        public void StartListening()
        {
            PlayerEvents.OnPlayerCompleteTurn += NextPlayerStartTurn;
        }

        public void StopListening()
        {
            PlayerEvents.OnPlayerCompleteTurn -= NextPlayerStartTurn;
        }

        #region Singleton

        private static TurnManager _instance;

        private TurnManager()
        {
        }

        public static TurnManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new TurnManager();
                }

                return _instance;
            }
        }

        #endregion
    }
}
