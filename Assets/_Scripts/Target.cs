using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{

    public bool isTargetMove, isWaiting;

    public float range, startSpeed, delay, startPos;

    float timer = 0, distance;

    public int dir;


    private void Update()
    {
        if (isTargetMove)
        {

            if (isWaiting)
                if (timer <= 0)
                {
                    isWaiting = false;

                    dir = -dir;

                    distance = range;
                }
                else timer -= Time.deltaTime;
            else if (distance > 0)
            {
                //float f = (Mathf.Abs(range/2)-distance)/(range/2);

                //float f = 1 - (Mathf.Abs(distance - (range / 2)) / (range / 2));

                float f = (distance + 1) / 5;
                float f1 = (range - distance + 1) / 5;

                float speed = startSpeed * Mathf.Clamp(f, 0.2f, 1f) * Mathf.Clamp(f1, 0.2f, 1f);


                distance -= speed * Time.deltaTime;

                transform.parent.Rotate(Vector3.forward * speed * Time.deltaTime * dir);
            }
            else if (distance <= 0)
            {
                timer = delay;

                isWaiting = true;
            }

        }
    }

}
