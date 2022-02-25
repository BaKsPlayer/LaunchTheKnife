using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KnifeButton : MonoBehaviour
{
    public int id, publicCost, indexInNonPurchased = -1;
    
    public bool isLocked;

    private SafeInt price;

    public int Price
    {
        get { return price; }
    }

    [SerializeField] private KnifeShop knifeShop;

    [SerializeField] private Image knifeIcon;

    [SerializeField] private Sprite colorSprite;
    [SerializeField] private Sprite nonColorSprite;

    public Image KnifeIcon => knifeIcon;

    private void Awake()
    {
        price = publicCost;

        knifeIcon = transform.GetChild(0).GetComponent<Image>();
    }

    public void Initialize()
    {
        SetDeselectedButtonPanel();
        SetDeselectedKnifeIcon();

        if (PlayerPrefsSafe.GetInt("KnifeLvl_" + id) == 1)
            knifeIcon.color = knifeShop.PurchasedKnifeColor;
        else
        {
            knifeIcon.color = knifeShop.NonPurchasedKnifeColor;

            knifeShop.nonPurchasedKnives.Add(this);
        }
    }

    public void Select()
    {
        if (isLocked)
        {
            GetComponent<Animation>()?.Play();
            VibrationManager.instance.Vibrate(VibrationType.Error);

            return;
        }

        knifeShop.SelectKnife(this);

        SetSelectedButtonPanel();

        knifeShop.SetShowKnife(this);

    }

    private void SetSelectedButtonPanel()
    {
        GetComponent<Image>().sprite = knifeShop.selectedPanel;
        GetComponent<RectTransform>().localScale = new Vector2(1.1f, 1.1f);
    }

    public void SetDeselectedButtonPanel()
    {
        GetComponent<Image>().sprite = knifeShop.unselectedPanel;
        GetComponent<RectTransform>().localScale = new Vector2(1f, 1f);
    }

    public void SetSelectedKnifeIcon()
    {
        knifeIcon.sprite = colorSprite;
        PlayerPrefsSafe.SetInt("NowKnifeSkin", id);
    }

    public void SetDeselectedKnifeIcon()
    {
        knifeIcon.sprite = nonColorSprite;
    }

}


