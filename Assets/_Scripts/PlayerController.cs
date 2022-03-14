using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    public float flySpeed, rotateSpeed, startSpeed, nowMultiplier;

    public Vector2 targetOriginalScale;

    [Range(0, 100)]
    public float targetScaleOffset;

    public GameObject target, knife, studyAim, rewardText;

    public GameObject[] knifeSpawns, knives;

    [HideInInspector]
    public GameObject nowKnife;

    public float knifeWaitRot, horizontalBorder;

    public bool isFly, isTimeToNewKnife;

    public enum KnifeStatus { toCenter, toEnd, waiting }

    public KnifeStatus knifeStatus;

    public AnimationCurve difficult;

    public SafeInt nowKnivesNumber;

    public bool isGameMode;

    int randomSpawn, randomEnd = 0;

    [HideInInspector]
    public Vector3 center = new Vector3(0, 1.57f);
    //private bool prevIsFly;

    public LayerMask toHit;
    RaycastHit2D hit;

    bool knifeInTarget, knifeInBackSide, isHitBackside, isTargetMoveMemory;

    GameManager gameManager;

    VibrationManager vibrator;

    [Header("Study")]
    public SpriteRenderer background;
    public Text studyTip;
    StudyManager studyManager;

    // Start is called before the first frame update
    void Start()
    {
        
        float nowK = (float)Screen.width / (float)Screen.height;
        float originalK = 1125f / 2435f;

        horizontalBorder = (nowK * 1.7f) / originalK;

        targetOriginalScale = target.transform.localScale;

        nowKnivesNumber = PlayerPrefsSafe.GetInt("KnivesNumber");

        gameManager = GetComponent<GameManager>();

        vibrator = GetComponent<VibrationManager>();

        studyManager = GetComponent<StudyManager>();

        float f = Mathf.InverseLerp(2436f / 1125f, 1920f / 1080f, (float)Screen.height / (float)Screen.width);

        
        target.transform.localPosition = Vector3.Lerp(new Vector3(1.77f, 0, 0), new Vector3(2.1f, 0, 0), f);

        //CreateKnife();
        //RandomTarget();
    }

    // Update is called once per frame
    void Update()
    {

        if (isGameMode)
        {

            if (nowKnife != null)
                if (!isFly)
                {
                    nowKnife.transform.Rotate(Vector3.forward * rotateSpeed * Time.deltaTime);


                    if (knifeStatus == KnifeStatus.toCenter)
                    {
                        float dist1 = Vector2.Distance(nowKnife.transform.position, center);
                        float dist2 = Vector2.Distance(knifeSpawns[randomSpawn].transform.position, center);

                        //float speed = startSpeed * Mathf.Clamp((dist1 / dist2) * 2, 0.1f, 1f);
                        float speed = startSpeed * Mathf.Clamp(dist1 / (dist2 * 0.4f), 0.05f, 1f);


                        nowKnife.transform.position = Vector3.MoveTowards(nowKnife.transform.position, center, speed * Time.deltaTime);

                        if (nowKnife.transform.position == center)
                        {
                            knifeStatus = KnifeStatus.waiting;

                            knifeWaitRot = 1080f;
                        }
                    }
                    else if (knifeStatus == KnifeStatus.waiting)
                    {
                        knifeWaitRot -= Mathf.Abs(rotateSpeed) * Time.deltaTime;

                        if (knifeWaitRot <= 0)
                            knifeStatus = KnifeStatus.toEnd;



                    }
                    else if (knifeStatus == KnifeStatus.toEnd)
                    {
                        float dist1 = Vector2.Distance(nowKnife.transform.position, center);
                        float dist2 = Vector2.Distance(knifeSpawns[randomEnd].transform.position, center);

                        float speed = startSpeed * Mathf.Clamp(dist1 / (dist2 * 0.4f), 0.05f, 1f);

                        nowKnife.transform.position = Vector3.MoveTowards(nowKnife.transform.position, knifeSpawns[randomEnd].transform.position, speed * Time.deltaTime);

                        if (studyManager && !studyManager.disabling)
                        {
                            StartCoroutine(studyManager.DisableStudy());
                        }

                        if (nowKnife.transform.position == knifeSpawns[randomEnd].transform.position)
                        {
                            isTimeToNewKnife = true;
                            isFly = false;

                            vibrator.Vibrate(VibrationType.Error);

                            if (nowKnife != null)
                                Destroy(nowKnife);

                            if (nowKnivesNumber == 0)
                                gameManager.OpenLoseMenu();
                            else
                                CreateKnife();
                        }


                    }

                }
                else
                {

                    if (hit)
                    {
                        if (Vector2.Distance(nowKnife.transform.GetChild(0).position, new Vector2(hit.point.x, hit.point.y)) > 0.3f && !knifeInTarget && !knifeInBackSide)
                            nowKnife.transform.Translate(Vector3.right * flySpeed * Time.deltaTime);
                        else if (isHitBackside)
                        {
                            nowKnife.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;

                            //nowKnife.GetComponent<Rigidbody2D>().AddForce(100)

                            knifeInBackSide = true;
                        }
                        else if (!knifeInTarget)
                        {
                            knifeInTarget = true;
                            Debug.Log("KnifeInTarget");

                            StartCoroutine(OnHit());
                        }
                        else if (knifeInTarget)
                            nowKnife.GetComponent<SpriteRenderer>().color = new Color(nowKnife.GetComponent<SpriteRenderer>().color.r, nowKnife.GetComponent<SpriteRenderer>().color.r, nowKnife.GetComponent<SpriteRenderer>().color.g, target.GetComponent<SpriteRenderer>().color.a);
                    }
                    else nowKnife.transform.Translate(Vector3.right * flySpeed * Time.deltaTime);
                }

            if (nowKnife != null)
                if (nowKnife.transform.position.x < -4 || nowKnife.transform.position.x > 4 || nowKnife.transform.position.y > 5.5f || nowKnife.transform.position.y < -5.5f)
                {
                    isTimeToNewKnife = true;
                    isFly = false;

                    vibrator.Vibrate(VibrationType.Error);

                    if (nowKnife != null)
                        Destroy(nowKnife);

                    if (nowKnivesNumber == 0)
                        gameManager.OpenLoseMenu();
                    else
                        CreateKnife();
                }
        }
    }


    public void LaunchKnife()
    {
        if (!isFly)
        {
            nowKnife.GetComponent<BoxCollider2D>().enabled = true;

            isFly = true;

            vibrator.Vibrate(VibrationType.Light);

            hit = Physics2D.Raycast(nowKnife.transform.position, nowKnife.transform.GetChild(0).position - nowKnife.transform.position, 100, toHit);

            isTargetMoveMemory = target.GetComponent<Target>().isMove;

            target.GetComponent<Target>().isMove = false;

            //print(hit.transform);

            if (hit.transform != null)
            {
                if (hit.collider.tag == "Target")
                {
                    if (Vector2.Distance(new Vector2(hit.point.x, hit.point.y), target.transform.position) <= 0.09f)
                    {
                        nowMultiplier = 2f;

                        //Debug.Log("YELLOW " + PlayerPrefsSafe.GetInt("MoneyForTarget") + "*" + nowMultiplier + "=" + (int)(PlayerPrefsSafe.GetInt("MoneyForTarget") * nowMultiplier));

                    }
                    else if (Vector2.Distance(new Vector2(hit.point.x, hit.point.y), target.transform.position) <= 0.22f)
                    {
                        nowMultiplier = 1.5f;

                        //Debug.Log("RED " + PlayerPrefsSafe.GetInt("MoneyForTarget") + "*" + nowMultiplier + "=" + (int)(PlayerPrefsSafe.GetInt("MoneyForTarget") * nowMultiplier));
                    }
                    else if (Vector2.Distance(new Vector2(hit.point.x, hit.point.y), target.transform.position) <= 0.37f)
                    {
                        nowMultiplier = 1.25f;
                        //Debug.Log("BLUE " + PlayerPrefsSafe.GetInt("MoneyForTarget") + "*" + nowMultiplier + "=" + (int)(PlayerPrefsSafe.GetInt("MoneyForTarget") * nowMultiplier));
                    }
                    else
                    {
                        nowMultiplier = 1f;
                        //Debug.Log("BLACK " + PlayerPrefsSafe.GetInt("MoneyForTarget") + "*" + nowMultiplier + "=" + (int)(PlayerPrefsSafe.GetInt("MoneyForTarget") * nowMultiplier));
                    }

                    target.transform.GetChild(1).GetComponent<SpriteRenderer>().sortingOrder = 3;

                    if (studyManager)
                    {
                        PlayerPrefs.SetString("IsStudyComplete", "YES");
                        StartCoroutine(studyManager.DisableStudy());
                    }
                }
                else if (hit.collider.tag == "TargetBack")
                {
                    isHitBackside = true;

                    print("isHitBackside");

                }

            }
            else Debug.Log("NULL");

            //studyTip.gameObject.SetActive(false);
            if (studyManager)
            {
                studyTip.color = new Color(1, 1, 1, 0);
            }

        }
    }

    public IEnumerator OnHit()
    {
        flySpeed = 0;

        vibrator.Vibrate(VibrationType.Heavy);

        nowKnife.transform.parent = target.transform;

        rewardText.transform.GetChild(0).GetComponent<Text>().text = "+" + ((int)(PlayerPrefsSafe.GetInt("MoneyForTarget") * nowMultiplier)).ToString()+" ";
        rewardText.SetActive(true);
        rewardText.GetComponent<Animation>().Play();

        target.transform.parent.GetComponent<Animator>().SetTrigger("Hit");
        target.transform.parent.GetComponent<Animator>().SetTrigger("Change");


        Invoke("RandomTargetWithDelay", 0.5f);

        //nowKnife.GetComponent<SpriteRenderer>().color = new Color(nowKnife.GetComponent<SpriteRenderer>().color.r, nowKnife.GetComponent<SpriteRenderer>().color.r, nowKnife.GetComponent<SpriteRenderer>().color.g, target.GetComponent<SpriteRenderer>().color.a);

        yield return new WaitForSeconds(1f);

        //Destroy(nowKnife);

        float startValue = gameManager.coinsForSession;
        float endValue = gameManager.coinsForSession + (int)(PlayerPrefsSafe.GetInt("MoneyForTarget") * nowMultiplier);

        StartCoroutine(gameManager.sessionCoinsText.GetComponent<CoinsFiller>().FillCoins(startValue, endValue));

        rewardText.SetActive(false);

        gameManager.coinsForSession += (int)(PlayerPrefsSafe.GetInt("MoneyForTarget") * nowMultiplier);

        isFly = false;

        knifeInTarget = false;

        gameManager.score++;

        gameManager.scoreText.text = gameManager.score.ToString();

        if (PlayerPrefsSafe.GetInt("BestScore") < gameManager.score)
        {
            PlayerPrefsSafe.SetInt("BestScore", gameManager.score);

            gameManager.bestScoreText.text = gameManager.score.ToString();
        }

        CreateKnife();

        //RandomTarget();
    }

    public void RandomTargetWithDelay()
    {
        RandomTarget();

        Destroy(nowKnife);
    }

    public void RandomTarget()
    {
        float rotZ = Random.Range(-180f, 180f);

        if (PlayerPrefs.GetString("IsStudyComplete") != "YES")
            rotZ = Random.Range(-164f, -16f);

        target.transform.parent.rotation = Quaternion.Euler(0, 0, rotZ);

        if (Mathf.Abs(rotZ) > 90f)
            rotZ = 90 - (Mathf.Abs(rotZ) - 90);

        //float f = Mathf.Clamp(Mathf.Abs(rotZ), 0, 90) / 90;

        //target.transform.localPosition = Vector3.Lerp(new Vector3(1.77f, 0, 0), new Vector3(1.92f, 0, 0), f);

        //target.transform.localPosition = new Vector2(1.77f, 0);

        //target.transform.position = new Vector3(Mathf.Clamp(target.transform.position.x, -horizontalBorder, horizontalBorder), target.transform.position.y, target.transform.position.z);

        float randomOffset = Random.Range(-targetScaleOffset, targetScaleOffset);

        target.transform.localScale = new Vector2(targetOriginalScale.x * (1 - randomOffset / 100), targetOriginalScale.y * (1 - randomOffset / 100));

        if (gameManager.score >= 6)
            if (Random.Range(0, 2) == 1)
            {
                target.GetComponent<Target>().isMove = true;

                target.GetComponent<Target>().isWaiting = true;

                target.GetComponent<Target>().range = Random.Range(40f, 60f);
                target.GetComponent<Target>().delay = 0;
                target.GetComponent<Target>().moveSpeed = 25;

                if (Random.Range(0, 2) == 0)
                    target.GetComponent<Target>().dir = 1;
                else
                    target.GetComponent<Target>().dir = -1;
            }
            else
                target.GetComponent<Target>().isMove = false;
        else
            target.GetComponent<Target>().isMove = false;

        target.transform.GetChild(1).GetComponent<SpriteRenderer>().sortingOrder = 1;

    }


    public void CreateKnife()
    {

        randomSpawn = Random.Range(0,2);

        isHitBackside = false;
        knifeInBackSide = false;

        flySpeed = 15;

        if (randomSpawn == 0)
            randomEnd = 1;
        else randomEnd = 0;

        nowKnife = Instantiate(knives[PlayerPrefsSafe.GetInt("NowKnifeSkin")], knifeSpawns[randomSpawn].transform.position, Quaternion.Euler(0, 0, Random.Range(0f, 360f)));

        knifeStatus = KnifeStatus.toCenter;

        if (PlayerPrefs.GetString("IsStudyComplete") != "YES")
        {
            rotateSpeed = 150f;
            GameObject nowStudyAim = Instantiate(studyAim, nowKnife.transform);

            float x = (float)Screen.height / Screen.width;

            float y = Mathf.InverseLerp(2.1f, 1.77f, x);

            float z = Mathf.Lerp(0, 0.35f, y);

            for (int i = 0; i < nowStudyAim.transform.childCount; i++)
                nowStudyAim.transform.GetChild(i).localPosition = new Vector2(nowStudyAim.transform.GetChild(i).localPosition.x + z * i, nowStudyAim.transform.GetChild(i).localPosition.y);
            
        }
        else
            rotateSpeed = difficult.Evaluate(gameManager.score);

        //rotateSpeed = 20;
        flySpeed = 12;
        //startSpeed = rotateSpeed / 35.7f;
        startSpeed = 6;

        if (PlayerPrefs.GetString("IsStudyComplete") == "YES")
            if (Random.Range(0, 2) == 0)
                rotateSpeed = -rotateSpeed;

        if (isTimeToNewKnife)
        {
            nowKnivesNumber--;

            gameManager.knivesText.text = "x"+nowKnivesNumber.ToString();

            isTimeToNewKnife = false;

            target.GetComponent<Target>().isMove = isTargetMoveMemory;
        }


    }
}
