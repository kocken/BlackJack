namespace BlackJack.Players
{
    class Host : Player
    {
        public Host(string name, Currency currency, double balance) : base(name, currency, balance, false) { }
    }
}
