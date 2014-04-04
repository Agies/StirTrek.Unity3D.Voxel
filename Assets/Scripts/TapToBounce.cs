using System;
using UnityEngine;
using System.Collections;

public class TapToBounce : MonoBehaviour {
    private Rigidbody ballRigidBody;

    // Use this for initialization
	void Start ()
	{
        ballRigidBody = rigidbody;
	}
	
	// Update is called once per frame
	void Update ()
	{
        if (Input.GetMouseButtonUp(0))
        {
            Touch();
        }
	}

    private void Touch()
    {
        Debug.Log("Touched!");
        ballRigidBody.AddForce(0, 5, 0, ForceMode.VelocityChange);
    }
}
