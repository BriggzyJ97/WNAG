using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageBossCutsceneTrigger : MonoBehaviour
{

    public List<GameObject> lightsToToggle =  new List<GameObject>();
    public List<GameObject> charactersToToggle = new List<GameObject>();

    private bool Triggered = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ToggleLights()
    {
        foreach (GameObject light in lightsToToggle)
        {
            if (light.activeSelf==true)
            {
                if (gameObject.GetComponent<AudioSource>()!=null)
                {
                    gameObject.GetComponent<AudioSource>().Stop();
                }
                
                light.SetActive(false);
            }
            else
            {
                if (gameObject.GetComponent<AudioSource>() != null)
                {
                    gameObject.GetComponent<AudioSource>().Play();
                }

                light.SetActive(true);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag=="Player"&&Triggered==false)
        {
            foreach (GameObject humanActor in charactersToToggle)
            {
                humanActor.GetComponent<HumanActor>().Trigger();
                humanActor.GetComponent<HumanActor>().cutsceneControl = gameObject;
            }
            
            Triggered = true;
        }
    }
}
