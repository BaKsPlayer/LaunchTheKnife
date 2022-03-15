using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardingState : State
{
    [SerializeField] private State movingState;

    private float timer;
    private bool isTargetCreated;

    public override void Init()
    {
        base.Init();

        timer = 0;
        isTargetCreated = false;

        _gameKnife.HitTarget();
    }

    public override void Update()
    {
        timer += Time.deltaTime;

        if (timer >= 0.5f && !isTargetCreated)
        {
            _gameKnife.Target.Create();
            isTargetCreated = true;
        }

        if (timer >= 1f)
        {
            _gameKnife.CalculateCoinsMultiplyer();
            int coinsAmount = Mathf.RoundToInt(_gameKnife.CoinsPerHit * _gameKnife.CoinsMultiplyer);
            GameStats.Instance.AddCoins(coinsAmount);

            GameStats.Instance.IncreaseScore();

            _gameKnife.SetState(movingState);
        }
    }
}
