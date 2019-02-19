using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseAndOpenDoorControl : MonoBehaviour {//this script controls opening and closing doors

    public bool doorOpen = false;//is the door open
    public GameObject leftDoor;// left part of door
    public GameObject rightDoor;// right part of door
    public Transform leftDoorTargetOpen;// where the left door should be when open
    public Transform rightDoorTargetOpen;//where the right door should be when open
    public Transform leftDoorTargetClose;// where the left door should be when closed
    public Transform rightDoorTargetClose;//where the right door should be when closed
    public float doorOpenSpeed;//how fast the door opens

    private AudioSource doorSound;//door audio transmitter
    private bool soundTriggered = true;

    public AudioClip doorOpeningSound;
    public AudioClip doorClosingSound;

    // Use this for initialization
    void Start()
    {
        doorSound = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //open door
        if (doorOpen == true)
        {
            if (soundTriggered == false)
            {
                doorSound.clip = doorOpeningSound;
                doorSound.volume = 0.086f;
                doorSound.Play();
                soundTriggered = true;
            }
            rightDoor.transform.position = Vector3.MoveTowards(rightDoor.transform.position, rightDoorTargetOpen.position, doorOpenSpeed * Time.deltaTime);
            leftDoor.transform.position = Vector3.MoveTowards(leftDoor.transform.position, leftDoorTargetOpen.position, doorOpenSpeed * Time.deltaTime);
        }
        //close door
        else
        {
            if (soundTriggered == false)
            {
                doorSound.clip = doorClosingSound;
                doorSound.volume = 0.096f;
                doorSound.Play();
                soundTriggered = true;
            }
            rightDoor.transform.position = Vector3.MoveTowards(rightDoor.transform.position, rightDoorTargetClose.position, doorOpenSpeed * Time.deltaTime);
            leftDoor.transform.position = Vector3.MoveTowards(leftDoor.transform.position, leftDoorTargetClose.position, doorOpenSpeed * Time.deltaTime);
        }
    }

    //open door
    public void OpenDoor()
    {
        doorOpen = true;
        soundTriggered = false;
        doorOpenSpeed = 0.6f;
    }

    //close doors
    public void CloseDoor()
    {
        doorOpen = false;
        soundTriggered = false;
        doorOpenSpeed = 3f;
    }
}

