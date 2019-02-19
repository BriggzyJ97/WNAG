using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour//the collision detection to trigger entering the console in the dialogue levels
{

    private GameObject gameManager;
    private bool triggered = false;

	//assign game manager
	void Start () {
		gameManager = GameObject.FindGameObjectWithTag("GameController");

	}
	
    //if player touches collider, trigger the console
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (triggered == false)
            {
                gameManager.GetComponent<GameStateManager>().currentGameState = GameStateManager.GameState.levelDialogue;
                triggered = true;
            }
            
        }
    }

    //reset when player leaves collider
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            triggered = false;

        }
    }
}
