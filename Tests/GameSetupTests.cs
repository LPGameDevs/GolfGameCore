using GameCore;
using GameCore.Players;
using NUnit.Framework;

namespace Tests;

public class GameSetupTests
{
    private SetupHelpers _setupHelpers = new SetupHelpers();

    [SetUp]
    public void Setup()
    {
        _setupHelpers.MinimalSetup();
    }

    [Test]
    public void TestCards()
    {
        _setupHelpers.SetPlayers();
        GameManager.Instance.StartNewGame();

        Player[] players = GameManager.Instance.GetPlayers();
        for (int i = 0; i < players.Length; i++)
        {
            Assert.That(players[i].Cards.Length, Is.EqualTo(4));
        }
    }

    [Test]
    public void TestPlayers()
    {
        _setupHelpers.SetPlayers(0);
        GameManager.Instance.StartNewGame();

        // No players have been set yet.
        Assert.Throws<NoPlayersException>(() => GameManager.Instance.GetPlayers());

        _setupHelpers.SetPlayers(1);
        GameManager.Instance.StartNewGame();

        int playerCount = GameManager.Instance.GetPlayers().Length;
        Assert.That(playerCount, Is.EqualTo(1));
    }

    [Test]
    public void TestPoints()
    {
        GameManager.Instance.StartNewGame();
        _setupHelpers.SetPlayers();

        Player[] players = GameManager.Instance.GetPlayers();
        for (int i = 0; i < players.Length; i++)
        {
            Assert.That(players[i].Points, Is.EqualTo(0));
        }
    }
}
