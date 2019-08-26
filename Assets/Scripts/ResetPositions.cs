using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPositions : MonoBehaviour {
    // the plotting data container

    // variables to store initial values
    public Vector3 initialScale;
    public Vector3 initialPosition;
    public Quaternion initialRotation;

    // variables for limits
    Vector3 scaleLimitHigh = new Vector3(3f, 3f, 3f);
    Vector3 scaleLimitLow = new Vector3(0.5f, 0.5f, 0.5f);
	// Use this for initialization
	void Start () {
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        initialScale = transform.localScale;
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (transform.localScale.x > scaleLimitHigh.x && transform.localScale.y > scaleLimitHigh.y && transform.localScale.z > scaleLimitHigh.z)
        {
            transform.localScale = scaleLimitHigh;
        }

        if (transform.localScale.x < scaleLimitLow.x && transform.localScale.y < scaleLimitLow.y && transform.localScale.z < scaleLimitLow.z)
        {
            transform.localScale = scaleLimitLow;
        }
        //transform.rotation = initialRotation;

        if (transform.position.y < 0.025f)
        {
            transform.position = new Vector3(transform.position.x, 0.025f, transform.position.z);
        }
    }
    public void ResetPosition()
    {
        transform.position = initialPosition;
        transform.localScale = initialScale;
        transform.rotation = initialRotation;
    }
}
