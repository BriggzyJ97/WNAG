using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossRayShooter : MonoBehaviour
{

    private bool hitPlayer = false;
    private bool hitTurret = false;
    
    public int playerHitCounter = 0;
    public int turretHitCounter = 0;

    public GameObject lastTurretHit;

    public bool hitBack = false;

    
   
	
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
        }else if (other.tag =="BossBack")
        {
            hitBack = true;
        }else if (other.tag=="Turret")
        {
            turretHitCounter += 1;
            lastTurretHit = other.gameObject;
            hitTurret = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            hitPlayer = false;
        }else if (other.tag =="Turret")
        {
            hitTurret = false;
        }
       
    }

}
