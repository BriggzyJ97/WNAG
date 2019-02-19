using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class forcefieldController : MonoBehaviour
    //This script controls the forcefields which are usually over the turrets. They are triggered from external script
{

    private bool forcefieldOn = false; //is the forcefield on or off
    public GameObject forceSegL; //The Left segment of forcefield
    public GameObject forceSegR; //The Right Segment of forcefield
    private float timer1; //timer for lerping the shader variables
	
	void Update () {
	    if (forcefieldOn==true)
	    {
	        //this lerps the shader variables to visible and increases distortion
            if (timer1<2)
	        {
	            timer1 += Time.deltaTime;
	            forceSegL.GetComponent<Renderer>().material.SetFloat("_FresnelWidth", timer1 +0.5f );
	            forceSegR.GetComponent<Renderer>().material.SetFloat("_FresnelWidth", timer1 +0.5f);
	            forceSegL.GetComponent<Renderer>().material.SetFloat("_Distort", timer1 * 150);
	            forceSegR.GetComponent<Renderer>().material.SetFloat("_Distort", timer1 * 150);

            }
	    }
	}

    //function called by external script
    public void TurnForceFieldOn()
    {
        //Debug.Log("in ff function");
        forcefieldOn = true; 
    }
}
