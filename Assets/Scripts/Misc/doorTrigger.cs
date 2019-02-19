using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorTrigger : MonoBehaviour//trigger collider that opens a door when touched by player
{

    public doorControl door;
    public GameObject doorBlocker;

    void OnTriggerEnter(Collider other)
    {
        //open door
        if (other.tag=="Player")
        {
            Debug.Log("closeDoor");
            door.doorOpen = true;
            doorBlocker.GetComponent<BoxCollider>().enabled = true;
        }
    }
}
