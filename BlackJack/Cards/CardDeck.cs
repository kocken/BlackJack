using System;
using System.Collections.Generic;

namespace BlackJack.Cards
{
    class CardDeck : PlayingCardData
    {
        public List<PlayingCard> Deck { get; }

        public int DeckAmount { get; set; }

        public CardDeck(int deckAmount)
        {
            Deck = new List<PlayingCard>();
            DeckAmount = deckAmount;
            ResetDeck();
        }

        public void ResetDeck()
        {
            Deck.Clear();
            FillDeck();
            ShuffleDeck();
        }

        private void FillDeck()
        {
            for (int i = 0; DeckAmount > i; i++)
            {
                foreach (string value in CardRanks)
                {
                    foreach (string suit in CardSuits)
                    {
                        Deck.Add(new PlayingCard(value, suit));
                    }
                }
            }
            Console.WriteLine("Reset card deck");
        }

        public void ShuffleDeck() // Fisher-Yates shuffle
        {
            Random rand = new Random();
            for (int i = Deck.Count - 1; i > 0; --i)
            {
                int r = rand.Next(i + 1);
                PlayingCard temp = Deck[i];
                Deck[i] = Deck[r];
                Deck[r] = temp;
            }
            Console.WriteLine("Shuffled card deck");
        }

        public bool DrawCard(string playerName, bool hideCard, bool sleep, out PlayingCard drawnCard)
        {
            if (Deck.Count > 0)
            {
                drawnCard = Deck[0];
                if (!hideCard)
                {
                    Console.WriteLine($"{playerName} drew card {drawnCard.ToString()}");
                }
                else
                {
                    Console.WriteLine($"{playerName} drew hidden card");
                }
                return Deck.Remove(drawnCard);
            }
            else
            {
                Console.WriteLine("Card deck is empty.");
            }
            drawnCard = null;
            return false;
        }
    }
}
