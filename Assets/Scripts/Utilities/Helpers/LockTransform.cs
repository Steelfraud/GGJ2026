using UnityEngine;

public class LockTransform : MonoBehaviour
{
    private Vector3 position;
    private Quaternion rotation;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        position = transform.position;
        rotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = position;
        transform.rotation = rotation;
    }
}
