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

        public Player(IPlayerBrain brain, PlayerId id = PlayerId.NoPlayer)
        {
            _brain = brain;
            _brain.SetPlayer(this);
            Id = id;

            Cards = new[]
            {
                new Card(1),
                new Card(7),
                new Card(3),
                new Card(4)
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
    }
}
