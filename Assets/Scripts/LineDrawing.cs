using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineDrawing : MonoBehaviour {

    public Vector3 start = new Vector3(0, 0, 0);
    public Vector3 end = new Vector3(0,0,0);
    public bool hasChangedPosition = false;
    public bool itemIsSelected = false;
    private Vector3 lastPos;
    private Vector3 newPos;
	// Use this for initialization
	void Start () {
        newPos = gameObject.transform.position;
        GetComponent<LineRenderer>().material.color = new Color(0.87f, 0.89f, 0.08f);
        GetComponent<LineRenderer>().startWidth = 0.001f;
        GetComponent<LineRenderer>().endWidth = 0.001f;
    }
	
	// Update is called once per frame
	void Update ()
    {
        newPos = transform.position;
        if(itemIsSelected)
        {
            GetComponent<LineRenderer>().enabled = true;
            GetComponent<LineRenderer>().SetPosition(0, newPos);
            GetComponent<LineRenderer>().SetPosition(1, end);
            
        }
        else if(!itemIsSelected)
        {
            GetComponent<LineRenderer>().enabled = false;
        }
	}
}
