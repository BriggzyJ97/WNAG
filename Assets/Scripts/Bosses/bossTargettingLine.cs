using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossTargettingLine : MonoBehaviour //this script manages the bosses targetting lines
{

    public LayerMask TargetLineLayerMask;
    public LineRenderer tempLineRenderer;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	    RaycastHit hit;
	    //Raycast is used to set the targetting line position and then set the start and end point of the targetting laser's line renderer.
        if (Physics.Raycast(new Vector3(gameObject.transform.position.x, gameObject.transform.position.y-0.7f, gameObject.transform.position.z), gameObject.transform.TransformDirection(Vector3.right), out hit, Mathf.Infinity, TargetLineLayerMask))
	    {
	        //Debug.DrawRay(gameObject.transform.position, gameObject.transform.TransformDirection(Vector3.forward) * hit.distance, Color.red);
	        tempLineRenderer.gameObject.transform.position =
	            gameObject.transform.position - ((gameObject.transform.position - hit.point) / 2);
	        tempLineRenderer.SetPosition(0, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y-0.7f, gameObject.transform.position.z));
	        tempLineRenderer.SetPosition(1, hit.point);
	        //Debug.Log("Did Hit");
	    }
	    else
	    {
	        //Debug.DrawRay(sparksEmitter.transform.position, sparksEmitter.transform.TransformDirection(Vector3.forward) * 1000, Color.white);
	        //Debug.Log("Did not Hit");
	    }
    }
}
