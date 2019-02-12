using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotator : MonoBehaviour {

    public enum RotationType
    {
        X,
        Y,
        Z,
        All
    }

    public RotationType ThisRotationType = RotationType.All;

    public float RotationSpeedMultiplyer = 1;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	    if (ThisRotationType== RotationType.X)
	    {
	        transform.Rotate(20 * Time.deltaTime*RotationSpeedMultiplyer, 0,0);
        }
        else
	    if (ThisRotationType == RotationType.Y)
	    {
	        transform.Rotate(0, 20 * Time.deltaTime*RotationSpeedMultiplyer,0);
        }
        else
	    if (ThisRotationType == RotationType.Z)
	    {
	        transform.Rotate(0, 0, 20 * Time.deltaTime*RotationSpeedMultiplyer);
        }
        else
	    if (ThisRotationType == RotationType.All)
	    {
	        transform.Rotate(20 * Time.deltaTime*RotationSpeedMultiplyer, 20 * Time.deltaTime*RotationSpeedMultiplyer, 20 * Time.deltaTime*RotationSpeedMultiplyer);

        }
	}
}
