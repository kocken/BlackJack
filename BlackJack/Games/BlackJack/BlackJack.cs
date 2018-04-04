using BlackJack.Cards;
using BlackJack.Players;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BlackJack.Games.BlackJack
{
    class BlackJack : CardGamblingGame, IGamblingGame
    {
        // these static variables are the default ones passed to CardGamblingGame when parameters assigned to this BlackJack class doesn't suffice
        private new static string GameName = "Blackjack";
        private new static string Currency = "kr";
        private static string PlayerName = "Player";
        private static string DealerName = "Dealer";
        private static double DealerStartBalance = 10000;

        public bool AllowDoubling { get; set; }
        public int DoubleMinValue { get; set; }
        public int DoubleMaxValue { get; set; }

        public bool AllowSplitting { get; set; }
        public int MaxSplits { get; set; }

        public BlackJack() : base(GameName, Currency, DealerName, DealerStartBalance, PlayerName)
        {
            Init();
        }

        public BlackJack(string currency) : base(GameName, currency, DealerName, DealerStartBalance, PlayerName)
        {
            Init();
        }

        public BlackJack(string currency, string playerName) : base(GameName, currency, DealerName, DealerStartBalance, playerName)
        {
            Init();
        }

        public BlackJack(string currency, string playerName, double deposit) : base(GameName, currency, DealerName, DealerStartBalance, playerName, deposit)
        {
            Init();
        }

        public BlackJack(string currency, string playerName, double deposit, int deckAmount, double minBet, double maxBet) :
            base(GameName, currency, DealerName, DealerStartBalance, playerName, deposit, deckAmount, minBet, maxBet)
        {
            Init();
        }

        public BlackJack(string currency, string playerName, double deposit, int deckAmount, double minBet, double maxBet, string dealerName, double dealerStartBalance) :
            base(GameName, currency, dealerName, dealerStartBalance, playerName, deposit, deckAmount, minBet, maxBet)
        {
            Init();
        }

        public BlackJack(string currency, string playerName, double deposit, int deckAmount, double minBet, double maxBet, string dealerName, double dealerStartBalance,
            bool allowDoubling, bool allowSplitting) :
            base(GameName, currency, dealerName, dealerStartBalance, playerName, deposit, deckAmount, minBet, maxBet)
        {
            Init(allowDoubling, allowSplitting);
        }

        public BlackJack(string currency, string playerName, double deposit, int deckAmount, double minBet, double maxBet, string dealerName, double dealerStartBalance,
            bool allowDoubling, int doubleMinValue, int doubleMaxValue, bool allowSplitting, int maxSplits) :
            base(GameName, currency, dealerName, dealerStartBalance, playerName, deposit, deckAmount, minBet, maxBet)
        {
            Init(allowDoubling, doubleMinValue, doubleMaxValue, allowSplitting, maxSplits);
        }

        private void Init()
        {
            Init(true, true);
        }

        private void Init(bool allowDoubling, bool allowSplitting)
        {
            Init(allowDoubling, 7, 11, allowSplitting, int.MaxValue);
        }

        private void Init(bool allowDoubling, int doubleMinValue, int doubleMaxValue, bool allowSplitting, int maxSplits)
        {
            AllowDoubling = allowDoubling;
            DoubleMinValue = doubleMinValue;
            DoubleMaxValue = doubleMaxValue;
            AllowSplitting = allowSplitting;
            MaxSplits = maxSplits;
        }

        public void StartGame()
        {
            while (Running)
            {
                Console.WriteLine($"{Player.Name}'s balance: {Player.Balance}{Currency}, {Host.Name}'s balance: {Host.Balance}{Currency}");
                if (Player.Wins > 0 || Player.Losses > 0 || Player.Ties > 0)
                {
                    Console.WriteLine("Wins: " + Player.Wins + ", Losses: " + Player.Losses + ", Pushes: " + Player.Ties);
                }
                if (Player.Balance == 0)
                {
                    DepositOrQuit(); // Gives the user a prompt to deposit money or quit the game
                }
                else if (DecideAction()) // Method returns true if the user wants to start a new round. All other actions (deposit, withdraw, quit) are handled by this method.
                {
                    if (HasEnoughBalance()) // If user have the exact or more money required for a min bet
                    {
                        if (GetInput($"You have {Player.Balance}{Currency} available. How much would you like to bet?", out double input))
                        {
                            NewRound(input);
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Not enough balance to start a new round, the minimum bet is {MinBet}{Currency}");
                    }
                }
            }
        }

        public void NewRound(double betAmount)
        {
            BlackJackRoundSettings settings = new BlackJackRoundSettings(AllowDoubling, DoubleMinValue, DoubleMaxValue, AllowSplitting, MaxSplits);
            List<CardHand> playerHands = new List<CardHand>() { new CardHand(betAmount) };
            CardHand dealerHand = new CardHand(betAmount);
            if (Bet(betAmount, playerHands[0]))
            {
                if (CardDeck.Deck.Count != 52)
                {
                    CardDeck.ResetDeck();
                    PrintLine();
                }
                for (int i = 0; i < 2; i++)
                {
                    Hit(Player, false, playerHands[0]);
                    Hit(Host, i == 0, dealerHand);
                }
                for (int hand = 0; playerHands.Count > hand; hand++)
                {
                    while (GetHighestCombo(playerHands[hand].Cards, true) < 21 && DecideAction(settings, playerHands, playerHands[hand], dealerHand))
                    {
                        Hit(Player, false, playerHands[hand]);
                    }
                }
                RevealHiddenCard(Host, dealerHand);
                foreach (CardHand playerHand in playerHands)
                {
                    while (GetHighestCombo(dealerHand.Cards, false) < 17 && GetHighestCombo(playerHand.Cards, true) <= 21)
                    {
                        Hit(Host, false, dealerHand);
                    }
                }
                PrintLine();
                ShowPoints(playerHands, dealerHand, false, true);
                for (int hand = 0; playerHands.Count > hand; hand++)
                {
                    if (playerHands.Count > 1)
                    {
                        Console.WriteLine("Hand " + (hand + 1) + ":");
                    }
                    if (GetHighestCombo(playerHands[hand].Cards, true) > 21 && GetHighestCombo(dealerHand.Cards, true) > 21 ||
                        GetHighestCombo(playerHands[hand].Cards, true) == GetHighestCombo(dealerHand.Cards, true))
                    {
                        Push(playerHands[hand].HandBet);
                    }
                    else if (GetHighestCombo(playerHands[hand].Cards, true) > 21 ||
                        GetHighestCombo(dealerHand.Cards, true) <= 21 && GetHighestCombo(dealerHand.Cards, true) > GetHighestCombo(playerHands[hand].Cards, true))
                    {
                        Lose(playerHands[hand].HandBet);
                    }
                    else
                    {
                        bool blackjack = GetHighestCombo(playerHands[hand].Cards, true) == 21;
                        Win((blackjack ? 3 : 2) * playerHands[hand].HandBet, playerHands[hand].HandBet, "", blackjack ? "on blackjack" : ""); // blackjack = x3 bet payout, otherwise x2
                    }
                }
                PressAKey("Press a key to continue");
                PrintLine();
            }
        }

        public void Hit(Player player, bool hideCard, CardHand hand)
        {
            DrawCard(player.Name, hideCard, hand, 1000, false);
            if (!hideCard)
            {
                int bestCombo = GetHighestCombo(hand.Cards, hideCard);
                if (bestCombo == 21)
                {
                    if (player is Host)
                    {
                        Console.WriteLine("Dealer got blackjack");
                    }
                    else
                    {
                        Console.WriteLine("Blackjack!");
                    }
                    Sleep(1000);
                }
                else if (bestCombo > 21)
                {
                    Console.WriteLine(player.Name + " went over 21");
                    Sleep(1000);
                }
            }
        }

        public void RevealHiddenCard(Player player, CardHand hand)
        {
            foreach (PlayingCard card in hand.Cards)
            {
                if (card.Hidden)
                {
                    card.Hidden = false;
                    Console.WriteLine($"{player.Name}'s hidden card was {card.ToString()}");
                    Sleep(1000);
                    if (GetHighestCombo(hand.Cards, true) == 21)
                    {
                        Console.WriteLine($"{player.Name} had blackjack");
                        Sleep(1000);
                    }
                }
            }
        }

        public bool DecideAction(BlackJackRoundSettings settings, List<CardHand> playerHands, CardHand currentHand, CardHand dealerHand)
        {
            bool doubleAvailable = false;
            bool splitAvailable = false;
            if (playerHands.Count > 1)
            {
                ShowPoints(currentHand, GetHandIndex(playerHands, currentHand) + 1, dealerHand, false);
            }
            else
            {
                ShowPoints(currentHand, -1, dealerHand, false);
            }
            PrintLine();
            Console.WriteLine("Press 1 to hit");
            Console.WriteLine("Press 2 to stay");
            if (currentHand.Cards.Count == 2 && Player.Balance >= currentHand.HandBet && Host.Balance >= currentHand.HandBet)
            {
                int lowestCombo = GetLowestCombo(currentHand.Cards, true); // aces counts as 1 when doubling
                if (settings.AllowDoubling && !settings.Doubled && lowestCombo >= settings.DoubleMinValue && lowestCombo <= settings.DoubleMaxValue)
                {
                    doubleAvailable = true;
                    Console.WriteLine("Press 3 to double");
                }
                if (settings.AllowSplitting && currentHand.Cards[0].Rank == currentHand.Cards[1].Rank && (settings.MaxSplits - settings.Splits) > 0)
                {
                    splitAvailable = true;
                    Console.WriteLine("Press " + (doubleAvailable ? 4 : 3) + " to split");
                }
            }
            PrintLine();
            ConsoleKey key = Console.ReadKey(true).Key; // returns typed key and hides key in console
            if (key == ConsoleKey.D1 || key == ConsoleKey.NumPad1) // draw
            {
                return true;
            }
            else if (key == ConsoleKey.D2 || key == ConsoleKey.NumPad2) // stay
            {
                return false;
            }
            else if ((doubleAvailable || splitAvailable) && (key == ConsoleKey.D3 || key == ConsoleKey.NumPad3)) // double or split
            {
                if (doubleAvailable)
                {
                    DoubleBet(currentHand);
                    settings.Doubled = true;
                }
                else
                {
                    Split(settings, playerHands, currentHand);
                }
            }
            else if (splitAvailable && (key == ConsoleKey.D4 || key == ConsoleKey.NumPad4)) // split
            {
                Split(settings, playerHands, currentHand);
            }
            else
            {
                Console.WriteLine("Input was incorrect");
            }
            return DecideAction(settings, playerHands, currentHand, dealerHand);
        }

        public void Split(BlackJackRoundSettings settings, List<CardHand> playerHands, CardHand currentHand)
        {
            Console.WriteLine($"{Player.Name} splitted hand " +
                $"({currentHand.Cards[0].ToString()} & {currentHand.Cards[1].ToString()}) and bet another {currentHand.HandBet}{Currency}");
            Player.Balance -= currentHand.HandBet;
            Host.Balance -= currentHand.HandBet;
            int index = GetHandIndex(playerHands, currentHand);
            PlayingCard temp = currentHand.RemoveLastCard();
            playerHands.Insert(index + 1, new CardHand(currentHand.HandBet));
            playerHands[index + 1].Cards.Add(temp);
            settings.Splits++;
        }

        public void ShowPoints(List<CardHand> playerHands, CardHand dealerHand, bool aceCombos, bool includeHiddenCards)
        {
            string player = Player.Name + "'s points: ";
            if (aceCombos)
            {
                for (int i = 0; playerHands.Count > i; i++)
                {
                    player += (i > 0 ? ", " : "") + (playerHands.Count > 1 ? "Hand " + (i + 1) + ": " : "");
                    player += string.Join(" or ", GetCardPoints(playerHands[i].Cards, includeHiddenCards).Select(x => x.ToString()).ToArray());
                }
                Console.WriteLine(player + ", " + Host.Name + "'s " + (ContainsHiddenCards(dealerHand, includeHiddenCards) ? "visible " : "") + "points: " +
                    string.Join(" or ", GetCardPoints(dealerHand.Cards, includeHiddenCards).Select(x => x.ToString()).ToArray()));
            }
            else
            {
                for (int i = 0; playerHands.Count > i; i++)
                {
                    player += (i > 0 ? ", " : "") + (playerHands.Count > 1 ? "Hand " + (i + 1) + ": " : "") + GetHighestCombo(playerHands[i].Cards, includeHiddenCards);
                }
                Console.WriteLine(player + ", " + Host.Name + "'s " + (ContainsHiddenCards(dealerHand, includeHiddenCards) ? "visible " : "") + "points: " +
                    GetHighestCombo(dealerHand.Cards, includeHiddenCards));
            }
        }

        public void ShowPoints(CardHand playerHands, int playerHand, CardHand dealerHand, bool includeHiddenCards) // only playerHand values over 1 will show
        {
            Console.WriteLine(Player.Name + "'s " + (playerHand > 0 ? "hand " + playerHand + " " : "") + "points: " + string.Join(" or ",
                GetCardPoints(playerHands.Cards, includeHiddenCards).Select(x => x.ToString()).ToArray()) +
                ", " + Host.Name + "'s " + (ContainsHiddenCards(dealerHand, includeHiddenCards) ? "visible " : "") + "points: " +
                string.Join(" or ", GetCardPoints(dealerHand.Cards, includeHiddenCards).Select(x => x.ToString()).ToArray()));
        }

        public bool ContainsHiddenCards(CardHand hand, bool includeHiddenCards)
        {
            if (!includeHiddenCards)
            {
                foreach (PlayingCard card in hand.Cards)
                {
                    if (card.Hidden)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public int GetHighestCombo(List<PlayingCard> hand, bool includeHiddenCards) // valid value or lowest combo
        {
            int max = 0;
            int[] points = GetCardPoints(hand, includeHiddenCards);
            foreach (int p in points)
            {
                if (p > max)
                {
                    max = p;
                }
            }
            return max;
        }

        public int GetLowestCombo(List<PlayingCard> hand, bool includeHiddenCards)
        {
            int min = int.MaxValue;
            int[] points = GetCardPoints(hand, includeHiddenCards);
            foreach (int p in points)
            {
                if (p < min)
                {
                    min = p;
                }
            }
            return min;
        }

        public int[] GetCardPoints(List<PlayingCard> hand, bool countHiddenPoints) // int[] return because returns ace combos. Only valid values or if above 21, the (single) lowest value.
        {
            int handPoints = 0;
            int aceCards = 0;
            foreach (PlayingCard card in hand)
            {
                if (countHiddenPoints || !card.Hidden)
                {
                    if (int.TryParse(card.Rank, out int intValue))
                    {
                        handPoints += intValue;
                    }
                    else
                    {
                        string value = card.Rank.ToLower();
                        if (value.Equals("jack") || value.Equals("queen") || value.Equals("king"))
                        {
                            handPoints += 10;
                        }
                        else if (value.Equals("ace"))
                        {
                            handPoints += 11;
                            aceCards++;
                        }
                        else
                        {
                            Console.WriteLine("Error: Value " + value + " is unknown.");
                        }
                    }
                }
            }
            if (aceCards == 0)
            {
                return new int[1] { handPoints };
            }
            else
            {
                List<int> values = new List<int>();
                for (int i = 0; aceCards >= i; i++)
                {
                    int algo = (handPoints - ((aceCards - i) * 10));
                    if (algo <= 21)
                    {
                        if (algo == 21)
                        {
                            return new int[1] { algo }; // blackjack - return the single blackjack value even if there are multiple ones
                        }
                        values.Add(algo);
                    }
                }
                if (values.Count == 0) // if no values were added to list = all values invalid (over 21)
                {
                    return new int[1] { handPoints - (aceCards * 10) }; // lowest combination value
                }
                return values.ToArray();
            }
        }
    }
}
