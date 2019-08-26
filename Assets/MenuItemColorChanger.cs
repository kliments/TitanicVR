using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuItemColorChanger : MonoBehaviour {

    public bool menuWasHit = false;
    public Color menuColor;
    // Use this for initialization
    void Start ()
    {
        menuColor = GetComponent<Renderer>().material.color;

    }
	
	// Update is called once per frame
	void Update () {
        if (menuWasHit)
        {
            GetComponent<Renderer>().material.color = new Color(0.87f, 0.89f, 0.08f);
        }

        else
        {
            GetComponent<Renderer>().material.color = menuColor;
        }
    }
}
