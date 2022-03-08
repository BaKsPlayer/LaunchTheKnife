using System;
using UnityEngine;
using UnityEngine.UI;

public class Gift : MonoBehaviour
{
    [SerializeField] private int value;

    [SerializeField] public GameObject giftOverlay;
    [SerializeField] public Text giftText;

    [SerializeField] private Text remainingTimeText;

    [SerializeField] private CoinsFiller coinsFiller;

    [SerializeField] private Image buttonImage;

    [SerializeField] private Sprite activeGiftSprite;
    [SerializeField] private Sprite inactiveGiftSprite;

    private bool isGiftReady;

    private SafeInt remainingTime;
    private float timer;

    private DateTime lastEnabled;

    private void Awake()
    {
        lastEnabled = DateTime.Now;
    }

    private void OnEnable()
    {
        TimeSpan ts = DateTime.Now - lastEnabled;
        int passedSeconds = (int)ts.TotalSeconds;

        remainingTime -= passedSeconds;
    }

    private void OnDisable()
    {
        lastEnabled = DateTime.Now;
    }

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        if (PlayerPrefs.HasKey("LastSession"))
        {
            TimeSpan ts = DateTime.Now - DateTime.Parse(PlayerPrefs.GetString("LastSession"));
            int passedSeconds = (int)ts.TotalSeconds;

            remainingTime = PlayerPrefsSafe.GetInt("RemainingTimeToGift") - passedSeconds;
        }
        else
            remainingTime = 600;

        RemainingTimeChanged();

        SaveManager.Instance.OnSaveData += SaveData;
    }

    private void Update()
    {
        if (isGiftReady)
            return;

        if (timer > 0)
            timer -= Time.deltaTime;
        else
        {
            remainingTime -= 1;
            RemainingTimeChanged();

            timer = 1;
        }
    }


    public void GetGift()
    {
        if (isGiftReady)
        {
            StartCoroutine(coinsFiller.FillCoins(Wallet.Instance.Coins, Wallet.Instance.Coins + value, 1.5f));
            Invoke("CloseGiftOverlayWithDelay", 1.5f);

            giftText.text = $"+{value} ";
            giftText.transform.parent.gameObject.SetActive(true);
            giftOverlay.GetComponent<Animation>().Play();

            Wallet.Instance.AddCoins(value);

            remainingTime = 1800;
            RemainingTimeChanged();

            VibrationManager.Instance.Vibrate(VibrationType.Success);
        }
    }


    private void RemainingTimeChanged()
    {
        isGiftReady = remainingTime <= 0;
        buttonImage.sprite = isGiftReady ? activeGiftSprite : inactiveGiftSprite;

        if (isGiftReady)
            remainingTimeText.text = "READY!";
        else
            remainingTimeText.text = remainingTime.ToTimeSpan().ToString("mm':'ss");

    }

    void CloseGiftOverlayWithDelay()
    {
        giftOverlay.SetActive(false);
    }

    private void SaveData()
    {
        PlayerPrefsSafe.SetInt("RemainingTimeToGift", remainingTime);
    }

    private void OnDestroy()
    {
        SaveManager.Instance.OnSaveData -= SaveData;
    }

}
