using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtScript : MonoBehaviour {

    public Transform target;
    private Vector3 lookAtPos;
    private Transform objects;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(gameObject.name == "GraphButtons")
        {
            lookAtPos.x = target.position.x;
            lookAtPos.y = transform.position.y;
            lookAtPos.z = -target.position.z;
            transform.LookAt(target);
            if (transform.eulerAngles.y > 100 && transform.eulerAngles.y < 150)
            {
                transform.eulerAngles = new Vector3(transform.position.x, -100, transform.position.z);
            }
        }
        //transform.Rotate(0, 60, 0, Space.Self);
        
        //transform.LookAt(lookAtPos);
      //  transform.eulerAngles = new Vector3(transform.position.x, target.eulerAngles.y, transform.position.z);
       
        /*
         var lookPos = target.position - transform.position;
         lookPos.y = 0;
         var rotation = Quaternion.LookRotation(lookPos);
         transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime);
        */
         

    }
}
