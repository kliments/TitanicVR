using UnityEngine;
using System.Collections;
using VRTK;
using System.Collections.Generic;

[System.Serializable]
public class PointerEventListener : MonoBehaviour {

    //all the game objects
    public List<GameObject> listObjects;

    // Axis Menu item
    public GameObject menuItem;

    //Data Plotter
    public GameObject DataPlot;


    //needd because as of 5.4 properties are not shown in the unity editor
    public MENU_ACTION defaultMenuAction = MENU_ACTION.ADD;

    //internal variable for the action to perform
    private MENU_ACTION menuAction= MENU_ACTION.ADD;

    //sets the menu action and the correct colors
    public MENU_ACTION MenuAction {
        get { return menuAction; }
        set
        {
            switch (value)
            {
                case MENU_ACTION.ADD:
                    setAddMode();
                    break;
                case MENU_ACTION.DELETE:
                    setDeleteMode();
                    break;
                case MENU_ACTION.MOVE:
                    setMoveMode();
                    break;
                case MENU_ACTION.ROTATE:
                    setRotationMode();
                    break;
                case MENU_ACTION.SCALE:
                    setScalingMode();
                    break;
                case MENU_ACTION.SELECTDATA:
                    setSelectDataMode();
                    break;
                default:
                    setSelectDataMode();
                    break;
            }
        }
    }

public LayerMask layersToIgnoreAdd = Physics.IgnoreRaycastLayer;
    public LayerMask layersToIgnoreModify = Physics.IgnoreRaycastLayer;
    public LayerMask layersToIgnoreSelect = Physics.IgnoreRaycastLayer;

    public Color addColor;
    public Color deleteColor;
    public Color moveColor;
    public Color rotateColor;
    public Color scaleColor;
    public Color selectDataColor;

    public GameObject BackUpScatterplot;
    VRTK_SimplePointer pointer;
    VRTK_ControllerEvents controller;

    GameObject selection = null;


    /// <summary>
    /// Prevents the selected object from being changed
    /// </summary>
    bool fixSelection = false;

    /// <summary>
    /// Indicates whether the rotation of the selected object should be updated
    /// </summary>
    bool updateRotation = false;
    Vector3 initialRotationEulerAngles;
    Vector3 initialRotationDiff;

    /// <summary>
    /// Indicates whether the scalingo f the selected object should be updated
    /// </summary>
    bool updateScaling = false;
    Vector3 initialPosition;
    Vector3 initialScale;

    private void Start()
    {


        if (GetComponent<VRTK_SimplePointer>() == null || GetComponent<VRTK_ControllerEvents>() == null)
        {
            Debug.LogError("PointerEventListener is required to be attached to a SteamVR Controller that has the VRTK_SimplePointer script attached to it");
            return;
        }

        pointer = GetComponent<VRTK_SimplePointer>();
        controller = GetComponent<VRTK_ControllerEvents>();

        //Setup pointer event listeners
        pointer.DestinationMarkerEnter += new DestinationMarkerEventHandler(selectObject);
        pointer.DestinationMarkerExit += new DestinationMarkerEventHandler(deselectObject);
        pointer.DestinationMarkerSet += new DestinationMarkerEventHandler(DoAction);

        //Setup controller button event Listener;
        controller.TriggerPressed += new ControllerInteractionEventHandler(triggerPressed);
        controller.TriggerReleased += new ControllerInteractionEventHandler(triggerReleased);

        //initial mode
        MenuAction = defaultMenuAction;
    }

    private void Update()
    {
        if (pointer==null)
        {
            pointer = GetComponent<VRTK_SimplePointer>();
        }
        //adapt rotation of the selection to track the parent
        if (updateRotation)
        {
            selection.transform.rotation = Quaternion.FromToRotation(initialRotationDiff,selection.transform.position-gameObject.transform.position)*Quaternion.Euler(initialRotationEulerAngles);
        }

        //adapt scaling based on the difference in height from the original starting point of the selection
        if(updateScaling)
        {
            float scaleDiff = 1+(gameObject.transform.position.y-initialPosition.y)*1.2f;
            selection.transform.localScale = initialScale*scaleDiff;
        }
    }

    private void DebugLogger(uint index, string action, Transform target, float distance, Vector3 tipPosition)
    {
        string targetName = (target ? target.name : "<NO VALID TARGET>");
        Debug.Log("Controller on index '" + index + "' is " + action + " at a distance of " + distance + " on object named " + targetName + " - the pointer tip position is/was: " + tipPosition);
    }

