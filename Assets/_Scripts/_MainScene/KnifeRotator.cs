using UnityEngine;


public class KnifeRotator : MonoBehaviour
{
    private int _dir = 0;
    private float _rotateSpeed = 250;
    private Transform _transform;

    private void Start()
    {
        _dir = (Random.Range(0, 2) == 0) ? -1 : 1;
        _transform = transform;
    }

    private void FixedUpdate()
    {
        _transform.Rotate(Vector3.forward * _dir * _rotateSpeed * Time.deltaTime);          
    }

    private void OnBecameInvisible()
    {
        gameObject.SetActive(false);
        _transform.position = Vector3.zero;
    }

    public void SetRandomRotateSpeed(float min, float max)
    {
        _rotateSpeed = Random.Range(min, max);
    }
}
