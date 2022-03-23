using System;
using UnityEngine;
using UnityEngine.Events;

public class SaveManager
{
    public event UnityAction OnSaveData;

    private static SaveManager instance;
    public static SaveManager Instance
    {
        get
        {
            if (instance == null)
                instance = new SaveManager();

            return instance;
        }
    }

    public void SaveData()
    {
        PlayerPrefsSafe.SetInt("Coins", Wallet.Instance.Coins);
        PlayerPrefs.SetString("LastSession", DateTime.Now.ToString());

        PlayerPrefsSafe.SetInt("BestScore", GameStats.Instance.BestScore);

        OnSaveData?.Invoke();

        Debug.Log("Data Saved");
    }

    public void LoadData()
    {
        Wallet.Instance.AddCoins(PlayerPrefsSafe.GetInt("Coins"));

        Debug.Log("Data Loaded");
     }

}
