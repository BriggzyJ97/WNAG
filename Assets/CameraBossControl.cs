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

    public float startTime;
    private float duration = 20f;

	// Update is called once per frame
	void Update () {
	    if (currentCameraStates == CameraStates.follow)
	    {

	    }
	    else if(currentCameraStates == CameraStates.moveUp)
	    {
            transform.SetParent(null);
	        if (Vector3.Distance(gameObject.transform.position, finalSpot.transform.position) > 0.5f)
	        {
	            float t = (Time.time - startTime) / duration;
                transform.position = new Vector3(Mathf.SmoothStep(transform.position.x, finalSpot.transform.position.x,t), Mathf.SmoothStep(transform.position.y, finalSpot.transform.position.y, t), Mathf.SmoothStep(transform.position.z, finalSpot.transform.position.z, t));
	        }
	        else
	        {
	            currentCameraStates = CameraStates.idle;
	        }
	    }else if (currentCameraStates == CameraStates.idle)
	    {

	    }
	}
}
