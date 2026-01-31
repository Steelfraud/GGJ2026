using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

public class FollowSplineMover : MonoBehaviour
{
    public SplineContainer SplineToFollow;
    public Transform TransformToMove;
    public Vector3 SplineOffset = Vector3.zero;
    public bool LoopSplineAfterComplete = true;
    public bool ReverseSplineAfterComplete = false;
    public float TimeToCompleteSpline = 5f;
    public float WaitTimeAfterCompletion = 0f;

    private bool isReversing = false;
    private float timeTraveled = 0f;
    private float timeWaited = 0f;
    
    // Update is called once per frame
    void Update()
    {
        if (SplineToFollow == null)
        {
            return;
        }

        if (TransformToMove == null)
        {
            TransformToMove = transform;
        }

        if (timeTraveled > TimeToCompleteSpline)
        {
            if (LoopSplineAfterComplete == false && ReverseSplineAfterComplete == false)
            {
                return;
            }
            else
            {
                if (WaitTimeAfterCompletion > 0)
                {
                    timeWaited += Time.deltaTime;

                    if (timeWaited < WaitTimeAfterCompletion)
                    {
                        return;
                    }
                }

                if (ReverseSplineAfterComplete)
                {
                    isReversing = !isReversing;
                }

                timeTraveled = 0f;
                timeWaited = 0f;
                return;
            }
        }

        timeTraveled += Time.deltaTime;
        float splineLerpPos = timeTraveled / TimeToCompleteSpline;

        if (isReversing)
        {
            splineLerpPos = 1 - splineLerpPos;
        }

        if (SplineToFollow.Evaluate(splineLerpPos, out float3 pos, out float3 tangent, out float3 up))
        {
            Vector3 posToSet = new Vector3(pos.x, pos.y, pos.z) + SplineOffset;
            Quaternion rotation = Quaternion.LookRotation(tangent);

            TransformToMove.position = posToSet;
            TransformToMove.rotation = rotation;
        }
        else
        {
            Debug.LogError("wtf?");
        }           
    }

}