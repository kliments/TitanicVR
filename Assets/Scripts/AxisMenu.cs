using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxisMenu : MonoBehaviour {

    // Menu Item
    public GameObject menuPrefab;
    private GameObject menu;
    private GameObject currentMenuItem;
    private GameObject menuChild;
    public bool menuWasHit = false;
    public Color menuColor;

    // Array to store data variables
    string[] data = new string[] { "Age_Of_Passenger", "Passenger_Class", "Siblings_Spouses", "Parents_Children" };

    // Current menu text
    string currentMenuText;

    private void Start()
    {
        menuColor = GetComponent<Renderer>().material.color;
    }

    public void LoadMenu(Transform hitInfo)
    {
        Debug.Log("It Works");
        currentMenuItem = hitInfo.gameObject;
        currentMenuText = currentMenuItem.transform.Find("MenuText").GetComponent<TextMesh>().text;
        float j = -1.05f;
        float k = 0.62f; 
        
        for (int i = 0; i < data.Length; i++)
        {
            if (data[i] == currentMenuText)
            {
                continue;
            }
            menu = Instantiate(menuPrefab, hitInfo.parent.transform);
            if(menu.transform.parent.parent.parent.parent.name == "PlottingData")
            {
                menu.tag = "reset";
            }
            else if(menu.transform.parent.parent.parent.parent.name == "PlottingData(Clone)")
            {
                menu.tag = "reset1";
            }
            if (hitInfo.parent.transform.name == "zAxisMenu")
            {
                menu.transform.localPosition = new Vector3(hitInfo.localPosition.x + j, hitInfo.localPosition.y, hitInfo.localPosition.z);
            }
            if ((hitInfo.parent.transform.name == "yAxisMenu"))
            {
                menu.transform.localPosition = new Vector3(hitInfo.localPosition.x, hitInfo.localPosition.y + k, hitInfo.localPosition.z);
            }
            if ((hitInfo.parent.transform.name == "xAxisMenu"))
            {
                menu.transform.localPosition = new Vector3(hitInfo.localPosition.x, hitInfo.localPosition.y, hitInfo.localPosition.z + j);
            }
            
            menu.transform.localScale = hitInfo.localScale;
            menu.transform.localRotation = hitInfo.localRotation;
            menu.transform.GetChild(0).localRotation = currentMenuItem.transform.Find("MenuText").localRotation;
            menuChild = menu.transform.Find("MenuText").gameObject;
            menuChild.GetComponent<TextMesh>().text = data[i];
            j += -1.05f;
            k += 0.62f;
        }
    }

    private void Update()
    {
        
        if(menuWasHit)
        {
            GetComponent<Renderer>().material.color = new Color(0.87f, 0.89f, 0.08f);
        }

        else
        {
            GetComponent<Renderer>().material.color = menuColor;
        }
    }
}
