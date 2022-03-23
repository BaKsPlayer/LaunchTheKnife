using UnityEngine.Events;

public class Wallet
{
    public int Coins { get; private set; }
    public event UnityAction OnValueChanged;

    private static Wallet instance;
    public static Wallet Instance
    {
        get
        {
            if (instance == null)
                instance = new Wallet();

            return instance;
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
