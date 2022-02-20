using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSceneKnivesManager : MonoBehaviour
{

    public float delay = 3;

    public GameObject[] knives, knifeSpwans;

    public int rotateSpeed, xForce = 250, yForce = 500;

    public float timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        timer = 1f;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void FixedUpdate()
    {
        if (timer <= 0)
        {
            int randomSpawnID = Random.Range(0, knifeSpwans.Length);

            Vector2 spawnPos = new Vector2(knifeSpwans[randomSpawnID].transform.position.x, Random.Range(-3.25f, -0.75f));

            GameObject newKnife = Instantiate(knives[Random.Range(0, knives.Length)], spawnPos, Quaternion.Euler(0, 0, Random.Range(0f, 360f)));

            newKnife.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;

            float randomScale = Random.Range(newKnife.transform.localScale.x * 0.9f, newKnife.transform.localScale.x * 1.1f);

            newKnife.transform.localScale = new Vector3(randomScale, randomScale, randomScale);

            xForce = Random.Range(200, 250);
            yForce = Random.Range(350, 500);

            if (randomSpawnID == 0)
                newKnife.GetComponent<Rigidbody2D>().AddForce(new Vector2(xForce, yForce));
            else
                newKnife.GetComponent<Rigidbody2D>().AddForce(new Vector2(-xForce, yForce));

            newKnife.AddComponent<RotateKnife>();

            newKnife.GetComponent<RotateKnife>().rotateSpeed = Random.Range(250, 300);

            timer = Random.Range(delay - 1, delay + 1);

            Destroy(newKnife, 1.5f);
        }
        else timer -= Time.deltaTime;


    }
}
