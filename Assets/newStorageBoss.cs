using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class newStorageBoss : MonoBehaviour
{

    public int bossPhase = 0;

    [Header("Boss Sections")]
    public GameObject boss;
    public GameObject bossHeadSection;
    public GameObject bossBaseSection;
    public List<GameObject> fireSpawners = new List<GameObject>();
    public GameObject bossRayHead;
    public GameObject bossRayBase;

    public GameObject WeakspotHolderLeft;
    public GameObject WeakspotHolderRight;

    [Header("Non-Boss Objects")]
    public Transform bossTargetPos;

    public GameObject firstBattlePhaseObjects;
    public List<GameObject> firstBattlePhaseTurrets = new List<GameObject>();
    
    public GameObject Player;
    public TurretManager turretManager;

    [Header("Boss Floor Canvas Images")]
    public GameObject bossWarningText;
    public GameObject bossAreaText;
    public List<GameObject> firstBattlePhaseWarnings = new List<GameObject>();


    [Header("Boss Variables")]
    

    private bool headLookingAtPlayer = false;

    private bool baseLookingAtPlayer = false;
    private float bossMoveSpeed = 10f;
    private float timer1;
    private float alpha = 0;
    private Transform tempTransform;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	    if (bossPhase==1)
	    {
	        timer1 += Time.deltaTime;
	        if (timer1>1f&&timer1<2f)
	        {
                bossAreaText.SetActive(true);
	        }

	        if (timer1>5f)
	        {
	            alpha += (Time.deltaTime / 2f);
                bossAreaText.GetComponent<textFlasher>().states = "turnOff";
	            bossWarningText.GetComponent<TextMeshProUGUI>().color = new Color(1, 0, 0, alpha);
	            bossWarningText.transform.parent.gameObject.GetComponent<Image>().color = new Color(1, 0, 0, alpha);
                
            }

	        if (timer1>7f)
	        {
	            alpha = 1;
	            bossWarningText.GetComponent<TextMeshProUGUI>().color = new Color(1, 0, 0, 0);
	            bossWarningText.transform.parent.gameObject.GetComponent<Image>().color = new Color(1, 0, 0, 0);
                bossPhase = 2;
	            
            }
	    }
	    else if (bossPhase == 2)
	    {
            
            if (Vector3.Distance(boss.transform.position, bossTargetPos.position) > 0.1f)
	        {
	            boss.transform.position = Vector3.MoveTowards(boss.transform.position, bossTargetPos.position, bossMoveSpeed * Time.deltaTime);
	        }
	        else
	        {
	            foreach (GameObject fireSpawner in fireSpawners)
	            {
	                fireSpawner.SetActive(true);
	            }
	            bossRayHead.SetActive(true);
                timer1 = 0;
	            bossPhase = 3;

	        }
        }
	    else if (bossPhase == 3)
	    {
	        bossHeadSection.transform.Rotate(0, 180f * Time.deltaTime, 0);
	        if (bossRayHead.GetComponent<bossRayShooter>().playerHitCounter>=2)
	        {
	            bossRayHead.SetActive(false);
	            alpha = 0;
	            headLookingAtPlayer = true;
                bossPhase = 4;
	        }
        }else if (bossPhase==4)
	    {
	        alpha += (Time.deltaTime / 2);
	        foreach (GameObject text in firstBattlePhaseWarnings)
	        {
	            text.GetComponent<TextMeshProUGUI>().color = new Color(1, 0, 0, alpha);
	            text.transform.parent.gameObject.GetComponent<Image>().color = new Color(1, 0, 0, alpha);
	        }

	        if (alpha>=1.5f)
	        {
	            bossPhase = 5;
	        }
        }
	    else if (bossPhase == 5)
	    {
	        firstBattlePhaseObjects.transform.Translate(0, 3f * Time.deltaTime, 0);
	        if (firstBattlePhaseObjects.transform.position.y >= 4.7f)
	        {
	            foreach (GameObject turret in firstBattlePhaseTurrets)
	            {
	                turret.transform.parent.parent.GetComponent<TurretTurner>().enabled = true;
                    turretManager.turretList.Add(turret);
	            }

	            foreach (GameObject text in firstBattlePhaseWarnings)
	            {
	                text.GetComponent<TextMeshProUGUI>().color = new Color(1, 0, 0, 0);
	                text.transform.parent.gameObject.GetComponent<Image>().color = new Color(1, 0, 0, 0);
	            }

                bossPhase = 6;
	        }
        }
        else if (bossPhase==6)
	    {

	        timer1 += Time.deltaTime;
	        if (timer1>1&&timer1<3)
	        {
                WeakspotHolderLeft.transform.Rotate(0,-50f*Time.deltaTime,0);
	            WeakspotHolderRight.transform.Rotate(0, 50f * Time.deltaTime, 0);
                
            }

	        if (timer1>3&&timer1<3.5f)
	        {
	            bossRayBase.SetActive(true);
            }
	        if (timer1>3.5f)
	        {
	            bossBaseSection.transform.Rotate(0, 180f * Time.deltaTime, 0);
	            if (bossRayBase.GetComponent<bossRayShooter>().playerHitCounter>=1)
	            {
	                bossRayBase.SetActive(false);
	                bossPhase = 7;
	            }
            }

	    }
        else if (bossPhase==7)
	    {
	        baseLookingAtPlayer = true;
	    }




	    if (headLookingAtPlayer==true)
	    {
	        //tempTransform.LookAt(Player.transform.position);
	        //bossHeadSection.transform.rotation = Quaternion.RotateTowards(bossHeadSection.transform.rotation,
	        //Player.transform.rotation, 1f * Time.deltaTime);

	        //Vector3 direction = Player.transform.position - bossHeadSection.transform.position;
	        //Quaternion toRotation = Quaternion.LookRotation(transform.up, direction);
	        //bossHeadSection.transform.rotation = Quaternion.Lerp(bossHeadSection.transform.rotation, toRotation, 50f * Time.time);
            bossHeadSection.transform.LookAt(new Vector3(Player.transform.position.x, transform.position.y, Player.transform.position.z));
        }

	    if (baseLookingAtPlayer==true)
	    {
	        bossBaseSection.transform.LookAt(new Vector3(Player.transform.position.x, transform.position.y, Player.transform.position.z));
        }
	}

    public void TurnOnBoss()
    {
        bossPhase = 1;
    }
}
