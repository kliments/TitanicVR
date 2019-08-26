using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Globalization;

public class DataPlotter : MonoBehaviour
{
    public string xValueString;
    public string yValueString;
    public string zValueString;

    private float xValue, yValue, zValue;
    private GameObject dataPoint;
    public GameObject lineToPoint;
    public GameObject[] resetGameObjects;
    float ageOffset1 = 0.0001f;
    float ageOffset2 = 0.0001f;
    float ageOffset3 = 0.0001f;
    float ageOffset4 = 0.0001f;
    float ageOffset5 = 0.0001f;
    float ageOffset6 = 0.0001f;
    float ageOffset7 = 0.0001f;
    float ageOffset8 = 0.0001f;
    float ageOffset9 = 0.0001f;
    float sibSpOffset0 = 0.0001f;
    float sibSpOffset1 = 0.0001f;
    float sibSpOffset2 = 0.0001f;
    float sibSpOffset3 = 0.0001f;
    float sibSpOffset4 = 0.0001f;
    float sibSpOffset5 = 0.0001f;
    float sibSpOffset6 = 0.0001f;
    float sibSpOffset7 = 0.0001f;
    float sibSpOffset8 = 0.0001f;
    float parChOffset0 = 0.0001f;
    float parChOffset1 = 0.0001f;
    float parChOffset2= 0.0001f;
    float parChOffset3 = 0.0001f;
    float parChOffset4 = 0.0001f;
    float parChOffset5 = 0.0001f;
    float parChOffset6 = 0.0001f;
    float pClassOffset1 = 0.0001f;
    float pClassOffset2 = 0.0001f;
    float pClassOffset3 = 0.0001f;

    //CSV TextAsset
    public TextAsset data;

    //parent
    public GameObject parentPlot;

    //List of all points
    public List<GameObject> list;

    // Axis Menu item
    public GameObject menuItem;

    // Gameobject for parent axis menu
    private GameObject xAxisMenu;
    private GameObject yAxisMenu;
    private GameObject zAxisMenu;

    private int counter1 = 0;
    private int counter2 = 0;
    //GameObject to Instantiate (male)
    public GameObject cube;

    //GameObject to Instantiate (female)
    public GameObject sphere;

    // GameObjects for axis textmesh
    private TextMesh xAxis;
    private TextMesh yAxis;
    private TextMesh zAxis;

    // The position vector for each data point
    Vector3 position;


    public List<Vector3> dataPositions;
    public List<string> classes;
    public Material dataMappedMaterial;
    public Material dataMappedTransparent;

    public float Passenger_Class = new float();
    public float Age_Of_Passenger = new float();
    public float Siblings_Spouses = new float();
    public float nullAge = 0.9f;
    public float Parents_Children = new float();

    public GameObject controller;
    //Material
    private Material materialColored;
    
