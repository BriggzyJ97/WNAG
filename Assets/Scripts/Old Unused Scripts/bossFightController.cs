using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;
using UnityEngine.UI;


//THIS IS FOR THE OLD VERSION OF BOSS FIGHT, NOT USED
public class bossFightController : MonoBehaviour {

    public enum BossStates
    {
        off,
        on,
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
    public GameObject stageTwoOneObjects;
    public List<GameObject> stageTwoOneWarningTexts = new List<GameObject>();
    public GameObject stageTwoTwoObjects;
    public List<GameObject> stageTwoTwoWarningTexts = new List<GameObject>();


    public GameObject turret1;
    public GameObject enemy1;
    public List<GameObject> enemies2 = new List<GameObject>();
    public GameObject turret2;
    public GameObject enemy3;
    public List<GameObject> enemies4 = new List<GameObject>();


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	    if (currentBossState==BossStates.off)
	    {
            
        }
	    else if(currentBossState == BossStates.on)
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
	                turret1.transform.parent.parent.GetComponent<TurretTurner>().enabled = false;
	                turretManager.turretList.Remove(turret1);
	                stageOneOneObjects.transform.Translate(0, -3f * Time.deltaTime, 0);
                    bossAreaText.GetComponent<TextMeshProUGUI>().text = "Evolving";
	                bossAreaText.GetComponent<textFlasher>().states = "flashing";
	                tempTimer += Time.deltaTime;
	                alpha += (Time.deltaTime / 2f);
	                foreach (GameObject text in stageTwoOneWarningTexts)
	                {
	                    text.GetComponent<TextMeshProUGUI>().color = new Color(1, 0, 0, alpha);
	                    text.transform.parent.gameObject.GetComponent<Image>().color = new Color(1, 0, 0, alpha);
	                }

	                if (tempTimer > 3f)
	                {
	                    subPhases = 5;
	                }
                }
	        }else if (subPhases == 5)
	        {
	            enemiesAlive = 1;
	            tempTimer = 0;
	            alpha = 0;
	            stageTwoOneObjects.transform.Translate(0, 3f * Time.deltaTime, 0);
	            if (stageTwoOneObjects.transform.position.y >= 0f)
	            {
	                turret2.transform.parent.parent.GetComponent<TurretTurner>().enabled = true;
	                turretManager.turretList.Add(turret2);
                    bossAreaText.GetComponent<textFlasher>().states = "turnOff";
	                enemy3.GetComponent<BoxCollider>().enabled = true;
	                enemy3.GetComponent<SphereCollider>().enabled = true;
	                enemy3.GetComponent<NavMeshAgent>().enabled = true;
                    //Debug.Log("before");
                    foreach (GameObject text in stageTwoOneWarningTexts)
	                {
	                    //Debug.Log("during");
                        text.GetComponent<TextMeshProUGUI>().color = new Color(1, 0, 0, 0);
	                    text.transform.parent.gameObject.GetComponent<Image>().color = new Color(1, 0, 0, 0);
	                    //Debug.Log("during2");
                    }
	                //Debug.Log("after");
                    subPhases = 6;
	            }
            }
            else if (subPhases == 6) 
	        {
	            if (enemiesAlive == 0)
	            {
	                //Debug.Log("subphase 6");
                    bossAreaText.GetComponent<TextMeshProUGUI>().text = "Exhausting";
	                bossAreaText.GetComponent<textFlasher>().states = "flashing";
	                tempTimer += Time.deltaTime;
	                alpha += (Time.deltaTime / 2f);
	                foreach (GameObject text in stageTwoTwoWarningTexts)
	                {
	                    text.GetComponent<TextMeshProUGUI>().color = new Color(1, 0, 0, alpha);
	                    text.transform.parent.gameObject.GetComponent<Image>().color = new Color(1, 0, 0, alpha);
	                }

	                if (tempTimer > 3f)
	                {
	                    subPhases = 7;
	                }
	            }
            }
            else if (subPhases == 7)
	        {
	            enemiesAlive = 4;
	            tempTimer = 0;
	            alpha = 0;
	            stageTwoTwoObjects.transform.Translate(0, 3f * Time.deltaTime, 0);

	            if (stageTwoTwoObjects.transform.position.y >= 0f)
	            {
	                foreach (GameObject enemy in enemies4)
	                {
	                    enemy.GetComponent<BoxCollider>().enabled = true;
	                    enemy.GetComponent<SphereCollider>().enabled = true;
	                    enemy.GetComponent<NavMeshAgent>().enabled = true;
	                }
                    bossAreaText.GetComponent<textFlasher>().states = "turnOff";
	                foreach (GameObject text in stageTwoTwoWarningTexts)
	                {
	                    text.GetComponent<TextMeshProUGUI>().color = new Color(1, 0, 0, 0);
	                    text.transform.parent.gameObject.GetComponent<Image>().color = new Color(1, 0, 0, 0);
	                }

	                subPhases = 8;
	            }
            }
            else if (subPhases == 8)
	        {
	            if (enemiesAlive == 0) { 

	            }
	        }
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
        currentBossState = BossStates.on;
    }
}
