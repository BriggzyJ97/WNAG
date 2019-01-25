using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossRayShooter : MonoBehaviour
{

    private bool hitPlayer = false;
    public int playerHitCounter = 0;
    
   
	
	// Update is called once per frame
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (hitPlayer==false)
            {
                playerHitCounter += 1;
                hitPlayer = true;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            hitPlayer = false;
        }
    }
}
