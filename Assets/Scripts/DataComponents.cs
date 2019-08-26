using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataComponents : MonoBehaviour {
    public string passengerID,survived, pClass, name, sex, age, sibSp, parCh, Ticket, Fare, Cabin, Embarked;
    public bool wasHit = false;
    public bool wasPressed = false;
    public Color color;

    private Vector3 oldPos;
    private Vector3 newPos;

    public GameObject lineToPoint;
    // Use this for initialization
    void Start () {
        newPos = transform.position;
        lineToPoint = GameObject.Find("LineToPoint");
}
	
	// Update is called once per frame
	void Update () {
        if(lineToPoint== null)
        {
            lineToPoint = GameObject.Find("LineToPoint");
        }
        oldPos = newPos;
        newPos = transform.position;
		if(wasHit)
        {
            GetComponent<Renderer>().material.color = new Color(0.87f, 0.89f, 0.08f);
        }
        else if(wasPressed)
        {
            GetComponent<Renderer>().material.color = new Color(0.87f, 0.89f, 0.08f);
            lineToPoint.GetComponent<LineDrawing>().end = newPos;
            lineToPoint.GetComponent<LineDrawing>().itemIsSelected = true;
            if (oldPos != newPos)
            {
                lineToPoint.GetComponent<LineDrawing>().end = newPos;
            }
        }
        else
        {
            GetComponent<Renderer>().material.color = color;
        }

    }
}
