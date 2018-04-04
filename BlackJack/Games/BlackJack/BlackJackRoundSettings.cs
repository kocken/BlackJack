namespace BlackJack.Games.BlackJack
{
    class BlackJackRoundSettings
    {
        public bool AllowDoubling { get; set; }
        public bool Doubled { get; set; }
        public int DoubleMinValue { get; set; }
        public int DoubleMaxValue { get; set; }

        public bool AllowSplitting { get; set; }
        public int Splits { get; set; }
        public int MaxSplits { get; set; }

        public BlackJackRoundSettings(bool allowDoubling, int doubleMinValue, int doubleMaxValue, bool allowSplitting, int maxSplits)
        {
            AllowDoubling = allowDoubling;
            DoubleMinValue = doubleMinValue;
            DoubleMaxValue = doubleMaxValue;
            AllowSplitting = allowSplitting;
            MaxSplits = maxSplits;
        }
    }
}
