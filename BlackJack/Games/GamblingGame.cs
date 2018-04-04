using BlackJack.Players;
using System;

namespace BlackJack.Games
{
    abstract class GamblingGame
    {
        public string GameName { get; protected set; }
        public string Currency { get; set; }

        public bool Running { get; protected set; }

        public Host Host { get; protected set; }
        public Player Player { get; protected set; }

        public double MinBet { get; protected set; }
        public double MaxBet { get; protected set; }

        public GamblingGame(string gameName, string currency, Host host, Player player)
        {
            Init(gameName, currency, host, player, 20, 1000);
        }

        public GamblingGame(string gameName, string currency, Host host, Player player, double minBet, double maxBet)
        {
            Init(gameName, currency, host, player, minBet, maxBet);
        }

        private void Init(string gameName, string currency, Host host, Player player, double minBet, double maxBet)
        {
            GameName = gameName;
            Currency = currency;
            Host = host;
            Player = player;
            MinBet = minBet;
            MaxBet = maxBet;
            if (player.Balance >= 0)
            {
                Running = true;
            }
            else
            {
                Quit("Account balance can't be negative.");
            }
        }

        protected bool Bet(double amount)
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
                    Console.WriteLine($"{Player.Name} bet {amount}{Currency}");
                    PrintLine();
                    return true;
                }
            }
            PrintLine();
            return false;
        }

        public void Win(double amount, double bet, string prefix, string suffix)
        {
            Player.Wins++;
            Host.Losses++;
            Player.Balance += amount;
            Console.WriteLine((!string.IsNullOrWhiteSpace(prefix) ? prefix + " " : "") +
                $"{Player.Name} won {amount}{Currency} (" + (amount - bet) + $"{Currency} profit)" +
                (!string.IsNullOrWhiteSpace(suffix) ? " " + suffix : ""));
        }

        public void Lose(double amount)
        {
            Player.Losses++;
            Host.Wins++;
            Host.Balance += amount * 2;
            if (Player.Balance <= 0 && (Player.Balance + amount) > 0)
            {
                Player.HaveBeenCleaned = true;
            }
            Console.WriteLine($"{Player.Name} lost {amount}{Currency}");
        }

        public void Push(double amount)
        {
            Player.Ties++;
            Host.Ties++;
            Player.Balance += amount;
            Host.Balance += amount;
            Console.WriteLine($"{Player.Name} got a push of {amount}{Currency}");
        }

        public bool HasEnoughBalance()
        {
            return Player.Balance >= MinBet;
        }

        public void DepositOrQuit()
        {
            if (Player.HaveBeenCleaned && Player.Balance == 0)
            {
                Console.WriteLine("You are out of balance");
            }
            PrintLine();
            string message = Player.HaveBeenCleaned ? "Press 1 to deposit more money and continue playing" : "Press 1 to deposit money and start playing";
            Console.WriteLine(message);
            Console.WriteLine("Press 2 to quit the game");
            PrintLine();
            ConsoleKey key = Console.ReadKey(true).Key; // returns typed key and hides key in console
            if (key == ConsoleKey.D1 || key == ConsoleKey.NumPad1)
            {
                Deposit();
            }
            else if (key == ConsoleKey.D2 || key == ConsoleKey.NumPad2)
            {
                Quit();
            }
            else
            {
                Console.WriteLine("Input was incorrect");
            }
        }

        public bool DecideAction()
        {
            PrintLine();
            Console.WriteLine($"Press 1 to start a new {GameName} round");
            Console.WriteLine("Press 2 to make a money deposit");
            Console.WriteLine("Press 3 to make a money withdraw");
            Console.WriteLine("Press 4 to quit the game");
            PrintLine();
            switch (Console.ReadKey(true).Key) // returns typed key and hides key in console
            {
                case ConsoleKey.D1:
                case ConsoleKey.NumPad1:
                    return true;

                case ConsoleKey.D2:
                case ConsoleKey.NumPad2:
                    Deposit();
                    break;

                case ConsoleKey.D3:
                case ConsoleKey.NumPad3:
                    Withdraw();
                    break;

                case ConsoleKey.D4:
                case ConsoleKey.NumPad4:
                    Quit();
                    break;

                default:
                    Console.WriteLine("Input was incorrect");
                    break;
            }
            return false;
        }

        public void PrintLine()
        {
            Console.WriteLine("------------------------------------------------");
        }

        public void Deposit()
        {
            if (GetInput($"Assign the amount (in {Currency}) that you'd like to deposit to the {GameName} game", out double input))
            {
                Player.Deposit(input, true);
            }
            PrintLine();
        }

        public void Withdraw()
        {
            if (GetInput($"Assign the amount (in {Currency}) that you'd like to withdraw from the {GameName} game", out double input))
            {
                Player.Withdraw(input, true);
            }
            PrintLine();
        }

        public bool GetInput(String message, out double value)
        {
            if (!string.IsNullOrWhiteSpace(message))
            {
                Console.WriteLine(message);
            }
            if (double.TryParse(Console.ReadLine(), out double val))
            {
                value = val;
                return true;
            }
            else
            {
                Console.WriteLine("Input was incorrect");
                value = 0;
                return false;
            }
        }

        public void PressAKey(string message)
        {
            if (!string.IsNullOrWhiteSpace(message))
            {
                Console.WriteLine(message);
            }
            Console.ReadKey(true);
        }

        public void Quit(string message)
        {
            if (!string.IsNullOrWhiteSpace(message))
            {
                Console.WriteLine(message);
            }
            PressAKey("Press a key to exit.");
            Quit();
        }

        public void Quit()
        {
            Running = false;
        }

        public void Sleep(int ms)
        {
            System.Threading.Thread.Sleep(ms);
        }
    }
}
