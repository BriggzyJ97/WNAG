using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class forcefieldController : MonoBehaviour
{

    private bool forcefieldOn = false;
    public GameObject forceSegL;
    public GameObject forceSegR;
    private float timer1;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	    if (forcefieldOn==true)
	    {
            
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

    public void TurnForceFieldOn()
    {
        //Debug.Log("in ff function");
        forcefieldOn = true;
    }
}
