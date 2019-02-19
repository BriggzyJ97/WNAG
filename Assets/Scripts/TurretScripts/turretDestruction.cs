using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turretDestruction : MonoBehaviour
//This script is used to destroy turrets, removing them from lists and spawning their death explosions
{

    private TurretManager turretManager;//list of all the turrets, used for shooting
    public GameObject turretShooter;//this turrets shooting script
    public GameObject turretDeathExplosion;//prefab of turret death explosion

    //assign turretManager variable
    void Start ()
	{
	    turretManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<TurretManager>();
	}

    // Called from the object destroying the turret (laser in boss fight)
    public void DestroyTurret()
    {
        turretManager.turretList.Remove(turretShooter);//remove the turret from the list of turrets to prevent errors
        Instantiate(turretDeathExplosion, transform.position, Quaternion.identity);//spawn the turret death explosion
        Destroy(turretShooter.GetComponent<TurretShooter>().tempLineRenderer);//destroy the targetting laser
        Destroy(this.gameObject);//destroy the turret
    }
}
