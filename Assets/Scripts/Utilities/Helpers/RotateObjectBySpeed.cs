using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObjectBySpeed : MonoBehaviour
{
    public Transform TransformToRotate;
    public Vector3 RotationPerSecond;
    public Vector3 RotationRandomRange;
    public bool PlayOnEnable;

    private bool going = false;
    private Vector3 randomRangeChosen = Vector3.zero;
    
    void OnEnable()
    {
        if (this.PlayOnEnable)
        {
            StartRotation();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (this.going == false || this.TransformToRotate == null)
        {
            return;
        }

        Vector3 currentSpeed = this.RotationPerSecond + this.randomRangeChosen;
        this.TransformToRotate.Rotate(currentSpeed * Time.deltaTime);
    }

    public void StartRotation()
    {
        this.going = true;

        if (this.RotationRandomRange.magnitude > 0.01)
        {
            this.randomRangeChosen = new Vector3(Random.Range(-this.RotationRandomRange.x, this.RotationRandomRange.x),
                Random.Range(-this.RotationRandomRange.y, this.RotationRandomRange.y),
                Random.Range(-this.RotationRandomRange.z, this.RotationRandomRange.z));
        }
        else
        {
            this.randomRangeChosen = Vector3.zero;
        }
    }

}