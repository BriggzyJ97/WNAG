using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossHealthController : MonoBehaviour//this script keeps track of boss health
{

    public int bossHealth = 0;

    public void LowerHealth()
    {
        bossHealth -= 1;
    }
}
