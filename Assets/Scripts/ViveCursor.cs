using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ViveCursor : MonoBehaviour {
    public enum AxisType
    {
        XAxis,
        ZAxis
    }
    /* Data to be Changed*/
    /////////////////////
    public GameObject Details;
    public GameObject PassengerName;
    public GameObject Survived;
    public GameObject Pclass;
    public GameObject Sex;
    public GameObject Age;
    public GameObject SibSp;
    public GameObject ParCh;
    public GameObject Ticket;
    public GameObject Fare;
    public GameObject Cabin;
    public GameObject Embarked;
    /////////////////////

    SteamVR_Controller.Device device;
    SteamVR_TrackedObject controller;

    public Color color;
    public float thickness = 0.002f;    
    public AxisType facingAxis = AxisType.XAxis;
    public float length = 100f;
    public bool showCursor = true;

    GameObject holder;
    GameObject pointer;
    GameObject cursor;

    Vector3 cursorScale = new Vector3(0.05f, 0.05f, 0.05f);
    float contactDistance = 0f;
    Transform contactTarget = null;

    void SetPointerTransform(float setLength, float setThicknes)
    {
        //if the additional decimal isn't added then the beam position glitches
        float beamPosition = setLength / (2 + 0.00001f);

        if (facingAxis == AxisType.XAxis)
        {
            pointer.transform.localScale = new Vector3(setLength, setThicknes, setThicknes);
            pointer.transform.localPosition = new Vector3(beamPosition, 0f, 0f);
            if (showCursor)
            {
                cursor.transform.localPosition = new Vector3(setLength - cursor.transform.localScale.x, 0f, 0f);
            }
        } else
        {
            pointer.transform.localScale = new Vector3(setThicknes, setThicknes, setLength);
            pointer.transform.localPosition = new Vector3(0f, 0f, beamPosition);

            if (showCursor)
            {
                cursor.transform.localPosition = new Vector3(0f, 0f, setLength - cursor.transform.localScale.z);
            }
        }
    }

    // Use this for initialization
    void Start () {
        controller = gameObject.GetComponent<SteamVR_TrackedObject>();

        Material newMaterial = new Material(Shader.Find("Unlit/Color"));
        newMaterial.SetColor("_Color", color);
        
        holder = new GameObject();
        holder.transform.parent = this.transform;
        holder.transform.localPosition = Vector3.zero;

        pointer = GameObject.CreatePrimitive(PrimitiveType.Cube);
        pointer.transform.parent = holder.transform;
        pointer.GetComponent<MeshRenderer>().material = newMaterial;

        pointer.GetComponent<BoxCollider>().isTrigger = true;
        pointer.AddComponent<Rigidbody>().isKinematic = true;
        pointer.layer = 2;

        if (showCursor)
        {
            cursor = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            cursor.transform.parent = holder.transform;
            cursor.GetComponent<MeshRenderer>().material = newMaterial;
            cursor.transform.localScale = cursorScale;

            cursor.GetComponent<SphereCollider>().isTrigger = true;
            cursor.AddComponent<Rigidbody>().isKinematic = true;
            cursor.layer = 2;
        }

        SetPointerTransform(length, thickness);        
    }

    float GetBeamLength(bool bHit, RaycastHit hit)
    {
        float actualLength = length;

        //reset if beam not hitting or hitting new target
        if (!bHit || (contactTarget && contactTarget != hit.transform))
        {
            contactDistance = 0f;
            contactTarget = null;
        }

        //check if beam has hit a new target
        if (bHit)
        {
            if (hit.distance <= 0)
            {

            }
            contactDistance = hit.distance;
            contactTarget = hit.transform;
        }

        //adjust beam length if something is blocking it
        if (bHit && contactDistance < length)
        {
            actualLength = contactDistance;
        }

        if (actualLength <= 0)
        {
            actualLength = length;
        }

        return actualLength; ;
    }
	
	void Update () {
        Ray raycast = new Ray(transform.position, transform.forward);
        
        RaycastHit hitObject;
        bool rayHit = Physics.Raycast(raycast, out hitObject);

        if(rayHit)
        {
            device = SteamVR_Controller.Input((int)controller.index);
            if (device.GetPress(SteamVR_Controller.ButtonMask.Trigger))
            {
                PassengerName.GetComponent<TextMesh>().text = hitObject.transform.gameObject.transform.GetComponent<DataComponents>().name;
                Survived.GetComponent<TextMesh>().text = hitObject.transform.gameObject.transform.GetComponent<DataComponents>().survived;
                Pclass.GetComponent<TextMesh>().text = hitObject.transform.gameObject.transform.GetComponent<DataComponents>().pClass;
                Sex.GetComponent<TextMesh>().text = hitObject.transform.gameObject.transform.GetComponent<DataComponents>().sex;
                Age.GetComponent<TextMesh>().text = hitObject.transform.gameObject.transform.GetComponent<DataComponents>().age;
                SibSp.GetComponent<TextMesh>().text = hitObject.transform.gameObject.transform.GetComponent<DataComponents>().sibSp;
                ParCh.GetComponent<TextMesh>().text = hitObject.transform.gameObject.transform.GetComponent<DataComponents>().parCh;
                Ticket.GetComponent<TextMesh>().text = hitObject.transform.gameObject.transform.GetComponent<DataComponents>().Ticket;
                Fare.GetComponent<TextMesh>().text = hitObject.transform.gameObject.transform.GetComponent<DataComponents>().Fare;
                Cabin.GetComponent<TextMesh>().text = hitObject.transform.gameObject.transform.GetComponent<DataComponents>().Cabin;
                Embarked.GetComponent<TextMesh>().text = hitObject.transform.gameObject.transform.GetComponent<DataComponents>().Embarked;
            }
        }

        float beamLength = GetBeamLength(rayHit, hitObject);
        SetPointerTransform(beamLength, thickness);
    }
}