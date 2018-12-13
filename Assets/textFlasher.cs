using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class textFlasher : MonoBehaviour
{

    private float alpha;
    private float t;

    public Color textColor;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
	    t = Mathf.PingPong(Time.time, 1);
	    //Debug.Log("t: " + t);
        alpha = Mathf.Lerp(0, 1, t);
        //Debug.Log("alpha: "+alpha);
        textColor = new Color(textColor.r,textColor.g,textColor.b,alpha);
	    gameObject.GetComponent<TextMeshProUGUI>().color = textColor;
	}
}
