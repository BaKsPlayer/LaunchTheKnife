using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KnifeShop : MonoBehaviour
{
    KnifeButton _activeButton;
    KnifeButton _activeKnife;

    [HideInInspector] public bool isGetingRandomKnife { get; private set; }

    [SerializeField] private Text knifePriceText;

    [SerializeField] private Button unlockRandomKnifeButton;

    [SerializeField] private Transform demoKnife;

    [SerializeField] private GameObject blackOverlay;
    [SerializeField] private GameObject buttonsOverlay;

    [SerializeField] private GameObject purchasePanel;
    [SerializeField] private GameObject boughtPanel;

    public GameManager gameManager;

    public Text coinsText;

    private SafeInt randomKnifePrice;
    public int publicRandomKnifeCost;
    public float rotateSpeed;

    [SerializeField] private KnifeButton[] allKnives;

    private List<KnifeButton> nonPurchasedKnives = new List<KnifeButton>();

    public void AddNonPurchasedKnife(KnifeButton knifeButton)
    {
        nonPurchasedKnives.Add(knifeButton);
    }

    private void Start()
    {
        randomKnifePrice = publicRandomKnifeCost;

    }

    private void Update()
    {
        demoKnife.Rotate(Vector3.forward * -rotateSpeed * Time.deltaTime);
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

    public void SetShowKnife(KnifeButton knifeButton)
    {
        demoKnife.GetComponent<Image>().color = knifeButton.KnifeIcon.color;
        demoKnife.GetComponent<Image>().sprite = knifeButton.KnifeIcon.sprite;
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
            unlockRandomKnifeButton.interactable = false;
    }


    public void BuyActiveButtonKnife()
    {
        if (_activeButton.Price <= GameManager.coins || isGetingRandomKnife)
        {
            if (!isGetingRandomKnife)
            {
                GameManager.coins -= _activeButton.Price;
                coinsText.text = gameManager.GetComponent<GameManager>().coinsText.text = GameManager.coins.ToString();
            }

            PlayerPrefsSafe.SetInt("KnifeLvl_" + _activeButton.Id, 1);
            _activeButton.Initialize();
            _activeButton.Select();

            nonPurchasedKnives.Remove(_activeButton);

            if (nonPurchasedKnives.ToArray().Length == 0)
                unlockRandomKnifeButton.interactable = false;

            VibrationManager.instance.Vibrate(VibrationType.Success);
        }

    }

    public void BuyRandomKnife()
    {
        if (GameManager.coins >= randomKnifePrice)
        {
            GameManager.coins -= randomKnifePrice;
            coinsText.text = gameManager.GetComponent<GameManager>().coinsText.text = GameManager.coins.ToString();

            if (nonPurchasedKnives.ToArray().Length >= 2)
                StartCoroutine(RandomKnife());
            else
            {
                isGetingRandomKnife = true;

                nonPurchasedKnives[0]?.Select();
                BuyActiveButtonKnife();

                isGetingRandomKnife = false;

                _activeButton.Select();
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

        BuyActiveButtonKnife();

        isGetingRandomKnife = false;

        _activeButton.Select();

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
        StartCoroutine(coinsText.GetComponent<CoinsFiller>().FillCoins(GameManager.coins, GameManager.coins + 50));

        GameManager.coins += 50;

        gameManager.GetComponent<GameManager>().coinsText.text = GameManager.coins.ToString();
    }


}