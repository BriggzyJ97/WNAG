using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//this script controls the enemies in an old design of the storage boss level 
public class enemyController : MonoBehaviour
{


    public bool isAlive = true;


    private NavMeshAgent myNavMesh;
    private GameObject player;
    private bossFightController bossFightController;
    public GameObject deathExplosion;

	// Use this for initialization
	void Start () {
	    myNavMesh = gameObject.GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
	    bossFightController = GameObject.FindGameObjectWithTag("BossFightController").GetComponent<bossFightController>();
	}
	
	// Update is called once per frame
	void Update ()
	{
        if(myNavMesh.enabled==true)
	    {
	        myNavMesh.destination = player.transform.position;
        }
	    
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag=="Player")
        {
            //Instantiate(sparks, other.transform.position, Quaternion.identity);
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
        }
    }

    public void Die()
    {
        isAlive = false;
        Instantiate(deathExplosion, transform.position, Quaternion.identity);
        //gameObject.GetComponent<AudioSource>().Play();
        bossFightController.enemiesAlive -= 1;
        Debug.Log("enemy killed");
        Destroy(this.gameObject);
    }
}
