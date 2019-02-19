using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

public class bulletController : MonoBehaviour//THIS SCRIPT MOVES THE BULLET AND MANAGES COLLISIONS
{
    #region Variables
    public float bulletSpeed; //Speed of bullet
    public GameObject sparks; //Sparks Prefab that spawns when bullet hits wall
    public GameObject puff;
    private Vector3 lastPosition; //saves the first position of bullet to calculate direction of the bullet
    public Vector3 directionOfBullet; //The direction that the bullet is moving

    private bool positionUpdateBool = false; //Bool used for updating "lastPosition" variable
    public bool isReflected = false;// Bool that keeps track of whether the bullet has been reflected
    public bool readyForReflected = true; //Makes sure that bullet doesnt keep reflecting infinitely when hitting mirro
    private float positionUpdateDelay = 0.1f;
    public GameObject thisBulletsTurretForceField;//the forcefield of the turret this bullet came from 

    public AudioSource bulletHitSound;

    private float CollisionDelay = 0.2f;
#endregion

    void Start ()
	{
	    lastPosition = transform.position; //Save first position to calculate direction
	    bulletHitSound = gameObject.GetComponent<AudioSource>();

	}
	
	// Update is called once per frame
	void Update ()
	{
        // the bullet can't collide with anything within the first fraction of second when spawning
	    if (CollisionDelay>0)
	    {
	        CollisionDelay -= Time.deltaTime;
	        if (CollisionDelay<0.1f)
	        {
	            gameObject.layer = 0;
	        }
        }

	    //moves this bullet
        if (isReflected==false)
	    {
            
	        transform.Translate(bulletSpeed * Time.deltaTime, 0, 0);
        }

        //moves bullet along new reflected direction
        else
        {
            
            transform.Translate(directionOfBullet*bulletSpeed*Time.deltaTime, Space.World);
	    }

	    //Makes sure th bullet doesnt infinitely reflect
        if (readyForReflected==false)
	    {
            
	        new WaitForSeconds(0.5f);
	        readyForReflected = true;
	    }

	    //a failsafe that makes sure that if the bullet stops moving for whatever reason it deletes itself
        if (directionOfBullet == new Vector3(0,0,0)&&CollisionDelay>0.5f)
	    {
            
            Destroy(this.gameObject);
	    }



    }

    void FixedUpdate()
    {
        //Calculate direction the bullet is going
        if (positionUpdateBool == false)
        {
            
            positionUpdateDelay -= Time.deltaTime;
            if (positionUpdateDelay<=0)
            {
                directionOfBullet = UpdateDirection();
                //Debug.Log(directionOfBullet);
                positionUpdateBool = true;
            }
            
        }
    }

