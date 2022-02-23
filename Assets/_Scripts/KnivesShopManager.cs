using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KnivesShopManager : MonoBehaviour
{
    GameObject activeButton;

    public GameManager gameManager;

    public GameObject showKnife, unlockPanel, zeroButton, unlockRandomKnifeButton, blackOverlay, buttonsOverlay;

    public Text shopCoinsText;

    public Sprite selectedPanel, unselectedPanel, randomPanel;

    public SafeInt randomKnifeCost;
    public int publicRandomKnifeCost;
    public float rotateSpeed;

    public KnifeButtonInfo[] allKnives;

    public List<KnifeButtonInfo> nonPurchasedKnives = new List<KnifeButtonInfo>();


    public Sprite[] knifeSprites, nonColorKnifeSprites;

    VibrationManager vibrator;

   public bool isTimeToVibrate;

    // Start is called before the first frame update
    void Start()
    {
        randomKnifeCost = publicRandomKnifeCost;

        activeButton = zeroButton;


        vibrator = gameManager.GetComponent<VibrationManager>();
    }

    public void CheckKnifeShopOnStart()
    {
        for (int i = 0; i < allKnives.Length; i++)
        {
            if (PlayerPrefsSafe.GetInt("KnifeLvl_" + i) != 1)
            {
                nonPurchasedKnives.Add(allKnives[i]);

                allKnives[i].transform.GetChild(0).GetComponent<Image>().sprite = nonColorKnifeSprites[allKnives[i].id];
                allKnives[i].transform.GetChild(0).GetComponent<Image>().color = new Color(0.48f, 0.48f, 0.48f, 0.7f);

                allKnives[i].GetComponent<Image>().sprite = unselectedPanel;
                allKnives[i].GetComponent<RectTransform>().localScale = new Vector2(1f, 1f);
                //allKnives[i].GetComponent<Animator>().SetTrigger("Normal");
            }
            else
            {
                if (i == PlayerPrefsSafe.GetInt("NowKnifeSkin"))
                {
                    allKnives[i].transform.GetChild(0).GetComponent<Image>().sprite = knifeSprites[allKnives[i].id];
                    allKnives[i].transform.GetChild(0).GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);

                    allKnives[i].GetComponent<Image>().sprite = selectedPanel;
                    allKnives[i].GetComponent<RectTransform>().localScale = new Vector2(1.1f, 1.1f);
                    //allKnives[i].GetComponent<Animator>().SetTrigger("Selected");

                    showKnife.GetComponent<Image>().color = new Color(1, 1, 1, 1);
                    showKnife.GetComponent<Image>().sprite = knifeSprites[allKnives[i].id];

                    activeButton = allKnives[i].gameObject;
                }
                else
                {
                    allKnives[i].transform.GetChild(0).GetComponent<Image>().sprite = nonColorKnifeSprites[allKnives[i].id];
                    allKnives[i].transform.GetChild(0).GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);

                    allKnives[i].GetComponent<Image>().sprite = unselectedPanel;
                    allKnives[i].GetComponent<RectTransform>().localScale = new Vector2(1f, 1f);
                    //allKnives[i].GetComponent<Animator>().SetTrigger("Normal");
                }
            }
        }

        if (nonPurchasedKnives.ToArray().Length == 0)
            unlockRandomKnifeButton.GetComponent<Button>().interactable = false;
    }

    // Update is called once per frame
    void Update()
    {
        showKnife.transform.Rotate(Vector3.forward * -rotateSpeed * Time.deltaTime);
    }

    public void OnPressKnifeButton(GameObject button)
    {
        if (button.GetComponent<KnifeButtonInfo>().isLocked)
        {
            if (button.GetComponent<Animation>() != null)
                button.GetComponent<Animation>().Play();

            vibrator.Vibrate(VibrationType.Error);
        }
        else
        {
            activeButton.GetComponent<Image>().sprite = unselectedPanel;
            button.GetComponent<Image>().sprite = selectedPanel;

            activeButton.GetComponent<RectTransform>().localScale = new Vector2(1f, 1f);
            //activeButton.GetComponent<Animator>().SetTrigger("Normal");
            button.GetComponent<RectTransform>().localScale = new Vector2(1.1f, 1.1f);
            //button.GetComponent<Animator>().SetTrigger("Selected");


            if (PlayerPrefsSafe.GetInt("KnifeLvl_" + button.GetComponent<KnifeButtonInfo>().id) == 1)
            {
                allKnives[PlayerPrefsSafe.GetInt("NowKnifeSkin")].transform.GetChild(0).GetComponent<Image>().sprite = nonColorKnifeSprites[allKnives[PlayerPrefsSafe.GetInt("NowKnifeSkin")].id];

                PlayerPrefsSafe.SetInt("NowKnifeSkin", button.GetComponent<KnifeButtonInfo>().id);

                showKnife.GetComponent<Image>().color = new Color(1, 1, 1, 1);
                showKnife.GetComponent<Image>().sprite = knifeSprites[button.GetComponent<KnifeButtonInfo>().id];

                button.transform.GetChild(0).GetComponent<Image>().sprite = knifeSprites[button.GetComponent<KnifeButtonInfo>().id];

                unlockPanel.transform.GetChild(0).gameObject.SetActive(false);
                unlockPanel.transform.GetChild(1).gameObject.SetActive(true);

                if (isTimeToVibrate)
                vibrator.Vibrate(VibrationType.Medium);
            }
            else
            {
                showKnife.GetComponent<Image>().color = new Color(0.48f, 0.48f, 0.48f, 0.7f);
                showKnife.GetComponent<Image>().sprite = nonColorKnifeSprites[button.GetComponent<KnifeButtonInfo>().id];

                unlockPanel.transform.GetChild(0).gameObject.SetActive(true);
                unlockPanel.transform.GetChild(1).gameObject.SetActive(false);

                Text unlockCostText = unlockPanel.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>();
                unlockCostText.text = button.GetComponent<KnifeButtonInfo>().cost.ToString();

                vibrator.Vibrate(VibrationType.Light);
            }


            activeButton = button;

        }

    }

    public void BuyKnife()
    {
        if (activeButton.GetComponent<KnifeButtonInfo>().cost <= GameManager.coins)
        {
            allKnives[PlayerPrefsSafe.GetInt("NowKnifeSkin")].transform.GetChild(0).GetComponent<Image>().sprite = nonColorKnifeSprites[allKnives[PlayerPrefsSafe.GetInt("NowKnifeSkin")].id];

            PlayerPrefsSafe.SetInt("KnifeLvl_" + activeButton.GetComponent<KnifeButtonInfo>().id, 1);
            PlayerPrefsSafe.SetInt("NowKnifeSkin", activeButton.GetComponent<KnifeButtonInfo>().id);

            GameManager.coins -= activeButton.GetComponent<KnifeButtonInfo>().cost;

            showKnife.GetComponent<Image>().color = new Color(1, 1, 1, 1);
            showKnife.GetComponent<Image>().sprite = knifeSprites[activeButton.GetComponent<KnifeButtonInfo>().id];

            activeButton.transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 1);
            activeButton.transform.GetChild(0).GetComponent<Image>().sprite = knifeSprites[activeButton.GetComponent<KnifeButtonInfo>().id];

            shopCoinsText.text = gameManager.GetComponent<GameManager>().coinsText.text = GameManager.coins.ToString();

            unlockPanel.transform.GetChild(0).gameObject.SetActive(false);
            unlockPanel.transform.GetChild(1).gameObject.SetActive(true);

            for (int i=0;i<nonPurchasedKnives.ToArray().Length;i++)
                if (activeButton.GetComponent<KnifeButtonInfo>()==nonPurchasedKnives[i])
                {
                    nonPurchasedKnives.RemoveAt(i);
                    break;
                }

            if (nonPurchasedKnives.ToArray().Length == 0)
                unlockRandomKnifeButton.GetComponent<Button>().interactable = false;

            vibrator.Vibrate(VibrationType.Success);
        }

    }

    public void BuyRandomKnife()
    {
        if (GameManager.coins >= randomKnifeCost)
        {

            if (nonPurchasedKnives.ToArray().Length >= 2)
                StartCoroutine(RandomKnife(0.1f));
            else
            {
                int randomID = Random.Range(0, nonPurchasedKnives.ToArray().Length);

                activeButton.GetComponent<Image>().sprite = unselectedPanel;
                allKnives[PlayerPrefsSafe.GetInt("NowKnifeSkin")].transform.GetChild(0).GetComponent<Image>().sprite = nonColorKnifeSprites[allKnives[PlayerPrefsSafe.GetInt("NowKnifeSkin")].id];

                PlayerPrefsSafe.SetInt("KnifeLvl_" + nonPurchasedKnives[randomID].gameObject.GetComponent<KnifeButtonInfo>().id, 1);
                PlayerPrefsSafe.SetInt("NowKnifeSkin", nonPurchasedKnives[randomID].gameObject.GetComponent<KnifeButtonInfo>().id);

                GameManager.coins -= randomKnifeCost;
                shopCoinsText.text = gameManager.GetComponent<GameManager>().coinsText.text = GameManager.coins.ToString();

                nonPurchasedKnives[randomID].GetComponent<Image>().sprite = selectedPanel;

                activeButton = nonPurchasedKnives[randomID].gameObject;

                showKnife.GetComponent<Image>().color = new Color(1, 1, 1, 1);
                showKnife.GetComponent<Image>().sprite = knifeSprites[activeButton.GetComponent<KnifeButtonInfo>().id];

                activeButton.transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 1);
                activeButton.transform.GetChild(0).GetComponent<Image>().sprite = knifeSprites[activeButton.GetComponent<KnifeButtonInfo>().id];

                unlockPanel.transform.GetChild(0).gameObject.SetActive(false);
                unlockPanel.transform.GetChild(1).gameObject.SetActive(true);

                nonPurchasedKnives.RemoveAt(randomID);

                if (nonPurchasedKnives.ToArray().Length == 0)
                    unlockRandomKnifeButton.GetComponent<Button>().interactable = false;

                vibrator.Vibrate(VibrationType.Success);
            }

        }


    }

    public IEnumerator RandomKnife(float startTimeToChange)
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

        PlayerPrefsSafe.SetInt("KnifeLvl_" + nonPurchasedKnives[knifeToUnlock].gameObject.GetComponent<KnifeButtonInfo>().id, 1);
        PlayerPrefsSafe.SetInt("NowKnifeSkin", nonPurchasedKnives[knifeToUnlock].gameObject.GetComponent<KnifeButtonInfo>().id);

        GameManager.coins -= randomKnifeCost;
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
        nonPurchasedKnives[nowKnifeID].GetComponent<Image>().sprite = unselectedPanel;
        nonPurchasedKnives[knifeToUnlock].GetComponent<Image>().sprite = selectedPanel;

        activeButton = nonPurchasedKnives[knifeToUnlock].gameObject;

        showKnife.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        showKnife.GetComponent<Image>().sprite = knifeSprites[activeButton.GetComponent<KnifeButtonInfo>().id];

        activeButton.transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 1);
        activeButton.transform.GetChild(0).GetComponent<Image>().sprite = knifeSprites[activeButton.GetComponent<KnifeButtonInfo>().id];

        unlockPanel.transform.GetChild(0).gameObject.SetActive(false);
        unlockPanel.transform.GetChild(1).gameObject.SetActive(true);

        nonPurchasedKnives.RemoveAt(knifeToUnlock);

        if (nonPurchasedKnives.ToArray().Length == 0)
            unlockRandomKnifeButton.GetComponent<Button>().interactable = false;

        blackOverlay.SetActive(false);
        buttonsOverlay.SetActive(false);

        vibrator.Vibrate(VibrationType.Success);

    }

    public void CheckKnifeShop()
    {
        //if (activeButton != allKnives[PlayerPrefsSafe.GetInt("NowKnifeSkin")].gameObject)
            OnPressKnifeButton(allKnives[PlayerPrefsSafe.GetInt("NowKnifeSkin")].gameObject);

        isTimeToVibrate = true;
    }

    public void Reward50Coins()
    {
        StartCoroutine(shopCoinsText.GetComponent<CoinsFiller>().FillCoins(GameManager.coins, GameManager.coins + 50));

        GameManager.coins += 50;

        gameManager.GetComponent<GameManager>().coinsText.text = GameManager.coins.ToString();
    }
}
