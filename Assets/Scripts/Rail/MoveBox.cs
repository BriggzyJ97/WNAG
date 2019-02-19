using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class MoveBox : MonoBehaviour
{
    //collision detection script to move the rail object

    public bool isThisLeftBox = false; //is this the left side detection box
    public railController railControlScript; //the main rail script

    

    void OnTriggerStay(Collider other)
    {
        // if the detection script touches a player set the rail to moving and also detect if payer is sprinting
        if (other!=transform.parent)
        {
            if (other.tag == "Player")
            {
                if (isThisLeftBox == true)
                {
                    railControlScript.CurrentDirectionOfRailMovement = railController.railMovement.movingLeft;
                }
                else
                {
                    railControlScript.CurrentDirectionOfRailMovement = railController.railMovement.movingRight;
                }

                if (other.GetComponent<playerController>() != null)
                {
                    if (other.GetComponent<playerController>().isPlayerSprinting == true)
                    {
                        railControlScript.isPlayerSprinting = true;
                    }
                }
                else if (other.GetComponent<playerDouble>() != null)
                {
                    if (other.GetComponent<playerDouble>().isPlayerSprinting == true)
                    {
                        railControlScript.isPlayerSprinting = true;
                    }
                }
                else
                {
                    railControlScript.isPlayerSprinting = false;

                }
            }
        }
        
        
        
    }

    void OnTriggerExit(Collider other)
    {
        //detect when the player moves away from the box
        if (other != transform.parent)
        {
            if (other.tag == "Player")
            {
                railControlScript.CurrentDirectionOfRailMovement = railController.railMovement.idle;
                railControlScript.isPlayerSprinting = false;
            }
        }

        
    }
}
