using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossRayShooter : MonoBehaviour//this script controls the detection for the boss rays
{

    private bool hitPlayer = false;//whether the box is hitting playr
    private bool hitTurret = false;//whether the box is hitting turret
    
    public int playerHitCounter = 0;//how many times the player has been hit with box
    public int turretHitCounter = 0;//how many times a turret has been hit with box

    public GameObject lastTurretHit;// the last turret that was hit

    public bool hitBack = false;// whether the back facing target has been hit

    
   
	
	// detection on what is being hit by box
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