    private void selectObject(object sender, DestinationMarkerEventArgs e)
    {

        if (fixSelection)
        {
            return;
        }

        if(!e.target)
        {
            selection = null;
            return;
        }
        selection = e.target.gameObject;

    }

    private void deselectObject(object sender, DestinationMarkerEventArgs e)
    {
        if (fixSelection)
        {
            return;
        }

        selection = null;
    }

    /// <summary>
    /// Performs an action (e.g. creating/deleting an data object)
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void DoAction(object sender, DestinationMarkerEventArgs e)
    {
        Transform target = e.target;
        if(!target)
        {
            Debug.LogError("NO VALID TArget");
        }

        //decide what to do
        switch(menuAction)
        {
            case MENU_ACTION.ADD:
                addNewObject(e.destinationPosition);
                break;
            case MENU_ACTION.DELETE:
                deleteObject(e.target.gameObject);
                break;
            case MENU_ACTION.SELECTDATA:
                selectDataMode(e.target.gameObject);
                break;
            default:
                break;
        }

    }

    /// <summary>
    /// Determines what happens when the trigger button is pressed outside of a destination event.
    /// </summary>
    private void triggerPressed(object sender,ControllerInteractionEventArgs e)
    {
        if(selection == null)
        {
            return;
        }

        switch(menuAction)
        {
            case MENU_ACTION.MOVE:
                moveSelection(selection, true);
                break;
            case MENU_ACTION.ROTATE:
                rotateSelection(selection, true);
                break;
            case MENU_ACTION.SCALE:
                scaleSelection(selection, true);
                break;
            default:
                break;
        }
    }

