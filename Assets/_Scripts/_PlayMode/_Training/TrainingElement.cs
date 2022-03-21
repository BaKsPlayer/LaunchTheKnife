using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TrainingElement : MonoBehaviour
{
    [SerializeField] private GameController gameController;
    [SerializeField] private GameKnife gameKnife;
    [SerializeField] private Target target;

    protected float opacity => 1 - Mathf.InverseLerp(0, 15, Mathf.Abs(target.transform.localEulerAngles.y - gameKnife.m_Transform.rotation.y) - 5);

    protected virtual void Awake()
    {
        if (PlayerPrefs.GetString("IsTrainingComplete") == "YES")
            TrainingCompleted();
    }

    private void OnEnable()
    {
        gameController.OnTrainingCompleted += TrainingCompleted;
    }

    private void OnDestroy()
    {
        gameController.OnTrainingCompleted -= TrainingCompleted;
    }

    public void TrainingCompleted()
    {
        Destroy(gameObject);
    }

    protected abstract void Update();

    private void Reset()
    {
        gameController = FindObjectOfType(typeof(GameController)) as GameController;
        gameKnife = FindObjectOfType(typeof(GameKnife)) as GameKnife;
        target = FindObjectOfType(typeof(Target)) as Target;
    }
}
