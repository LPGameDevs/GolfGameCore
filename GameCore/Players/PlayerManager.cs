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
        public static event Action<List<Player>> OnPlayersRefreshed;

        private List<Player> _players = new List<Player>();

        public PlayerManager()
        {
        }

        public Player[] GetPlayers()
        {
            return _players.ToArray();
        }

        public void Initialize(GameState state)
        {
            Refresh(state);
        }

        public void Refresh(GameState state)
        {
            // Cyclical reference.
            // DeckManager.Instance.Shuffle();
            _players.Clear();


            int i = 1;
            foreach (var user in state.users)
            {
                IPlayerBrain brain = new AIBrain();

                Player player = new Player(brain, (PlayerId) i);
                player.SetNetworkId(user);
                player.SetCards(state.hands[user]);
                _players.Add(player);

                i++;
            }

            OnPlayersRefreshed?.Invoke(_players);
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
