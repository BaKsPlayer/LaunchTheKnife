using UnityEngine;
using System.Collections.Generic;

public class TrainingAim : TrainingElement
{
    private SpriteRenderer[] childs;

    protected override void Awake()
    {
        base.Awake();
        childs = GetComponentsInChildren<SpriteRenderer>();

        //foreach (var child in childs)
        //{
        //    Vector2 newPosition = new Vector2(0,0);
        //    child.transform.localPosition = newPosition;
        //}
    }

    protected override void Update()
    {
        foreach (var spriteRenderer in childs)
        {
            spriteRenderer.color = new Color(1, 1, 1, knifeToTargetAngleRatio);
        }
    }
}