    private void triggerReleased(object sender,ControllerInteractionEventArgs e)
    {
        if(selection == null)
        {
            return;
        }

        switch(menuAction)
        {
            case MENU_ACTION.MOVE:
                moveSelection(selection, false);
                break;
            case MENU_ACTION.ROTATE:
                rotateSelection(selection, false);
                break;
            case MENU_ACTION.SCALE:
                scaleSelection(selection, false);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Adds a new Object to the world
    /// </summary>
    /// <param name="position">The world position of where to add the object</param>
    private void addNewObject(Vector3 position)
    {
        /*
        GameObject addObject = GameObject.FindGameObjectWithTag("ScatterplotObject");
        
        //draw backup scatterplot
        if (addObject ==null )
        {
            if (BackUpScatterplot == null)
            {
                return;
            }
            Instantiate(BackUpScatterplot, position + new Vector3(0, 0.7f, 0), Quaternion.identity);
            return;
        }

        Instantiate(addObject,position+new Vector3(0,0.7f,0),Quaternion.identity);
        */

    }

    /// <summary>
    /// Deletes an object from the world
    /// </summary>
    /// <param name="selectedObject">The object to remove</param>
    private void deleteObject(GameObject selectedObject)
    {
        //Dataset choosers wont be affected
        if (selectedObject.tag == "WallChooser")
        {
            return;
        }
        Destroy(selectedObject);

    }


    /// <summary>
    /// Creates new object with the selected data from the wall
    /// </summary>
    /// <param name="selectedObject">Creates new </param>
    private void selectDataMode(GameObject selectedObject)
    {

    }
    /// <summary>
    /// Starts or Ends the movement of a selected object by parenting it to the controller
    /// </summary>
    /// <param name="selectedObject"> The selected object to move</param>
    /// <param name="start">if true the object is parented to the controller. if false it's unparented again</param>
    private void moveSelection(GameObject selectedObject,bool start)
    {
        //Dataset choosers wont be affected
        if (selectedObject.layer == 8)
        {
            return;
        }

        if (start)
        {
            selectedObject.transform.parent = gameObject.transform;
            //disallow the changing of the selection while the move is progress
            fixSelection = true;
        }
        else
        {
            selectedObject.transform.parent = null;

            ////TODO remove for release? (restore parent if it changed)
            DesiredParent desiredParent = selectedObject.GetComponent<DesiredParent>();
            if (desiredParent != null)
            {
                if (desiredParent.desiredParent != null)
                {
                    selectedObject.transform.parent = desiredParent.desiredParent.transform;
                }
            }


            //free selection again
            fixSelection = false;
        }
    }

    /// <summary>
    /// Starts or Ends the rotation of a selected object by having it track the location of the controller
    /// </summary>
    /// <param name="selectedObject"> The selected object to rotate</param>
    /// <param name="start">if true the rotation mode is started if false it's disabled</param>
    private void rotateSelection(GameObject selectedObject, bool start)
    {
        //Dataset choosers wont be affected
        if (selectedObject.tag == "WallChooser")
        {
            return;
        }

        if (start)
        {
            //disallow the changing of the selection while the move is progress
            fixSelection = true;
            //enable updating the rotation
            updateRotation = true;

            //set the original rotation vectors
            initialRotationEulerAngles = selectedObject.transform.rotation.eulerAngles;
            initialRotationDiff = selectedObject.transform.position - gameObject.transform.position;
        }
        else
        {
            //free selection again
            fixSelection = false;
            //disable updatin the roation
            updateRotation = false;

        }
    }

    /// <summary>
    /// Starts or Ends the rotation of a selected object by tracking the height comoponent of the controller
    /// </summary>
    /// <param name="selectedObject">The selected object to rotate</param>
    /// <param name="start">if true the scaling mode is started if false it's disabled</param>
    private void scaleSelection(GameObject selectedObject,bool start)
    {
        //Dataset choosers wont be affected
        if (selectedObject.tag == "WallChooser")
        {
            return;
        }

        if (start)
        {
            //dissalow the changing of the selection while the scaling is in process
            fixSelection = true;
            //enable updating the rotation
            updateScaling = true;

            //inital position of the controller (to compare the 
            initialPosition = gameObject.transform.position;
            initialScale = selectedObject.transform.localScale;
        }
        else
        {
            //free selection again
            fixSelection = false;
            //disable scale update
            updateScaling = false;

        }
    }

    public void setAddMode()
    {
        menuAction = MENU_ACTION.ADD;
        pointer.layersToIgnore = layersToIgnoreAdd;
        pointer.pointerHitColor = addColor;
        pointer.pointerMissColor = new Color(addColor.r, addColor.g, addColor.b, 0.3f);
        GetComponent<RaycastColor>().color = addColor;
    }

    public void setDeleteMode()
    {
        menuAction = MENU_ACTION.DELETE;
        pointer.layersToIgnore = layersToIgnoreModify;
        pointer.pointerHitColor = deleteColor;
        pointer.pointerMissColor = new Color(deleteColor.r, deleteColor.g, deleteColor.b, 0.3f);
        GetComponent<RaycastColor>().color = deleteColor;
    }

    public void setMoveMode()
    {
        menuAction = MENU_ACTION.MOVE;
        GetComponent<VRTK_SimplePointer>().layersToIgnore = layersToIgnoreModify;
        pointer.pointerHitColor = moveColor;
        pointer.pointerMissColor = new Color(moveColor.r, moveColor.g, moveColor.b, 0.3f);
        GetComponent<RaycastColor>().color = moveColor;
    }

    public void setRotationMode()
    {
        menuAction = MENU_ACTION.ROTATE;
        GetComponent<VRTK_SimplePointer>().layersToIgnore = layersToIgnoreModify;
        pointer.pointerHitColor = rotateColor;
        pointer.pointerMissColor = new Color(rotateColor.r, rotateColor.g, rotateColor.b, 0.3f);
        GetComponent<RaycastColor>().color = rotateColor;
    }
    public void setSelectDataMode()
    {
        menuAction = MENU_ACTION.SELECTDATA;
        GetComponent<VRTK_SimplePointer>().layersToIgnore = layersToIgnoreSelect;
        pointer.pointerHitColor = selectDataColor;
        pointer.pointerMissColor = new Color(selectDataColor.r, selectDataColor.g, selectDataColor.b, 0.3f);
        GetComponent<RaycastColor>().color = selectDataColor;
    }

    public void setScalingMode()
    {
        menuAction = MENU_ACTION.SCALE;
        GetComponent<VRTK_SimplePointer>().layersToIgnore = layersToIgnoreModify;
        pointer.pointerHitColor = scaleColor;
        pointer.pointerMissColor = new Color(scaleColor.r, scaleColor.g, scaleColor.b, 0.3f);
        GetComponent<RaycastColor>().color = scaleColor;
    }
}
