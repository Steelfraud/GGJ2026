using UnityEngine;

public class MoveObjectByVector : MonoBehaviour {

    public Transform objectToMove;
    [Range(-1, 1)]
    public float xMovement = 0;
    [Range(-1, 1)]
    public float yMovement = 0;
    [Range(-1, 1)]
    public float zMovement = 0;
    public float speed;

    // Use this for initialization
    void Start () {

        if (objectToMove == null)
            objectToMove = gameObject.transform;
	}
	
	// Update is called once per frame
	void Update () 
    {        
        if (objectToMove != null)
            objectToMove.transform.position = Vector3.MoveTowards(objectToMove.position, new Vector3(objectToMove.position.x + xMovement, objectToMove.position.y + yMovement, objectToMove.position.z + zMovement), speed * Time.deltaTime);   
	}

}
