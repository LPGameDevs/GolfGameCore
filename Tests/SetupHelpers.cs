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
    }

    public void SetPlayers(int numberOfPlayers = 4)
    {
        _currentPlayerCount = numberOfPlayers;
    }

    public void OnFetchPlayers(List<Player> playerList)
    {
        for (int i = 0; i < _currentPlayerCount; i++)
        {
            AIBrain brain = new AIBrain();
            playerList.Add(new Player(brain));
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
