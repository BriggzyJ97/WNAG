using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossRayShooterBase : MonoBehaviour {

    private static bool hitPlayer = false;
    public static int playerHitCounter = 0;

    private static int numberOfRaysNotHitting = 3;
    private bool Hitting = false;

    void FixedUpdate()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity))
        {

            if (hit.collider.tag == "Player")
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.green);
                if (Hitting == false)
                {
                    numberOfRaysNotHitting += 1;
                    Hitting = true;
                }
                if (hitPlayer == false)
                {
                    playerHitCounter += 1;

                    hitPlayer = true;
                }


            }
            else
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.red);

                if (Hitting == true)
                {
                    numberOfRaysNotHitting -= 1;
                    Hitting = false;
                }

                if (numberOfRaysNotHitting == 3)
                {
                    hitPlayer = false;
                }
            }
        }
        //Debug.Log(hitPlayer);
    }
}
