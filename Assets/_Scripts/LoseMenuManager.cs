using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoseMenuManager : MonoBehaviour
{
    public GameManager gameManager;

    public Text scoreText, bestScoreText, coinsText, sessionCoinsText, adButtonText;

    public GameObject rewardText;

    public SafeInt adRewardSum;

    public Sprite inactive_AdButton, active_AdButton;

    private void Awake()
    {
        adButtonText.transform.parent.parent.parent.GetComponent<Button>().onClick.AddListener(delegate { gameManager.GetComponent<AdvertManager>().ShowAd(AdvertManager.AdType.AdditionalReward); });
    }

    public void SetLoseMenu()
    {
        scoreText.text = gameManager.score.ToString();
        bestScoreText.text = PlayerPrefsSafe.GetInt("BestScore").ToString();

        sessionCoinsText.text = gameManager.coinsForSession.ToString();

        if (gameManager.coinsForSession > 0)
        {
            rewardText.transform.GetChild(0).GetComponent<Text>().text = "+" + gameManager.coinsForSession.ToString() + " ";

            Invoke("CallMoneyAnim", 0.35f);
            StartCoroutine(coinsText.GetComponent<CoinsFiller>().FillCoins(GameManager.coins, GameManager.coins + gameManager.coinsForSession, 1.35f));
        }

        adRewardSum = (int)Mathf.Clamp(gameManager.coinsForSession / 2, PlayerPrefsSafe.GetInt("MoneyForTarget"), Mathf.Infinity);

        adButtonText.text = "+" + adRewardSum.ToString();

        adButtonText.transform.parent.parent.parent.GetComponent<Button>().interactable = true;
        adButtonText.transform.parent.parent.GetComponent<Image>().sprite = active_AdButton;

        coinsText.text = GameManager.coins.ToString();
        GameManager.coins += gameManager.coinsForSession;

    }

    public void CallMoneyAnim()
    {
        rewardText.SetActive(true);
        rewardText.GetComponent<Animation>().Play();

        
    }



    public void RewardLoseMenu()
    {
        rewardText.transform.GetChild(0).GetComponent<Text>().text = "+" + adRewardSum + " ";
        CallMoneyAnim();
        StartCoroutine(coinsText.GetComponent<CoinsFiller>().FillCoins(GameManager.coins, GameManager.coins + adRewardSum, 1f));

        GameManager.coins += adRewardSum;
        adButtonText.transform.parent.parent.parent.GetComponent<Button>().interactable = false;
        adButtonText.transform.parent.parent.GetComponent<Image>().sprite = inactive_AdButton;
        //adButtonText.transform.parent.parent.GetComponent<Animator>().SetTrigger("Disabled");

        StartCoroutine(sessionCoinsText.GetComponent<CoinsFiller>().FillCoins(gameManager.coinsForSession, gameManager.coinsForSession + adRewardSum));

    }
}
