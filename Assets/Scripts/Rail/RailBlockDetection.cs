using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailBlockDetection : MonoBehaviour {

    //this script detects if the rail is blocked by another object

    public bool isThisLeftBox = false;
    public railController railControlScript;
    
    void OnTriggerEnter(Collider other)
    {
        // if theres an object touching one side of rail object, stop it moving in that way
        if (other.tag == "Wall" || other.tag == "Mirror" || other.tag == "Turret")
        {
            Debug.Log("entering wall 1");
            if (isThisLeftBox == true)
            {
                //Debug.Log("entering wall L");
                railControlScript.isLeftBlocked = true;
            }
            else
            {
                //Debug.Log("entering wall R");
                railControlScript.isRightBlocked = true;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        //enable movement if it was blocked
        if (other.tag == "Wall" || other.tag == "Mirror" || other.tag == "Turret")
        {
            if (isThisLeftBox == true)
            {
                railControlScript.isLeftBlocked = false;
            }
            else
            {
               railControlScript.isRightBlocked = false;
            }
        }
    }
}
