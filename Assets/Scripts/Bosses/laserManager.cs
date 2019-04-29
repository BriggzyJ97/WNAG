using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class laserManager : MonoBehaviour// this script manages the laser animation 
{
    #region Variables
    private float timer1; //  timer used to sequence laser
    public GameObject gatheringSparks;//the sparks that gather into the fireball
    public GameObject fireball;
    public GameObject godRays;//rays coming from fireball
    public GameObject lineSparks;//sparks that fly from fireball in line
    public GameObject laserLine;
    public GameObject laserColider;

    public GameObject cameraMain;
    public bool isLaserDying = false;
    public GameObject reflectedLaser;
#endregion
    // Use this for initialization
    void Start () {
		cameraMain = GameObject.FindGameObjectWithTag("MainCamera");
	}
	
	// Update is called once per frame
	void Update ()
	{
	    timer1 += Time.deltaTime;
	    if (isLaserDying==false)
	    {
            //enlarge fireball
	        if (timer1 > 3 && timer1 < 6)
	        {
	            godRays.GetComponent<ParticleSystem>().Play();
	            fireball.GetComponent<Light>().intensity += 1f * Time.deltaTime;
	            fireball.transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
	            godRays.transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
	        }

            //stop the gathering sparks
	        if (timer1 > 3.5f)
	        {
	            gatheringSparks.GetComponent<ParticleSystem>().Stop();
	        }

            //start line sparks
	        if (timer1 > 4f)
	        {
	            lineSparks.GetComponent<ParticleSystem>().Play();
	        }

            //grow actual laser
	        if (timer1 > 7f && timer1 < 9f)
	        {
	            laserColider.GetComponent<CapsuleCollider>().radius += (0.06f * Time.deltaTime);
	            laserColider.GetComponent<CapsuleCollider>().enabled = true;
	            cameraMain.GetComponent<CameraBossControl>().ScreenShakeAmount += (0.2f * Time.deltaTime);
	            laserLine.GetComponent<linePulser>().widthMultiplier += 0.7f * Time.deltaTime;
	        }

            //shrink laser and fireball then start it dying
            if (timer1 > 11f && reflectedLaser == null)
	        {
	            laserColider.GetComponent<CapsuleCollider>().radius -= (0.06f * Time.deltaTime);
                laserLine.GetComponent<linePulser>().widthMultiplier -= 0.7f * Time.deltaTime;
	            cameraMain.GetComponent<CameraBossControl>().ScreenShakeAmount -= (0.2f * Time.deltaTime);
	            fireball.GetComponent<Light>().intensity -= 1f * Time.deltaTime;
	            fireball.transform.localScale -= new Vector3(0.155f, 0.155f, 0.155f);
	            godRays.transform.localScale -= new Vector3(0.155f, 0.155f, 0.155f);
	            godRays.GetComponent<ParticleSystem>().Stop();
                
	            if (Vector3.Distance(fireball.transform.localScale, new Vector3(0, 0, 0)) < 1)
	            {
	                cameraMain.GetComponent<CameraBossControl>().startTime = Time.time;
	                cameraMain.GetComponent<CameraBossControl>().duration = 4f;
	                isLaserDying = true;
	            }
	        }
        }

        //destroy laser
	    else
	    {
	        laserColider.GetComponent<CapsuleCollider>().enabled = false;
	        laserLine.GetComponent<linePulser>().widthMultiplier = 0;
	        cameraMain.GetComponent<CameraBossControl>().ScreenShakeAmount = 0;
	        fireball.GetComponent<Light>().intensity = 0;
	        fireball.transform.localScale = new Vector3(0,0,0);
	        godRays.transform.localScale = new Vector3(0,0,0);
            lineSparks.GetComponent<ParticleSystem>().Stop();
	        if (timer1>18f)
	        {
                Destroy(this.gameObject);
	        }
        }
	    
	}
}
