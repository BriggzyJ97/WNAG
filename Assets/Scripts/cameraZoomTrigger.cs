using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraZoomTrigger : MonoBehaviour
{

    private CameraBossControl camera;

	// Use this for initialization
	void Start ()
	{
	    camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraBossControl>();
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            camera.currentCameraStates = CameraBossControl.CameraStates.moveUp;
            camera.startTime = Time.time;
        }
    }
}
