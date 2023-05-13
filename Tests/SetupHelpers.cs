using GameCore;

namespace Tests;

public class SetupHelpers
{
    public void MinimalSetup()
    {
        IStateMachine stateMachine = new GolfStateMachine();
        GameManager.Instance.SetStateMachine(stateMachine);
        GameManager.Instance.StartListening();
    }

    public void AddPlayers(int numberOfPlayers = 4)
    {
        Player[] players = new Player[numberOfPlayers];
        for (int i = 0; i < numberOfPlayers; i++)
        {
            players[i] = new Player();
        }
        GameManager.Instance.SetPlayers(players);
    }
}
