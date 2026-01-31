using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

public class FollowSplineMover : MonoBehaviour
{
    public SplineContainer SplineToFollow;
    public bool LoopSplineAfterComplete = true;
    public float TimeToCompleteSpline = 5f;

    private float timeTraveled = 0f;
    
    // Update is called once per frame
    void Update()
    {
        if (SplineToFollow == null)
        {
            return;
        }

        if (timeTraveled > TimeToCompleteSpline)
        {
            if (LoopSplineAfterComplete == false)
            {
                return;
            }
            else
            {
                timeTraveled = 0f;
                return;
            }
        }

        timeTraveled += Time.deltaTime;
        float splineLerpPos = timeTraveled / TimeToCompleteSpline;

        if (SplineToFollow.Evaluate(splineLerpPos, out float3 pos, out float3 tangent, out float3 up))
        {
            Vector3 posToSet = new Vector3(pos.x, pos.y, pos.z);
            Quaternion rotation = Quaternion.LookRotation(tangent);
            
            transform.position = posToSet;
            transform.rotation = rotation;
        }
        else
        {
            Debug.LogError("wtf?");
        }           
    }

}