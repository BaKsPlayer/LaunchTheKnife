using UnityEngine;

public abstract class TrainingElement : MonoBehaviour
{
    [SerializeField] protected GameController gameController;
    [SerializeField] private GameKnife gameKnife;
    [SerializeField] private Target target;
    [SerializeField] private ClickHandler tapHandler;

    protected float knifeToTargetAngleRatio => 1 - Mathf.InverseLerp(0, 15, Mathf.Abs(target.parentTransform.localEulerAngles.z - gameKnife.m_Transform.localEulerAngles.z) - 5);

    protected virtual void Awake()
    {
        if (PlayerPrefs.GetString("IsTrainingComplete") == "YES")
        {
            TrainingCompleted();
            return;
        }

        Deactivate();

        tapHandler.OnClick += Deactivate;
        gameKnife.OnKnivesCountChanged += Deactivate;

        gameKnife.OnKnifeReachedCenter += Activate;

        gameController.OnTrainingCompleted += TrainingCompleted;
    }

    private void TrainingCompleted() => Destroy(gameObject);

    private void Activate() => gameObject.SetActive(true);
    private void Deactivate() => gameObject.SetActive(false);
    
    protected abstract void Update();

    private void OnDestroy()
    {
        gameController.OnTrainingCompleted -= TrainingCompleted;

        gameKnife.OnKnifeReachedCenter -= Activate;

        tapHandler.OnClick -= Deactivate;
        gameKnife.OnKnivesCountChanged -= Deactivate;
    }

    private void Reset()
    {
        gameController = FindObjectOfType<GameController>();
        gameKnife = FindObjectOfType<GameKnife>(includeInactive: true);
        target = FindObjectOfType<Target>(includeInactive: true);
        tapHandler = FindObjectOfType<ClickHandler>(includeInactive: true);
    }
}
