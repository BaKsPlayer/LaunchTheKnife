using UnityEngine;

public class TrainingTimeController : TrainingElement
{
    protected override void Update()
    {
        Time.timeScale = gameController.CurrentTimeScale * Mathf.Lerp(1f, 0.1f, knifeToTargetAngleRatio);
    }

    private void OnDisable() => Time.timeScale = gameController.CurrentTimeScale;
}
