using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TurretShooter : MonoBehaviour //Controls the turret shooting
{
    #region Variables
    public TurretManager.turretStates thisTurretsState = TurretManager.turretStates.idle; 

    private float turretDuration; //this  stops the turret shooting forever when activated;
    public float turretDurationMax;

    private float shootingCooldown; //this shooting cooldown makes sure the bullets aren't shot too quickly
    public float shootingCooldownMax;

    public GameObject bullet; //the bullet prefab
    public GameObject sparks; // the flash of the turret firing
    public GameObject sparksEmitter; //where the turret flash comes from
    private AudioSource shootingSound;

    public GameObject baseBox;// the big square box part of the turret
    private Material baseMat;// the material of the base of turret
    public GameObject thisTurretsForceField;//this turrets forcefield (if it has one)

    public bool isAutoShooter = false; //if this turret is a purple auto shooter one
    private Color purpleBaseColor = new Color(0.7f,0,0.7f,1); // the purple base colour for dynamic tweaking
    private Color redBaseColor = new Color(0.7f,0,0,1);

    public bool isTargettingLaserOn = false; //is the targetting laser on
    public GameObject lineRenderer;//prefab of targetting laser
    public LineRenderer tempLineRenderer;//this turrets targetting laser
    private bool hasLineRendererBeenSpawned = false; //bool gate to make sure the targetting laser only spawns once
    public LayerMask targetLineLayerMask;//custom mixed layermask for the 
#endregion
    // Use this for initialization
    void Start ()
    {
        //sets cooldowns to the base number
        turretDuration = turretDurationMax;
        if (isAutoShooter==true)
        {
            shootingCooldown = 0;
        }
        else
        {
            shootingCooldown = shootingCooldownMax;
        }
        
        shootingSound = gameObject.GetComponent<AudioSource>(); //sound of turret shooting
    }
	
	// Update is called once per frame
	void Update () {
       
	    //if this turret is an auto shooter and its not down then have it shoot every now and again, spawning bullet and gunflash
        if (isAutoShooter==true)
	    {
	        
	            turretDuration -= Time.deltaTime;
	            shootingCooldown += Time.deltaTime;
	            if (shootingCooldown >= shootingCooldownMax)
	            {
	                if (transform.parent.parent.GetComponent<TurretTurner>().turretDown == false)
	                {
	                    shootingSound.Play();
	                    Instantiate(bullet, transform.position, transform.rotation);
	                    Instantiate(sparks, sparksEmitter.transform.position, sparksEmitter.transform.rotation);
                    }
                    
                    shootingCooldown = 0f;
	            }
	        

	        Color finalColor = purpleBaseColor * Mathf.LinearToGammaSpace(shootingCooldown/shootingCooldownMax);//change the colour of the emission marks on the turret base to show it charging up 
            baseBox.GetComponent<Renderer>().material.SetColor("_EmissionColor", finalColor);
	        if (turretDuration <= 0)//stop the turret from shooting forever
	        {
	            thisTurretsState = TurretManager.turretStates.idle;
	            turretDuration = turretDurationMax;
	        }
        }
        //if this turret is not an auto shooter then shoot when its not down and the state is shooting, which is changed by the turret manager
        else if (isAutoShooter==false)
	    {
	        shootingCooldown += Time.deltaTime;
            if (thisTurretsState == TurretManager.turretStates.shooting && transform.parent.parent.GetComponent<TurretTurner>().turretDown == false)
	        {
	            turretDuration -= Time.deltaTime;
	            
	            if (shootingCooldown >= shootingCooldownMax)
	            {
	                shootingSound.Play();
                    GameObject tempBullet = Instantiate(bullet, transform.position, transform.rotation);
	                if (thisTurretsForceField !=null)
	                {
	                    tempBullet.GetComponent<bulletController>().thisBulletsTurretForceField = thisTurretsForceField;

	                }
	                Instantiate(sparks, sparksEmitter.transform.position, sparksEmitter.transform.rotation);
                    shootingCooldown = 0;
	            }
	        }
	        Color finalColor = redBaseColor * Mathf.Min(Mathf.LinearToGammaSpace(shootingCooldown / shootingCooldownMax),1);//change the colour of the emission marks on the turret base to show it charging up 
	        baseBox.GetComponent<Renderer>().material.SetColor("_EmissionColor", finalColor);
            if (turretDuration <= 0)
	        {
	            thisTurretsState = TurretManager.turretStates.idle;
	            turretDuration = turretDurationMax;
	        }

        }
	    //targetting laser controller
        if (isTargettingLaserOn==true&&transform.parent.parent.GetComponent<TurretTurner>().enabled==true)
        {
            if (hasLineRendererBeenSpawned==false)//if the targetting laser hasn't been spawned, spawn it
            {
                tempLineRenderer=Instantiate(lineRenderer, transform.position, Quaternion.identity).GetComponent<LineRenderer>();
                hasLineRendererBeenSpawned = true;//stop more being spawned
            }
            RaycastHit hit;
            //Raycast is used to set the targetting line position and then set the start and end point of the targetting laser's line renderer.
            if (Physics.Raycast(sparksEmitter.transform.position, sparksEmitter.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, targetLineLayerMask))
	        {
                
	            //Debug.DrawRay(sparksEmitter.transform.position, sparksEmitter.transform.TransformDirection(Vector3.forward) * hit.distance, Color.red);
	            tempLineRenderer.gameObject.transform.position =
	                sparksEmitter.transform.position - ((sparksEmitter.transform.position - hit.point) / 2);
                tempLineRenderer.SetPosition(0,new Vector3(sparksEmitter.transform.position.x, sparksEmitter.transform.position.y, sparksEmitter.transform.position.z));
                tempLineRenderer.SetPosition(1,hit.point);
                //Debug.Log("Did Hit");
            }
	        else
	        {
	            //Debug.DrawRay(sparksEmitter.transform.position, sparksEmitter.transform.TransformDirection(Vector3.forward) * 1000, Color.white);
	            //Debug.Log("Did not Hit");
	        }
        }
        // if there isn't supposed to be a laser but there is, destroy it (including if setting turned off mid game)
        else
        {
            if (tempLineRenderer!=null)
            {
                Destroy(tempLineRenderer);
                hasLineRendererBeenSpawned = false;
            }
        }
    }
}
