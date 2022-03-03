using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    [SerializeField] private SettingsManager settings;
    [SerializeField] private KnifeShop knifeShop;

    public GameManager gameManager;

    private void Start()
    {
        if (PlayerPrefsSafe.GetInt("IsGameLaunchedYet") != 1)
            InitOnFirsLaunch();

        settings.Initialize();
        knifeShop.Initialize();

        SaveManager.LoadData();

        gameManager.coinsText.text = GameManager.coins.ToString();
        gameManager.nowCostMoneyText.text = GameManager.nowCostMoneyUpgrade.ToString();
        gameManager.nowCostKnifeText.text = GameManager.nowCostKnifeUpgrade.ToString();

        gameManager.bestScoreText.text = PlayerPrefsSafe.GetInt("BestScore").ToString();
    }


    private void InitOnFirsLaunch()
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
}
