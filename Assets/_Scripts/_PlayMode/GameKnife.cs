using UnityEngine;
using UnityEngine.Events;

public class GameKnife : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Target target;
    public Target Target => target;

    [SerializeField] private KnifeImprover moneyPerHitImprover;
    public int CoinsPerHit => moneyPerHitImprover.CurrentValue;

    [SerializeField] private KnifeImprover knivesNumberImprover;

    [SerializeField] private GameController gameController;

    [SerializeField] private LoseMenuManager loseMenu;
    [SerializeField] private TapHandler tapHandler;

    [SerializeField] private CoinsFiller coinsTextFiller;
    public CoinsFiller CoinsTextFiller => coinsTextFiller;

    [Header("States")]
    [SerializeField] private State movingState;
    [SerializeField] private State flyingState;

    [Header("Speeds")]
    [SerializeField] private float flightSpeed;
    [SerializeField] private float moveSpeed;

    [Space(height: 15f)]
    [SerializeField] private Vector2 border;
    [SerializeField] private AnimationCurve difficult;
    [SerializeField] private LayerMask toHit;

    [Space(height: 15f)]
    [SerializeField] private Transform[] spawns;

    public Vector2 CenterPosition => new Vector2(0, 1.57f);
    public Transform m_Transform{ get; private set; }

    public UnityAction OnKnivesCountChanged;

    public SafeFloat RotateSpeed { get; private set; }
    public SafeFloat CoinsMultiplyer { get; private set; }
    public SafeInt LeftKnivesCount { get; private set; }

    public State CurrentState { get; private set; }
    public RaycastHit2D Hit { get; private set; }

    private int spawnIndex;

    public Vector2 Position => m_Transform.position;
    public Vector2 FlightDirection => m_Transform.GetChild(0).position - m_Transform.position;
    public bool IsAbroad => Mathf.Abs(Position.x) > border.x || Mathf.Abs(Position.y) > border.y;

    public Transform finalPoint
    {
        get
        {
            int finalPointIndex = spawnIndex == 0 ? 1 : 0;
            return spawns[finalPointIndex];
        }
    }

    private void Awake()
    {
        m_Transform = GetComponent<Transform>();

        gameController.OnGameStarted += GameStarted;
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        gameController.OnGameStarted -= GameStarted;
    }

    private void Update()
    {
        CurrentState?.Update();
    }

    public void SetState(State state)
    {
        if (state == null)
        {
            CurrentState = null;
            return;
        }

        CurrentState = Instantiate(state);
        CurrentState.SetGameKnife(this);
        CurrentState.Init();
    }

    public void Launch()
    {
        target.Stop();
        Hit = Physics2D.Raycast(m_Transform.position, FlightDirection, 100, toHit);
        SetState(flyingState);

        CalculateCoinsMultiplyer();
        
        tapHandler.enabled = false;
    }

    public void Fly()
    {
        transform.Translate(Vector3.right * flightSpeed * Time.deltaTime);
    }

    public void MoveTo(Vector2 target)
    {
        float dist1 = Vector2.Distance(m_Transform.position, CenterPosition);
        float dist2 = Vector2.Distance(target, CenterPosition);

        float speed = moveSpeed * Mathf.Clamp(dist1 / (dist2 * 0.4f), 0.05f, 1f);

        m_Transform.position = Vector3.MoveTowards(m_Transform.position, target, speed * Time.deltaTime);
    }

    public void Rotate()
    {
       m_Transform.Rotate(Vector3.forward * RotateSpeed * Time.deltaTime);
    }

    public void HitTarget()
    {
        VibrationManager.Instance.Vibrate(VibrationType.Heavy);

        transform.SetParent(target.transform, true);
        GameStats.Instance.IncreaseScore();

        target.Hit();
    }

    public void LoseKnife()
    {
        gameObject.SetActive(false);
        LeftKnivesCount--;
        OnKnivesCountChanged?.Invoke();

        if (LeftKnivesCount >= 0)
        {
            Spawn();
            SetState(movingState);

            target.RestoreMovement();
        }
        else
        {
            loseMenu.Open();
            SetState(null);
        }
    }

    public void Spawn()
    {
        transform.SetParent(null, true);
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;

        spawnIndex = Random.Range(0, 2);
        m_Transform.position = spawns[spawnIndex].position;

        RotateSpeed = difficult.Evaluate(GameStats.Instance.Score);

        if (Random.Range(0, 2) == 0)
            RotateSpeed = -RotateSpeed;

        gameObject.SetActive(true);
        tapHandler.enabled = true;
    }

    public void CalculateCoinsMultiplyer()
    {
        float distanceFromTargetCenter = Vector2.Distance(Hit.point, target.transform.position);

        if (distanceFromTargetCenter <= 0.09f)
            CoinsMultiplyer = 2f;
        else if (distanceFromTargetCenter <= 0.22f)
            CoinsMultiplyer = 1.5f;
        else if (distanceFromTargetCenter <= 0.37f)
            CoinsMultiplyer = 1.25f;
        else
            CoinsMultiplyer = 1f;
    }

    public void SetSkin(Sprite skin)
    {
        GetComponent<SpriteRenderer>().sprite = skin;
    }

    private void GameStarted()
    {
        Spawn();
        SetState(movingState);
        LeftKnivesCount = knivesNumberImprover.CurrentValue - 1;
        OnKnivesCountChanged?.Invoke();
    }

   
}
