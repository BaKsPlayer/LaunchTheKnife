public class GameStats
{
    public int CoinsForSession { get; private set; }

    public int Score { get; private set; }
    public int BestScore { get; private set; }


    private static GameStats instance;
    public static GameStats Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameStats();
            }

            return instance;
        }
    }

    public void AddCoins(int amount)
    {
        CoinsForSession += amount;
    }

    public void IncreaseScore()
    {
        Score++;
    }

    public void SetBestScore(int value)
    {
        BestScore = value;
    }

}
