using UnityEngine;

public class TrainingAim : TrainingElement
{
    private SpriteRenderer[] childs;

    protected override void Awake()
    {
        base.Awake();
        childs = GetComponentsInChildren<SpriteRenderer>();
    }

    protected override void Update()
    {
        foreach (var spriteRenderer in childs)
        {
            spriteRenderer.color = new Color(1, 1, 1, knifeToTargetAngleRatio);
        }
    }
}
