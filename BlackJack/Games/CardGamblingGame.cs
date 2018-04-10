using BlackJack.Cards;
using BlackJack.Players;
using System;
using System.Collections.Generic;

namespace BlackJack.Games
{
    class CardGamblingGame : GamblingGame
    {
        public CardDeck CardDeck { get; protected set; }

        public CardGamblingGame(string gameName, Currency currency, string hostName, double dealerBalance) :
            base(gameName, currency, new Host(hostName, currency, dealerBalance), new Player("Player", currency))
        {
            Init();
        }

        public CardGamblingGame(string gameName, Currency currency, string hostName, double dealerBalance, string playerName) :
            base(gameName, currency, new Host(hostName, currency, dealerBalance), new Player(playerName, currency))
        {
            Init();
        }

        public CardGamblingGame(string gameName, Currency currency, string hostName, double dealerBalance, string playerName, double deposit) :
            base(gameName, currency, new Host(hostName, currency, dealerBalance), new Player(playerName, currency, deposit, true))
        {
            Init();
        }

        public CardGamblingGame(string gameName, Currency currency, string hostName, double dealerBalance, string playerName, double deposit, int deckAmount) :
            base(gameName, currency, new Host(hostName, currency, dealerBalance), new Player(playerName, currency, deposit, true))
        {
            Init(deckAmount);
        }

        public CardGamblingGame(string gameName, Currency currency, string hostName, double dealerBalance, string playerName, double deposit, int deckAmount, double minBet, double maxBet) :
            base(gameName, currency, new Host(hostName, currency, dealerBalance), new Player(playerName, currency, deposit, true), minBet, maxBet)
        {
            Init(deckAmount);
        }

        private void Init()
        {
            Init(1);
        }

        private void Init(int deckAmount)
        {
            CardDeck = new CardDeck(deckAmount);
        }

        public bool DrawCard(string playerName, bool hideCard, CardHand hand, int sleep, bool pressAKey)
        {
            bool ret = false;
            if (CardDeck.DrawCard(playerName, hideCard, true, out PlayingCard drawnCard))
            {
                hand.Cards.Add(drawnCard);
                if (hideCard)
                {
                    hand.LastCard.Hidden = true;
                }
                ret = true;
            }
            if (sleep > 0)
            {
                Sleep(sleep);
            }
            if (pressAKey)
            {
                PressAKey("Press a key to continue");
            }
            return ret;
        }

        public void DoubleBet(CardHand hand)
        {
            Console.WriteLine($"{Player.Name} doubled bet {hand.HandBet} {CurrencyUtil.GetCode(Currency)} to {hand.HandBet * 2} {CurrencyUtil.GetCode(Currency)}");
            Player.Balance -= hand.HandBet;
            Host.Balance -= hand.HandBet;
            hand.HandBet = hand.HandBet * 2;
        }

        public int GetHandIndex(List<CardHand> hands, CardHand hand)
        {
            for (int i = 0; hands.Count > i; i++)
            {
                if (hands[i].Equals(hand))
                {
                    return i;
                }
            }
            return -1;
        }
    }
}
