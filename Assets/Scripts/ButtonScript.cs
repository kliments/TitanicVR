using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour {
    public bool wasHit = false;
    private Color color;
    private Vector3 localSize;
    private Vector3 newSize;
    private Transform parent;
	// Use this for initialization
	void Start () {
        color = GetComponent<Renderer>().material.color;
        localSize = transform.localScale;
        parent = transform.parent;
    }
	
	// Update is called once per frame
	void Update () {
        newSize = transform.lossyScale;
        if (wasHit)
        {
            GetComponent<Renderer>().material.color = new Color(0.87f, 0.89f, 0.08f);
        }
        else
        {
            GetComponent<Renderer>().material.color = color;
        }
	}
}
