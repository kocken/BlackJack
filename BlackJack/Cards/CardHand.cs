using System.Collections.Generic;

namespace BlackJack.Cards
{
    class CardHand
    {
        public List<PlayingCard> Cards { get; set; }

        public double HandBet { get; set; }

        public PlayingCard LastCard
        {
            get
            {
                return Cards[Cards.Count - 1];
            }
        }

        public CardHand()
        {
            Init(0);
        }

        public CardHand(double handBet)
        {
            Init(handBet);
        }

        public void Init(double handBet)
        {
            Cards = new List<PlayingCard>();
            HandBet = handBet;
        }

        public PlayingCard RemoveLastCard()
        {
            PlayingCard temp = LastCard;
            Cards.Remove(temp);
            return temp;
        }
    }
}
