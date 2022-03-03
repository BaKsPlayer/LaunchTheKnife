using UnityEngine;
using UnityEngine.UI;
using ButtonParameters;

public class KnifeButton : MonoBehaviour
{
    [SerializeField] private bool isLocked;

    [SerializeField] private int id;
    public int Id => id;

    [SerializeField] private int price;
    private SafeInt _price;
    public int Price => _price;

    [SerializeField] private Image knifeIcon;
    public Image KnifeIcon => knifeIcon;

    [SerializeField] private KnifeShop knifeShop;
    [SerializeField] private KnifeButtonParameters buttonParams;

    [SerializeField] private Sprite colorSprite;
    [SerializeField] private Sprite nonColorSprite;

    private void Awake()
    {
        _price = price;

        knifeIcon = transform.GetChild(0).GetComponent<Image>();
    }

    public void Initialize()
    {
        SetDeselectedButtonPanel();
        SetDeselectedKnifeIcon();

        if (PlayerPrefsSafe.GetInt("KnifeLvl_" + Id) == 1)
            knifeIcon.color = buttonParams.PurchasedKnifeColor;
        else
        {
            knifeIcon.color = buttonParams.NonPurchasedKnifeColor;

            knifeShop.AddNonPurchasedKnife(this);
            
        }
    }

    public void Select()
    {
        if (isLocked)
        {
            GetComponent<Animation>()?.Play();
            VibrationManager.Instance.Vibrate(VibrationType.Error);

            return;
        }

        if (PlayerPrefsSafe.GetInt("KnifeLvl_" + Id) == 1)
            VibrationManager.Instance.Vibrate(VibrationType.Medium);
        else
            VibrationManager.Instance.Vibrate(VibrationType.Light);

        knifeShop.SelectKnife(this);

        if (knifeShop.isGetingRandomKnife)
            SetSelectedButtonPanel(buttonParams.RandomButtonSprite);
        else
            SetSelectedButtonPanel(buttonParams.SelectedButtonSprite);

        knifeShop.DemoKnife.SetKnifeImage(this);
    }

    private void SetSelectedButtonPanel(Sprite selectedPanel)
    {
        GetComponent<Image>().sprite = selectedPanel;
        GetComponent<RectTransform>().localScale = new Vector2(1.1f, 1.1f);
    }

    public void SetDeselectedButtonPanel()
    {
        GetComponent<Image>().sprite = buttonParams.UnselectedButtonSprite;
        GetComponent<RectTransform>().localScale = new Vector2(1f, 1f);
    }

    public void SetSelectedKnifeIcon()
    {
        knifeIcon.sprite = colorSprite;
    }

    public void SetDeselectedKnifeIcon()
    {
        knifeIcon.sprite = nonColorSprite;
    }

}


