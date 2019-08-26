using UnityEngine;
using System.Collections;
using System;
using VRTK;
using UnityEngine.Events;

[System.Serializable]
public enum MENU_ACTION
{
    ADD,
    DELETE,
    MOVE,
    ROTATE,
    SCALE,
    SELECTDATA
}


public class MenuButtonController : MonoBehaviour {

    public bool add = false;
    public bool remove = false;
    public bool select = true;
    public bool scale = false;
    public bool move = false;

    public UnityEvent addListeners;
    public UnityEvent deleteListeners;
    public UnityEvent moveListeners;
    //public UnityEvent rotationModeListeners;
    public UnityEvent scalingModeListeners;
    public UnityEvent selectDataListeners;

    public void AddModeSelected ()
    {
        Debug.Log("Add Mode Selected");
        addListeners.Invoke();
        add = true;
        remove = false;
        select = false;
        scale = false;
        move = false;
}

    public void RemoveModeSelected()
    {
        Debug.Log("Remove Mode Selected");
        deleteListeners.Invoke();
        add = false;
        remove = true;
        select = false;
        scale = false;
        move = false;
    }

    public void MoveModeSelected()
    {
        Debug.Log("Move Mode Selected");
        moveListeners.Invoke();
        add = false;
        remove = false;
        select = false;
        scale = false;
        move = true;
    }

    /*public void RotationModeSelected()
    {
        Debug.Log("Rotation Mode Selected");
        rotationModeListeners.Invoke();
    }*/

    public void ScalingModeSelected()
    {
        Debug.Log("Scaling Mode Selected");
        scalingModeListeners.Invoke();
        add = false;
        remove = false;
        select = false;
        scale = true;
        move = false;
    }

    public void SelectDataModeSelected()
    {
        Debug.Log("Add Mode Selected");
        selectDataListeners.Invoke();
        add = false;
        remove = false;
        select = true;
        scale = false;
        move = false;
    }

}
