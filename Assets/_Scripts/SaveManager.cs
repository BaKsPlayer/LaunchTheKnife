using System;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager
{
    [SerializeField] private List<KnifeImprover> knifeImprovers = new List<KnifeImprover>();

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

        foreach (var knifeImprover in knifeImprovers)
        {
            PlayerPrefsSafe.SetInt(knifeImprover.ImprovementType + "LVL", knifeImprover.Lvl);
        }

        //PlayerPrefsSafe.SetInt("TimeToGift", GetComponent<GiftManager>().timeToGift);

        PlayerPrefs.SetString("LastSession", DateTime.Now.ToString());

        Debug.Log("Data Saved");

    }

    public void LoadData()
    {
        Wallet.Instance.AddCoins(PlayerPrefsSafe.GetInt("Coins"));

        Debug.Log("Data Loaded");
    }

    public void AddKnifeImprover(KnifeImprover knifeImprover)
    {
        knifeImprovers.Add(knifeImprover);
    }
}
