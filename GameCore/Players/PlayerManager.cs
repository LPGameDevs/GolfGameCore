using System;
using System.Collections.Generic;

namespace GameCore.Players
{
    /**
     * Manager class to hold the current group of players.
     *
     * Basically just fetches a list of players form "somewhere" when an new game starts.
     */
    public class PlayerManager
    {
        public static event Action<List<Player>> OnFetchPlayers;

        private List<Player> _players = new List<Player>();

        public PlayerManager()
        {
            Refresh();
        }

        public Player[] GetPlayers()
        {
            return _players.ToArray();
        }

        public void Refresh()
        {
            DeckManager.Instance.Shuffle();
            _players.Clear();
            OnFetchPlayers?.Invoke(_players);
        }

        public Player GetPlayer(PlayerId id)
        {
            foreach (var player in _players)
            {
                if (player.Id == id)
                {
                    return player;
                }
            }

            throw new PlayerNotFoundException();
        }
    }

    public class PlayerNotFoundException : Exception
    {
    }
}
