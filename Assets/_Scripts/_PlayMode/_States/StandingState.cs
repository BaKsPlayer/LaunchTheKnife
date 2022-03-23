using UnityEngine;

[CreateAssetMenu(fileName = "StandingState", menuName = "GameKnifeStates/StandingState", order = 1)]
public class StandingState : State
{
    private float leftDegreesToChangeState;
    [SerializeField] private State movingState;

    public override void Init()
    {
        base.Init();

        _gameKnife.OnKnifeReachedCenter?.Invoke();
        leftDegreesToChangeState = 1080f;
    }

    public override void Update()
    {
        _gameKnife.Rotate();
        leftDegreesToChangeState -= Mathf.Abs(_gameKnife.RotateSpeed) * Time.deltaTime;

        if (leftDegreesToChangeState <= 0)
            _gameKnife.SetState(movingState);
    }
}