    // Use this for initialization
    void Start()
    {
        list.Clear();
        ageOffset1 = 0.0001f;
        ageOffset2 = 0.0001f;
        ageOffset3 = 0.0001f;
        ageOffset4 = 0.0001f;
        ageOffset5 = 0.0001f;
        ageOffset6 = 0.0001f;
        ageOffset7 = 0.0001f;
        ageOffset8 = 0.0001f;
        ageOffset9 = 0.0001f;
        sibSpOffset0 = 0.0001f;
        sibSpOffset1 = 0.0001f;
        sibSpOffset2 = 0.0001f;
        sibSpOffset3 = 0.0001f;
        sibSpOffset4 = 0.0001f;
        sibSpOffset5 = 0.0001f;
        sibSpOffset6 = 0.0001f;
        sibSpOffset7 = 0.0001f;
        sibSpOffset8 = 0.0001f;
        parChOffset0 = 0.0001f;
        parChOffset1 = 0.0001f;
        parChOffset2 = 0.0001f;
        parChOffset3 = 0.0001f;
        parChOffset4 = 0.0001f;
        parChOffset5 = 0.0001f;
        parChOffset6 = 0.0001f;
        pClassOffset1 = 0.0001f;
        pClassOffset2 = 0.0001f;
        pClassOffset3 = 0.0001f;


        //remove the yellow line, once Start function starts
         lineToPoint.GetComponent<LineDrawing>().itemIsSelected = false;

        controller.GetComponent<PointerEventListener>().listObjects.Clear();
        var dictionary = new Dictionary<String, int>();
        dictionary["zVar"] = 200;
        Debug.Log(dictionary["zVar"]);

        string[] lines = data.text.Split('\n');
        float survived = new float();
        string dataToDisplay, sex;
        if(transform.parent.name == "PlottingData")
        {
            xAxis = GameObject.Find("PlottingData/Axis/xAxis/xAxisMenu/MenuItem/MenuText").GetComponent<TextMesh>();
            yAxis = GameObject.Find("PlottingData/Axis/yAxis/yAxisMenu/MenuItem/MenuText").GetComponent<TextMesh>();
            zAxis = GameObject.Find("PlottingData/Axis/zAxis/zAxisMenu/MenuItem/MenuText").GetComponent<TextMesh>();

            GameObject.Find("PlottingData/Axis/xAxis/xAxisMenu").transform.tag = "close";
            GameObject.Find("PlottingData/Axis/yAxis/yAxisMenu").transform.tag = "close";
            GameObject.Find("PlottingData/Axis/zAxis/zAxisMenu").transform.tag = "close";
        }
        else if(transform.parent.name == "PlottingData(Clone)")
        {
            xAxis = GameObject.Find("PlottingData(Clone)/Axis/xAxis/xAxisMenu/MenuItem/MenuText").GetComponent<TextMesh>();
            yAxis = GameObject.Find("PlottingData(Clone)/Axis/yAxis/yAxisMenu/MenuItem/MenuText").GetComponent<TextMesh>();
            zAxis = GameObject.Find("PlottingData(Clone)/Axis/zAxis/zAxisMenu/MenuItem/MenuText").GetComponent<TextMesh>();

            GameObject.Find("PlottingData(Clone)/Axis/xAxis/xAxisMenu").transform.tag = "close";
            GameObject.Find("PlottingData(Clone)/Axis/yAxis/yAxisMenu").transform.tag = "close";
            GameObject.Find("PlottingData(Clone)/Axis/zAxis/zAxisMenu").transform.tag = "close";
        }
        for (int i = 1; i < lines.Length - 1; i++)
        {
            //split line
            lines[i] = lines[i].Replace("\r", "");
            string[] attributes = lines[i].Split(',');
            materialColored = new Material(Shader.Find("Diffuse"));

            survived = float.Parse(attributes[1], CultureInfo.InvariantCulture.NumberFormat);

            Passenger_Class = updateOffsetPClass(attributes[2]);
            Age_Of_Passenger = updateOffsetAge(attributes[6]);
            Siblings_Spouses = updateOffsetSibSp(attributes[7]);
            Parents_Children = updateOffsetParCh(attributes[8]);


            sex = attributes[5];
            dataToDisplay = attributes[3] + " " + attributes[4] + " " + attributes[9] + " " + attributes[10] + " " + attributes[11] + " " + attributes[12];

            xValue = (float)this.GetType().GetField(xValueString).GetValue(this);
            yValue = (float)this.GetType().GetField(yValueString).GetValue(this);
            zValue = (float)this.GetType().GetField(zValueString).GetValue(this);

            position = new Vector3(xValue, yValue, zValue);

            if (sex == "male")
            {
                dataPoint = Instantiate(cube);
            }

            else
            {
                dataPoint = Instantiate(sphere);
            }


            if (survived == 0)
            {
                materialColored.color = new Color(0.6f, 0f, 0.04f);
                dataPoint.GetComponent<DataComponents>().color = new Color(0.6f, 0f, 0.04f);
            }
            else
            {
                materialColored.color = new Color(0.18f, 0.22f, 1);
                dataPoint.GetComponent<DataComponents>().color = new Color(0.18f, 0.22f, 1);
            }
            dataPoint.GetComponent<Renderer>().material = materialColored;
            dataPoint.transform.parent = parentPlot.transform;
            dataPoint.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
            dataPoint.transform.localPosition = position;
            dataPoint.name = "dataPoint";
            dataPositions.Add(position);
            dataPoint.tag = "reset";        // Need this tag to find and destroy these when loading the scene with new axis values
            if (dataPoint.transform.parent.parent.name == "PlottingData(Clone)")
            {
                dataPoint.tag = "reset1";
            }

            dataPoint.GetComponent<DataComponents>().passengerID = "PassengerID: " + getData(attributes[0]);
            dataPoint.GetComponent<DataComponents>().survived = survivedOrNot(attributes[1]);
            dataPoint.GetComponent<DataComponents>().pClass = "Ticket Class: " + getData(attributes[2]);
            dataPoint.GetComponent<DataComponents>().name = "Last Name: " + getData(attributes[3]) + "\r\nFirst Name:" + getData(attributes[4]);
            dataPoint.GetComponent<DataComponents>().name = dataPoint.GetComponent<DataComponents>().name.Replace("\"", "");
            dataPoint.GetComponent<DataComponents>().sex = "Sex: " + getData(attributes[5]);
            dataPoint.GetComponent<DataComponents>().age = "Age: " + getData(attributes[6]);
            dataPoint.GetComponent<DataComponents>().sibSp = "Number of Siblings/Spouses: " + getData(attributes[7]);
            dataPoint.GetComponent<DataComponents>().parCh = "Number of Parents/Children: " + getData(attributes[8]);
            dataPoint.GetComponent<DataComponents>().Ticket = "Ticket Number: " + getData(attributes[9]);
            dataPoint.GetComponent<DataComponents>().Fare = "Passenger Fare: " + getData(attributes[10]);
            dataPoint.GetComponent<DataComponents>().Cabin = "Cabin Number: " + getData(attributes[11]);
            dataPoint.GetComponent<DataComponents>().Embarked = "Embarked From: " + portAbbreviation(attributes[12]);
            controller.GetComponent<PointerEventListener>().listObjects.Add(dataPoint);
            list.Add(dataPoint);
        }

        if(gameObject.transform.parent.name == "PlottingData(Clone)")
        {
            foreach (Transform child in gameObject.transform)
            {
                if(child.tag == "reset")
                {
                    Destroy(child.gameObject);
                }
            }
        }

        }

