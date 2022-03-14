using UnityEngine;

[CreateAssetMenu(fileName = "MovingState", menuName = "GameKnifeStates/MovingState", order = 0)]
public class MovingState : State
{
    public Vector2 target { get; set; }

    public override void Init()
    {
        base.Init();

        if (_gameKnife.m_Transform.position == _gameKnife.CenterPosition)
            target = _gameKnife.finalPoint.position;
        else
            target = _gameKnife.CenterPosition;
    }

    public override void Update()
    {
        _gameKnife.MoveTo(target);
    }
}
