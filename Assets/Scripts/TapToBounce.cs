using System;
using UnityEngine;
using System.Collections;

public class TapToBounce : MonoBehaviour {
    private Rigidbody ballRigidBody;

    void Start ()
	{
        ballRigidBody = rigidbody;
	}
	
	void Update ()
	{
        if (Input.GetMouseButtonUp(0) && Input.GetMouseButtonUp(1))
        {
            Jump();
            return;
        }

        //Left Mouse or First Touch
        if (Input.GetMouseButtonUp(0))
        {
            Push(-1);
        }
        //Right Mouse or Second Touch
        if (Input.GetMouseButtonUp(1))
	    {
	        Push(1);
	    }
	}

    private void Push(int direction)
    {
        ballRigidBody.AddForce(5 * direction, 0, 0, ForceMode.Force);
    }

    private void Jump()
    {
        ballRigidBody.AddForce(0, 5, 0, ForceMode.VelocityChange);
    }
}
