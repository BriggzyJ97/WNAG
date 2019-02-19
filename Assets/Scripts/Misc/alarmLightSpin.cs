using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class alarmLightSpin : MonoBehaviour {//this script rotates the alarm lights
   
	void Update () {
		transform.Rotate(0,10f,0);
	}
}
