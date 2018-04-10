namespace BlackJack
{
    class Program 
    {
        // A "Black Jack" console game in C#, written as a small hobby-project november 2017 while attending a C# course.
        // Slightly changed structure 2018-04-04 - structured each class to have its own file and added the "currency" parameter.

        // Neatly OOP-coded, relatively easy to reuse code and add support for other gambling games.

        // Notes & differences in this blackjack game: (compared to "real" blackjack, such as the rules listed at https://www.pagat.com/banking/blackjack.html)
        // * Dealer's balance doesn't increase or decrease when the player deposits/withdraws money
        // * Insurance & surrender functions are not included
        // * When the user doubles/splits, the game allows the max bet to be overridden (maybe not a flaw?)
        // * The user is always paid out 1.5x on blackjack, even if the blackjack is hit later in the game after the second card is drawn (maybe not a flaw?)
        // * If the dealer gets a blackjack on the second card it's not disclosed, the user gets to continue making choices (maybe not a flaw?)
        // * Splitting doesn't work the exact same way as described; 
        //  - can not split different 10-point cards (etc. king and queen)
        //  - dealer doesn't automatically deal second cards
        //  - splitting aces does not disallow taking further cards
        //  - blackjack bonus is still paid out if hitting 21 on split hands 
        //  - can double then split on hands (maybe not a flaw?)

        // Inspect/modify Games.BlackJack.BlackJack.cs to see/change the rules.

        static void Main(string[] args)
        {
            Games.BlackJack.BlackJack blackjack = new Games.BlackJack.BlackJack(); // game without parameters; default rules and values
            blackjack.StartGame(); // starts and plays the game until the user chooses to quit the game through console input
        }

    }
}