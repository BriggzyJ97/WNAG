using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseAndOpenDoorControl : MonoBehaviour {

    public bool doorOpen = false;
    public GameObject leftDoor;
    public GameObject rightDoor;
    public Transform leftDoorTargetOpen;
    public Transform rightDoorTargetOpen;
    public Transform leftDoorTargetClose;
    public Transform rightDoorTargetClose;
    public float doorOpenSpeed;

    private AudioSource doorSound;
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

    public void OpenDoor()
    {
        doorOpen = true;
        soundTriggered = false;
        doorOpenSpeed = 0.6f;
    }
    public void CloseDoor()
    {
        doorOpen = false;
        soundTriggered = false;
        doorOpenSpeed = 3f;
    }
}

