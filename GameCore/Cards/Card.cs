namespace GameCore.Cards
{
    public class Card
    {
        public int Index { get; private set; }
        public CardDto CardData { get; set; }
        public PlayerId Player { get; set; }

        public Card(CardDto cardData, int index, PlayerId player)
        {
            CardData = cardData;
            Index = index;
            Player = player;
        }
    }
}