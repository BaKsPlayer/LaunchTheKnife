using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class AdvertManager : MonoBehaviour
{
    public GameObject loseMenu, knifeShop, shopButton;

    public enum AdType { AdditionalReward, CoinsReward };

    AdType nowAdType;

    bool isShown = false;

    private void Awake()
    {
        if (Advertisement.isSupported)
        {
#if UNITY_IOS
            Advertisement.Initialize("4389440", false); 
#else
            Advertisement.Initialize("4389441", false);
#endif
        }

        shopButton.GetComponent<Button>().onClick.AddListener(delegate { ShowAd(AdType.CoinsReward); });
    }

    public void ShowAd(AdType adType)
    {
#if UNITY_IOS
        if (Advertisement.IsReady("Rewarded_iOS") && !isShown)
        {
            var options = new ShowOptions { resultCallback = HandleShowResult };
            Advertisement.Show("Rewarded_iOS", options);

            isShown = true;
        }

        nowAdType = adType;
#else
        if (Advertisement.IsReady("Rewarded_Android") && !isShown)
        {
            var options = new ShowOptions { resultCallback = HandleShowResult };
            Advertisement.Show("Rewarded_Android", options);

            isShown = true;
        }

        nowAdType = adType;
#endif
    }

    private void HandleShowResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                Debug.Log("The ad was successfully shown. " + nowAdType);
                if (nowAdType == AdType.AdditionalReward)
                    RewardLoseMenu();
                else if (nowAdType == AdType.CoinsReward)
                    Reward50Coins();

                break;
        }

        isShown = false;

    }

    public void RewardLoseMenu()
    {
        loseMenu.GetComponent<LoseMenuManager>().RewardLoseMenu();
    }

    public void Reward50Coins()
    {
        knifeShop.GetComponent<KnifeShop>().Reward50Coins();
    }

}
