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

    [SerializeField] private Text priceText;

    [SerializeField] private Image buttonImage;
    [SerializeField] private Text buttonText;

    [SerializeField] private Sprite activeUpgradeSprite;
    [SerializeField] private Sprite inactiveUpgradeSprite;

    [SerializeField] private Color activeOutlineColor;
    [SerializeField] private Color inactiveOutlineColor;

    private SafeInt startValue;
    private SafeInt improvementValue;

    private SafeInt currentValue;
    public SafeInt CurrentValue => currentValue;

    private SafeInt price;
    private SafeFloat priceMultiplier;

    private SafeInt lvl;
    public SafeInt Lvl => lvl;

    private void Awake()
    {
        Initialize();
    }

    private void OnEnable()
    {
        Wallet.Instance.OnValueChanged += CoinsChanged;
    }

    private void OnDisable()
    {
        Wallet.Instance.OnValueChanged -= CoinsChanged;
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

        lvl = PlayerPrefsSafe.GetInt(ImprovementType + "LVL");
        currentValue = startValue + lvl * improvementValue;

        startPrice = StartPrice;
        price = GetPrice();

        priceText.text = price.ToString();
        buttonText.text = $"x{currentValue}<size=42>+{improvementValue}</size>";

        SaveManager.Instance.OnSaveData += SaveData;
    }

    public void Improve()
    {
        if (Wallet.Instance.Coins < price)
            return;

        lvl++;
        currentValue += improvementValue;
        Wallet.Instance.SpendCoins(price);

        price = GetPrice();
        priceText.text = price.ToString();
        buttonText.text = $"x{currentValue}<size=42>+{improvementValue}</size>";

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
        return (SafeInt)(startPrice * Mathf.Pow(priceMultiplier, lvl));
    }

    private void SaveData()
    {
        PlayerPrefsSafe.SetInt(ImprovementType + "LVL", Lvl);
    }

    private void OnDestroy()
    {
        SaveManager.Instance.OnSaveData -= SaveData;
    }
}
