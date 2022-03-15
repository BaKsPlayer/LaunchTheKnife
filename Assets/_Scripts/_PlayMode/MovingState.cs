using UnityEngine;

[CreateAssetMenu(fileName = "MovingState", menuName = "GameKnifeStates/MovingState", order = 0)]
public class MovingState : State
{
    private Vector2 target;
    private bool isMovingToFinalPoint;

    [SerializeField] private State standingState;

    public override void Init()
    {
        base.Init();

        isMovingToFinalPoint = _gameKnife.Position == _gameKnife.CenterPosition;

        if (isMovingToFinalPoint)
            target = _gameKnife.finalPoint.position;
        else
            target = _gameKnife.CenterPosition;
    }

    public override void Update()
    {
        _gameKnife.Rotate();
        _gameKnife.MoveTo(target);

        if (_gameKnife.Position == target)
        {
            if (isMovingToFinalPoint)
                _gameKnife.LoseKnife();
            else
                _gameKnife.SetState(standingState);
        }
    }
}
