using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyTransform : MonoBehaviour
{

    public Transform TransformToFollow;
    public Transform TransformToMove;

    private void Awake()
    {
        if (this.TransformToMove == null)
        {
            this.TransformToMove = transform;
        }
    }

    private void FixedUpdate()
    {
        if (this.TransformToMove == null || TransformToFollow == null)
        {
            return;
        }

        this.TransformToMove.position = this.TransformToFollow.position;
    }

}