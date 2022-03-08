using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    [SerializeField] private SettingsManager settings;
    [SerializeField] private KnifeShop knifeShop;

    public GameManager gameManager;

    private void Start()
    {
        if (PlayerPrefsSafe.GetInt("IsGameLaunchedYet") != 1)
            InitOnFirstLaunch();

        settings.Initialize();
        knifeShop.Initialize();

        SaveManager.Instance.LoadData();

        gameManager.coinsText.text = Wallet.Instance.Coins.ToString();

        gameManager.bestScoreText.text = PlayerPrefsSafe.GetInt("BestScore").ToString();
    }


    private void InitOnFirstLaunch()
    {
        PlayerPrefsSafe.SetInt("IsGameLaunchedYet", 1);

        PlayerPrefsSafe.SetInt("KnifeLvl_" + 0, 1);

        PlayerPrefsSafe.SetInt("KnivesNumber", 1);
        PlayerPrefsSafe.SetInt("NowCostKnifeUpgrade", 100);

        PlayerPrefsSafe.SetInt("MoneyForTarget", 10);
        PlayerPrefsSafe.SetInt("NowCostMoneyUpgrade", 50);

        PlayerPrefsSafe.SetInt("Sound", 0);
        PlayerPrefsSafe.SetInt("Music", 0);
        PlayerPrefsSafe.SetInt("Vibration", 1);
        PlayerPrefsSafe.SetInt("LeftHand", 0);
    }

    private void OnApplicationQuit()
    {
        SaveManager.Instance.SaveData();
    }

#if !UNITY_EDITOR
    private void OnApplicationFocus(bool focus)
    {
        if (!focus)
            SaveManager.Instance.SaveData();
    }
#endif
}
