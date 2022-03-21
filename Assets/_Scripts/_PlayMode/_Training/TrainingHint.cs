using UnityEngine;
using UnityEngine.UI;

public class TrainingHint : TrainingElement
{
    [Range(1f, 2f)]
    [SerializeField] private float scaleOffset = 1.1f;

    private Text text;
    private Transform m_Transform;

    private Vector2 initialScale;

    protected override void Awake()
    {
        base.Awake();

        text = GetComponent<Text>();
        m_Transform = transform;

        initialScale = m_Transform.localScale;
    }

    protected override void Update()
    {
        float a = Mathf.Lerp(0.7f, 1, opacity); ;
        text.color = new Color(1, 1, 1, a);

        m_Transform.localScale = Vector2.Lerp(initialScale, initialScale * scaleOffset, a);
    }
}
