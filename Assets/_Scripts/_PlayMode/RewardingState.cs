using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardingState : State
{
    [SerializeField] private State movingState;

    private float timer;
    private bool isKnifeCtreated;

    public override void Init()
    {
        base.Init();

        timer = 0;
        isKnifeCtreated = false;

        _gameKnife.HitTarget();
    }

    public override void Update()
    {
        timer += Time.deltaTime;

        if (timer >= 0.5f && !isKnifeCtreated)
        {
            _gameKnife.Target.Create();
            isKnifeCtreated = true;
        }

        if (timer >= 1f)
        {
            _gameKnife.RewardText.transform.parent.gameObject.SetActive(false);

            _gameKnife.CalculateCoinsMultiplyer();
            int coinsAmount = Mathf.RoundToInt(_gameKnife.CoinsPerHit * _gameKnife.CoinsMultiplyer);
            GameStats.Instance.AddCoins(coinsAmount);

            GameStats.Instance.IncreaseScore();

            _gameKnife.SetState(movingState);
        }
    }
}
