using System.Collections.Generic;
using GameCore;
using GameCore.Players;

namespace Tests;

public class SetupHelpers
{
    private int _currentPlayerCount = 0;

    public SetupHelpers()
    {
        StartListening();
    }

    public void MinimalSetup()
    {
        GameManager.Instance.StartNewGame();
    }

    public void SetPlayers(int numberOfPlayers = 4)
    {
        _currentPlayerCount = numberOfPlayers;
    }

    public void OnFetchPlayers(List<Player> playerList)
    {
        Dictionary<int, PlayerId> playermap = new Dictionary<int, PlayerId>()
        {
            {0, PlayerId.Player1},
            {1, PlayerId.Player2},
            {2, PlayerId.Player3},
            {3, PlayerId.Player4},
        };

        for (int i = 0; i < _currentPlayerCount; i++)
        {
            AIBrain brain = new AIBrain();
            playerList.Add(new Player(brain, playermap[i]));
        }
    }

    private void StartListening()
    {
        PlayerManager.OnFetchPlayers += OnFetchPlayers;
    }

    public void StopListening()
    {
        PlayerManager.OnFetchPlayers -= OnFetchPlayers;
    }
}
