using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class laserCollision : MonoBehaviour //this script is the collision script for the big lasers
{

    private float TurretDeathTimer;
    private Color FFColor;
    public GameObject line;
    private GameObject basicTarget;
    public GameObject reflectedLaserPrefab;

	// Use this for initialization
	void Start ()
	{
	    basicTarget = line.GetComponent<linePulser>().targetGameObject;
	}

    void OnCollisionEnter(Collision other)
    {
        //kill players if they touch it
        if (other.gameObject.CompareTag("Player"))
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

        if (reflectedLaserPrefab != null)
        {
            if (other.gameObject.CompareTag("Mirror"))
            {
                ContactPoint contact = other.contacts[0];
                
                //Debug.Log("Before: "+bulletMain.directionOfBullet);
                linePulser linePulser = line.GetComponent<linePulser>();
                linePulser.targetGameObject.transform.position = contact.point;
                Vector3 directionOfLaser = (gameObject.transform.position - contact.point).normalized;
                Vector3 newDirectionOfLaser = Vector3.Reflect(directionOfLaser, Vector3.forward).normalized;

                Quaternion lookRotation = Quaternion.LookRotation(newDirectionOfLaser);
                GameObject newStartedLaser = Instantiate(reflectedLaserPrefab, contact.point, lookRotation);
                laserManager mainLaser = gameObject.transform.parent.parent.gameObject.GetComponent<laserManager>();
                mainLaser.reflectedLaser = newStartedLaser;
                newStartedLaser.GetComponent<newReflectedLaser>().mainLaser = mainLaser;
            }
        }
        
    }

    void OnCollisionStay(Collision other)
    {
        //change the colour of forcefields when they hit it
         if (other.gameObject.CompareTag("ForceField"))
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
        if (other.gameObject.CompareTag("Turret"))
        {
            TurretDeathTimer += Time.deltaTime;
            if (TurretDeathTimer>3f)
            {
                other.gameObject.GetComponent<turretDestruction>().DestroyTurret();
            }
        }

        
    }
}
