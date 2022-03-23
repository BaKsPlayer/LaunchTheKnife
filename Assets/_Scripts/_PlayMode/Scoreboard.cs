using UnityEngine;
using UnityEngine.UI;

public class Scoreboard : MonoBehaviour
{
    [SerializeField] private GameKnife gameKnife;

    [Space(height:10f)]
    [SerializeField] private Text coinsForSessionText;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text bestScoreText;
    [SerializeField] private Text knivesCountText;

    private void OnEnable()
    {
        GameStats.Instance.OnCoinsChanged += CoinsChanged;
        GameStats.Instance.OnScoreChanged += ScoreChanged;
        GameStats.Instance.OnBestScoreChanged += BestScoreChanged;
        gameKnife.OnKnivesCountChanged += KnivesCountChanged;

        coinsForSessionText.text = GameStats.Instance.CoinsForSession.ToString();
        scoreText.text = GameStats.Instance.Score.ToString();
        bestScoreText.text = GameStats.Instance.BestScore.ToString();
        knivesCountText.text = gameKnife.LeftKnivesCount.ToString();
    }

    private void OnDisable()
    {
        GameStats.Instance.OnCoinsChanged -= CoinsChanged;
        GameStats.Instance.OnScoreChanged -= ScoreChanged;
        GameStats.Instance.OnBestScoreChanged -= BestScoreChanged;

        gameKnife.OnKnivesCountChanged -= KnivesCountChanged;
    }

    private void CoinsChanged()
    {
        coinsForSessionText.text = GameStats.Instance.CoinsForSession.ToString();
    }

    private void ScoreChanged()
    {
        scoreText.text = GameStats.Instance.Score.ToString();
    }

    private void BestScoreChanged()
    {
        bestScoreText.text = GameStats.Instance.BestScore.ToString();
    }

    private void KnivesCountChanged()
    {
        int knivesCount = (int)Mathf.Clamp(gameKnife.LeftKnivesCount, 0, Mathf.Infinity);
        knivesCountText.text = $"x{knivesCount}";
    }


}
