using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boxRandomiser : MonoBehaviour {//this script randomises which boxes are on the shelf prop with 45% chance for each box to be there
    
	void Start ()
	{
	    int random = Random.Range(0, 100);
	    if (random>65)
	    {
            Destroy(gameObject);
	    }
	        
	}
	
}
