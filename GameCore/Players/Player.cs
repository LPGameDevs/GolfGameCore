using System;
using System.Collections.Generic;

namespace GameCore.Players
{
    public class Player
    {
        public static event Action<bool> OnPlayerHoldCard;

        private IPlayerBrain _brain;

        public int Points() => 0;

        public int TurnsPlayed { get; private set; } = 0;

        public Card[] Cards { get; }
        public PlayerId Id { get; private set; }
        public bool CanGo => _brain.CanGo();

        private Card _holdingCard = null;

        public Player(IPlayerBrain brain, PlayerId id = PlayerId.NoPlayer)
        {
            _brain = brain;
            _brain.SetPlayer(this);
            Id = id;

            Cards = new[]
            {
                new Card(DeckManager.Instance.DrawCard(), 0, id),
                new Card(DeckManager.Instance.DrawCard(), 1, id),
                new Card(DeckManager.Instance.DrawCard(), 2, id),
                new Card(DeckManager.Instance.DrawCard(), 3, id),
            };
        }

        public void StartTurn()
        {
            _brain.Reset();
        }

        public void TakeTurn()
        {
            _brain.Action();
        }

        public void EndTurn()
        {
            TurnsPlayed++;
        }


        public void HoldCard(CardDto card)
        {
            _holdingCard = new Card(card, -1, PlayerId.NoPlayer);
            OnPlayerHoldCard?.Invoke(true);
        }

        public void PlaceCard(int index)
        {
            // Swap holding card with card at index.
            CardDto holdingCard = _holdingCard.CardData;

            _holdingCard.CardData = Cards[index].CardData;
            Cards[index].CardData = holdingCard;

            GameManager.Instance.DiscardCard();
        }

        public void DiscardCard()
        {
            DeckManager.Instance.DiscardCard(_holdingCard.CardData);
            _holdingCard = null;
            OnPlayerHoldCard?.Invoke(false);
        }

        public CardDto GetHoldingCard()
        {
            return _holdingCard?.CardData ?? new CardDto(CardName.None, Suit.None);
        }

        public int Score()
        {
            int score = 0;

            var cards = new Dictionary<CardName, int>();

            // Count cards.
            foreach (var card in Cards)
            {
                cards[card.CardData.CardName] = cards.TryGetValue(card.CardData.CardName, out var existingCardCount) ? existingCardCount + 1 : 1;
            }

            foreach (KeyValuePair<CardName,int> cardGroup in cards)
            {
                switch (cardGroup.Value)
                {
                    case 2:
                    case 4:
                        break;

                    case 3:
                    case 1:
                        // @todo Add a mapping of enum to score value.
                        score += (int) cardGroup.Key;
                        break;

                }
            }
            return score;
        }
    }
}