    void OnTriggerEnter(Collider other)
    {
        //When bullet hits wall, spawn sparks and delete itself
        if (other.gameObject.tag == "Wall")
        {
            bulletHitSound.Play();
            Instantiate(sparks, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }

        //Or if bullet hits wall turret, spawn sparks and delete itself
        else if (other.gameObject.tag == "WallTurret")
        {
            bulletHitSound.Play();
            //Debug.Log("wallturret hit");
            Instantiate(sparks, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }

        //Or if bullet hits turret, shut it down, make it smoke, spawn sparks and delete bullet
        else if (other.gameObject.tag == "Turret")
        {
            bulletHitSound.Play();
            other.GetComponentInChildren<TurretTurner>().ChangeToSmokeSound();
            other.GetComponentInChildren<AudioSource>().Play();
            other.GetComponentInChildren<TurretTurner>().turretDown = true;
            other.GetComponentInChildren<TurretTurner>().turretDownTimer =
                other.GetComponentInChildren<TurretTurner>().turretDownTimerMax;
            other.GetComponentInChildren<ParticleSystem>().Play();
            Instantiate(sparks, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }

        // if bullet hits  mirror, Reflect bullet direction , rotate the bullet and make it now move by new direction
        //MIRROR COLLLISION PERFORMED BY SEPERATE SCRIPT NOW
        else if (other.gameObject.tag == "VerticleMirror")
        {
            if (readyForReflected==true)
            {
                //bulletHitSound.Play();
                other.gameObject.GetComponent<AudioSource>().Play();
                isReflected = true;
                directionOfBullet = (Vector3.Reflect(directionOfBullet, Vector3.right)).normalized;
                transform.LookAt(transform.position + directionOfBullet);
                transform.Rotate(0,90,0);
                //Debug.Log(directionOfBullet);
                readyForReflected = false;
            }
        }
        
        else if (other.gameObject.tag == "DiagonalRightMirror") 
        {
            if (readyForReflected == true)
            {
                //bulletHitSound.Play();
                other.gameObject.GetComponent<AudioSource>().Play();
                isReflected = true;
                float tempDirectionXValue = directionOfBullet.z;
                float tempDirectionZValue = directionOfBullet.x;
                
                directionOfBullet = new Vector3(tempDirectionXValue,0,tempDirectionZValue);
                transform.LookAt(transform.position + directionOfBullet);
                transform.Rotate(0, 90, 0);
                //Debug.Log(directionOfBullet);
                readyForReflected = false;
            }
        }
        
        else if (other.gameObject.tag == "DiagonalLeftMirror" ) 
        {
            if (readyForReflected == true)
            {
                //bulletHitSound.Play();
                other.gameObject.GetComponent<AudioSource>().Play();
                isReflected = true;
                float tempDirectionXValue = -directionOfBullet.z;
                float tempDirectionZValue = -directionOfBullet.x;

                directionOfBullet = new Vector3(tempDirectionXValue, 0, tempDirectionZValue);
                transform.LookAt(transform.position + directionOfBullet);
                transform.Rotate(0, 90, 0);
                //Debug.Log(directionOfBullet);
                readyForReflected = false;
            }
        }
        
        else if (other.gameObject.tag == "HorizontalMirror")
        {
            if (readyForReflected == true)
            {
                //bulletHitSound.Play();
                other.gameObject.GetComponent<AudioSource>().Play();
                isReflected = true;
                directionOfBullet = (Vector3.Reflect(directionOfBullet, Vector3.forward)).normalized;
                transform.LookAt(transform.position + directionOfBullet);
                transform.Rotate(0, 90, 0);
                //Debug.Log(directionOfBullet);
                readyForReflected = false;
            }
        }

        //if bullet hits player, spawn sparks, make the player die, make the game end and destroy the bullet
        else if (other.gameObject.tag =="Player")
        {
            Instantiate(sparks, transform.position, Quaternion.identity);
            if (other.gameObject.GetComponent<playerController>()!=null)
            {
                other.gameObject.GetComponent<playerController>().Die();
            }
            if (other.gameObject.GetComponent<playerDouble>() != null)
            {
                other.gameObject.GetComponent<playerDouble>().Die();
            }
            GameObject.FindGameObjectWithTag("GameController").GetComponent<GameStateManager>().currentGameState =
                GameStateManager.GameState.levelLose;
            Destroy(this.gameObject);
            
        }

        //if bullet hits box prop, destroy it
        else if (other.gameObject.tag == "Box")
        {
            Instantiate(sparks, transform.position, Quaternion.identity);
            Instantiate(puff, other.gameObject.transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            Destroy(this.gameObject);

        }

        //if bullet hits enemy, destroy enemy and bullet
        else if (other.gameObject.tag == "WalkingEnemy")
        {
            if (other.gameObject.GetComponent<enemyController>().isAlive==true)
            {
                Instantiate(sparks, transform.position, Quaternion.identity);
                other.gameObject.GetComponent<enemyController>().Die();
                Destroy(this.gameObject);
            }
            
        }

        //if bullet hits boss weak spot, lower health
        else if (other.gameObject.tag=="BossWeakSpot")
        {
            Instantiate(sparks, transform.position, Quaternion.identity);
            other.gameObject.GetComponent<bossHealthController>().LowerHealth();
            Destroy(this.gameObject);
        }

        //if bullet hits forcefield, destroy bulet
        else if (other.gameObject.tag=="ForceField" && other.gameObject != thisBulletsTurretForceField)
        {
            //Debug.Log("bulletHittingForceField");
            bulletHitSound.Play();
            Instantiate(sparks, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }

    void OnTriggerExit(Collider other)
    {
        // another thing to stop bullet infinitely reflecting
        if (other.gameObject.tag=="Mirror")
        {
            readyForReflected = true;
        }
    }

    //calculate direction of bullet
    private Vector3 UpdateDirection()
    {
        Vector3 directionMoving = (transform.position - lastPosition).normalized;
        lastPosition = transform.position;
        return directionMoving;
    }
}
