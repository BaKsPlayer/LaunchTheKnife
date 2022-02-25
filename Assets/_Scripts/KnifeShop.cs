using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KnifeShop : MonoBehaviour
{
    KnifeButton activeButton;
    KnifeButton activeKnife;

    [SerializeField] private Color purchasedKnifeColor;
    [SerializeField] private Color nonPurchasedKnifeColor;

    public Color PurchasedKnifeColor => purchasedKnifeColor;
    public Color NonPurchasedKnifeColor => nonPurchasedKnifeColor;

    [SerializeField] private Text unlockPriceText;

    public GameManager gameManager;

    public GameObject showKnife, unlockPanel, zeroButton, unlockRandomKnifeButton, blackOverlay, buttonsOverlay;

    public Text shopCoinsText;

    public Sprite selectedPanel, unselectedPanel, randomPanel;

    public SafeInt randomKnifePrice;
    public int publicRandomKnifeCost;
    public float rotateSpeed;

    public KnifeButton[] allKnives;

    public List<KnifeButton> nonPurchasedKnives = new List<KnifeButton>();


    public Sprite[] knifeSprites, nonColorKnifeSprites;

    VibrationManager vibrator;

    public bool isTimeToVibrate;

    private void Start()
    {
        randomKnifePrice = publicRandomKnifeCost;

        vibrator = gameManager.GetComponent<VibrationManager>();
    }

    private void Update()
    {
        showKnife.transform.Rotate(Vector3.forward * -rotateSpeed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.C))
        {
            GameManager.coins += 1000;
            Debug.Log(GameManager.coins);
        }
    }

    public void SelectKnife(KnifeButton knifeButton)
    {
        activeButton?.SetDeselectedButtonPanel();
        activeButton = knifeButton;

        if (PlayerPrefsSafe.GetInt("KnifeLvl_" + knifeButton.id) == 1)
        {
            SetActivePurchasePanel(false);

            ChangeKnife(knifeButton);

            if (isTimeToVibrate)
                vibrator.Vibrate(VibrationType.Medium);
        }
        else
        {
            SetActivePurchasePanel(true);

            if (isTimeToVibrate)
                vibrator.Vibrate(VibrationType.Light);
        }
    }

    private void ChangeKnife(KnifeButton knifeButton)
    {
        activeKnife?.SetDeselectedKnifeIcon();
        knifeButton.SetSelectedKnifeIcon();
        activeKnife = knifeButton;

        PlayerPrefsSafe.SetInt("NowKnifeSkin", knifeButton.id);
    }

    public void SetShowKnife(KnifeButton knifeButton)
    {
        showKnife.GetComponent<Image>().color = knifeButton.KnifeIcon.color;
        showKnife.GetComponent<Image>().sprite = knifeButton.KnifeIcon.sprite;
    }

    private void SetActivePurchasePanel(bool activate)
    {
        unlockPanel.transform.GetChild(0).gameObject.SetActive(activate);
        unlockPanel.transform.GetChild(1).gameObject.SetActive(!activate);

        unlockPriceText.text = activeButton.Price.ToString();
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

                activeButton = allKnives[i];
                activeKnife = allKnives[i];
            }
        }

        if (nonPurchasedKnives.ToArray().Length == 0)
            unlockRandomKnifeButton.GetComponent<Button>().interactable = false;
    }


    public void BuyActiveButtonKnife()
    {
        if (activeButton.Price <= GameManager.coins)
        {
            PlayerPrefsSafe.SetInt("KnifeLvl_" + activeButton.id, 1);

            GameManager.coins -= activeButton.Price;
            shopCoinsText.text = gameManager.GetComponent<GameManager>().coinsText.text = GameManager.coins.ToString();

            activeButton.Initialize();
            activeButton.Select();

            nonPurchasedKnives.Remove(activeButton);

            if (nonPurchasedKnives.ToArray().Length == 0)
                unlockRandomKnifeButton.GetComponent<Button>().interactable = false;

            vibrator.Vibrate(VibrationType.Success);
        }

    }

    public void BuyRandomKnife()
    {
        if (GameManager.coins >= randomKnifePrice)
        {
            if (nonPurchasedKnives.ToArray().Length >= 2)
                StartCoroutine(RandomKnife(0.1f));
            else
            {
                nonPurchasedKnives[0]?.Select();
                BuyActiveButtonKnife();
            }
        }
    }

    IEnumerator RandomKnife(float startTimeToChange)
    {
        blackOverlay.SetActive(true);
        buttonsOverlay.SetActive(true);

        float changeTimeRange = 0.5f;
        int maxChangesNumber = 16;

        int changesNumber = 0;
        float timeToChange = startTimeToChange + changeTimeRange * ((float)changesNumber / maxChangesNumber);

        int maxKnifeID = nonPurchasedKnives.ToArray().Length - 1;

        int randomID = Random.Range(0, nonPurchasedKnives.ToArray().Length);
        int nowKnifeID = randomID;

        int knifeToUnlock = (randomID + maxChangesNumber) % (maxKnifeID+1);

        activeButton.GetComponent<Image>().sprite = unselectedPanel;
        allKnives[PlayerPrefsSafe.GetInt("NowKnifeSkin")].transform.GetChild(0).GetComponent<Image>().sprite = nonColorKnifeSprites[allKnives[PlayerPrefsSafe.GetInt("NowKnifeSkin")].id];

        PlayerPrefsSafe.SetInt("KnifeLvl_" + nonPurchasedKnives[knifeToUnlock].gameObject.GetComponent<KnifeButton>().id, 1);
        PlayerPrefsSafe.SetInt("NowKnifeSkin", nonPurchasedKnives[knifeToUnlock].gameObject.GetComponent<KnifeButton>().id);

        GameManager.coins -= randomKnifePrice;
        shopCoinsText.text = gameManager.GetComponent<GameManager>().coinsText.text = GameManager.coins.ToString();

        while (changesNumber < maxChangesNumber)
        {
            timeToChange -= Time.deltaTime;

            if (timeToChange <= 0)
            {
                nonPurchasedKnives[nowKnifeID].GetComponent<Image>().sprite = unselectedPanel;

                if (nowKnifeID < maxKnifeID)
                    nowKnifeID++;
                else
                    nowKnifeID = 0;
                
                nonPurchasedKnives[nowKnifeID].GetComponent<Image>().sprite = randomPanel;

                changesNumber++;

                timeToChange = startTimeToChange + changeTimeRange * ((float)changesNumber / maxChangesNumber);

                vibrator.Vibrate(VibrationType.Medium);
            }

            yield return null;
        }

        nonPurchasedKnives[knifeToUnlock].Initialize();
        nonPurchasedKnives[knifeToUnlock].Select();

        nonPurchasedKnives.RemoveAt(knifeToUnlock);

        if (nonPurchasedKnives.ToArray().Length == 0)
            unlockRandomKnifeButton.GetComponent<Button>().interactable = false;

        blackOverlay.SetActive(false);
        buttonsOverlay.SetActive(false);

        vibrator.Vibrate(VibrationType.Success);

    }

    public void CheckKnifeShop()
    {
        activeKnife.Select();

        isTimeToVibrate = true;
    }

    public void Reward50Coins()
    {
        StartCoroutine(shopCoinsText.GetComponent<CoinsFiller>().FillCoins(GameManager.coins, GameManager.coins + 50));

        GameManager.coins += 50;

        gameManager.GetComponent<GameManager>().coinsText.text = GameManager.coins.ToString();
    }


}
