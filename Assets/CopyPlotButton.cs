using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyPlotButton : MonoBehaviour {
    public GameObject plotMain;
    public GameObject controller;
    private GameObject plotClone = null;
    public bool buttonWasMoved = false;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
            if(transform.parent.name != "GraphButtons")
            {
            controller.GetComponent<RaycastColor>().copyButtonWasMoved = true;
            }
        }
	}

