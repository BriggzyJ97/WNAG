using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class laserCollision : MonoBehaviour //this script is the collision script for the big lasers
{

    private float TurretDeathTimer;
    private Color FFColor;
    public GameObject line;
    private GameObject basicTarget;

	// Use this for initialization
	void Start ()
	{
	    basicTarget = line.GetComponent<linePulser>().targetGameObject;
	}

    void OnTriggerEnter(Collider other)
    {
        //kill players if they touch it
        if (other.tag=="Player")
        {
            if (other.gameObject.GetComponent<playerController>() != null)
            {
                other.gameObject.GetComponent<playerController>().Die();
            }
            if (other.gameObject.GetComponent<playerDouble>() != null)
            {
                other.gameObject.GetComponent<playerDouble>().Die();
            }
            GameObject.FindGameObjectWithTag("GameController").GetComponent<GameStateManager>().currentGameState =
                GameStateManager.GameState.levelLose;
            //Destroy(this.gameObject);
        }
    }

    void OnTriggerStay(Collider other)
    {
        //change the colour of forcefields when they hit it
         if (other.tag == "ForceField")
         {
             line.GetComponent<linePulser>().targetGameObject.transform.position = other.transform.position;
            FFColor = other.gameObject.GetComponent<forcefieldController>().forceSegL.GetComponent<Renderer>().material.GetColor("_MainColor");
            FFColor = Color.Lerp(FFColor, Color.white, 5 * Time.deltaTime);
            other.gameObject.GetComponent<forcefieldController>().forceSegL.GetComponent<Renderer>().material
                .SetColor("_MainColor", FFColor);
            other.gameObject.GetComponent<forcefieldController>().forceSegR.GetComponent<Renderer>().material
                .SetColor("_MainColor", FFColor);
        }

        //destroy turrets after a short duration
        if (other.tag=="Turret")
        {
            TurretDeathTimer += Time.deltaTime;
            if (TurretDeathTimer>3f)
            {
                //Debug.Log("Destroy turret");
                other.gameObject.GetComponent<turretDestruction>().DestroyTurret();
            }
        }
    }
}
