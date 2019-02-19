using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageBossCutsceneTrigger : MonoBehaviour//this script triggers the boss fight starting
{

    public List<GameObject> lightsToToggle =  new List<GameObject>();//all the lights to turn on/off for the boss fight
    public List<GameObject> charactersToToggle = new List<GameObject>();//the actors to trigger

    private bool Triggered = false;//whether the boss fight is triggered

    //toggle all the lights
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

    //when the player enters area, trigger the actor
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
