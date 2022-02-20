using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    public Image LoadingProgressBar;

    AsyncOperation loadingSceneOperation;






    public bool restartGame;

    public void SwitchToScene(string sceneName)
    {

        loadingSceneOperation = SceneManager.LoadSceneAsync(sceneName);

        // Чтобы сцена не начала переключаться пока играет анимация closing:
        loadingSceneOperation.allowSceneActivation = true;

        LoadingProgressBar.fillAmount = 0;
    }




    // Start is called before the first frame update
    void Awake()
    {

        //#if UNITY_ANDROID
        //versionText.text = Application.version + "(" + PlayerSettings.Android.bundleVersionCode + ")";
        //#else
        //        versionText.text = Application.version + "(" + PlayerSettings.iOS.buildNumber + ")";
        //#endif
        if (restartGame)
            PlayerPrefs.DeleteAll();


        
    }

    private void Start()
    {
        //SwitchToScene("SampleScene");
    }

    // Update is called once per frame
    void Update()
    {


        if (loadingSceneOperation != null)
        {
            //LoadingPercentage.text = Mathf.RoundToInt(loadingSceneOperation.progress * 100) + "%";

            // Просто присвоить прогресс:
            //LoadingProgressBar.fillAmount = loadingSceneOperation.progress; 

            // Присвоить прогресс с быстрой анимацией, чтобы ощущалось плавнее:
            LoadingProgressBar.fillAmount = Mathf.Lerp(LoadingProgressBar.fillAmount, loadingSceneOperation.progress,
                Time.deltaTime * 3f);

            //if ()
        }


    }


}
