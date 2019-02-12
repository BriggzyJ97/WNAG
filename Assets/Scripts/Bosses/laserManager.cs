using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class laserManager : MonoBehaviour
{

    private float timer1;
    public GameObject gatheringSparks;
    public GameObject fireball;
    public GameObject godRays;
    public GameObject lineSparks;
    public GameObject laserLine;
    public GameObject laserColider;

    public GameObject cameraMain;
    public bool isLaserDying = false;
    
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
	        if (timer1 > 3 && timer1 < 6)
	        {
	            godRays.GetComponent<ParticleSystem>().Play();
	            fireball.GetComponent<Light>().intensity += 1f * Time.deltaTime;
	            fireball.transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
	            godRays.transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
	        }

	        if (timer1 > 3.5f)
	        {
	            gatheringSparks.GetComponent<ParticleSystem>().Stop();
	        }

	        if (timer1 > 4f)
	        {
	            lineSparks.GetComponent<ParticleSystem>().Play();
	        }

	        if (timer1 > 7f && timer1 < 9f)
	        {
	            laserColider.GetComponent<CapsuleCollider>().radius += (0.06f * Time.deltaTime);
	            laserColider.GetComponent<CapsuleCollider>().enabled = true;
	            cameraMain.GetComponent<CameraBossControl>().ScreenShakeAmount += (0.2f * Time.deltaTime);
	            laserLine.GetComponent<linePulser>().widthMultiplier += 0.7f * Time.deltaTime;
	        }

	        if (timer1 > 11f)
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
