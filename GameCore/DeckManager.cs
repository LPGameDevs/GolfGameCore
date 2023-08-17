using System;
using System.Collections.Generic;
using GameCore.Exceptions;

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
        public static event Action OnRefresh;
        public static event Action OnCardDiscarded;

        private GameState _gameState;

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

            CardDto card = _discard.First.Value;
            _discard.RemoveFirst();
            return card;
        }

        public CardDto GetDiscardTopCard()
        {
            if (_discard.Count == 0)
            {
                throw new DrawFromEmptyDiscardException();
            }

            return _discard.First.Value;
        }

        public void DiscardCard(CardDto card)
        {
            _discard.AddFirst(card);
            OnCardDiscarded?.Invoke();
        }

        public void AddDeckCards(CardDto[] cards)
        {
            _deck.Clear();

            foreach (var card in cards)
            {
                _deck.Enqueue(card);
            }
        }

        public void AddDiscardCards(CardDto[] cards)
        {
            _discard.Clear();

            foreach (var card in cards)
            {
                // @todo is this the right way around?
                _discard.AddLast(card);
            }
        }

        public void Shuffle()
        {
            _deck.Clear();

            // @todo Shuffle discard pile into deck.
            foreach (var card in _discard)
            {
                _deck.Enqueue(card);
            }
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

        public void Initialize(GameState game)
        {
            _gameState = game;

            CardDto[] cards = game.deck.Cards;

            if (cards == null || cards.Length == 0)
            {
                throw new TryingToCreateDeckManagerWithoutCardsException();
            }

            CardDto[] discardCards = game.discard.Cards;

            if (discardCards == null || discardCards.Length == 0)
            {
                throw new TryingToCreateDeckManagerWithoutDiscardException();
            }

            Refresh();
        }

        public void Refresh()
        {
            AddDeckCards(_gameState.deck.Cards);
            AddDiscardCards(_gameState.discard.Cards);
            OnRefresh?.Invoke();
        }
    }
}
