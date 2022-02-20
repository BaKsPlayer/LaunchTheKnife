using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateKnife : MonoBehaviour
{
    int dir = 0;
    public float rotateSpeed = 250;

    // Start is called before the first frame update
    void Start()
    {
        if (Random.Range(0, 2) == 0)
            dir = -1;
        else
            dir = 1;
    }


    private void FixedUpdate()
    {
        transform.Rotate(Vector3.forward * rotateSpeed * Time.deltaTime * dir);

            
    }
}
