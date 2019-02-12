using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorTrigger : MonoBehaviour
{

    public doorControl door;
    public GameObject doorBlocker;

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("somethingEntered");
        if (other.tag=="Player")
        {
            Debug.Log("closeDoor");
            door.doorOpen = true;
            doorBlocker.GetComponent<BoxCollider>().enabled = true;
        }
    }
}
