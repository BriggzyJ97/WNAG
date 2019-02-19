using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndDialogueLevelTrigger : MonoBehaviour// trigger collider to end the level in a win
{

    public doorControl door;
    private GameStateManager gameManager;

	// Use this for initialization
	void Start ()
	{
	    gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameStateManager>();
	}
	
    void OnTriggerEnter(Collider other)
    {
        //win level
        if (other.tag == "Player")
        {
            if (door.doorOpen==true)
            {
                gameManager.currentGameState = GameStateManager.GameState.levelWin;
            }
        }
    }
}
