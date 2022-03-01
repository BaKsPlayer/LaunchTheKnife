using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KnifeShop : MonoBehaviour
{
    KnifeButton _activeButton;
    KnifeButton _activeKnife;
    public bool isGetingRandomKnife { get; private set; }

    [Header("Buy Random Knife")]
    [SerializeField] private int randomKnifePrice;
    private SafeInt _randomKnifePrice;

    [SerializeField] private Button buyRandomKnifeButton;
    [SerializeField] private GameObject blackOverlay;
    [SerializeField] private GameObject buttonsOverlay;

    [Header("Buy Knife")]
    [SerializeField] private Text knifePriceText;
    [SerializeField] private GameObject purchasePanel;
    [SerializeField] private GameObject boughtPanel;

    [Header("Other")]
    [SerializeField] private DemoKnife demoKnife;
    public DemoKnife DemoKnife => demoKnife;

    [SerializeField] private Text coinsText;
    public Text CoinsText => coinsText;

    public GameManager gameManager;

    [SerializeField] private KnifeButton[] allKnives;
    private List<KnifeButton> nonPurchasedKnives = new List<KnifeButton>();

    public void AddNonPurchasedKnife(KnifeButton knifeButton)
    {
        nonPurchasedKnives.Add(knifeButton);
    }

    public void SelectKnife(KnifeButton knifeButton)
    {
        _activeButton?.SetDeselectedButtonPanel();
        _activeButton = knifeButton;

        if (PlayerPrefsSafe.GetInt("KnifeLvl_" + knifeButton.Id) == 1)
        {
            SetActivePurchasePanel(false);
            ChangeKnife(knifeButton);
        }
        else
            SetActivePurchasePanel(true);
    }

    private void ChangeKnife(KnifeButton knifeButton)
    {
        _activeKnife?.SetDeselectedKnifeIcon();
        knifeButton.SetSelectedKnifeIcon();
        _activeKnife = knifeButton;

        PlayerPrefsSafe.SetInt("NowKnifeSkin", knifeButton.Id);
    }

    private void SetActivePurchasePanel(bool activate)
    {
        purchasePanel.SetActive(activate);
        boughtPanel.SetActive(!activate);

        knifePriceText.text = _activeButton.Price.ToString();
    }

    public void Initialize()
    {
        int nowKnifeSkinID = PlayerPrefsSafe.GetInt("NowKnifeSkin");

        _randomKnifePrice = randomKnifePrice;

        for (int i = 0; i < allKnives.Length; i++)
        {
            allKnives[i].Initialize();

            if (i == nowKnifeSkinID)
            {
                allKnives[i].Select();

                _activeButton = allKnives[i];
                _activeKnife = allKnives[i];
            }
        }

        if (nonPurchasedKnives.ToArray().Length == 0)
            buyRandomKnifeButton.interactable = false;
    }


    public void BuyActiveButtonKnife(bool isFree = false)
    {
        if (_activeButton.Price <= GameManager.coins || isFree)
        {
            if (!isFree)
            {
                GameManager.coins -= _activeButton.Price;
                CoinsText.text = gameManager.GetComponent<GameManager>().coinsText.text = GameManager.coins.ToString();
            }

            PlayerPrefsSafe.SetInt("KnifeLvl_" + _activeButton.Id, 1);
            _activeButton.Initialize();
            _activeButton.Select();

            nonPurchasedKnives.Remove(_activeButton);

            if (nonPurchasedKnives.ToArray().Length == 0)
                buyRandomKnifeButton.interactable = false;

            VibrationManager.instance.Vibrate(VibrationType.Success);
        }

    }

    public void BuyRandomKnife()
    {
        if (GameManager.coins >= _randomKnifePrice)
        {
            GameManager.coins -= _randomKnifePrice;
            CoinsText.text = gameManager.GetComponent<GameManager>().coinsText.text = GameManager.coins.ToString();

            if (nonPurchasedKnives.ToArray().Length >= 2)
                StartCoroutine(RandomKnife());
            else
            {
                nonPurchasedKnives[0]?.Select();
                BuyActiveButtonKnife(isFree: true);
            }
        }
    }

    IEnumerator RandomKnife()
    {
        blackOverlay.SetActive(true);
        buttonsOverlay.SetActive(true);
        isGetingRandomKnife = true;

        int totalIterationCount = 17;
        int currentIteration = 0;

        float currentIterationTime = 0;

        int nowKnifeID = Random.Range(0, nonPurchasedKnives.ToArray().Length);

        while (currentIteration < totalIterationCount)
        {
            currentIterationTime -= Time.deltaTime;

            if (currentIterationTime <= 0)
            {
                if (nowKnifeID < nonPurchasedKnives.Count - 1)
                    nowKnifeID++;
                else
                    nowKnifeID = 0;

                nonPurchasedKnives[nowKnifeID].Select();

                currentIteration++;
                currentIterationTime = Mathf.Lerp(0.1f, 0.6f, (float)currentIteration / totalIterationCount);
            }

            yield return null;
        }

        isGetingRandomKnife = false;

        BuyActiveButtonKnife(isFree: true);

        blackOverlay.SetActive(false);
        buttonsOverlay.SetActive(false);
    }

    public void ResetKnifeShop()
    {
        PlayerPrefsSafe.SetInt("Vibration", 0);

        _activeKnife.Select();

        PlayerPrefsSafe.SetInt("Vibration", 1);
    }

    public void Reward50Coins()
    {
        StartCoroutine(CoinsText.GetComponent<CoinsFiller>().FillCoins(GameManager.coins, GameManager.coins + 50));

        GameManager.coins += 50;

        gameManager.GetComponent<GameManager>().coinsText.text = GameManager.coins.ToString();
    }


}