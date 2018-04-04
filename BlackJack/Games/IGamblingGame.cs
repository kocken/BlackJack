namespace BlackJack.Games
{
    interface IGamblingGame
    {
        void StartGame();
        void NewRound(double betAmount);
    }
}
