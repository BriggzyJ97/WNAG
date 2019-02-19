using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanActor : MonoBehaviour//this script manages the actor objects that play out in cutscenes
{
    #region Variables

    public string actorName;//name of the actor, used for different movements
    public List<Transform> thisActorsMarks = new List<Transform>();//list of positions the actor should move to
    private bool actorOn = false; //whether the actor is on

    private float actingTimer = 0;//timer used for sequencing actor movements
    private bool lookingAtPlayer = false;// whether the actor is lookig at the player
    private GameObject player;
    public GameObject cutsceneControl;// control of the cutscene to trigger lights 
    private bool lightsToggled = false;// whether to toggle the lights

    private AudioSource audioSource;//footstep sound
    private bool audioOn = false;

    public CloseAndOpenDoorControl doorToClose;// door to close or open
    public newStorageBoss bossToTurnOn; //boss

#endregion

    // Use this for initialization
    void Start ()
	{
	    player = GameObject.FindGameObjectWithTag("Player");
	    audioSource = gameObject.GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
	    if (actorOn == true)
	    {
            //movements and actions for actor one in storage boss stage
	        if (actorName == "storageBoss1")
	        {
	            actingTimer += Time.deltaTime;
	            
	            if (actingTimer > 0f && actingTimer < 1f)
	            {
	                if (audioOn==false)
	                {
                        audioSource.Play();
	                    audioOn = true;
	                }
                    gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, thisActorsMarks[0].position, 10f*Time.deltaTime);
	                lookingAtPlayer = true;
	            }

	            if (actingTimer >1f&&actingTimer<2.5f)
	            {
	                if (audioOn==true)
	                {
                        audioSource.Stop();
	                    audioOn = false;
	                }
	            }
	            
                if (actingTimer>2.5f &&actingTimer<3.5f)
	            {
	                if (audioOn == false)
	                {
	                    audioSource.Play();
	                    audioOn = true;
	                }
                    lookingAtPlayer = false;
	                gameObject.transform.position =
	                    Vector3.MoveTowards(gameObject.transform.position, thisActorsMarks[1].position, 10f * Time.deltaTime);
	                gameObject.transform.LookAt(thisActorsMarks[1]);
                }
	            if (actingTimer > 3.5f && actingTimer<4f)
	            {
	                if (audioOn == true)
	                {
	                    audioSource.Stop();
	                    audioOn = false;
	                }

                }

                if (actingTimer>4f&& actingTimer<4.25f)
	            {
	                if (lightsToggled==false)
	                {
	                    cutsceneControl.GetComponent<StorageBossCutsceneTrigger>().ToggleLights();
	                    lightsToggled = true;
	                }
                    
	            }
                if (actingTimer>4.25f &&actingTimer<5f)
	            {
	                if (audioOn == false)
	                {
	                    audioSource.Play();
	                    audioOn = true;
	                }
                    gameObject.transform.position =
	                    Vector3.MoveTowards(gameObject.transform.position, thisActorsMarks[2].position, 10f * Time.deltaTime);
	                gameObject.transform.LookAt(thisActorsMarks[2]);
	                audioSource.volume -= 0.003f;
	            }

	            if (actingTimer > 5f && actingTimer < 7f)
	            {
	                audioSource.volume -= 0.003f;
                    gameObject.transform.position =
	                    Vector3.MoveTowards(gameObject.transform.position, thisActorsMarks[3].position, 10f * Time.deltaTime);
	                gameObject.transform.LookAt(thisActorsMarks[3]);
                }
	            if (actingTimer>7f)
	            {
	                if (audioOn == true)
	                {
	                    audioSource.Stop();
	                    audioOn = false;
	                }

	            }

                if (lookingAtPlayer==true)
	            {
	                gameObject.transform.LookAt(player.transform.position);
                }
	        }

	        //movements and actions for actor two in storage boss stage
            else if (actorName == "storageBoss2")
	        {
	            actingTimer += Time.deltaTime;
	            if (actingTimer > 1f && actingTimer < 1.5f)
	            {
	                if (audioOn == false)
	                {
	                    audioSource.Play();
	                    audioOn = true;
	                }
                    gameObject.transform.position =
	                    Vector3.MoveTowards(gameObject.transform.position, thisActorsMarks[0].position, 10f*Time.deltaTime);
	                lookingAtPlayer = true;
                }
	            if (actingTimer > 1.5f && actingTimer < 3f)
	            {
	                if (audioOn == true)
	                {
	                    audioSource.Stop();
	                    audioOn = false;
	                }
	            }
                if (actingTimer > 3f && actingTimer < 4f)
	            {
	                if (audioOn == false)
	                {
	                    audioSource.Play();
	                    audioOn = true;
	                }
                    lookingAtPlayer = false;
	                gameObject.transform.position =
	                    Vector3.MoveTowards(gameObject.transform.position, thisActorsMarks[1].position, 15f * Time.deltaTime);
	                gameObject.transform.LookAt(thisActorsMarks[1]);
                }
	            if (actingTimer > 4f && actingTimer < 5.5f)
	            {
	                audioSource.volume -= 0.003f;
	                gameObject.transform.position =
	                    Vector3.MoveTowards(gameObject.transform.position, thisActorsMarks[2].position, 15f * Time.deltaTime);
	                gameObject.transform.LookAt(thisActorsMarks[2]);
                }
	            if (actingTimer>5.5f)
	            {
	                if (audioOn == true)
	                {
	                    audioSource.Stop();
	                    audioOn = false;
	                }

	            }
                if (lookingAtPlayer == true)
	            {
	                gameObject.transform.LookAt(player.transform.position);
	            }
            }

	        //movements and actions for actor three in storage boss stage
            else if (actorName == "storageBoss3")
            {
                actingTimer += Time.deltaTime;

                if (actingTimer > 0f && actingTimer < 1f)
                {
                    if (audioOn == false)
                    {
                        audioSource.Play();
                        audioOn = true;
                    }
                    gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, thisActorsMarks[0].position, 15f * Time.deltaTime);
                }

                if (actingTimer > 1f && actingTimer < 1.5f)
                {
                    if (audioOn == true)
                    {
                        audioSource.Stop();
                        audioOn = false;
                    }
                }

                if (actingTimer > 1.5f && actingTimer < 1.75f)
                {
                    if (lightsToggled == false)
                    {
                        cutsceneControl.GetComponent<StorageBossCutsceneTrigger>().ToggleLights();
                        doorToClose.CloseDoor();
                        bossToTurnOn.TurnOnBoss();
                        lightsToggled = true;
                    }
                }
                if (actingTimer > 1.75f && actingTimer < 3.5f)
                {
                    if (audioOn == false)
                    {
                        audioSource.Play();
                        audioOn = true;
                    }
                    gameObject.transform.position =
                        Vector3.MoveTowards(gameObject.transform.position, thisActorsMarks[1].position, 10f * Time.deltaTime);
                    gameObject.transform.LookAt(thisActorsMarks[1]);
                    audioSource.volume -= 0.003f;

                }
                
                if (actingTimer > 3.5f)
                {
                    if (audioOn == true)
                    {
                        audioSource.Stop();
                        audioOn = false;
                    }

                }

                if (lookingAtPlayer == true)
                {
                    gameObject.transform.LookAt(player.transform.position);
                }
            }
        }
	    
	}

    //turn the actor on
    public void Trigger()
    {
        actorOn = true;
    }
}
