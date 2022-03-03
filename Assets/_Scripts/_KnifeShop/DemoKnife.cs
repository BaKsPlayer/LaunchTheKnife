using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DemoKnife : MonoBehaviour
{
    Transform _transform;
    Image _image;

    [SerializeField] float rotateSpeed;

    // Start is called before the first frame update
    void Awake()
    {
        _transform = transform;
        _image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        _transform.Rotate(Vector3.back * rotateSpeed * Time.deltaTime);
    }

    public void SetKnifeImage(KnifeButton knifeButton)
    {
        _image.color = knifeButton.KnifeIcon.color;
        _image.sprite = knifeButton.KnifeIcon.sprite;
    }
}
