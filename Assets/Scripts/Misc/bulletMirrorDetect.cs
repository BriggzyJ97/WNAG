using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletMirrorDetect : MonoBehaviour//this script manages bullet collision 
{

    public bulletController bulletMain;//the main bullet controller

    void OnCollisionEnter(Collision other)
    {
        //bullet reflection code
        if (other.gameObject.tag=="Mirror")
        {
            ContactPoint contact = other.contacts[0];
            //Debug.Log("Before: "+bulletMain.directionOfBullet);
            bulletMain.directionOfBullet = Vector3.Reflect(bulletMain.directionOfBullet, contact.normal).normalized;
            
            bulletMain.transform.LookAt(bulletMain.transform.position + bulletMain.directionOfBullet);
            //Debug.Log("After: " + bulletMain.directionOfBullet);
            bulletMain.isReflected = true;
            bulletMain.transform.Rotate(0, 90, 0);
            bulletMain.readyForReflected = false;
            //bulletMain.bulletHitSound.Play();
            other.gameObject.GetComponent<AudioSource>().Play();
        }
    }

    void OnCollisionExit(Collision other)
    {
        bulletMain.readyForReflected = true;
    }
}
