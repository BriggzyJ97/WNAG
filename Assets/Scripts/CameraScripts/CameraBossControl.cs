using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBossControl : MonoBehaviour {
    //This script moves the camera in the boss level so that it starts by following the player then zooms out to stationary position
    //also does screen shake
    #region Variables
    public enum CameraStates//these are the different states of the camera
    {
        follow,
        moveUp,
        idle
    }

    public CameraStates currentCameraStates = CameraStates.follow;

    public GameObject player;//the player gameObject
    public GameObject finalSpot;//where the camera should be during the boss fight

    public float startTime;//this is the start time of either movement or screen shake, to be able to use smoothstep based on difference between start time and current time
    public float duration = 20f;//duration of movement

    public float ScreenShakeAmount = 0;
    #endregion

    // Update is called once per frame
    void Update () {
        // apply screen shake by moving the camera randomly in a sphere multiplied by amount of screen shake
        if (ScreenShakeAmount>0)
	    {
	        transform.localPosition = transform.localPosition + Random.insideUnitSphere * ScreenShakeAmount;
        }

        // never lets screen shake go negative
        else
        if (ScreenShakeAmount<0)
	    {
	        ScreenShakeAmount = 0;
	    }

        
        if (currentCameraStates == CameraStates.follow)
	    {

	    }
	    else if(currentCameraStates == CameraStates.moveUp)
	    {
	        //stop camera being child of player
            if (transform.parent!=null)
	        {
	            transform.SetParent(null);
            }

	        //move camera to resting spot then change to idle state
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
            //after screen-shake, move camera back to proper position
            if (ScreenShakeAmount==0&&Vector3.Distance(transform.position, finalSpot.transform.position)>0.5f)
            {
                float t = (Time.time - startTime) / duration;
                transform.position = new Vector3(Mathf.SmoothStep(transform.position.x, finalSpot.transform.position.x, t), Mathf.SmoothStep(transform.position.y, finalSpot.transform.position.y, t), Mathf.SmoothStep(transform.position.z, finalSpot.transform.position.z, t));
            }
            
        }
	}
}
