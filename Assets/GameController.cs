using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameKnife gameKnife;
    [SerializeField] private Target target;

    [SerializeField] private GameObject inGameHUD;
    [SerializeField] private GameObject mainSceneHUD;

    [SerializeField] private LoseMenuManager loseMenu;

    [SerializeField] private GameObject startGameOverlay;
    [SerializeField] private GameObject knifeSpawner;

    [SerializeField] private GameObject background;
    [SerializeField] private TapHandler tapHandler;

    [Header("Training Prefabs")]
    [SerializeField] private GameObject hintPrefab;
    [SerializeField] private GameObject aimPrefab;

    public Action OnGameStarted;
    public Action OnTrainingCompleted;

    private bool isGameRestarting;

    public static GameController Instance { get; private set; }

    public void StartGame()
    {
        isGameRestarting = false;
        StartCoroutine(BeginGame());
    }

    public void RestartGame()
    {
        isGameRestarting = true;
        StartCoroutine(BeginGame());
    }

    public IEnumerator BeginGame()
    {
        GameStats.Instance.ResetStats();

        if (isGameRestarting)
            loseMenu.Close();
        else
        {
            mainSceneHUD.GetComponent<Animator>().SetTrigger("StartGame");
            knifeSpawner.SetActive(false);
        }
            
        inGameHUD.SetActive(true);
        inGameHUD.GetComponent<Animator>().SetTrigger("Open");

        startGameOverlay.SetActive(true);

        yield return new WaitForSeconds(0.7f);

        mainSceneHUD.SetActive(false);

        target.gameObject.SetActive(true);
        target.Create();

        if (PlayerPrefs.GetString("IsTrainingComplete") != "YES")
            tapHandler.GetComponent<Button>().onClick.AddListener(CompleteTraining);
        
        yield return new WaitForSeconds(1.15f);

        startGameOverlay.SetActive(false);
        OnGameStarted?.Invoke();
    }

    private void CompleteTraining()
    {
        PlayerPrefs.SetString("IsTrainingComplete", "YES");
        tapHandler.GetComponent<Button>().onClick.RemoveListener(CompleteTraining);

        OnTrainingCompleted?.Invoke();
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void UnpauseGame()
    {
        Time.timeScale = 1;
    }
}
