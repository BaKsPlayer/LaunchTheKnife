using UnityEngine;
using UnityEngine.Advertisements;

public enum AdType { LoseMenuReward, KnifeShopReward };

public class AdvertManager : MonoBehaviour
{
    [SerializeField] private LoseMenuManager loseMenu;
    [SerializeField] private KnifeShop knifeShop;

    private AdType nowAdType;

    private bool isShown = false;

    private string advertID;
    private string gameID;

    public static AdvertManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            if (Instance != this)
                Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
    }

    public void Initialize()
    {
        if (Advertisement.isSupported)
        {
#if UNITY_IOS
            gameID = "4389440";
            advertID = "Rewarded_iOS";
#else
            gameID = "4389441";
            advertID = "Rewarded_Android";
#endif

            Advertisement.Initialize(gameID, false);
        }
    }

    public void ShowAd(AdType adType)
    {
        if (Advertisement.IsReady(advertID) && !isShown)
        {
            var options = new ShowOptions { resultCallback = HandleShowResult };
            Advertisement.Show(advertID, options);

            isShown = true;
        }

        nowAdType = adType;
    }

    private void HandleShowResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                Debug.Log("The ad was successfully shown. " + nowAdType);
                if (nowAdType == AdType.LoseMenuReward)
                    RewardLoseMenu();
                else if (nowAdType == AdType.KnifeShopReward)
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
