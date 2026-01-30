using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTransformRigidBody : MonoBehaviour
{

    public Transform TransformToFollow;
    public Rigidbody MyRigidbody;
    public bool RepulseFromPlayerInstead = false;
    public bool UseDistanceCurveForPowerMultiplier = false;
    public float MinDistanceToPlayer;
    public float MaxDistanceToPlayer;
    public float MinDistanceForSpeed;
    public float MaxDistanceForSpeed;
    public float PowerToApply;
    public AnimationCurve PowerToDistanceCurve;

    private void FixedUpdate()
    {
        if (this.MyRigidbody == null || TransformToFollow == null)
        {
            return;
        }

        if (this.MyRigidbody.isKinematic)
        {
            return;
        }

        Vector3 playerPos = TransformToFollow.position;
        float distanceToTarget = Vector3.Distance(this.MyRigidbody.position, playerPos);

        if (distanceToTarget < this.MinDistanceToPlayer)
        {
            return;
        }

        if (distanceToTarget > this.MaxDistanceToPlayer)
        {
            return;
        }

        ApplyForceTowardsTarget(playerPos, distanceToTarget);
    }

    private void ApplyForceTowardsTarget(Vector3 playerPos, float distance)
    {
        Vector3 targetDir = this.RepulseFromPlayerInstead ? this.MyRigidbody.position - playerPos : playerPos - this.MyRigidbody.position;
        targetDir.Normalize();
        targetDir *= this.PowerToApply;

        if (this.UseDistanceCurveForPowerMultiplier)
        {
            float relevantDistance = distance - this.MinDistanceForSpeed;
            float distanceLerp = Mathf.Clamp01(relevantDistance / (this.MaxDistanceForSpeed - this.MinDistanceForSpeed));
            targetDir *= this.PowerToDistanceCurve.Evaluate(distanceLerp);
        }

        this.MyRigidbody.AddForce(targetDir, ForceMode.Impulse);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(this.transform.position, this.MaxDistanceToPlayer);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, this.MinDistanceToPlayer);
    }

}