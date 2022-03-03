using System;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static void SaveData()
    {
        PlayerPrefsSafe.SetInt("Coins", GameManager.coins);
        PlayerPrefsSafe.SetInt("NowCostMoneyUpgrade", GameManager.nowCostMoneyUpgrade);
        PlayerPrefsSafe.SetInt("NowCostKnifeUpgrade", GameManager.nowCostKnifeUpgrade);

        //PlayerPrefsSafe.SetInt("TimeToGift", GetComponent<GiftManager>().timeToGift);

        PlayerPrefs.SetString("LastSession", DateTime.Now.ToString());

    }

    public static void LoadData()
    {
        Debug.Log("Data Loaded");

        Wallet.Instance.AddCoins(PlayerPrefsSafe.GetInt("Coins"));

        GameManager.coins = PlayerPrefsSafe.GetInt("Coins");
        GameManager.nowCostMoneyUpgrade = PlayerPrefsSafe.GetInt("NowCostMoneyUpgrade");
        GameManager.nowCostKnifeUpgrade = PlayerPrefsSafe.GetInt("NowCostKnifeUpgrade");
    }

    private void OnApplicationQuit()
    {
        SaveData();
    }

#if !UNITY_EDITOR
    private void OnApplicationFocus(bool focus)
    {
        if (!focus)
            SaveData();
    }
#endif
}
