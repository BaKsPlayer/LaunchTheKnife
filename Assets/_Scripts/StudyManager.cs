using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StudyManager : MonoBehaviour
{
    PlayerController playerController;

    public bool disabling;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<PlayerController>();

        if (PlayerPrefs.GetString("IsStudyComplete") == "YES")
        {
            Destroy(playerController.studyTip.gameObject);

            Destroy(GetComponent<StudyManager>());

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerController.isGameMode && playerController.nowKnife && PlayerPrefs.GetString("IsStudyComplete") != "YES" && !disabling)
        {

            float dist = Vector2.Distance(playerController.nowKnife.transform.position, playerController.center);

            if (dist <= 0.3f)
            {
                //playerController.studyTip.gameObject.SetActive(true);

                float a = 1 - (Mathf.Abs(playerController.target.transform.parent.localEulerAngles.z - playerController.nowKnife.transform.localEulerAngles.z) - 5) / 15;
                playerController.rotateSpeed = Mathf.Lerp(15f, 150f, 1 - a);
                playerController.background.color = Color.Lerp(new Color(1, 1, 1), new Color(0.75f, 0.75f, 0.75f), a);

                playerController.nowKnife.transform.GetChild(1).GetChild(0).GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, a);
                playerController.nowKnife.transform.GetChild(1).GetChild(1).GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, a);
                playerController.nowKnife.transform.GetChild(1).GetChild(2).GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, a);
                playerController.nowKnife.transform.GetChild(1).GetChild(3).GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, a);

                playerController.studyTip.color = Color.Lerp(new Color(1, 1, 1, 0.7f), new Color(1, 1, 1, 1), a);
                playerController.studyTip.transform.localScale = Vector3.Lerp(new Vector3(1, 1, 1), new Vector3(1.1f, 1.1f, 1.1f), a);
            }
        }
    }


    public IEnumerator DisableStudy()
    {
        float a = Mathf.Clamp01(1 - (Mathf.Abs(playerController.target.transform.parent.localEulerAngles.z - playerController.nowKnife.transform.localEulerAngles.z) - 5) / 15);

        disabling = true;


        while (a > 0)
        {
            a -= Time.deltaTime*7;

            playerController.background.color = Color.Lerp(new Color(1, 1, 1), new Color(0.75f, 0.75f, 0.75f), a);

            if (playerController.nowKnife)
            {
                playerController.nowKnife.transform.GetChild(1).GetChild(0).GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, a);
                playerController.nowKnife.transform.GetChild(1).GetChild(1).GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, a);
                playerController.nowKnife.transform.GetChild(1).GetChild(2).GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, a);
                playerController.nowKnife.transform.GetChild(1).GetChild(3).GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, a);
            }


            yield return null;
        }

        if (PlayerPrefs.GetString("IsStudyComplete") == "YES")
        {
        Destroy(GetComponent<StudyManager>());
            print(PlayerPrefs.GetString("IsStudyComplete"));
        }
    }

}
