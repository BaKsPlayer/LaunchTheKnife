using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTargetScript : MonoBehaviour
{

    public GameObject nowKnife, target;

    public LayerMask toHit;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            //nowKnife.GetComponent<BoxCollider2D>().enabled = true;

            //isFly = true;

            RaycastHit2D hit = Physics2D.Raycast(nowKnife.transform.position, nowKnife.transform.GetChild(0).position - nowKnife.transform.position, 100, toHit);

            //print(hit.transform);

            //if (hit.transform != null)
            //{

            if (!hit)
                return;

                if (hit.collider.tag == "Target")
                    print("(" + hit.point.x + ", " + hit.point.y + "), Distance - " + Vector2.Distance(new Vector2(hit.point.x, hit.point.y), target.transform.position));
                else if (hit.collider.tag == "TargetBack")
                    print("Ты не туда попал! Не лезьте блять в хипхоп!");

            //}
            //else Debug.Log("NULL");
        }
    }
}
