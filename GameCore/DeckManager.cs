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

        private Queue<int> _deck = new Queue<int>();
        private LinkedList<int> _discard = new LinkedList<int>();

        public int DrawCard(DeckType deck = DeckType.Draw)
        {
            int card;
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

        private int DrawDeckCard()
        {
            if (_deck.Count == 0)
            {
                Shuffle();
            }

            return _deck.Dequeue();
        }

        private int DrawDiscardCard()
        {
            if (_discard.Count == 0)
            {
                throw new DrawFromEmptyDiscardException();
            }

            int card = _discard.Last.Value;
            _discard.RemoveLast();
            return card;
        }

        public int GetDiscardTopCard()
        {
            if (_discard.Count == 0)
            {
                throw new DrawFromEmptyDiscardException();
            }

            return _discard.Last.Value;
        }

        public void DiscardCard(int number)
        {
            _discard.AddLast(number);
            OnCardDiscarded?.Invoke();
        }

        public void Shuffle()
        {
            _deck.Clear();
            _deck.Enqueue(1);
            _deck.Enqueue(2);
            _deck.Enqueue(3);
            _deck.Enqueue(4);
            _deck.Enqueue(5);
            _deck.Enqueue(6);
            _deck.Enqueue(7);
            _deck.Enqueue(8);
            _deck.Enqueue(9);
            _deck.Enqueue(10);
        }

        public void TurnOverDeck()
        {
            int drawCard = DrawDeckCard();
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

        private DeckManager()
        {
            Shuffle();
        }

        public static DeckManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DeckManager();
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
