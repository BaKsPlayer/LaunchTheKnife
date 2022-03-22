using UnityEngine;

public class TrainingBackground : TrainingElement
{
    private SpriteRenderer spriteRenderer;

    [SerializeField] private Color initialColor, darkenedColor;

    protected override void Awake()
    {
        base.Awake();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected override void Update()
    {
        spriteRenderer.color = Color.Lerp(initialColor, darkenedColor, knifeToTargetAngleRatio);
    }
}
