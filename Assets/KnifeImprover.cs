using UnityEngine;
using UnityEngine.UI;
using AppsTools;

public class KnifeImprover : MonoBehaviour
{
    enum ImproveType { KnivesNumber, MoneyPerHit}

    [SerializeField] private ImproveType improvementType;

    [SerializeField] private int StartPrice;
    private SafeInt startPrice;

    [SerializeField] private Text priceText;

    private SafeFloat priceMultiplier;

    private SafeInt price;
    private SafeInt lvl;
    private SafeInt improvementValue;
    private SafeInt currentValue;

    [SerializeField] private Image buttonImage;
    [SerializeField] private Text buttonText;

    [SerializeField] private Sprite activeUpgradeSprite;
    [SerializeField] private Sprite inactiveUpgradeSprite;

    [SerializeField] private Color activeTextColor;
    [SerializeField] private Color inactiveTextColor;

    private void Start()
    {
        startPrice = StartPrice;

        Debug.Log(improvementType);

        if (improvementType == ImproveType.KnivesNumber)
        {
            priceMultiplier = 2f;
            improvementValue = 1;

        }
        else
        {
            priceMultiplier = 1.1f;
            improvementValue = 2;
        }

        price = CalculatePrice();

    }

    //private void OnEnable()
    //{
    //    Wallet.Instance.OnValueChanged += CoinsChanged;
    //}

    //private void OnDisable()
    //{
    //    Wallet.Instance.OnValueChanged -= CoinsChanged;
    //}

    public void Improve()
    {
        if (Wallet.Instance.Coins < price)
            return;

        lvl++;
        currentValue += improvementValue;

        Wallet.Instance.SpendCoins(price);
        price = CalculatePrice();

        priceText.text = price.ToString();

        buttonText.text = $"x{currentValue}<size=42>+{improvementValue}</size>";

        VibrationManager.Instance.Vibrate(VibrationType.Success);
    }

    private void CoinsChanged()
    {
        if (Wallet.Instance.Coins < price)
        {
            buttonImage.sprite = inactiveUpgradeSprite;
            buttonText.color = inactiveTextColor;
        }
        else
        {
            buttonImage.sprite = activeUpgradeSprite;
            buttonText.color = inactiveTextColor;
        }
    }

    private SafeInt CalculatePrice()
    {
        return (SafeInt)(startPrice * Mathf.Pow(priceMultiplier, lvl));
    }
}
