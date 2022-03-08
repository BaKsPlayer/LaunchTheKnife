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
        score = 0;
        scoreText.text = score.ToString();

        coinsForSession = 0;
        sessionCoinsText.text = coinsForSession.ToString();

        Application.targetFrameRate = 60;

        if (PlayerPrefs.HasKey("LastSession"))
            ts = DateTime.Now - DateTime.Parse(PlayerPrefs.GetString("LastSession"));

        GetComponent<GiftManager>().CheckGift((int)ts.TotalSeconds);

        vibrator = GetComponent<VibrationManager>();

		Destroy(startScreen, 0.6f);

        //CheckUpgrades();
    }



    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyUp(KeyCode.C))
        {
            Wallet.Instance.AddCoins(100);
            coinsText.text = Wallet.Instance.Coins.ToString();
        }


    }

    private void OnEnable()
    {
        Wallet.Instance.OnValueChanged += CoinsChanged;
    }

    private void OnDisable()
    {
        Wallet.Instance.OnValueChanged -= CoinsChanged;
    }

    private void CoinsChanged()
    {
        coinsText.text = Wallet.Instance.Coins.ToString();
    }

    public void OpenRateUs()
    {
        itDoesntWork.SetTrigger("Show");
    }

    public void OpenLeaderboard()
    {
        itDoesntWork.SetTrigger("Show");
    }

    public void OpenShop()
    {
        itDoesntWork.SetTrigger("Show");
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

        GetComponent<MainSceneKnifeSpawner>().enabled = false;

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

        //CheckUpgrades();

        inGame.SetActive(false);

        //loseMenu.SetActive(false);

        loseMenu.GetComponent<Animator>().SetTrigger("Close");
        Invoke("CloseLoseMenuWithDelay", 0.4f);

        mainScene.SetActive(true);

        GetComponent<MainSceneKnifeSpawner>().enabled = true;

        //GetComponent<MainSceneKnifeSpawner>()._timer = 1f;
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
