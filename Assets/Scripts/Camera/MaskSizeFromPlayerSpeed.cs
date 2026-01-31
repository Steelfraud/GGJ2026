using UnityEngine;

public class MaskSizeFromPlayerSpeed : MonoBehaviour
{
    public Transform MaskTransform;
    public float MaximumSpeed;
    public AnimationCurve SizeSpeedCurve;

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance == null || GameManager.Instance.PlayerVehicle == null)
        {
            return;
        }

        float lerpSpeed = Mathf.Clamp01(GameManager.Instance.PlayerVehicle.CurrentSpeedKMH / MaximumSpeed);
        MaskTransform.localScale = Vector3.one * SizeSpeedCurve.Evaluate(lerpSpeed);
    }

}