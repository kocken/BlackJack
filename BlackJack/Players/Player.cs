﻿using System;

namespace BlackJack.Players
{
    class Player
    {
        public string Name { get; set; }
        public double Balance { get; set; }

        public Currency Currency { get; set; }

        public int Wins { get; set; }
        public int Losses { get; set; }
        public int Ties { get; set; }

        public bool HaveBeenCleaned { get; set; }

        public Player(string name, Currency currency)
        {
            Init(name, currency, 0, false);
        }

        public Player(string name, Currency currency, double balance, bool printMessage)
        {
            Init(name, currency, balance, printMessage);
        }

        private void Init(string name, Currency currency, double balance, bool printMessage) {
            Name = name;
            Currency = currency;
            Balance += balance;
            if (printMessage)
            {
                Console.WriteLine($"Added {balance} {CurrencyUtil.GetCode(Currency)} to {Name}'s account");
            }
        }

        public void Deposit(double amount, bool printMessage)
        {
            if (amount > 0)
            {
                Balance += amount;
                if (printMessage)
                {
                    Console.WriteLine($"{Name} deposited {amount} {CurrencyUtil.GetCode(Currency)}");
                }
            }
            else
            {
                Console.WriteLine($"Must deposit a positive amount above 0 {CurrencyUtil.GetCode(Currency)}");
            }
        }

        public void Withdraw(double amount, bool printMessage)
        {
            if (Balance >= amount)
            {
                if (amount > 0)
                {
                    Balance -= amount;
                    if (printMessage)
                    {
                        Console.WriteLine($"{Name} withdrew {amount} {CurrencyUtil.GetCode(Currency)}");
                    }
                }
                else
                {
                    Console.WriteLine($"Must withdraw a positive amount above 0 {CurrencyUtil.GetCode(Currency)}");
                }
            }
            else
            {
                Console.WriteLine($"Invalid withdraw amount, {amount} {CurrencyUtil.GetCode(Currency)} " +
                    $"is higher than account balance {Balance} {CurrencyUtil.GetCode(Currency)}");
            }
        }
    }
}
