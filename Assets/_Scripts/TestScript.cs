using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{

    public GameObject knife, target, testPoint;

    public LayerMask toHit;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.L))
        {
            RaycastHit2D hit = Physics2D.Raycast(knife.transform.position, knife.transform.GetChild(0).position - knife.transform.position, 100, toHit);

            print("(" + hit.point.x + ", " + hit.point.y + "), Distance - " + Vector2.Distance(new Vector2(hit.point.x, hit.point.y), target.transform.position));

            if (Vector2.Distance(new Vector2(hit.point.x, hit.point.y), target.transform.position) <= 0.12f)
                Debug.Log("YELLOW");
            else if (Vector2.Distance(new Vector2(hit.point.x, hit.point.y), target.transform.position) <= 0.25f)
                Debug.Log("RED");
            else if (Vector2.Distance(new Vector2(hit.point.x, hit.point.y), target.transform.position) <= 0.37f)
                Debug.Log("BLUE");
            else 
                Debug.Log("BLACK");

            print(hit.transform);
        }

    }
}
