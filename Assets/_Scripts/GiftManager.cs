using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GiftManager : MonoBehaviour
{

    public bool isGiftReady;

    public SafeInt timeToGift;

    float timer;

    public GameObject giftButton, rewardText;

    public Text timerText;

    public CoinsFiller coinsText;

    public Sprite activeGift, inactiveGift;

    VibrationManager vibrator;

    // Start is called before the first frame update
    void Start()
    {
        vibrator = GetComponent<VibrationManager>();
    }

    public void CheckGift(int passedTime)
    {

        if (PlayerPrefs.HasKey("LastSession"))
            timeToGift = PlayerPrefsSafe.GetInt("TimeToGift") - passedTime;
        else
            timeToGift = 600;

        if (timeToGift > 0)
            giftButton.GetComponent<Image>().sprite = inactiveGift;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isGiftReady)
        {
            if (timeToGift > 0)
            {
                if (timer > 0)
                    timer -= Time.deltaTime;
                else
                {
                    timeToGift -= 1;
                    timerText.text = IntToTime(timeToGift);

                    timer = 1;
                }
            }
            else
            {
                isGiftReady = true;

                giftButton.GetComponent<Image>().sprite = activeGift;

                timerText.text = "READY!";
            }
        }


    }


    public void GetGift()
    {
        if (isGiftReady)
        {

            StartCoroutine(coinsText.FillCoins(GameManager.coins, GameManager.coins + 150, 1.5f));
            Invoke("CloseGiftOverlayWithDelay", 1.5f);

            rewardText.transform.GetChild(0).GetComponent<Text>().text = "+" + 150 + " ";
            rewardText.transform.parent.gameObject.SetActive(true);
            rewardText.transform.parent.GetComponent<Animation>().Play();

            GameManager.coins += 150;

            GetComponent<GameManager>().CheckUpgrades();

            giftButton.GetComponent<Image>().sprite = inactiveGift;
            isGiftReady = false;

            timeToGift = 1800;
            timerText.text = IntToTime(timeToGift);

            vibrator.Vibrate(VibrationManager.VibraType.Success);
        }
    }

    void CloseGiftOverlayWithDelay()
    {
        rewardText.transform.parent.gameObject.SetActive(false);
    }

    string IntToTime(int time)
    {
        int minutes = time / 60;
        int seconds = time % 60;

        string s = ":";

        if (seconds >= 10)
            s = s.Insert(1, seconds.ToString());
        else
            s = s.Insert(1, "0" + seconds.ToString());

        if (minutes >= 10)
            s = s.Insert(0, minutes.ToString());
        else
            s = s.Insert(0, "0" + minutes.ToString());

        return s;
    }

}
