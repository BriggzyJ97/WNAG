using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletMirrorDetect : MonoBehaviour
{

    public bulletController bulletMain;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter(Collision other)
    {
        Debug.Log("hitanything");
        if (other.gameObject.tag=="EnemyMirror")
        {
            Debug.Log("hitmirror");
            Debug.Log("before hit: "+bulletMain.directionOfBullet);
            ContactPoint contact = other.contacts[0];
            bulletMain.directionOfBullet = Vector3.Reflect(bulletMain.directionOfBullet, contact.normal);
            Debug.Log("after hit: " + bulletMain.directionOfBullet);
            bulletMain.transform.LookAt(bulletMain.transform.position + bulletMain.directionOfBullet);
            bulletMain.transform.Rotate(0, 90, 0);
        }
    }
}
