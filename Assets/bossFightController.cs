using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossFightController : MonoBehaviour {

    public enum BossStates
    {
        off,
        startup,
        phaseOne,
        phaseTwo,
        phaseThree,
        death,
        end
    }

    public BossStates currentBossState = BossStates.off;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	    if (currentBossState==BossStates.off)
	    {

        }
	    else if(currentBossState == BossStates.startup)
	    {

	    }
	    else if (currentBossState == BossStates.phaseOne)
	    {

	    }
	    else if (currentBossState == BossStates.phaseTwo)
	    {

	    }
	    else if (currentBossState == BossStates.phaseThree)
	    {

	    }
	    else if (currentBossState == BossStates.death)
	    {

	    }
	    else if (currentBossState == BossStates.end)
	    {

	    }
    }

    public void TurnOnBoss()
    {
        currentBossState = BossStates.startup;
    }
}
