using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FlyingState", menuName = "GameKnifeStates/FlyingState", order = 2)]
public class FlyingState : State
{
    [SerializeField] private State rewardingState;

    [SerializeField] private Vector2 border; 

    private RaycastHit2D hit;

    private bool isHitBackSide;

    public override void Init()
    {
        base.Init();

        hit = _gameKnife.Hit;

        if (hit && hit.collider.CompareTag("TargetBack"))
            isHitBackSide = false;
    }

    public override void Update()
    {
        _gameKnife.Fly();

        if (IsKnifeAbroad())
            _gameKnife.LoseKnife();

        if (!hit)
            return;

        if (Vector2.Distance(_gameKnife.m_Transform.position, hit.transform.position) < 0.3f)
        {
            if (isHitBackSide)
                _gameKnife.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            else
                _gameKnife.SetState(rewardingState);
        }
      
    }

    private bool IsKnifeAbroad()
    {
        return Mathf.Abs(_gameKnife.Position.x) > border.x || Mathf.Abs(_gameKnife.Position.y) > border.y;
    }
}
