using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DemoKnife : MonoBehaviour
{
    [SerializeField] float rotateSpeed;

    private Transform _transform;
    private Image _image;

    private void Awake()
    {
        _transform = transform;
        _image = GetComponent<Image>();
    }

    private void Update()
    {
        _transform.Rotate(Vector3.back * rotateSpeed * Time.deltaTime);
    }

    public void SetKnifeImage(KnifeButton knifeButton)
    {
        _image.color = knifeButton.KnifeIcon.color;
        _image.sprite = knifeButton.KnifeIcon.sprite;
    }
}
