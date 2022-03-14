using UnityEngine;
using UnityEngine.UI;

public class GameKnife : MonoBehaviour
{
    [SerializeField] private Target target;
    public Target Target => target;

    [SerializeField] private Text rewardText;
    public Text RewardText => rewardText;

    [SerializeField] private KnifeImprover moneyPerHitImprover;
    public int CoinsPerHit => moneyPerHitImprover.CurrentValue;

    [SerializeField] private KnifeImprover knivesNumberImprover;

    [SerializeField] private State movingState;
    [SerializeField] private State standingState;
    [SerializeField] private State flyingState;

    [SerializeField] private float flightSpeed;
    [SerializeField] private float moveSpeed;

    [SerializeField] private float rotateSpeed;
    public float RotateSpeed => rotateSpeed;

    [SerializeField] private AnimationCurve difficult;

    [SerializeField] private Transform[] spawns;

    [SerializeField] private LayerMask toHit;

    public Vector3 CenterPosition => new Vector2(0, 1.57f);
    public Transform m_Transform{ get; private set; }

    private State CurrentState;

    private float coinsMultiplyer;
    public float CoinsMultiplyer => coinsMultiplyer;

    private int spawnIndex;
    private float currentFlightSpeed;

    private RaycastHit2D hit;
    public RaycastHit2D Hit => hit;



    public Transform finalPoint
    {
        get
        {
            int finalPointIndex = spawnIndex == 0 ? 1 : 0;
            return spawns[finalPointIndex];
        }
    }

    private void Start()
    {
        m_Transform = GetComponent<Transform>();
    }

    private void Update()
    {
        m_Transform.Rotate(Vector3.forward * rotateSpeed * Time.deltaTime);
        CurrentState.Update();
    }

    public void SetState(State state)
    {
        CurrentState = Instantiate(state);
        CurrentState.Init();
        CurrentState.SetGameKnife(this);
    }

    public void Launch()
    {
        SetState(flyingState);
        hit = Physics2D.Raycast(m_Transform.position, m_Transform.GetChild(0).position - m_Transform.position, 100, toHit);
        target.Stop();
    }

    public void Fly()
    {
        transform.Translate(Vector3.right * currentFlightSpeed * Time.deltaTime);
    }

    public void MoveTo(Vector2 target)
    {
        float dist1 = Vector2.Distance(m_Transform.position, CenterPosition);
        float dist2 = Vector2.Distance(target, CenterPosition);

        float speed = moveSpeed * Mathf.Clamp(dist1 / (dist2 * 0.4f), 0.05f, 1f);

        m_Transform.position = Vector3.MoveTowards(m_Transform.position, target, speed * Time.deltaTime);
    }

    public void HitTarget()
    {
        VibrationManager.Instance.Vibrate(VibrationType.Heavy);

        currentFlightSpeed = 0;

        rewardText.text = $"+{moneyPerHitImprover.CurrentValue} ";

        rewardText.transform.parent.gameObject.SetActive(true);
        rewardText.transform.parent.GetComponent<Animation>().Play();

        target.m_Animator.SetTrigger("Hit");
        target.m_Animator.SetTrigger("Change");

        target.Hit();
    }


    public void SpawnKnife()
    {
        spawnIndex = Random.Range(0, 2);
        m_Transform.position = spawns[spawnIndex].position;

        currentFlightSpeed = flightSpeed;

        rotateSpeed = difficult.Evaluate(GameStats.Instance.Score);

        if (Random.Range(0, 2) == 0)
            rotateSpeed = -rotateSpeed;
    }


    public void CalculateCoinsMultiplyer()
    {
        float distanceFromCenter = Vector2.Distance(hit.point, target.transform.position);

        if (distanceFromCenter <= 0.09f)
            coinsMultiplyer = 2f;
        else if (distanceFromCenter <= 0.22f)
            coinsMultiplyer = 1.5f;
        else if (distanceFromCenter <= 0.37f)
            coinsMultiplyer = 1.25f;
        else
            coinsMultiplyer = 1f;
    }
}
