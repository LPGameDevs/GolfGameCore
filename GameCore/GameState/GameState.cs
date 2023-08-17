using System.Collections.Generic;

namespace GameCore
{
    public class GameState
    {
        public string id;
        public string[] users;
        public DeckDto deck;
        public DeckDto discard;
        public Dictionary<string, CardDto[]> hands;
        public int turn;
    }

    public class DeckDto
    {
        public CardDto[] Cards;
    }

    public class CardDto
    {
        public CardName CardName;
        public Suit Suit;

        public CardDto(CardName name, Suit suit)
        {
            CardName = name;
            Suit = suit;
        }

        public override string ToString()
        {
            return $"{CardName} of {Suit}";
        }
    }

    public class HandDto
    {
        public CardDto[] Cards;
        public HandDto(CardDto[] cards)
        {
            Cards = cards;
        }
    }

    public enum Suit {
        Clubs = 0,
        Spades = 1,
        Diamonds = 2,
        Hearts = 3,
        None = -1,
    }

    public enum CardName {
        Ace = 0,
        Two = 1,
        Three = 2,
        Four = 3,
        Five = 4,
        Six = 5,
        Seven = 6,
        Eight = 7,
        Nine = 8,
        Ten = 9,
        Jack = 10,
        Queen = 11,
        King = 12,
        Joker = 13,
        None = -1
    }
}
