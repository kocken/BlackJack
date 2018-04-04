using BlackJack.Cards;
using BlackJack.Players;
using System;
using System.Collections.Generic;

namespace BlackJack.Games
{
    class CardGamblingGame : GamblingGame
    {
        public CardDeck CardDeck { get; protected set; }

        public CardGamblingGame(string gameName, string currency, string hostName, double dealerBalance) :
            base(gameName, currency, new Host(hostName, currency, dealerBalance), new Player("Player", currency))
        {
            Init();
        }

        public CardGamblingGame(string gameName, string currency, string hostName, double dealerBalance, string playerName) :
            base(gameName, currency, new Host(hostName, currency, dealerBalance), new Player(playerName, currency))
        {
            Init();
        }

        public CardGamblingGame(string gameName, string currency, string hostName, double dealerBalance, string playerName, double deposit) :
            base(gameName, currency, new Host(hostName, currency, dealerBalance), new Player(playerName, currency, deposit, true))
        {
            Init();
        }

        public CardGamblingGame(string gameName, string currency, string hostName, double dealerBalance, string playerName, double deposit, int deckAmount) :
            base(gameName, currency, new Host(hostName, currency, dealerBalance), new Player(playerName, currency, deposit, true))
        {
            Init(deckAmount);
        }

        public CardGamblingGame(string gameName, string currency, string hostName, double dealerBalance, string playerName, double deposit, int deckAmount, double minBet, double maxBet) :
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

        protected bool Bet(double amount, CardHand hand)
        {
            if (Player.Balance == 0)
            {
                DepositOrQuit();
            }
            else if (amount > Player.Balance)
            {
                Console.WriteLine($"Can't bet amounts higher than {Player.Name}'s balance {Player.Balance}{Currency}");
            }
            else
            {
                if (amount < MinBet)
                {
                    Console.WriteLine($"Can't bet amounts lower than the minimum bet amount {MinBet}{Currency}");
                }
                else if (amount > MaxBet)
                {
                    Console.WriteLine($"Can't bet amounts higher than the max bet amount {MaxBet}{Currency}");
                }
                else if (amount > Host.Balance)
                {
                    Console.WriteLine($"Can't bet amounts higher than the {Host.Name}'s balance {Host.Balance}{Currency}");
                }
                else
                {
                    Player.Balance -= amount;
                    Host.Balance -= amount;
                    hand.HandBet = amount;
                    Console.WriteLine($"{Player.Name} bet {amount}{Currency}");
                    PrintLine();
                    return true;
                }
            }
            PrintLine();
            return false;
        }

        public void DoubleBet(CardHand hand)
        {
            Console.WriteLine($"{Player.Name} doubled bet {hand.HandBet}{Currency} to {hand.HandBet * 2}{Currency}");
            Player.Balance -= hand.HandBet;
            Host.Balance -= hand.HandBet;
            hand.HandBet = hand.HandBet * 2;
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
