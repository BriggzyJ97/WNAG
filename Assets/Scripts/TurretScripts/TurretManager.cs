﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretManager : MonoBehaviour {//This script manages all of the different turrets 

    #region Variables
    public List<GameObject> turretList = new List<GameObject>(); //list of all the turrets in the level

    public float turretCooldown; //
    private float turretDuration = 0; // this is a temp variable for shooting
    public float maxTurretDuration; // this is how often the turret can shoot

    private CompletionKeeper completionKeeper;

    public enum turretStates //different states of the turret 
    {
        shooting,
        idle
    }

    public turretStates globalTurretState = turretStates.idle;
    #endregion
    // turns the targetting laser on for all turrets in the scene if the option is selected in options
    void Start ()
	{
	    
        completionKeeper = GameObject.FindGameObjectWithTag("CompletionKeeper").GetComponent<CompletionKeeper>();
	    if (completionKeeper!=null)
	    {
	        if (completionKeeper.toggleLasers==false)
	        {
	            foreach (GameObject turret in turretList)
	            {
	                turret.GetComponent<TurretShooter>().isTargettingLaserOn = false;
	            }
            }
	        else
	        {
	            foreach (GameObject turret in turretList)
	            {
	                turret.GetComponent<TurretShooter>().isTargettingLaserOn = true;
	            }
            }
	    }
	}
	
	// Update is called once per frame
	void Update () {
	    //cooldown to stop all turrets shooting too quick, must be below 0
        if (turretDuration>0)
	    {
	        turretDuration -= Time.deltaTime;
	    }

	    //when the player clicks the mouse the turrets shoot
        if (Input.GetMouseButtonDown(0)&&turretDuration<=0&&gameObject.GetComponent<GameStateManager>().isGamePaused==false) 
	    {
	        foreach (GameObject turret in turretList)//make all turrets in list of turrets shoot
	        {
	            if (turret.transform.parent.transform.parent.GetComponent<TurretTurner>().turretDown==false)
	            {
	                turret.GetComponent<TurretShooter>().thisTurretsState = turretStates.shooting;
	                turretDuration = maxTurretDuration;
                }
	            
	        }
	    }

	    //changes the gameState to win if all the turrets are down
        if (AreAllTurretsDead()==true&&gameObject.GetComponent<GameStateManager>().currentGameState==GameStateManager.GameState.levelPlaying)
	    {
	        gameObject.GetComponent<GameStateManager>().currentGameState = GameStateManager.GameState.levelWin;
	    }
	}
    //checks if all of the turrets are dead and returns true if they are
    private bool AreAllTurretsDead()
    {
        for (int i = 0; i < turretList.Count; i++)
        {
            if (turretList[i].transform.parent.parent.GetComponent<TurretTurner>().turretDown==false&& turretList[i].transform.parent.parent.tag!= "WallTurret")
            {
                return false;
            }
        }

        foreach (GameObject turret in turretList)
        {
            turret.transform.parent.parent.GetComponent<TurretTurner>().turretDownTimer = 100f;
        }
        return true;
    }
}
