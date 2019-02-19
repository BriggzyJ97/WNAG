using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class railController : MonoBehaviour
{
    //this script controls the rail mechanic
    #region Variables

    public float railMoveSpeed;// movement speed of rail
    private float widthAdjustmentMultiplier; //scaling multiplier based on the width of the rail

    public Transform endStop1;//one end of the rail
    public Transform endStop2;//the other end of the rail

    public Transform rail1; //the rail object

    public GameObject movingObject; //the object on the rail

    public string startPoint; //where the rail starts either left middle or right

    public Transform endPointLeft; //where the rail object ends
    public Transform endPointRight; //where the rail object ends

    public bool isLeftBlocked = false; //is the left side of the rail object blocked, detected by external script
    public bool isRightBlocked = false; //is the left side of the rail object blocked, detected by external script

    public bool isPlayerSprinting = false; //detection if the player is sprinting when touching the object

    public enum railMovement //movement direction of object
    {
        idle,
        movingLeft,
        movingRight
    }

    public railMovement CurrentDirectionOfRailMovement = railMovement.idle;

    #endregion

    // position rail between the end points and make them look at each other, scaling correctly. 
    //Set the endPoints depending on width of object so that it ends by hitting the end rails
    //set the position and rotation of moving object depending on its start point 
    void Start () {
        // position rail between the end points and make them look at each other
        endStop1.LookAt(endStop2);
        endStop2.LookAt(endStop1);
	    Vector3 VectorThreeBetweenEndPoints = endStop1.position - endStop2.position;
	    rail1.position = endStop1.position - (VectorThreeBetweenEndPoints / 2);
        rail1.LookAt(endStop1.position);
        rail1.Rotate(0,90,0);

        //scale the rail
	    float DistanceBetweenEndPoints = Vector3.Distance(endStop1.position, endStop2.position);
        rail1.localScale = new Vector3((DistanceBetweenEndPoints-0.167f)/9.16f,1,1);
	    
        //set the endPoints depending on width of object so that it ends by hitting the end rails
	    float WidthOfMovingObject = movingObject.GetComponent<Renderer>().bounds.size.x;
        Debug.Log(WidthOfMovingObject);
	    widthAdjustmentMultiplier = 0.642f - (0.0507f * Mathf.Log(WidthOfMovingObject));
        Debug.Log(widthAdjustmentMultiplier);
	    endPointLeft.position = endStop1.transform.position - ((VectorThreeBetweenEndPoints.normalized) * (WidthOfMovingObject *widthAdjustmentMultiplier));
	    endPointRight.position = endStop2.transform.position + ((VectorThreeBetweenEndPoints.normalized) * (WidthOfMovingObject *widthAdjustmentMultiplier));

        //set the position and rotation of moving object depending on its start point 
	    if (startPoint == "left")
	    {
	        movingObject.transform.position = endPointLeft.position;
	        movingObject.transform.localPosition = new Vector3(movingObject.transform.localPosition.x, 1.48f, movingObject.transform.localPosition.z);
	    }
	    else if (startPoint == "right")
	    {
	        movingObject.transform.position = endPointRight.position;
	        movingObject.transform.localPosition = new Vector3(movingObject.transform.localPosition.x, 1.48f, movingObject.transform.localPosition.z);
	    }
	    else
	    {
	        movingObject.transform.position = endStop1.position - (VectorThreeBetweenEndPoints / 2);
	        movingObject.transform.localPosition = new Vector3(movingObject.transform.localPosition.x, 1.48f, movingObject.transform.localPosition.z);
	    }

	    movingObject.transform.LookAt(new Vector3(endStop1.transform.position.x, movingObject.transform.localPosition.y, endStop1.transform.position.z));
	    movingObject.transform.rotation = Quaternion.Euler(0, movingObject.transform.rotation.eulerAngles.y, 0);
    }
	
	// Update is called once per frame
	void Update () {

        //if the rail is moving left and is not blocked, move left, double speed if player is sprinting
	    if (CurrentDirectionOfRailMovement == railMovement.movingLeft&&isRightBlocked==false)
	    {
	        if (isPlayerSprinting==true)
	        {
	            movingObject.transform.position = Vector3.MoveTowards(movingObject.transform.position, new Vector3(endPointLeft.position.x,movingObject.transform.position.y, endPointLeft.position.z), 
	                railMoveSpeed *2f* Time.deltaTime);
            }
	        else
	        {
	            movingObject.transform.position = Vector3.MoveTowards(movingObject.transform.position, new Vector3(endPointLeft.position.x, movingObject.transform.position.y, endPointLeft.position.z),
	                railMoveSpeed * Time.deltaTime);
            }
	        
	    }
        //same as above with right
	    else if (CurrentDirectionOfRailMovement==railMovement.movingRight&&isLeftBlocked==false)
	    {
	        if (isPlayerSprinting == true)
	        {
	            movingObject.transform.position = Vector3.MoveTowards(movingObject.transform.position, new Vector3(endPointRight.position.x, movingObject.transform.position.y, endPointRight.position.z),
	                railMoveSpeed *2f* Time.deltaTime);
            }
	        else
	        {
	            movingObject.transform.position = Vector3.MoveTowards(movingObject.transform.position, new Vector3(endPointRight.position.x, movingObject.transform.position.y, endPointRight.position.z),
	                railMoveSpeed * Time.deltaTime);
            }

	        
        }
	}
}
