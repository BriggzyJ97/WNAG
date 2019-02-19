using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class textFlasher : MonoBehaviour //Makes text slowly increase and decrease in alpha
{

    private float alpha = 0;
    private float t;
    public string states = "flashing";
    public Color textColor;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
	    //pingpong alpha between on and off
	    if (states=="flashing")
	    {
	        t = Mathf.PingPong(Time.time, 1);
	        //Debug.Log("t: " + t);
	        alpha = Mathf.Lerp(0, 1, t);
	        //Debug.Log("alpha: "+alpha);
	        textColor = new Color(textColor.r, textColor.g, textColor.b, alpha);
	        gameObject.GetComponent<TextMeshProUGUI>().color = textColor;
        }
        //otherwise just decreasea alpha till its zero
	    else if (states =="turnOff")
	    {
	        if (alpha>0)
	        {
	            alpha -= Time.deltaTime / 2;
            }
	        else
	        {
	            states = "off";
	        }
	        
	        textColor = new Color(textColor.r, textColor.g, textColor.b, alpha);
	        gameObject.GetComponent<TextMeshProUGUI>().color = textColor;
        }
	}
}
