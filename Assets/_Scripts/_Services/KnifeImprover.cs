using UnityEngine;
using UnityEngine.UI;
using AppsTools;

public class KnifeImprover : MonoBehaviour
{
    public enum ImproveType { KnivesNumber, MoneyPerHit }

    [SerializeField] private ImproveType improvementType;
    public ImproveType ImprovementType => improvementType;

    [SerializeField] private int StartPrice;
    private SafeInt startPrice;

    [SerializeField] private CoinsFiller coinsTextFiller;
    [SerializeField] private Text priceText;

    [SerializeField] private Image buttonImage;
    [SerializeField] private Text buttonText;

    [SerializeField] private Sprite activeUpgradeSprite;
    [SerializeField] private Sprite inactiveUpgradeSprite;

    [SerializeField] private Color activeOutlineColor;
    [SerializeField] private Color inactiveOutlineColor;

    public SafeInt CurrentValue { get; private set; }
    public SafeInt Lvl { get; private set; }

    private SafeInt startValue;
    private SafeInt improvementValue;

    private SafeInt price;
    private SafeFloat priceMultiplier;

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        if (improvementType == ImproveType.KnivesNumber)
        {
            priceMultiplier = 2f;
            improvementValue = 1;

            startValue = 1;
        }

        if (improvementType == ImproveType.MoneyPerHit)
        {
            priceMultiplier = 1.1f;
            improvementValue = 2;

            startValue = 10;
        }

        Lvl = PlayerPrefsSafe.GetInt(ImprovementType + "Lvl");
        CurrentValue = startValue + Lvl * improvementValue;

        startPrice = StartPrice;
        price = GetPrice();

        priceText.text = price.ToString();
        buttonText.text = $"x{CurrentValue}<size=42>+{improvementValue}</size>";

        SaveManager.Instance.OnSaveData += SaveData;
        Wallet.Instance.OnValueChanged += CoinsChanged;
        CoinsChanged();
    }

    public void Improve()
    {
        if (Wallet.Instance.Coins < price)
            return;

        Lvl++;
        CurrentValue += improvementValue;

        Wallet.Instance.SpendCoins(price);
        coinsTextFiller.Fill(Wallet.Instance.Coins + price, Wallet.Instance.Coins);

        price = GetPrice();
        priceText.text = price.ToString();
        buttonText.text = $"x{CurrentValue}<size=42>+{improvementValue}</size>";

        VibrationManager.Instance.Vibrate(VibrationType.Success);
    }

    private void CoinsChanged()
    {
        int price = GetPrice();

        if (Wallet.Instance.Coins < price)
        {
            buttonImage.sprite = inactiveUpgradeSprite;
            buttonText.GetComponent<ImageSolidColorOutline>().OutlineColor = inactiveOutlineColor;
        }
        else
        {
            buttonImage.sprite = activeUpgradeSprite;
            buttonText.GetComponent<ImageSolidColorOutline>().OutlineColor = activeOutlineColor;
        }

        buttonText.GetComponent<ImageSolidColorOutline>().Refresh();
    }

    private SafeInt GetPrice()
    {
        return (SafeInt)(startPrice * Mathf.Pow(priceMultiplier, Lvl));
    }

    private void SaveData()
    {
        PlayerPrefsSafe.SetInt(ImprovementType + "Lvl", Lvl);
    }

    private void OnDestroy()
    {
        SaveManager.Instance.OnSaveData -= SaveData;
        Wallet.Instance.OnValueChanged -= CoinsChanged;
    }
}
