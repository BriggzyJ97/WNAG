using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class newReflectedLaser : MonoBehaviour
{

    #region Variables
    private float timer1; //  timer used to sequence laser
    public GameObject gatheringSparks;//the sparks that gather into the fireball
    public GameObject fireball;
    public GameObject godRays;//rays coming from fireball
    public GameObject lineSparks;//sparks that fly from fireball in line
    public GameObject laserLine;
    public GameObject laserColider;
    public laserManager mainLaser;

    public GameObject cameraMain;
    public bool isLaserDying = false;
    #endregion

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        timer1 += Time.deltaTime;
        if (isLaserDying == false)
        {
            

            //grow actual laser
            if (timer1 > 0f && timer1 < 2f)
            {
                laserColider.GetComponent<CapsuleCollider>().radius += (0.06f * Time.deltaTime);
                laserColider.GetComponent<CapsuleCollider>().enabled = true;
                laserLine.GetComponent<linePulser>().widthMultiplier += 0.7f * Time.deltaTime;
            }

            //shrink laser and fireball then start it dying
            if (timer1 > 4f)
            {
                laserColider.GetComponent<CapsuleCollider>().radius -= (0.06f * Time.deltaTime);
                laserLine.GetComponent<linePulser>().widthMultiplier -= 0.7f * Time.deltaTime;

                fireball.GetComponent<Light>().intensity -= 1f * Time.deltaTime;
                fireball.transform.localScale -= new Vector3(0.155f, 0.155f, 0.155f);
                godRays.transform.localScale -= new Vector3(0.155f, 0.155f, 0.155f);
                godRays.GetComponent<ParticleSystem>().Stop();
                Debug.Log("HAPPENING");
                if (Vector3.Distance(fireball.transform.localScale, new Vector3(0, 0, 0)) < 1)
                {
                    isLaserDying = true;
                }
            }
        }

        //destroy laser
        else
        {
            laserColider.GetComponent<CapsuleCollider>().enabled = false;
            laserLine.GetComponent<linePulser>().widthMultiplier = 0;
            fireball.GetComponent<Light>().intensity = 0;
            fireball.transform.localScale = new Vector3(0, 0, 0);
            godRays.transform.localScale = new Vector3(0, 0, 0);
            lineSparks.GetComponent<ParticleSystem>().Stop();
            if (timer1 > 4f)
            {
                mainLaser.reflectedLaser = null;
                Destroy(this.gameObject);
            }
        }
    }
}