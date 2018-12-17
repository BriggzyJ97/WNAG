using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;
using UnityEngine.UI;

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

    private int subPhases = 0;
    private float tempTimer = 0;
    private float alpha = 0;
    public int enemiesAlive = 0;

    public TurretManager turretManager;
    public GameObject bossAreaText;
    [Header("Stage One Vars")]
    public GameObject stageOneOneObjects;

    public List<GameObject> stageOneOneWarningTexts = new List<GameObject>();
    public GameObject stageOneTwoObjects;
    public List<GameObject> stageOneTwoWarningTexts = new List<GameObject>();


    public GameObject turret1;
    public GameObject enemy1;
    public List<GameObject> enemies2 = new List<GameObject>();


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
	        if (subPhases==0)
	        {
	            tempTimer += Time.deltaTime;
	            if (tempTimer>1f)
	            {
	                bossAreaText.SetActive(true);

                }

	            if (tempTimer>2f)
	            {
	                alpha += (Time.deltaTime / 2f);
	                foreach (GameObject text in stageOneOneWarningTexts)
	                {
	                    text.GetComponent<TextMeshProUGUI>().color = new Color(1, 0, 0, alpha);
	                    text.transform.parent.gameObject.GetComponent<Image>().color = new Color(1, 0, 0, alpha);
	                }
                    
	            }

	            if (tempTimer>5f)
	            {
	                enemiesAlive = 1;
	                subPhases = 1;
	            }
	            

            }
	        else
	        if (subPhases == 1)
	        {
	            tempTimer = 0;
	            alpha = 0;
                stageOneOneObjects.transform.Translate(0,3f*Time.deltaTime,0);
	            if (stageOneOneObjects.transform.position.y>=4.7f)
	            {
	                turret1.transform.parent.parent.GetComponent<TurretTurner>().enabled = true;
	                bossAreaText.GetComponent<textFlasher>().states = "turnOff";
                    turretManager.turretList.Add(turret1);
	                enemy1.GetComponent<BoxCollider>().enabled = true;
	                foreach (GameObject text in stageOneOneWarningTexts)
	                {
	                    text.GetComponent<TextMeshProUGUI>().color = new Color(1, 0, 0, 0);
	                    text.transform.parent.gameObject.GetComponent<Image>().color = new Color(1, 0, 0, 0);
	                }
                    enemy1.GetComponent<SphereCollider>().enabled = true;
	                enemy1.GetComponent<NavMeshAgent>().enabled = true;
                    subPhases = 2;
	            }
	        }
            else if (subPhases==2)
	        {
	            if (enemiesAlive<=0)
	            {
	                bossAreaText.GetComponent<TextMeshProUGUI>().text = "Escalating";
	                bossAreaText.GetComponent<textFlasher>().states = "flashing";
	                tempTimer += Time.deltaTime;
	                alpha += (Time.deltaTime / 2f);
	                foreach (GameObject text in stageOneTwoWarningTexts)
	                {
	                    text.GetComponent<TextMeshProUGUI>().color = new Color(1, 0, 0, alpha);
	                    text.transform.parent.gameObject.GetComponent<Image>().color = new Color(1, 0, 0, alpha);
	                }

	                if (tempTimer>3f)
	                {
	                    subPhases = 3;
                    }
                    
	            }
	        }else if (subPhases ==3)
	        {
	            enemiesAlive = 4;
	            tempTimer = 0;
	            alpha = 0;
	            stageOneTwoObjects.transform.Translate(0, 3f * Time.deltaTime, 0);
	            if (stageOneTwoObjects.transform.position.y >= 4.7f)
	            {
	                bossAreaText.GetComponent<textFlasher>().states = "turnOff";
	                foreach (GameObject enemy in enemies2)
	                {
	                    enemy.GetComponent<BoxCollider>().enabled = true;
	                    enemy.GetComponent<SphereCollider>().enabled = true;
	                    enemy.GetComponent<NavMeshAgent>().enabled = true;
	                }
	                foreach (GameObject text in stageOneTwoWarningTexts)
	                {
	                    text.GetComponent<TextMeshProUGUI>().color = new Color(1, 0, 0, 0);
	                    text.transform.parent.gameObject.GetComponent<Image>().color = new Color(1, 0, 0, 0);
	                }

                    subPhases = 4;
	            }
            }
	        else if (subPhases == 4)
	        {
	            if (enemiesAlive == 0)
	            {
	                subPhases = 5;
	            }
	        }
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
