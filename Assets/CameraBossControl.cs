using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBossControl : MonoBehaviour {

    public enum CameraStates
    {
        follow,
        moveUp,
        idle
    }

    public CameraStates currentCameraStates = CameraStates.follow;

    public GameObject player;
    public GameObject finalSpot;
    
	
	// Update is called once per frame
	void Update () {
	    if (currentCameraStates == CameraStates.follow)
	    {

	    }
	    else if(currentCameraStates == CameraStates.moveUp)
	    {

	    }else if (currentCameraStates == CameraStates.idle)
	    {

	    }
	}
}