    public TextMesh Details;
    public TextMesh PassengerName;
    public TextMesh Survived;
    public TextMesh Pclass;
    public TextMesh Sex;
    public TextMesh Age;
    public TextMesh Sibsb;
    public TextMesh Parch;
    public TextMesh Ticket;
    public TextMesh Fare;
    public TextMesh Cabin;
    public TextMesh Embarked;

    void Update()
    {
        foreach(GameObject data in list)
        {
            if(data!=null)
            {
                if (data.transform.localScale.x > 0)
                {
                    data.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
                }
                
            }
        }
        /*
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hitInfo;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            bool hit = Physics.Raycast(ray, out hitInfo, 100.0f);
            Debug.DrawLine(ray.origin, ray.direction * 100, Color.cyan, 25.0f);

            //Variables for menu items
            Transform parent;
            GameObject selectedMenu;
            string selectedText;

            if (hit)
            {
                Debug.Log("You selected the " + hitInfo.transform.name);
                if (hitInfo.transform.name == "MenuItem" && hitInfo.transform.parent.childCount == 1)
                {
                    menuItem.GetComponent<AxisMenu>().LoadMenu(hitInfo.transform);
                }

                else if (hitInfo.transform.name == "MenuItem(Clone)")
                {
                    Debug.Log(hitInfo.transform.name);
                    parent = hitInfo.transform.parent;
                    selectedMenu = hitInfo.transform.GetChild(0).gameObject;
                    selectedText = selectedMenu.GetComponent<TextMesh>().text;
                    this.updatePositions(parent, selectedText);

                }
                else
                {

                    PassengerName.text = hitInfo.transform.gameObject.transform.GetComponent<DataComponents>().name;
                    Survived.text = hitInfo.transform.gameObject.transform.GetComponent<DataComponents>().survived;
                    Pclass.text = hitInfo.transform.gameObject.transform.GetComponent<DataComponents>().pClass;
                    Sex.text = hitInfo.transform.gameObject.transform.GetComponent<DataComponents>().sex;
                    Age.text = hitInfo.transform.gameObject.transform.GetComponent<DataComponents>().age;
                    Sibsb.text = hitInfo.transform.gameObject.transform.GetComponent<DataComponents>().sibSp;
                    Parch.text = hitInfo.transform.gameObject.transform.GetComponent<DataComponents>().parCh;
                    Ticket.text = hitInfo.transform.gameObject.transform.GetComponent<DataComponents>().Ticket;
                    Fare.text = hitInfo.transform.gameObject.transform.GetComponent<DataComponents>().Fare;
                    Cabin.text = hitInfo.transform.gameObject.transform.GetComponent<DataComponents>().Cabin;
                    Embarked.text = hitInfo.transform.gameObject.transform.GetComponent<DataComponents>().Embarked;
                }
            }
            else
            {
                Debug.Log("No hit");
            }
        }
        */
        updateAxisLegend(xValueString, yValueString, zValueString);
    }


    private void updateAxisLegend(string x, string y, string z)
    {
        xAxis.text = x;
        yAxis.text = y;
        zAxis.text = z;
    }

