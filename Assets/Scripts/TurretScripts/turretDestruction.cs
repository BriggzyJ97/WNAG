using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turretDestruction : MonoBehaviour
{

    private TurretManager turretManager;
    public GameObject turretShooter;
    public GameObject turretDeathExplosion;

	// Use this for initialization
	void Start ()
	{
	    turretManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<TurretManager>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void DestroyTurret()
    {
        turretManager.turretList.Remove(turretShooter);
        Instantiate(turretDeathExplosion, transform.position, Quaternion.identity);
        Destroy(turretShooter.GetComponent<TurretShooter>().tempLineRenderer);
        Destroy(this.gameObject);
    }
}
