using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public Text scoreText, bestScoreText, knivesText;

    public Image knivesUpgradeButton, moneyUpgradeButton;

    public Animator itDoesntWork;

    public GameObject settings, knivesShop, loseMenu, mainScene, inGame, startText, startGameOverlay, startScreen;

    public SafeInt score, coinsForSession;


    public Text coinsText, nowCostKnifeText, nowCostMoneyText, moneyUpgradeButtonText, knivesUpgradeButtonText, sessionCoinsText;

    public static SafeInt coins, nowCostKnifeUpgrade, nowCostMoneyUpgrade;


    public Sprite activeUpgrade, inactiveUpgrade;

    public TimeSpan ts;

    VibrationManager vibrator;

    

    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.DeleteAll();

        if (PlayerPrefsSafe.GetInt("IsGameLaunchedYet") != 1)
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

        settings.GetComponent<SettingsManager>().InitSettings();

        knivesShop.GetComponent<KnifeShop>().Initialize();

        coins = PlayerPrefsSafe.GetInt("Coins");
        nowCostMoneyUpgrade = PlayerPrefsSafe.GetInt("NowCostMoneyUpgrade");
        nowCostKnifeUpgrade = PlayerPrefsSafe.GetInt("NowCostKnifeUpgrade");

        coinsText.text = coins.ToString();
        nowCostMoneyText.text = nowCostMoneyUpgrade.ToString();
        nowCostKnifeText.text = nowCostKnifeUpgrade.ToString();

        bestScoreText.text = PlayerPrefsSafe.GetInt("BestScore").ToString();

        score = 0;
        scoreText.text = score.ToString();

        coinsForSession = 0;
        sessionCoinsText.text = coinsForSession.ToString();

        Application.targetFrameRate = 60;


        CloseKnivesShop();

        if (PlayerPrefs.HasKey("LastSession"))
            ts = DateTime.Now - DateTime.Parse(PlayerPrefs.GetString("LastSession"));

        GetComponent<GiftManager>().CheckGift((int)ts.TotalSeconds);

        vibrator = GetComponent<VibrationManager>();

		Destroy(startScreen, 0.6f);

        //CheckUpgrades();
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

    void SaveData()
    {
        PlayerPrefsSafe.SetInt("Coins", coins);
        PlayerPrefsSafe.SetInt("NowCostMoneyUpgrade", nowCostMoneyUpgrade);
        PlayerPrefsSafe.SetInt("NowCostKnifeUpgrade", nowCostKnifeUpgrade);

        PlayerPrefsSafe.SetInt("TimeToGift", GetComponent<GiftManager>().timeToGift);

        PlayerPrefs.SetString("LastSession", DateTime.Now.ToString());
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyUp(KeyCode.C))
        {
            coins += 1000;
            coinsText.text = coins.ToString();
        }


    }



    public void OpenSettings()
    {
        settings.SetActive(true);

        settings.GetComponent<Animator>().SetTrigger("Open");
    }

    public void OpenKnivesShop()
    {
        knivesShop.GetComponent<KnifeShop>().CoinsText.text = coins.ToString();

        knivesShop.GetComponent<KnifeShop>().ResetKnifeShop();

        knivesShop.SetActive(true);
    }

    public void OpenRateUs()
    {
        itDoesntWork.SetTrigger("Show");
    }

    public void OpenLeaderboard()
    {
        itDoesntWork.SetTrigger("Show");
    }

    public void UpgradeKnivesNumber()
    {
        if (coins >= nowCostKnifeUpgrade)
        {
            coins -= nowCostKnifeUpgrade;

            nowCostKnifeUpgrade = (SafeInt)(nowCostKnifeUpgrade * 2f);

            PlayerPrefsSafe.SetInt("KnivesNumber", PlayerPrefsSafe.GetInt("KnivesNumber") + 1);

            coinsText.text = coins.ToString();
            nowCostKnifeText.text = nowCostKnifeUpgrade.ToString();

            CheckUpgrades();

            vibrator.Vibrate(VibrationType.Success);
        }

    }

    public void UpgradeMoneyForTarget()
    {
        if (coins >= nowCostMoneyUpgrade)
        {
            coins -= nowCostMoneyUpgrade;

            nowCostMoneyUpgrade = (SafeInt)(nowCostMoneyUpgrade * 1.1f);

            PlayerPrefsSafe.SetInt("MoneyForTarget", PlayerPrefsSafe.GetInt("MoneyForTarget") + 2);

            coinsText.text = coins.ToString();
            nowCostMoneyText.text = nowCostMoneyUpgrade.ToString();

            CheckUpgrades();

            vibrator.Vibrate(VibrationType.Success);
        }
    }

    public void CheckUpgrades()
    {
        if (coins < nowCostMoneyUpgrade)
        {
            moneyUpgradeButton.sprite = inactiveUpgrade;

            moneyUpgradeButton.transform.GetChild(1).gameObject.SetActive(false);
            moneyUpgradeButton.transform.GetChild(2).gameObject.SetActive(true);

            moneyUpgradeButton.transform.GetChild(2).GetComponent<Text>().text = "x" + PlayerPrefsSafe.GetInt("MoneyForTarget") + "<size=42>+2</size>";
        }
        else
        {
            moneyUpgradeButton.sprite = activeUpgrade;

            moneyUpgradeButton.transform.GetChild(1).gameObject.SetActive(true);
            moneyUpgradeButton.transform.GetChild(2).gameObject.SetActive(false);

            moneyUpgradeButton.transform.GetChild(1).GetComponent<Text>().text = "x" + PlayerPrefsSafe.GetInt("MoneyForTarget") + "<size=42>+2</size>";
        }

        if (coins < nowCostKnifeUpgrade)
        {
            knivesUpgradeButton.sprite = inactiveUpgrade;

            knivesUpgradeButton.transform.GetChild(1).gameObject.SetActive(false);
            knivesUpgradeButton.transform.GetChild(2).gameObject.SetActive(true);

            knivesUpgradeButton.transform.GetChild(2).GetComponent<Text>().text = "x" + PlayerPrefsSafe.GetInt("KnivesNumber") + "<size=42>+1</size>";
        }
        else
        {
            knivesUpgradeButton.sprite = activeUpgrade;

            knivesUpgradeButton.transform.GetChild(1).gameObject.SetActive(true);
            knivesUpgradeButton.transform.GetChild(2).gameObject.SetActive(false);

            knivesUpgradeButton.transform.GetChild(1).GetComponent<Text>().text = "x" + PlayerPrefsSafe.GetInt("KnivesNumber") + "<size=42>+1</size>";
        }
    }

    public void OpenShop()
    {
        itDoesntWork.SetTrigger("Show");
    }

    public void CloseSettings()
    {
        //settings.SetActive(false);

        settings.GetComponent<Animator>().SetTrigger("Close");

        Invoke("CloseSettingsWithDelay", 0.5f);
    }

    public void CloseSettingsWithDelay()
    {
        settings.SetActive(false);

    }

    public void CloseKnivesShop()
    {
        knivesShop.GetComponent<Animator>().SetTrigger("Close");

        Invoke("CloseKnivesShopWithDelay", 0.5f);

        CheckUpgrades();
    }

    public void CloseKnivesShopWithDelay()
    {
        knivesShop.SetActive(false);

    }

    public void OpenLoseMenu()
    {
        GetComponent<PlayerController>().isGameMode = false;


        loseMenu.SetActive(true);
        loseMenu.GetComponent<LoseMenuManager>().SetLoseMenu();


    }

    public void StartGame()
    {
        coinsForSession = 0;
        sessionCoinsText.text = coinsForSession.ToString();

        //mainScene.SetActive(false);
        mainScene.GetComponent<Animator>().SetTrigger("StartGame");
        Invoke("CloseMainSceneWithDelay", 0.7f);

        inGame.SetActive(true);
        startText.SetActive(true);
        startGameOverlay.SetActive(true);

        GetComponent<PlayerController>().nowKnivesNumber = PlayerPrefsSafe.GetInt("KnivesNumber");

        GetComponent<MainSceneKnivesManager>().enabled = false;

        Invoke("StartGameWithDelay", 1.85f);
    }

    public void CloseMainSceneWithDelay()
    {
        mainScene.SetActive(false);

        GetComponent<PlayerController>().target.SetActive(true);
        GetComponent<PlayerController>().RandomTarget();
    }

    public void StartGameWithDelay()
    {
        startText.SetActive(false);

        startGameOverlay.SetActive(false);

        GetComponent<PlayerController>().isGameMode = true;


        GetComponent<PlayerController>().CreateKnife();
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void ContinueGame()
    {
        Time.timeScale = 1;
    }

    public void RestartGame()
    {
        coinsForSession = 0;
        sessionCoinsText.text = coinsForSession.ToString();

        score = 0;
        scoreText.text = score.ToString();

        loseMenu.GetComponent<Animator>().SetTrigger("Close");
        Invoke("CloseLoseMenuWithDelay", 0.7f);
        Invoke("RandomTargetWithDelay", 0.7f);

        inGame.SetActive(false);
        inGame.SetActive(true);
        startText.SetActive(true);
        startGameOverlay.SetActive(true);

        GetComponent<PlayerController>().nowKnivesNumber = PlayerPrefsSafe.GetInt("KnivesNumber");

        Invoke("StartGameWithDelay", 1.85f);

        if (GetComponent<StudyManager>())
            GetComponent<StudyManager>().disabling = false;

    }

    public void RandomTargetWithDelay()
    {
        GetComponent<PlayerController>().RandomTarget();
    }

    public void GoHome()
    {
        coinsForSession = 0;
        sessionCoinsText.text = coinsForSession.ToString();

        score = 0;
        scoreText.text = score.ToString();

        coinsText.text = coins.ToString();

        GetComponent<PlayerController>().target.SetActive(false);

        if (GetComponent<StudyManager>())
            GetComponent<StudyManager>().disabling = false;

        CheckUpgrades();

        inGame.SetActive(false);

        //loseMenu.SetActive(false);

        loseMenu.GetComponent<Animator>().SetTrigger("Close");
        Invoke("CloseLoseMenuWithDelay", 0.4f);

        mainScene.SetActive(true);

        GetComponent<MainSceneKnivesManager>().enabled = true;

        GetComponent<MainSceneKnivesManager>().timer = 1f;
    }

    public void CloseLoseMenuWithDelay()
    {
        loseMenu.SetActive(false);

        loseMenu.GetComponent<LoseMenuManager>().rewardText.SetActive(false);

        //if (isRestart)
        //{
        //    GetComponent<PlayerController>().target.SetActive(true);
        //    GetComponent<PlayerController>().RandomTarget();
        //}
    }
}
