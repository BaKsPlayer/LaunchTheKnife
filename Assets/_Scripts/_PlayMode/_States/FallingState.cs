using UnityEngine;

[CreateAssetMenu(fileName = "FallingState", menuName = "GameKnifeStates/FallingState", order = 5)]
public class FallingState : State
{
    public override void Init()
    {
        base.Init();
        _gameKnife.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
    }

    public override void Update()
    {
        if (_gameKnife.IsAbroad)
            _gameKnife.LoseKnife();
    }
}
