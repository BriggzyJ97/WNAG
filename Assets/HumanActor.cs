using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanActor : MonoBehaviour
{

    public string actorName;
    public List<Transform> thisActorsMarks = new List<Transform>();
    private bool actorOn = false;

    private float actingTimer = 0;
    private bool lookingAtPlayer = false;
    private GameObject player;
    public GameObject cutsceneControl;
    private bool lightsToggled = false;

    private AudioSource audioSource;

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
	        if (actorName == "storageBoss1")
	        {
	            actingTimer += Time.deltaTime;
	            if (actingTimer == 0f) 
	            {
                    audioSource.Play();
	            }
	            if (actingTimer > 0f && actingTimer < 1f)
	            {
	                audioSource.Play();
                    gameObject.transform.position =
	                    Vector3.MoveTowards(gameObject.transform.position, thisActorsMarks[0].position, 10f*Time.deltaTime);

	                lookingAtPlayer = true;
	            }

	            if (actingTimer ==1f)
	            {
                    audioSource.Stop();

	            }
	            if (actingTimer == 2.5f)
	            {
	                audioSource.Play();

	            }
                if (actingTimer>2.5f &&actingTimer<3.5f)
	            {
	                lookingAtPlayer = false;
	                gameObject.transform.position =
	                    Vector3.MoveTowards(gameObject.transform.position, thisActorsMarks[1].position, 10f * Time.deltaTime);
	                gameObject.transform.LookAt(thisActorsMarks[1]);
                }
	            if (actingTimer == 3.5f)
	            {
	                audioSource.Stop();

	            }

                if (actingTimer>4f&& actingTimer<4.25f)
	            {
	                if (lightsToggled==false)
	                {
	                    cutsceneControl.GetComponent<StorageBossCutsceneTrigger>().ToggleLights();
	                    lightsToggled = true;
	                }
                    
	            }

	            if (actingTimer == 4.25f)
	            {
	                audioSource.Play();

	            }
                if (actingTimer>4.25f &&actingTimer<5f)
	            {
	                gameObject.transform.position =
	                    Vector3.MoveTowards(gameObject.transform.position, thisActorsMarks[2].position, 10f * Time.deltaTime);
	                gameObject.transform.LookAt(thisActorsMarks[2]);
	                audioSource.volume -= 0.003f;
	            }

	            if (actingTimer > 5f && actingTimer < 8f)
	            {
	                audioSource.volume -= 0.003f;
                    gameObject.transform.position =
	                    Vector3.MoveTowards(gameObject.transform.position, thisActorsMarks[3].position, 10f * Time.deltaTime);
	                gameObject.transform.LookAt(thisActorsMarks[3]);
                }

                if (lookingAtPlayer==true)
	            {
	                gameObject.transform.LookAt(player.transform.position);
                }
	        }
	        else if (actorName == "storageBoss2")
	        {
	            actingTimer += Time.deltaTime;
	            if (actingTimer > 1f && actingTimer < 1.5f)
	            {
	                gameObject.transform.position =
	                    Vector3.MoveTowards(gameObject.transform.position, thisActorsMarks[0].position, 10f*Time.deltaTime);
	                lookingAtPlayer = true;
                }
	            if (actingTimer > 3f && actingTimer < 4f)
	            {
	                lookingAtPlayer = false;
	                gameObject.transform.position =
	                    Vector3.MoveTowards(gameObject.transform.position, thisActorsMarks[1].position, 15f * Time.deltaTime);
	                gameObject.transform.LookAt(thisActorsMarks[1]);
                }
	            if (actingTimer > 4f && actingTimer < 5.5f)
	            {
	                
	                gameObject.transform.position =
	                    Vector3.MoveTowards(gameObject.transform.position, thisActorsMarks[2].position, 15f * Time.deltaTime);
	                gameObject.transform.LookAt(thisActorsMarks[2]);
                }
                if (lookingAtPlayer == true)
	            {
	                gameObject.transform.LookAt(player.transform.position);
	            }
            }
        }
	    
	}

    public void Trigger()
    {
        actorOn = true;
    }
}
