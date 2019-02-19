using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraZoomTrigger : MonoBehaviour //This is the trigger that is put on a trigger collider to zoom the camera out
{

    private CameraBossControl cameraScript;

	// Use this for initialization
	void Start ()
	{
	    cameraScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraBossControl>();
	}

    //if the player hits this trigger zoom camera out
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            cameraScript.currentCameraStates = CameraBossControl.CameraStates.moveUp;
            cameraScript.startTime = Time.time;
        }
    }
}
