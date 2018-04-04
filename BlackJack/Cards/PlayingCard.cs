using System;
using System.Linq;

namespace BlackJack.Cards
{
    class PlayingCard : PlayingCardData
    {
        public string Rank { get; protected set; }
        public string Suit { get; protected set; }

        public bool Hidden { get; set; }

        public PlayingCard(string rank, string suit)
        {
            Init(rank, suit, false);
        }

        public PlayingCard(string rank, string suit, bool hide)
        {
            Init(rank, suit, hide);
        }

        private void Init(string rank, string suit, bool hide)
        {
            if (CardRanks.Contains(rank))
            {
                if (CardSuits.Contains(suit))
                {
                    Rank = rank;
                    Suit = suit;
                    Hidden = hide;
                }
                else
                {
                    throw new ArgumentException("Suit must be one of the following types: " + string.Join(", ", CardSuits));
                }
            }
            else
            {
                throw new ArgumentException("Rank must be one of the following types: " + string.Join(", ", CardRanks));
            }
        }

        public override string ToString()
        {
            return $"{Rank} of {Suit}";
        }
    }
}
