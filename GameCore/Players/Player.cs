namespace GameCore.Players
{
    public class Player
    {
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
                new Card(DeckManager.Instance.DrawCard()),
                new Card(DeckManager.Instance.DrawCard()),
                new Card(DeckManager.Instance.DrawCard()),
                new Card(DeckManager.Instance.DrawCard()),
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


        public void HoldCard(int card)
        {
            _holdingCard = new Card(card);
        }

        public void PlaceCard(int index)
        {
            // Swap holding card with card at index.
            (Cards[index], _holdingCard) = (_holdingCard, Cards[index]);
            GameManager.Instance.DiscardCard();
        }

        public void DiscardCard()
        {
            DeckManager.Instance.DiscardCard(_holdingCard.Number);
            _holdingCard = null;
        }

        public int GetHoldingCard()
        {
            return _holdingCard?.Number ?? -1;
        }
    }
}
