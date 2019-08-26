using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeSpriteChanger : MonoBehaviour {
    public GameObject selectMode;
    public GameObject moveMode;
    public GameObject scaleMode;
    public GameObject selectedOption;
	// Use this for initialization
	void Start () {
        selectMode.SetActive(false);
        moveMode.SetActive(false);
        scaleMode.SetActive(false);

    }
	
	// Update is called once per frame
	void Update () {
        if(selectedOption.GetComponent<MenuButtonController>().select)
        {
            selectMode.SetActive(true);
            moveMode.SetActive(false);
            scaleMode.SetActive(false);
        }
        else if(selectedOption.GetComponent<MenuButtonController>().move)
        {
            selectMode.SetActive(false);
            moveMode.SetActive(true);
            scaleMode.SetActive(false);
        }
        else
        {
            selectMode.SetActive(false);
            moveMode.SetActive(false);
            scaleMode.SetActive(true);
        }
		
	}
}