    private float updateOffsetAge(string attribute)
    {
        float ageValueInteger;
        float ageValue = 0;
        
        if (float.TryParse(attribute, out ageValueInteger))
        {
            ageValue = (float)(ageValueInteger);
        }
        if(ageValue<10)
        {
            ageValue = (ageValue/80) * 1.15f + ageOffset1;
            ageOffset1 += 0.0005f;
        }
        else if(ageValue < 20)
        {
            ageValue = (ageValue / 80) * 1.15f + ageOffset2;
            ageOffset2 += 0.0005f;
        }
        else if(ageValue <30)
        {
            ageValue = (ageValue / 80) * 1.15f + ageOffset3;
            ageOffset3 += 0.0005f;
        }
        else if (ageValue < 40)
        {
            ageValue = (ageValue / 80) * 1.15f + ageOffset4;
            ageOffset4 += 0.0005f;
        }
        else if (ageValue < 50)
        {
            ageValue = (ageValue / 80) * 1.15f + ageOffset5;
            ageOffset5 += 0.0005f;
        }
        else if (ageValue < 60)
        {
            ageValue = (ageValue / 80) * 1.15f + ageOffset6;
            ageOffset6 += 0.0005f;
        }
        else if (ageValue < 70)
        {
            ageValue = (ageValue / 80) * 1.15f + ageOffset7;
            ageOffset7 += 0.0005f;
        }
        else
        {
            ageValue = (ageValue / 80) * 1.15f + ageOffset8;
            ageOffset8 += 0.0005f;
        }
        return ageValue;

    }

    private float updateOffsetSibSp(string attribute)
    {
        float sibSpValue = 0.0f;
        switch (int.Parse(attribute))
        {
            case 0:
                sibSpValue = (float.Parse(attribute)) + sibSpOffset0; 
                sibSpOffset0 += 0.0001f;
                break;
            case 1:
                sibSpValue = ((float.Parse(attribute)) / 5)*1.15f + sibSpOffset1; 
                sibSpOffset1 += 0.00001f;
                break;
            case 2:
                sibSpValue = (float.Parse(attribute) / 5) * 1.15f + sibSpOffset2; 
                sibSpOffset2 += 0.0001f;
                break;

            case 3:
                sibSpValue = (float.Parse(attribute) / 5) * 1.15f + sibSpOffset3;
                sibSpOffset3 += 0.0001f;
                break;
            case 4:
                sibSpValue = (float.Parse(attribute) / 5) * 1.15f + sibSpOffset4;
                sibSpOffset4 += 0.0001f;
                break;
            case 5:
                sibSpValue = (float.Parse(attribute) / 5) * 1.15f + sibSpOffset5;
                sibSpOffset5 += 0.0001f;
                break;
         /*   case 6:
                sibSpValue = (float.Parse(attribute) / 10) * 1.25f + sibSpOffset6;
                sibSpOffset6 += 0.0001f;
                break;
            case 7:
                sibSpValue = (float.Parse(attribute) / 10) * 1.25f + sibSpOffset7;
                sibSpOffset7 += 0.0001f;
                break;
            case 8:
                sibSpValue = (float.Parse(attribute) / 10) * 1.25f + sibSpOffset8;
                sibSpOffset8 += 0.0001f;
                break;*/
        }
        return sibSpValue;
    }

    private float updateOffsetParCh(string attribute)
    {
        float parChValue = float.Parse(attribute);

        if (parChValue == 0)
        {
            parChValue = (parChValue / 6) * 1.15f + parChOffset0;
            parChOffset0 += 0.00005f;
        }
        else if(parChValue == 1)
        {
            parChValue = (parChValue / 6) * 1.15f + parChOffset1;
            parChOffset1 += 0.0005f;
        }
        else if (parChValue == 2)
        {
            parChValue = (parChValue / 6) * 1.15f + parChOffset2;
            parChOffset2 += 0.0005f;
        }
        else if (parChValue == 3)
        {
            parChValue = (parChValue / 6) * 1.15f + parChOffset3;
            parChOffset3 += 0.0005f;
        }
        else if (parChValue == 4)
        {
            parChValue = (parChValue / 6) * 1.15f + parChOffset4;
            parChOffset4 += 0.0005f;
        }
        else if(parChValue == 5)
        {
            parChValue = (parChValue / 6) * 1.15f + parChOffset5;
            parChOffset5 += 0.0005f;
        }
        else
        {
            parChValue = (parChValue / 6) * 1.15f + parChOffset6;
            parChOffset6 += 0.0005f;
        }
        return parChValue;
    }



