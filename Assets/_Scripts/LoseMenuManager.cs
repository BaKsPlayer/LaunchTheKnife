using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoseMenuManager : MonoBehaviour
{
    [SerializeField] private Text scoreText;
    [SerializeField] private Text bestScoreText;

    [SerializeField] private Text sessionCoinsText;

    [Space(height: 10f)]
    [SerializeField] private Text advertButtonText;
    [SerializeField] private Button advertButton;

    [Space(height: 10f)]
    [SerializeField] private Animation advertTextAnimation;
    [SerializeField] private Text advertText;

    [Space(height: 10f)]
    [SerializeField] private Text rewardText;
    [SerializeField] private Animation rewardTextAnimation;

    [Space(height: 10f)]
    [SerializeField] private GameKnife gameKnife;

    [Header("AdvertButton Sprites")]
    [SerializeField] private Sprite activeAdvertButtonSprite;
    [SerializeField] private Sprite inactiveAdvertButtonSprite;
    

    private SafeInt advertRewardSum;

    private void Start()
    {
        advertButton.onClick.AddListener(delegate { AdvertManager.Instance.ShowAd(AdType.LoseMenuReward); });
        //TODO: Подписать на событие Wallet.Instance.OnValueChanged coinsText
    }

    public void CallRewardTextAnimation()
    {
        rewardText.text = $"+{GameStats.Instance.CoinsForSession} ";

        rewardTextAnimation.gameObject.SetActive(true);
        rewardTextAnimation.Play();
    }

    public void Open()
    {


        Set();
        Activate();
    }

    private void Set()
    {
        scoreText.text = GameStats.Instance.Score.ToString();
        bestScoreText.text = GameStats.Instance.BestScore.ToString();

        sessionCoinsText.text = GameStats.Instance.CoinsForSession.ToString();

        if (GameStats.Instance.CoinsForSession > 0)
            CallRewardTextAnimation();

        advertRewardSum = (int)Mathf.Clamp(GameStats.Instance.CoinsForSession / 2f, gameKnife.CoinsPerHit, Mathf.Infinity);
        advertButtonText.text = $"+{advertRewardSum}";

        SetActiveAdvertButton(true);

        Wallet.Instance.AddCoins(GameStats.Instance.CoinsForSession);

    }

    private void Activate()
    {
        gameObject.SetActive(true);
        GetComponent<Animator>().SetTrigger("Open");
    }

    public void Close()
    {
        

        GetComponent<Animator>().SetTrigger("Close");
        StartCoroutine(Deactivate(0.4f));
    }

    private IEnumerator Deactivate(float delay)
    {
        yield return new WaitForSeconds(delay);

        advertTextAnimation.gameObject.SetActive(false);
        rewardTextAnimation.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    public void RewardLoseMenu()
    {
        CallAdvertTextAnimation();

        Wallet.Instance.AddCoins(advertRewardSum);
        SetActiveAdvertButton(false);

        //TODO: Сделать вызов заполнения coinsForSessionText
        //StartCoroutine(sessionCoinsText.GetComponent<CoinsFiller>().FillCoins(gameManager.coinsForSession, gameManager.coinsForSession + adRewardSum));
    }

    public void CallAdvertTextAnimation()
    {
        advertText.text = $"+{advertRewardSum} ";

        advertTextAnimation.gameObject.SetActive(true);
        advertTextAnimation.Play();

    }

    public void SetActiveAdvertButton(bool value)
    {
        advertButton.interactable = value;

        Sprite buttonSprite = value ? activeAdvertButtonSprite : inactiveAdvertButtonSprite;
        advertButton.transform.GetChild(0).GetComponent<Image>().sprite = buttonSprite;
    } 
}
