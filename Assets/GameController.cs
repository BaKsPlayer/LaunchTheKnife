using System.Collections;
using System;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameKnife gameKnife;
    [SerializeField] private Target target;

    [SerializeField] private GameObject inGameHUD;
    [SerializeField] private GameObject mainSceneHUD;

    [SerializeField] private LoseMenuManager loseMenu;

    [SerializeField] private GameObject startGameOverlay;
    [SerializeField] private GameObject knifeSpawner;

    public Action OnGameStarted;

    private bool isGameRestarting;

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.C))
        {
            Wallet.Instance.AddCoins(100);
        }
    }

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

        yield return new WaitForSeconds(1.15f);

        startGameOverlay.SetActive(false);
        OnGameStarted?.Invoke();
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
