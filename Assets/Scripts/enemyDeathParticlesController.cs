using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyDeathParticlesController : MonoBehaviour
{

    private float lifeTime = 0;
    private float alpha = 1;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
	    lifeTime += Time.deltaTime;
	    if (lifeTime>5f)
	    {
	        alpha -= (Time.deltaTime / 4f);
            gameObject.GetComponent<ParticleSystemRenderer>().material.SetColor("_EmissionColor", new Color(alpha,0,0,alpha));
	        if (alpha<=0)
	        {
                Destroy(this.gameObject);
	        }
	    }
	}
}
