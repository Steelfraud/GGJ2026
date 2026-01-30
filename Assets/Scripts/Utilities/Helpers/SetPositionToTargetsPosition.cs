using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPositionToTargetsPosition : MonoBehaviour
{

    public Transform transformToCopy;
    public Vector3 offSet;

    private void OnEnable()
    {
        if (this.transformToCopy == null)
        {
            return;
        }

        this.transform.position = this.transformToCopy.position + this.offSet;
    }

    protected virtual void Update()
    {
        if (this.transformToCopy == null)
        {
            return;
        }

        this.transform.position = this.transformToCopy.position + this.offSet;
    }

}
