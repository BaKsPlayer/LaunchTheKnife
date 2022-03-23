using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    [SerializeField] private SettingsManager settings;
    [SerializeField] private KnifeShop knifeShop;
    [SerializeField] private GameObject startScreen;

    private void Awake()
    {
        SaveManager.Instance.LoadData();
    }

    private void Start()
    {
        AdvertManager.Instance.Initialize();

        if (PlayerPrefsSafe.GetInt("IsGameLaunchedYet") != 1)
            InitOnFirstLaunch();

        settings.Initialize();
        knifeShop.Initialize();

        Application.targetFrameRate = 60;

        Destroy(startScreen, 0.6f);
    }


    private void InitOnFirstLaunch()
    {
        PlayerPrefsSafe.SetInt("IsGameLaunchedYet", 1);

        PlayerPrefsSafe.SetInt("KnifeLvl_" + 0, 1);

        settings.FirstLaunchInitialize();
    }

    private void OnApplicationQuit()
    {
        SaveManager.Instance.SaveData();
    }

#if !UNITY_EDITOR
    private void OnApplicationFocus(bool focus)
    {
        if (!focus)
            SaveManager.Instance.SaveData();
    }
#endif
}
