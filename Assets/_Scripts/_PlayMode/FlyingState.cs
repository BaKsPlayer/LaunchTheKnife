using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FlyingState", menuName = "GameKnifeStates/FlyingState", order = 2)]
public class FlyingState : State
{
    [SerializeField] private State rewardingState;

    private RaycastHit2D hit;

    private bool isHitBackSide;
    private bool knifeInTarget;

    public override void Init()
    {
        base.Init();
        knifeInTarget = false;

        hit = _gameKnife.Hit;

        if (hit && hit.collider.CompareTag("TargetBack"))
            isHitBackSide = false;
    }

    public override void Update()
    {
        if (Vector2.Distance(_gameKnife.m_Transform.position, hit.transform.position) < 0.3f)
        {
            if (isHitBackSide)
            {
                _gameKnife.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            }
            else
            {
                if (!knifeInTarget)
                {
                    Debug.Log("KnifeIntTarget");
                    _gameKnife.SetState(rewardingState);

                    knifeInTarget = true;
                }
            }

            return;
        }

        _gameKnife.Fly();
    }
}
