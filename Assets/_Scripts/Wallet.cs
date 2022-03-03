using System;
using UnityEngine;

public class Wallet : MonoBehaviour
{
    public int Coins { get; private set; }

    public event Action OnValueChanged;

    private static Wallet instance;

    public static Wallet Instance
    {
        get
        {
            return instance;
        }

        private set
        {
            instance = value;
        }
    }

    private void Awake()
    {
        if (Instance != null)
        {
            if (Instance != this)
                Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }

    }

    public void AddCoins(int amount)
    {
        Coins += amount;
        OnValueChanged?.Invoke();
    }

    public void SpendCoins(int amount)
    {
        Coins -= amount;
        OnValueChanged?.Invoke();
    }


    
}
