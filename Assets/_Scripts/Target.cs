using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    [Range(0,100)]
    [SerializeField] private float scaleOffset;

    public event Action OnHit;

    private Vector2 originalScale;

    private Transform _parentTransform;

    private Animator m_Animator;

    private bool isMove;
    private bool isMoveMemory;

    private float moveSpeed;
    private float distance;
    private float range;
    private int dir;

    private void Awake()
    {
        originalScale = transform.localScale;
        _parentTransform = transform.parent;

        m_Animator = _parentTransform.GetComponent<Animator>();
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        if (!isMove)
            return;

        if (distance > 0)
        {
            float f = (distance + 1) / 5;
            float f1 = (range - distance + 1) / 5;

            float speed = moveSpeed * Mathf.Clamp(f, 0.2f, 1f) * Mathf.Clamp(f1, 0.2f, 1f) ;

            distance -= speed * Time.deltaTime;

            _parentTransform.Rotate(Vector3.forward * speed * Time.deltaTime * dir);
        }
        else
        {
            dir = -dir;
            distance = range;
        }

        
    }

    public void Create()
    {
        float rotZ = UnityEngine.Random.Range(-180f, 180f);
        _parentTransform.rotation = Quaternion.Euler(0, 0, rotZ);

        float randomScaleOffset = UnityEngine.Random.Range(-scaleOffset, scaleOffset);
        float offset = 1 - (randomScaleOffset / 100);

        transform.localScale = new Vector2(originalScale.x * offset, originalScale.y * offset);

        if (GameStats.Instance.Score <= 6)
        {
            isMove = false;
            return;
        }

        if (UnityEngine.Random.Range(0,2) == 0)
        {
            isMove = true;
            range = UnityEngine.Random.Range(40f, 60f);
            moveSpeed = 25;

            dir = UnityEngine.Random.Range(0, 2) == 0 ? 1 : -1;

        }
    }

    public void Hit()
    {
        m_Animator.SetTrigger("Hit");
        m_Animator.SetTrigger("Change");

        OnHit?.Invoke();
    }

    public void Stop()
    {
        isMoveMemory = isMove;
        isMove = false;
    }

    public void RestoreMovement()
    {
        isMove = isMoveMemory;
    }
}
