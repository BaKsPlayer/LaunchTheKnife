using System.Collections.Generic;
using UnityEngine;

public class MainSceneKnifeSpawner : MonoBehaviour
{
    [SerializeField] private Range<float> rotateSpeed;
    [SerializeField] private Range<float> delay;

    [SerializeField] private Range<float> xForce;
    [SerializeField] private Range<float> yForce;

    [SerializeField] private Transform[] knifeSpawnPostions;
    [SerializeField] private List<GameObject> knivesPool = new List<GameObject>();

    [SerializeField] private GameObject knifePrefab;

    [SerializeField] private Sprite[] knifeSprites;

    private float _timer = 0;

    private void Start()
    {
        _timer = 1f;
    }

    private void Update()
    {
        _timer -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if (_timer <= 0)
        {
            GameObject knife = GetKnife();

            LaunchKnife(knife);

            _timer = Random.Range(delay.min, delay.max);
        }
    }

    private GameObject GetKnife()
    {
        foreach (var knife in knivesPool)
        {
            if (!knife.activeSelf)
                return knife;
        }

        return Instantiate(knifePrefab);
    }

    private void LaunchKnife(GameObject knife)
    {
        int spawnIndex = Random.Range(0, knifeSpawnPostions.Length);

        SetKnifeParams(knife, spawnIndex);

        knife.SetActive(true);

        float randomForceX = Random.Range(xForce.min, xForce.max);
        float randomForceY = Random.Range(yForce.min, yForce.max);

        int xDir = spawnIndex == 0 ? 1 : -1;
        knife.GetComponent<Rigidbody2D>().AddForce(new Vector2(randomForceX * xDir, randomForceY));
    }

    private void SetKnifeParams(GameObject knife, int spawnIndex)
    {
        knife.GetComponent<KnifeRotator>().SetRandomRotateSpeed(rotateSpeed.min, rotateSpeed.max);

        float randomY = Random.Range(knifeSpawnPostions[spawnIndex].position.y, knifeSpawnPostions[spawnIndex].position.y + 2.5f);
        knife.transform.position = new Vector2(knifeSpawnPostions[spawnIndex].position.x, randomY);

        knife.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));

        float randomScale = Random.Range(knifePrefab.transform.localScale.x * 0.9f, knifePrefab.transform.localScale.x * 1.1f);
        knife.transform.localScale = new Vector3(randomScale, randomScale, randomScale);

        int knifeSpriteIndex = Random.Range(0, knifeSprites.Length);
        knife.GetComponent<SpriteRenderer>().sprite = knifeSprites[knifeSpriteIndex];
    }
}
