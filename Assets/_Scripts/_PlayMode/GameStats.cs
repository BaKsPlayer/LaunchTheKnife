using UnityEngine;
using System;

public class GameStats
{
    public SafeInt CoinsForSession { get; private set; }

    public SafeInt Score { get; private set; }
    public SafeInt BestScore { get; private set; }

    public Action OnCoinsChanged;

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
        OnCoinsChanged?.Invoke();
    }

    public void ResetStats()
    {
        Score = 0;
        CoinsForSession = 0;

        OnCoinsChanged?.Invoke();
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
