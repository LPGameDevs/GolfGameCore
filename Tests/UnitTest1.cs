using GameCore;
using GameCore.States;
using NUnit.Framework;

namespace Tests
{
    public class PlayerTurnTests
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

            foreach (var player in players)
            {
                Assert.That(player.Cards.Length, Is.EqualTo(4));
            }

            Assert.That(players[0].Cards[0].Number, Is.EqualTo(1));
            Assert.That(players[0].Cards[1].Number, Is.EqualTo(2));
            Assert.That(players[0].Cards[2].Number, Is.EqualTo(3));
            Assert.That(players[0].Cards[3].Number, Is.EqualTo(4));

            Assert.That(players[1].Cards[0].Number, Is.EqualTo(5));
            Assert.That(players[1].Cards[1].Number, Is.EqualTo(6));
            Assert.That(players[1].Cards[2].Number, Is.EqualTo(7));
            Assert.That(players[1].Cards[3].Number, Is.EqualTo(8));

            Assert.That(TurnManager.Instance.GetCurrentTurn(), Is.EqualTo(PlayerId.NoPlayer));

            TurnManager.Instance.NextPlayerStartTurn();

            Assert.That(TurnManager.Instance.GetCurrentTurn(), Is.EqualTo(PlayerId.Player1));
            Assert.That(GameManager.Instance.GetCurrentState(), Is.EqualTo(nameof(DrawCard)));

            int holdingCard = players[0].GetHoldingCard();
            Assert.That(holdingCard, Is.EqualTo(-1));

            GameManager.Instance.DrawCard();
            Assert.That(GameManager.Instance.GetCurrentState(), Is.EqualTo(nameof(DiscardCard)));
            holdingCard = players[0].GetHoldingCard();
            Assert.That(holdingCard, Is.EqualTo(10));

            GameManager.Instance.PlaceCard(players[0].Cards[1]);
            Assert.That(GameManager.Instance.GetCurrentState(), Is.EqualTo(nameof(CompleteTurn)));
            holdingCard = players[0].GetHoldingCard();
            Assert.That(holdingCard, Is.EqualTo(-1));

            Assert.That(players[0].Cards[0].Number, Is.EqualTo(1));
            Assert.That(players[0].Cards[1].Number, Is.EqualTo(10));
            Assert.That(players[0].Cards[2].Number, Is.EqualTo(3));
            Assert.That(players[0].Cards[3].Number, Is.EqualTo(4));
        }

        [Test]
        public void TestPlayersDrawDeckAndDiscard()
        {
            _setupHelpers.SetPlayers(2);
            GameManager.Instance.StartNewGame();
            var players = GameManager.Instance.GetPlayers();

            Assert.That(players[0].Cards[0].Number, Is.EqualTo(1));
            Assert.That(players[0].Cards[1].Number, Is.EqualTo(2));
            Assert.That(players[0].Cards[2].Number, Is.EqualTo(3));
            Assert.That(players[0].Cards[3].Number, Is.EqualTo(4));

            TurnManager.Instance.NextPlayerStartTurn();


            GameManager.Instance.DrawCard();
            GameManager.Instance.DiscardCard();

            Assert.That(players[0].Cards[0].Number, Is.EqualTo(1));
            Assert.That(players[0].Cards[1].Number, Is.EqualTo(2));
            Assert.That(players[0].Cards[2].Number, Is.EqualTo(3));
            Assert.That(players[0].Cards[3].Number, Is.EqualTo(4));
        }

        [Test]
        public void TestPlayersDrawDiscardAndPlace()
        {
            _setupHelpers.SetPlayers(2);
            GameManager.Instance.StartNewGame();
            var players = GameManager.Instance.GetPlayers();

            Assert.That(players[0].Cards[0].Number, Is.EqualTo(1));
            Assert.That(players[0].Cards[1].Number, Is.EqualTo(2));
            Assert.That(players[0].Cards[2].Number, Is.EqualTo(3));
            Assert.That(players[0].Cards[3].Number, Is.EqualTo(4));

            TurnManager.Instance.NextPlayerStartTurn();


            GameManager.Instance.DrawCard(DeckType.Discard);

            int holdingCard = players[0].GetHoldingCard();
            Assert.That(holdingCard, Is.EqualTo(9));

            GameManager.Instance.PlaceCard(players[0].Cards[3]);

            Assert.That(players[0].Cards[0].Number, Is.EqualTo(1));
            Assert.That(players[0].Cards[1].Number, Is.EqualTo(2));
            Assert.That(players[0].Cards[2].Number, Is.EqualTo(3));
            Assert.That(players[0].Cards[3].Number, Is.EqualTo(9));
        }

        [Test]
        public void TestPlayersDrawDiscardAndDiscard()
        {
            _setupHelpers.SetPlayers(2);
            GameManager.Instance.StartNewGame();
            var players = GameManager.Instance.GetPlayers();

            Assert.That(players[0].Cards[0].Number, Is.EqualTo(1));
            Assert.That(players[0].Cards[1].Number, Is.EqualTo(2));
            Assert.That(players[0].Cards[2].Number, Is.EqualTo(3));
            Assert.That(players[0].Cards[3].Number, Is.EqualTo(4));

            TurnManager.Instance.NextPlayerStartTurn();


            GameManager.Instance.DrawCard(DeckType.Discard);
            GameManager.Instance.DiscardCard();

            Assert.That(players[0].Cards[0].Number, Is.EqualTo(1));
            Assert.That(players[0].Cards[1].Number, Is.EqualTo(2));
            Assert.That(players[0].Cards[2].Number, Is.EqualTo(3));
            Assert.That(players[0].Cards[3].Number, Is.EqualTo(4));
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
