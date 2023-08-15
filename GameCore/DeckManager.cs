using System;
using System.Collections.Generic;

namespace GameCore
{
    public enum DeckType
    {
        Discard = 0,
        Draw = 1
    }

    /**
     * Turn Manager for progressing current players turn.
     *
     * Also moves to the next player in the correct order to advance the game.
     */
    public class DeckManager
    {
        public static event Action OnCardDiscarded;

        private Queue<CardDto> _deck = new Queue<CardDto>();
        private LinkedList<CardDto> _discard = new LinkedList<CardDto>();

        public CardDto DrawCard(DeckType deck = DeckType.Draw)
        {
            CardDto card;
            if (deck == DeckType.Draw)
            {
                card = DrawDeckCard();
            }
            else
            {
                card = DrawDiscardCard();
            }

            return card;
        }

        private CardDto DrawDeckCard()
        {
            if (_deck.Count == 0)
            {
                Shuffle();
            }

            return _deck.Dequeue();
        }

        private CardDto DrawDiscardCard()
        {
            if (_discard.Count == 0)
            {
                throw new DrawFromEmptyDiscardException();
            }

            CardDto card = _discard.Last.Value;
            _discard.RemoveLast();
            return card;
        }

        public CardDto GetDiscardTopCard()
        {
            if (_discard.Count == 0)
            {
                throw new DrawFromEmptyDiscardException();
            }

            return _discard.Last.Value;
        }

        public void DiscardCard(CardDto card)
        {
            _discard.AddLast(card);
            OnCardDiscarded?.Invoke();
        }

        public void AddCards(CardDto[] cards)
        {
            _deck.Clear();

            foreach (var card in cards)
            {
                _deck.Enqueue(card);
            }
        }

        public void Shuffle()
        {
            _deck.Clear();
            // @todo Shuffle discard pile into deck.
        }

        public void TurnOverDeck()
        {
            CardDto drawCard = DrawDeckCard();
            DiscardCard(drawCard);
        }

        public void StartListening()
        {
        }

        public void StopListening()
        {
        }

        #region Singleton

        private static DeckManager _instance;

        private DeckManager(CardDto[] cards)
        {
            AddCards(cards);
        }

        public static DeckManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    CardDto[] cards = GameManager.Instance.GetCards();
                    _instance = new DeckManager(cards);
                }

                return _instance;
            }
        }

        #endregion
    }

    public class DrawFromEmptyDiscardException : Exception
    {
    }
}
