using GameCore;
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
    public void TestPlayers()
    {
        GameManager.Instance.StartNewGame();

        // No players have been set yet.
        Assert.Throws<NoPlayersException>(() => GameManager.Instance.GetPlayers());

        _setupHelpers.AddPlayers(1);

        int playerCount = GameManager.Instance.GetPlayers().Length;
        Assert.That(playerCount, Is.EqualTo(1));
    }

    [Test]
    public void TestPoints()
    {
        GameManager.Instance.StartNewGame();
        _setupHelpers.AddPlayers();

        Player[] players = GameManager.Instance.GetPlayers();
        for (int i = 0; i < players.Length; i++)
        {
            Assert.That(players[i].Points, Is.EqualTo(0));
        }
    }

    [Test]
    public void TestCards()
    {
        GameManager.Instance.StartNewGame();
        _setupHelpers.AddPlayers();

        Player[] players = GameManager.Instance.GetPlayers();
        for (int i = 0; i < players.Length; i++)
        {
            Assert.That(players[i].Cards.Length, Is.EqualTo(4));
        }
    }
}