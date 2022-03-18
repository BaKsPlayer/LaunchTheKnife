using UnityEngine;
using System;

public class GameStats
{
    public SafeInt CoinsForSession { get; private set; }

    public SafeInt Score { get; private set; }
    public SafeInt BestScore { get; private set; }

    public Action OnCoinsChanged;
    public Action OnScoreChanged;
    public Action OnBestScoreChanged;

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

    private GameStats()
    {
        BestScore = PlayerPrefsSafe.GetInt("BestScore");
    } 

    public void AddCoins(int amount)
    {
        CoinsForSession += amount;
    }

    public void ResetStats()
    {
        Score = 0;
        OnScoreChanged?.Invoke();

        CoinsForSession = 0;
        OnCoinsChanged?.Invoke();
    }

    public void IncreaseScore()
    {
        Score++;
        OnScoreChanged?.Invoke();

        if (Score > BestScore)
        {
            BestScore = Score;
            OnBestScoreChanged?.Invoke();
        }
    }
}
