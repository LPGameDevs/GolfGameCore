using System;
using GameCore;
using GameCore.States;
using NUnit.Framework;

namespace Tests
{
    public class PointsTests
    {
        private SetupHelpers _setupHelpers;

        [SetUp]
        public void Setup()
        {
            _setupHelpers = new SetupHelpers();
            _setupHelpers.MinimalSetup();
        }

        [Test]
        public void TestPlayers()
        {
            _setupHelpers.SetPlayers(2);
            GameManager.Instance.StartNewGame();
            var players = GameManager.Instance.GetPlayers();

            // Player 1 has 1, 2, 3, 4.
            Assert.That(players[0].Score, Is.EqualTo(10));

            // Player 2 has 5, 6, 7, 8.
            Assert.That(players[1].Score, Is.EqualTo(26));

            TurnManager.Instance.NextPlayerStartTurn();

            GameManager.Instance.DrawCard();
            GameManager.Instance.PlaceCard(players[0].Cards[0]);

            // Player 1 has 10, 2, 3, 4.
            Assert.That(players[0].Score, Is.EqualTo(19));

            TurnManager.Instance.NextPlayerStartTurn();

            GameManager.Instance.DrawCard();
            GameManager.Instance.PlaceCard(players[1].Cards[3]);

            // Console.WriteLine(players[1].Cards[0].Number);
            // Console.WriteLine(players[1].Cards[1].Number);
            // Console.WriteLine(players[1].Cards[2].Number);
            // Console.WriteLine(players[1].Cards[3].Number);

            // Player 2 has 5, 6, 7, 1.
            Assert.That(players[1].Score, Is.EqualTo(19));

            TurnManager.Instance.NextPlayerStartTurn();

            GameManager.Instance.DrawCard();
            GameManager.Instance.PlaceCard(players[0].Cards[0]);

            Assert.That(players[1].Score, Is.EqualTo(19));

            // Player 1 has 2, 2, 3, 4.
            Assert.That(players[0].Score, Is.EqualTo(7));
        }


        [TearDown]
        public void TearDown()
        {
            GameManager.Instance.StopListening();
            TurnManager.Instance.StopListening();
            _setupHelpers.StopListening();
        }
    }
}
