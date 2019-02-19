using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//this script checks for if the different departments shoulc be unlocked on the department select screen
public class WorldSelectCheckForLevels : MonoBehaviour
{

    public int levelsNeededToBeCompletedForUnlock;
    private CompletionKeeper completionKeeper;

	// Use this for initialization
	void Start ()
	{
	    completionKeeper = GameObject.FindGameObjectWithTag("CompletionKeeper").GetComponent<CompletionKeeper>();
	}

    public void checkForUnlock()
    {
        if (completionKeeper.howManyLevelsCompleted>=levelsNeededToBeCompletedForUnlock)
        {
            gameObject.GetComponent<Button>().interactable = true;
        }
    }
}
