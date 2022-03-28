using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameKnife gameKnife;
    [SerializeField] private Target target;

    [SerializeField] private GameObject inGameHUD;
    [SerializeField] private GameObject mainSceneHUD;

    [SerializeField] private LoseMenuManager loseMenu;

    [SerializeField] private GameObject startGameOverlay;
    [SerializeField] private GameObject knifeSpawner;

    public UnityAction OnGameStarted;
    public UnityAction OnTrainingCompleted;

    private bool isGameRestarting;

    public float CurrentTimeScale { get; private set; } = 1;

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
            target.OnHit += CompleteTraining;
        
        yield return new WaitForSeconds(1.15f);

        startGameOverlay.SetActive(false);
        OnGameStarted?.Invoke();
    }

    private void CompleteTraining()
    {
        PlayerPrefs.SetString("IsTrainingComplete", "YES");
        target.OnHit -= CompleteTraining;

        OnTrainingCompleted?.Invoke();
    }

    public void PauseGame()
    {
        CurrentTimeScale = 0;
        Time.timeScale = CurrentTimeScale;
    }

    public void UnpauseGame()
    {
        CurrentTimeScale = 1;
        Time.timeScale = CurrentTimeScale;
    }
}
