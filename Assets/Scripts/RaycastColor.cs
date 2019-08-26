using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastColor : MonoBehaviour {
    private GameObject currentHitObject;
    private GameObject lastHitObject;
    private Color currentHitColor;
    private GameObject lastMenuObject;
    private GameObject currentMenuObject;
    private GameObject lastMainMenuObject;
    private GameObject currentMainMenuObject;
    public GameObject selectedOption;
    private Transform menuParent;
    private GameObject selectedObject;
    public GameObject dataPlot;
    public GameObject menuItem;
    public Color color = new Color(0,07f, 0f, 1f);
    private GameObject secondPlot;


    public GameObject plotMain;
    private Transform plotMainParent;

    private GameObject scaleUpButton;
    private GameObject scaleDownButton;
    private GameObject anchorButton;
    private GameObject moveButton;
    private GameObject resetButton;
    private GameObject copyButton;
    public GameObject secondGraph = null;
    public GameObject room;
    public GameObject lineToPoint;
    private bool cloneWasCreated = false;
    public bool copyButtonWasMoved = false;

    // the plotting data container
    private GameObject plottingData;
    // 1
    public GameObject laserPrefab;
    // 2
    private GameObject laser;
    // 3
    private Transform laserTransform;
    // 4
    private Vector3 hitPoint;

    private int layerMask;

    private SteamVR_TrackedObject trackedObj;

    private SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }

    void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }
    // Use this for initialization
    void Start () {
        laser = Instantiate(laserPrefab);
        laserTransform = laser.transform;
        layerMask = ~(1 << LayerMask.NameToLayer("Scatterplot"));
        
        plotMainParent = plotMain.transform.parent;
        

    }

    // Update is called once per frame
    void Update () {
        laser.GetComponent<Renderer>().material.color = color;
        RaycastHit hit;
        
        if(selectedOption.GetComponent<MenuButtonController>().move || selectedOption.GetComponent<MenuButtonController>().scale)
        {
            if (Physics.Raycast(trackedObj.transform.position, transform.forward, out hit, 100))
            {
                hitPoint = hit.point;
                ShowLaser(hit);
            }

            else
            {
                laser.SetActive(false);
            }
        }

        // 2
        else if (Physics.Raycast(trackedObj.transform.position, transform.forward, out hit, 100, layerMask))
         {
             hitPoint = hit.point;
             ShowLaser(hit);
            if (hit.transform.gameObject.name == "dataPoint" && selectedOption.GetComponent<MenuButtonController>().select)
            {
                selectedObject = hit.transform.gameObject;
                if (Controller.GetHairTriggerDown())
                {
                    foreach (GameObject dataObject in GameObject.FindGameObjectsWithTag("reset"))
                    {
                        dataObject.GetComponent<DataComponents>().wasPressed = false;
                    }
                    foreach (GameObject dataObject in GameObject.FindGameObjectsWithTag("reset1"))
                    {
                        dataObject.GetComponent<DataComponents>().wasPressed = false;
                    }
                    selectedObject.GetComponent<DataComponents>().wasPressed = true;
                    selectedObject.GetComponent<Renderer>().material.color = new Color(0.87f, 0.89f, 0.08f);
                    dataPlot.GetComponent<DataPlotter>().PassengerName.text = selectedObject.transform.gameObject.transform.GetComponent<DataComponents>().name;
                    dataPlot.GetComponent<DataPlotter>().Survived.text = selectedObject.transform.gameObject.transform.GetComponent<DataComponents>().survived;
                    dataPlot.GetComponent<DataPlotter>().Pclass.text = selectedObject.transform.gameObject.transform.GetComponent<DataComponents>().pClass;
                    dataPlot.GetComponent<DataPlotter>().Sex.text = selectedObject.transform.gameObject.transform.GetComponent<DataComponents>().sex;
                    dataPlot.GetComponent<DataPlotter>().Age.text = selectedObject.transform.gameObject.transform.GetComponent<DataComponents>().age;
                    dataPlot.GetComponent<DataPlotter>().Sibsb.text = selectedObject.transform.gameObject.transform.GetComponent<DataComponents>().sibSp;
                    dataPlot.GetComponent<DataPlotter>().Parch.text = selectedObject.transform.gameObject.transform.GetComponent<DataComponents>().parCh;
                    dataPlot.GetComponent<DataPlotter>().Ticket.text = selectedObject.transform.gameObject.transform.GetComponent<DataComponents>().Ticket;
                    dataPlot.GetComponent<DataPlotter>().Fare.text = selectedObject.transform.gameObject.transform.GetComponent<DataComponents>().Fare;
                    dataPlot.GetComponent<DataPlotter>().Cabin.text = selectedObject.transform.gameObject.transform.GetComponent<DataComponents>().Cabin;
                    dataPlot.GetComponent<DataPlotter>().Embarked.text = selectedObject.transform.gameObject.transform.GetComponent<DataComponents>().Embarked;
                }

                lastHitObject = currentHitObject;
                currentHitObject = hit.transform.gameObject;
                if (lastHitObject == currentHitObject)
                {
                    currentHitObject.GetComponent<DataComponents>().wasHit = true;
                }
                else if (lastHitObject != null)
                {
                    lastHitObject.GetComponent<DataComponents>().wasHit = false;
                    if (lastMenuObject != null)
                    {
                        lastMenuObject.GetComponent<AxisMenu>().menuWasHit = false;
                    }
                }

                //bring back original color if we hit MainMenu directly after DataPoint
                if (lastMainMenuObject != null)
                {
                    lastMainMenuObject.GetComponent<MenuItemColorChanger>().menuWasHit = false;
                }

                //bring back original color if we hit MenuClone directly after DataPopint
                if (lastMenuObject != null)
                {
                    lastMenuObject.GetComponent<AxisMenu>().menuWasHit = false;
                }
            }
            //Main Menu
            else if (hit.transform.gameObject.name == "MenuItem")
            {
                selectedObject = hit.transform.gameObject;
                if (Controller.GetHairTriggerDown() && selectedObject.tag == "Axis")
                {
                    if (selectedObject.transform.parent.tag == "close" && selectedObject.transform.parent.parent.parent.parent.name == "PlottingData")
                    {
                        menuItem.GetComponent<AxisMenu>().LoadMenu(selectedObject.transform);
                        selectedObject.transform.parent.tag = "open";
                    }
                    else if(selectedObject.transform.parent.tag == "open" && selectedObject.transform.parent.parent.parent.parent.name == "PlottingData")
                    {
                        dataPlot.GetComponent<DataPlotter>().updatePositions(selectedObject.transform.parent, selectedObject.transform.GetChild(0).gameObject.GetComponent<TextMesh>().text);
                    }

                    if (selectedObject.transform.parent.tag == "close" && selectedObject.transform.parent.parent.parent.parent.name == "PlottingData(Clone)")
                    {
                        menuItem.GetComponent<AxisMenu>().LoadMenu(selectedObject.transform);
                        selectedObject.transform.parent.tag = "open";
                    }
                    else if (selectedObject.transform.parent.tag == "open" && selectedObject.transform.parent.parent.parent.parent.name == "PlottingData(Clone)")
                    {
                        secondPlot.GetComponent<DataPlotter>().updatePositions(selectedObject.transform.parent, selectedObject.transform.GetChild(0).gameObject.GetComponent<TextMesh>().text);
                    }

                }

                lastMainMenuObject = currentMainMenuObject;
                currentMainMenuObject = hit.transform.gameObject;
                if (lastMainMenuObject == currentMainMenuObject)
                {
                    currentMainMenuObject.GetComponent<MenuItemColorChanger>().menuWasHit = true;
                }
                else if (lastMainMenuObject != null)
                {
                    lastMainMenuObject.GetComponent<MenuItemColorChanger>().menuWasHit = false;
                }

                //bring back original color if we hit MenuClone directly after MainMenu was hit
                if (lastMenuObject != null)
                {
                    lastMenuObject.GetComponent<AxisMenu>().menuWasHit = false;
                }

                //bring back original color if we hit DataPoint directly after MainMenu was hit
                if (lastHitObject != null)
                {
                    lastHitObject.GetComponent<DataComponents>().wasHit = false;
                }
            }

            else if (hit.transform.gameObject.name == "MenuItem(Clone)")
            {
                selectedObject = hit.transform.gameObject;
                if (Controller.GetHairTriggerDown())
                {
                    if(selectedObject.transform.parent.parent.parent.parent.name == "PlottingData")
                    {
                        dataPlot.GetComponent<DataPlotter>().updatePositions(selectedObject.transform.parent, selectedObject.transform.GetChild(0).GetComponent<TextMesh>().text);
                    }
                    
                    else if(selectedObject.transform.parent.parent.parent.parent.name == "PlottingData(Clone)")
                    {
                        secondPlot.GetComponent<DataPlotter>().updatePositions(selectedObject.transform.parent, selectedObject.transform.GetChild(0).GetComponent<TextMesh>().text);
                    }
                }
                lastMenuObject = currentMenuObject;
                currentMenuObject = hit.transform.gameObject;
                if (lastMenuObject == currentMenuObject)
                {
                    currentMenuObject.GetComponent<AxisMenu>().menuWasHit = true;
                }
                else if (lastMenuObject != null)
                {
                    lastMenuObject.GetComponent<AxisMenu>().menuWasHit = false;
                }

                //bring back original color if we hit MainMenu directly after MenuClone was hit
                if (lastMainMenuObject != null)
                {
                    lastMainMenuObject.GetComponent<MenuItemColorChanger>().menuWasHit = false;
                }

                //bring back original color if we hit DataPoint directly after MenuClone was hit
                if (lastHitObject != null)
                {
                    lastHitObject.GetComponent<DataComponents>().wasHit = false;
                }
            }
            else if (hit.transform.gameObject.name == "AnchorButton")
            {
                DeselectButtons();
                anchorButton = hit.transform.gameObject;
                anchorButton.GetComponent<ButtonScript>().wasHit = true;
                if (Controller.GetHairTriggerDown())
                {

                    if (anchorButton.transform.parent.parent.parent.name == "PlottingData")
                    {
                        plottingData = GameObject.Find("PlottingData");
                        plottingData.transform.position = new Vector3(plottingData.transform.position.x, plottingData.transform.position.y, plottingData.transform.position.z);
                        plottingData.transform.rotation = new Quaternion(0.0f, plottingData.transform.rotation.y, 0.0f, plottingData.transform.rotation.w);
                    }
                    else if (anchorButton.transform.parent.parent.parent.name == "PlottingData(Clone)")
                    {
                        plottingData = GameObject.Find("PlottingData(Clone)");
                        plottingData.transform.position = new Vector3(plottingData.transform.position.x, plottingData.transform.position.y, plottingData.transform.position.z);
                        plottingData.transform.rotation = new Quaternion(0.0f, plottingData.transform.rotation.y, 0.0f, plottingData.transform.rotation.w);
                    }
                }
            }
            
            else if (hit.transform.gameObject.name == "MoveButton")
            {
                DeselectButtons();
                moveButton = hit.transform.gameObject;
                moveButton.GetComponent<ButtonScript>().wasHit = true;
                if (Controller.GetHairTriggerDown())
                {
                    if (moveButton.transform.parent.parent.parent.name == "PlottingData")
                    {
                        plotMain.transform.parent = gameObject.transform;
                    }
                    else if(moveButton.transform.parent.parent.parent.name == "PlottingData(Clone)")
                    {
                        secondGraph.transform.parent = gameObject.transform;
                    }
                }

                else if(Controller.GetHairTriggerUp())
                {
                    if (moveButton.transform.parent.parent.parent.name == "PlottingData")
                    {
                        plotMain.transform.parent = plotMainParent;
                    }
                    else if (moveButton.transform.parent.parent.parent.name == "PlottingData(Clone)")
                    {
                        secondGraph.transform.parent = plotMainParent;
                    }
                }
            }
            
            else if (hit.transform.gameObject.name == "ResetButton")
            {
                DeselectButtons();
                resetButton = hit.transform.gameObject;
                resetButton.GetComponent<ButtonScript>().wasHit = true;
                if (Controller.GetHairTriggerDown())
                {
                    if (resetButton.transform.parent.parent.parent.name == "PlottingData")
                    {
                        plottingData = GameObject.Find("PlottingData");
                        plottingData.GetComponent<ResetPositions>().ResetPosition();
                    }
                    else if (resetButton.transform.parent.parent.parent.name == "PlottingData(Clone)")
                    {
                        plottingData = GameObject.Find("PlottingData(Clone)");
                        plottingData.GetComponent<ResetPositions>().ResetPosition();
                    }
                }
            }

            else if (hit.transform.gameObject.name == "CopyButton")
            {
                DeselectButtons();
                copyButton = hit.transform.gameObject;
                copyButton.GetComponent<ButtonScript>().wasHit = true;
                if (Controller.GetHairTriggerDown())
                {
                    copyButton.transform.parent = gameObject.transform;
                    copyButtonWasMoved = true;
                    if (secondGraph == null)
                       {
                            secondGraph = Instantiate(plotMain);
                            secondGraph.transform.parent = gameObject.transform;
                            secondGraph.transform.localPosition = copyButton.transform.localPosition;
                            //copyButton.transform.localScale = new Vector3 (0.1f, 0.001f, 0.1f);
                            secondPlot = secondGraph.transform.GetChild(1).gameObject;

                    }                    
                }
                else if (Controller.GetHairTriggerUp())
                {
                    copyButtonWasMoved = false;
                    copyButton.SetActive(false);
                    secondGraph.transform.localScale = new Vector3(1, 1, 1);
                    secondGraph.transform.parent = room.transform;
                    secondGraph.GetComponent<ResetPositions>().initialRotation = new Quaternion(0,secondGraph.transform.rotation.y,0,secondGraph.transform.rotation.w);
                    secondGraph.GetComponent<ResetPositions>().initialPosition = secondGraph.transform.position;
                }

                else if(copyButtonWasMoved)
                {
                    secondGraph.transform.localScale = new Vector3(0.51f, 0.51f, 0.51f);
                }
            }
            
            else if(hit.transform.gameObject.name == "ScalePlusButton")
            {
                DeselectButtons();
                scaleUpButton = hit.transform.gameObject;
                scaleUpButton.GetComponent<ButtonScript>().wasHit = true;
                if (Controller.GetHairTriggerDown())
                {
                    if (scaleUpButton.transform.parent.parent.parent.name == "PlottingData")
                    {
                        plotMain.transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
                    }
                    else if (scaleUpButton.transform.parent.parent.parent.name == "PlottingData(Clone)")
                    {
                        secondGraph.transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
                    }
                }
            }
            
            else if (hit.transform.gameObject.name == "ScaleMinusButton")
            {
                DeselectButtons();
                scaleDownButton = hit.transform.gameObject;
                scaleDownButton.GetComponent<ButtonScript>().wasHit = true;
                if (Controller.GetHairTriggerDown())
                {
                    if (scaleDownButton.transform.parent.parent.parent.name == "PlottingData")
                    {
                        plotMain.transform.localScale -= new Vector3(0.1f, 0.1f, 0.1f);
                    }
                    else if (scaleDownButton.transform.parent.parent.parent.name == "PlottingData(Clone)")
                    {
                        secondGraph.transform.localScale -= new Vector3(0.1f, 0.1f, 0.1f);
                    }
                }
            }

            else if (hit.transform.gameObject.tag == "OutdoorWall")
            {
                if (lastHitObject != null)
                {
                    lastHitObject.GetComponent<DataComponents>().wasHit = false;
                }

                if (lastMenuObject != null)
                {
                    lastMenuObject.GetComponent<AxisMenu>().menuWasHit = false;
                }

                if (lastMainMenuObject != null)
                {
                    lastMainMenuObject.GetComponent<MenuItemColorChanger>().menuWasHit = false;
                }
                DeselectButtons();
                if (Controller.GetHairTriggerDown())
                {
                    if (lastHitObject != null)
                    {
                        lastHitObject.GetComponent<DataComponents>().wasPressed = false;
                    }
                    lineToPoint.GetComponent<LineDrawing>().itemIsSelected = false;

                    if(secondPlot!=null)
                    {
                        foreach (GameObject data in secondPlot.GetComponent<DataPlotter>().list)
                        {
                            if (data.GetComponent<DataComponents>().wasPressed == true)
                            {
                                data.GetComponent<DataComponents>().wasPressed = false;
                            }
                        }
                    }

                    foreach (GameObject data in dataPlot.GetComponent<DataPlotter>().list)
                    {
                        if (data.GetComponent<DataComponents>().wasPressed == true)
                        {
                            data.GetComponent<DataComponents>().wasPressed = false;
                        }

                        dataPlot.GetComponent<DataPlotter>().PassengerName.text = "Passenger name: ";
                        dataPlot.GetComponent<DataPlotter>().Survived.text = "Survived: ";
                        dataPlot.GetComponent<DataPlotter>().Pclass.text = "Passenger Class: ";
                        dataPlot.GetComponent<DataPlotter>().Sex.text = "Sex: ";
                        dataPlot.GetComponent<DataPlotter>().Age.text = "Age: ";
                        dataPlot.GetComponent<DataPlotter>().Sibsb.text = "Siblings/Spouses: ";
                        dataPlot.GetComponent<DataPlotter>().Parch.text = "Parents/Children: ";
                        dataPlot.GetComponent<DataPlotter>().Ticket.text = "Ticket Number: ";
                        dataPlot.GetComponent<DataPlotter>().Fare.text = "Fare: ";
                        dataPlot.GetComponent<DataPlotter>().Cabin.text = "Cabin: ";
                        dataPlot.GetComponent<DataPlotter>().Embarked.text = "Embarked: ";
                    }


                }                
            }
             
          }

         else
          {
            if(currentHitObject != null || lastHitObject != null)
            {
              currentHitObject.GetComponent<DataComponents>().wasHit = false;
                lastHitObject.GetComponent<DataComponents>().wasHit = false;
            }

            if(currentMenuObject != null || lastMenuObject!=null)
            {
                currentMenuObject.GetComponent<AxisMenu>().menuWasHit = false;
                lastMenuObject.GetComponent<AxisMenu>().menuWasHit = false;
            }
             laser.SetActive(false);
          }

        // Toggle details panel when grip is pressed
        if (Controller.GetPressDown(SteamVR_Controller.ButtonMask.Grip))
        {
            foreach(Transform child in gameObject.transform)
            {
                if(child.gameObject.name == "Plane")
                {
                    if(child.gameObject.activeSelf)
                    {
                        child.gameObject.SetActive(false);
                    }
                    else
                    {
                        child.gameObject.SetActive(true);
                    }
                }
            }
        }

    }

    private void ShowLaser(RaycastHit hit)
    {
        // 1
       laser.SetActive(true);
        // 2
        laserTransform.position = Vector3.Lerp(trackedObj.transform.position, hitPoint, .5f);
        // 3
        laserTransform.LookAt(hitPoint);
        // 4
        laserTransform.localScale = new Vector3(laserTransform.localScale.x, laserTransform.localScale.y, hit.distance);
    }


    //deselecting buttons, setting color back to normal instead of yellow
    private void DeselectButtons()
    {
        if (resetButton != null)
        {
            resetButton.GetComponent<ButtonScript>().wasHit = false;
        }
        if (copyButton != null)
        {
            copyButton.GetComponent<ButtonScript>().wasHit = false;
        }
        if (scaleDownButton != null)
        {
            scaleDownButton.GetComponent<ButtonScript>().wasHit = false;
        }
        if (scaleUpButton != null)
        {
            scaleUpButton.GetComponent<ButtonScript>().wasHit = false;
        }
        if (anchorButton != null)
        {
            anchorButton.GetComponent<ButtonScript>().wasHit = false;
        }
        if (moveButton != null)
        {
            moveButton.GetComponent<ButtonScript>().wasHit = false;
        }
    }

    private void ClonePlot()
    {

    }
}