    private float updateOffsetPClass(string attribute)
    {
        float pClassValue = 0.0f;

        switch (int.Parse(attribute))
        {
            case 1:
                pClassValue = (float.Parse(attribute) / 3) + pClassOffset1;
            //    pClassOffset1 += 0.0005f;
                break;
            case 2:
                pClassValue = (float.Parse(attribute) / 3) + pClassOffset2;
         //       pClassOffset2 += 0.0005f;
                break;
            case 3:
                pClassValue = (float.Parse(attribute) / 3) + pClassOffset3;
         //       pClassOffset3 += 0.0005f;
                break;
        }
        return pClassValue;

    }

    private string getData(string attribute)
    {
        //if value is NULL
        if(attribute == "")
        {
            return "Unknown Data";
        }
        else
        {
            return attribute;
        }
    }

    private string portAbbreviation(string attribute)
    {
        // Abbreviations for different ports of Embarkation
        if (attribute == "C")
        {
            return "Cherbourg";
        }
        else if (attribute == "Q")
        {
            return "Queenstown";
        }
        else if (attribute == "S")
        {
            return "Southampton";
        }
        else
        {
            return "Unknown Data";
        }
    }

    private string survivedOrNot(string attribute)
    {
        if (attribute == "0")
        {
            return "Didn't Survive";
        }
        else
        {
            return "Survived";
        }
    }

    private void startAgain(Vector3 position, float x, float y, float z)
    {
        this.Start();
     }

    public void setSelectionSphere(Vector3 center, float radius)
    {
        dataMappedMaterial.SetFloat("_SelectionSphereRadiusSquared", radius * radius);
        dataMappedMaterial.SetVector("_SelectionSphereCenter", center);

        dataMappedTransparent.SetFloat("_SelectionSphereRadiusSquared", radius * radius);
        dataMappedTransparent.SetVector("_SelectionSphereCenter", center);


        //return;

        //update selected statistics TODO all that follows is not performant
        int count = 0;
        float squaredRad = radius * radius;

        Dictionary<string, int> selectedClasses = new Dictionary<string, int>();


        for (int i = 0; i < dataPositions.Count; i++)
        {
            Vector3 pos = dataPositions[i];
            Vector3 diff = pos - center;
            float squaredDistance = diff.x * diff.x + diff.y * diff.y + diff.z * diff.z;
            if (squaredDistance < squaredRad)
            {
                count++;
                string dataClass = classes[i];
                //insert class count
                if (!selectedClasses.ContainsKey(dataClass))
                {
                    selectedClasses[dataClass] = 1;
                }
                else
                {
                    int oldCount = selectedClasses[dataClass];
                    selectedClasses[dataClass] = ++oldCount;
                }
            }
        }
    }
    

    public void updatePositions(Transform parent, string text)
    {
        resetGameObjects = null;
        if(resetGameObjects!=null)
        {
            Array.Clear(resetGameObjects, 0, resetGameObjects.Length);
        }
        if(gameObject.transform.parent.name == "PlottingData")
        {
            resetGameObjects = GameObject.FindGameObjectsWithTag("reset");
        }
        else if(gameObject.transform.parent.name == "PlottingData(Clone)")
        {
            resetGameObjects = GameObject.FindGameObjectsWithTag("reset1");
        }
        if (parent.gameObject.name == "zAxisMenu")
        {
            zValueString = text;
        }
        else if(parent.gameObject.name == "yAxisMenu")
        {
            yValueString = text;
        }
        else
        {
            xValueString = text;
        }

        for (var i = 0; i < resetGameObjects.Length; i++)
        {
            Destroy(resetGameObjects[i]);       // Destroy all gameobjects with reset tag
        }
        // Start the scene with new axis value
        emptyPanel();
        this.Start();
    }

    private void emptyPanel()
    {

        GetComponent<DataPlotter>().PassengerName.text = "Passenger name: ";
        GetComponent<DataPlotter>().Survived.text = "Survived: ";
        GetComponent<DataPlotter>().Pclass.text = "Passenger Class: ";
        GetComponent<DataPlotter>().Sex.text = "Sex: ";
        GetComponent<DataPlotter>().Age.text = "Age: ";
        GetComponent<DataPlotter>().Sibsb.text = "Siblings/Spouses: ";
        GetComponent<DataPlotter>().Parch.text = "Parents/Children: ";
        GetComponent<DataPlotter>().Ticket.text = "Ticket Number: ";
        GetComponent<DataPlotter>().Fare.text = "Fare: ";
        GetComponent<DataPlotter>().Cabin.text = "Cabin: ";
        GetComponent<DataPlotter>().Embarked.text = "Embarked: ";
    }

}
