using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class newStorageBoss : MonoBehaviour
{
    #region Variables

    public int bossPhase = 0;

    [Header("Boss Sections")]
    public GameObject boss;
    public GameObject bossHeadSection;
    public GameObject bossBaseSection;
    public List<GameObject> fireSpawners = new List<GameObject>();
    public GameObject bossRayHead;
    public GameObject bossRayBase;
    
    public GameObject bossWeakSpot;
    private bossHealthController bossHealth;
    public GameObject bulletEmitter;
    public GameObject bulletEmitterL;
    public GameObject bulletEmitterR;
    public GameObject backFacingTarget;
    public GameObject laserEmitter;

    public GameObject bossForceFieldL;
    public GameObject bossForceFieldR;
    public GameObject WeakspotHolderLeft;
    public GameObject WeakspotHolderRight;

    [Header("Non-Boss Objects")]
    public Transform bossTargetPos;

    public GameObject firstBattlePhaseObjects;
    public List<GameObject> firstBattlePhaseTurrets = new List<GameObject>();
    public List<forcefieldController> firstBattleForceFields = new List<forcefieldController>();

    public GameObject secondBattlePhaseObjects;
    
    public GameObject Player;
    public TurretManager turretManager;

    [Header("Boss Floor Canvas Images")]
    public GameObject bossWarningText;
    public GameObject bossAreaText;
    public List<GameObject> firstBattlePhaseWarnings = new List<GameObject>();
    public List<GameObject> secondBattlePhaseWarnings = new List<GameObject>();


    [Header("Boss Variables")]
    

    private bool headLookingAtPlayer = false;

    private bool closeWeakSpotCover = false;
    private GameObject headTarget;

    private float shootingCooldown = 0f;
    private float shootingCooldownMax = 0.5f;
    public GameObject bulletPrefab;
    private string baseLookingAt ="";
    private float bossMoveSpeed = 10f;
    private float timer1;
    private float timer2;
    private float alpha = 0;
    private Transform tempTransform;

    public GameObject laser;
#endregion

    // Use this for initialization
    void Start ()
    {
        bossHealth = bossWeakSpot.GetComponent<bossHealthController>();
    }
	
	// Update is called once per frame
	void Update () {

        /*
            ---------------------------------------INDEX OF BOSS FIGHT STAGES-----------------------------------------------
            1- //flash the boss warning circle
            2- //move boss into room and start fire
            3- //spin boss head until looking at player
            4- //first turret warnings
            5- //move turrets for first battle phase into room, and turn them on
            6- //turn on turret forcefields
            7- //open boss weak spot and spin base to face player
            8- //first battle phase
            9- //rotate base to face away from player
            10-//second battle phase
            11-//rotate boss head until it faces turret
            12-//shoot laser at turret 1
            13-//wait for laser to shoot
            14-//rotate head to face other turret
            15-//shoot laser at turret 2
            16-//wait for laser to shoot
            17-//warnings for mirrors
            18-//bring up mirrors
            19-//turn on boss forcefield
            20-//rotate head to face player
            21-//turn on boss targetting lines and wait a sec
            22-//battle phase 3
            23-
            24-
            25-
            26-
            27-
            --------------------------------------------------------------------------------------------------------------

        */
        //flash the boss warning circle
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

        //move boss into room and start fire
	    else if (bossPhase == 2)
	    {
            
            if (Vector3.Distance(boss.transform.position, bossTargetPos.position) > 0.1f)
            {
                bossBaseSection.GetComponent<CapsuleCollider>().enabled = true;
                bossWeakSpot.GetComponent<CapsuleCollider>().enabled = true;
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

        //spin boss head until looking at player
	    else if (bossPhase == 3)
	    {
	        bossHeadSection.transform.Rotate(0, 180f * Time.deltaTime, 0);
	        if (bossRayHead.GetComponent<bossRayShooter>().playerHitCounter>=2)
	        {
	            bossRayHead.SetActive(false);
	            alpha = 0;
	            headTarget = Player;
	            headLookingAtPlayer = true;
                bossPhase = 4;
	        }
        }

        //first turret warnings
	    else if (bossPhase==4)
	    {
	        alpha += (Time.deltaTime / 1.5f);
	        foreach (GameObject text in firstBattlePhaseWarnings)
	        {
	            text.GetComponent<TextMeshProUGUI>().color = new Color(1, 0, 0, alpha);
	            text.transform.parent.gameObject.GetComponent<Image>().color = new Color(1, 0, 0, alpha);
	        }

	        if (alpha>=0.75f)
	        {
	            bossPhase = 5;
	        }
        }

        //move turrets for first battle phase into room, and turn them on
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

        //turn on turret forcefields
        else if (bossPhase == 6)
	    {
	        timer1 += Time.deltaTime;
	        if (timer1 >= 0f)
	        {
                //Debug.Log("timer up");
	            foreach (forcefieldController fF in firstBattleForceFields)
	            {
	                //Debug.Log("fftrigger");
                    fF.TurnForceFieldOn();
	            }
	        }

	        if (timer1 > 1.5f)
	        {
	            bossPhase = 7;
	        }
        }

        //open boss weak spot and spin base to face player 
        else if (bossPhase==7)
	    {

	        timer1 += Time.deltaTime;
	        if (timer1>1.5f&&timer1<3.5f)
	        {
                WeakspotHolderLeft.transform.Rotate(0,-50f*Time.deltaTime,0);
	            WeakspotHolderRight.transform.Rotate(0, 50f * Time.deltaTime, 0);
                
            }

	        if (timer1>3.5f&&timer1<4f)
	        {
	            bossRayBase.SetActive(true);
            }
	        if (timer1>4f)
	        {
	            bossBaseSection.transform.Rotate(0, 180f * Time.deltaTime, 0);
	            if (bossRayBase.GetComponent<bossRayShooter>().playerHitCounter>=1)
	            {
	                bossRayBase.SetActive(false);
	                bossHealth.bossHealth = 3;
	                bossPhase = 8;
	            }
            }

	    }

        //first battle phase
        else if (bossPhase==8)
	    {
	        baseLookingAt = "Player";
	        shootingCooldown += Time.deltaTime;
            bulletEmitter.GetComponentInChildren<LineRenderer>().enabled = true;
	        bulletEmitterL.GetComponentInChildren<LineRenderer>().enabled = true;
	        bulletEmitterR.GetComponentInChildren<LineRenderer>().enabled = true;
	        if (Input.GetMouseButton(0)&&shootingCooldown > shootingCooldownMax)
	        {
	            Instantiate(bulletPrefab, bulletEmitter.transform.position, bulletEmitter.transform.rotation);
	            Instantiate(bulletPrefab, bulletEmitterL.transform.position, bulletEmitterL.transform.rotation);
	            Instantiate(bulletPrefab, bulletEmitterR.transform.position, bulletEmitterR.transform.rotation);
                shootingCooldown = 0;
	        }

	        if (bossHealth.bossHealth==0)
	        {
	            baseLookingAt = "";
	            timer1 = 0;
                bossRayBase.SetActive(true);
	            bossRayBase.GetComponent<bossRayShooter>().hitBack = false;
	            bossPhase = 9;
	        }
	    }

        //rotate base to face away from player
	    else if (bossPhase==9)
	    {
	        bossBaseSection.transform.Rotate(0, 180f * Time.deltaTime, 0);
	        timer1 += Time.deltaTime;
	        if (bossRayBase.GetComponent<bossRayShooter>().hitBack==true)
	        {
	            baseLookingAt = "Back";
                bossRayBase.SetActive(false);
	            bossHealth.bossHealth = 3;
	            bossPhase = 10;
	        }
        }

        //second battle phase
	    else if (bossPhase == 10)
	    {
	        shootingCooldown += Time.deltaTime;
	       
            if (Input.GetMouseButton(0) && shootingCooldown > shootingCooldownMax)
	        {
	            Instantiate(bulletPrefab, bulletEmitter.transform.position, bulletEmitter.transform.rotation);
	            Instantiate(bulletPrefab, bulletEmitterL.transform.position, bulletEmitterL.transform.rotation);
	            Instantiate(bulletPrefab, bulletEmitterR.transform.position, bulletEmitterR.transform.rotation);
	            shootingCooldown = 0;
	        }
	        if (bossHealth.bossHealth == 0)
	        {
	            baseLookingAt = "";
	            timer1 = 0;

	            bossRayHead.SetActive(true);
	            bossRayHead.GetComponent<bossRayShooter>().turretHitCounter = 0;
	            headLookingAtPlayer = false;
	            bossRayBase.GetComponent<bossRayShooter>().hitBack = false;
	            bulletEmitter.GetComponentInChildren<LineRenderer>().enabled = false;
	            bulletEmitterL.GetComponentInChildren<LineRenderer>().enabled = false;
	            bulletEmitterR.GetComponentInChildren<LineRenderer>().enabled = false;
                bossPhase = 11;
	        }
        }

        //rotate boss head until it faces turret
	    else if (bossPhase == 11)
	    {
	        bossHeadSection.transform.Rotate(0, 180f * Time.deltaTime, 0);
	        closeWeakSpotCover = true;
            if (bossRayHead.GetComponent<bossRayShooter>().turretHitCounter >= 2)
	        {
	            bossRayHead.SetActive(false);
	            headTarget = bossRayHead.GetComponent<bossRayShooter>().lastTurretHit;
	            headLookingAtPlayer = true;
	            timer1 = 0;
	            
	            bossPhase = 12;
	        }
        }

        //shoot laser at turret 1
	    else if (bossPhase == 12)
	    {
	        headTarget = null;
	        Instantiate(laser, laserEmitter.transform.position, laserEmitter
	            .transform.rotation);

	        bossPhase = 13;
	    }

        //wait for laser to shoot
	    else if (bossPhase == 13)
	    {
	        timer1 += Time.deltaTime;
	        if (timer1>13f)
	        {
	            bossRayHead.SetActive(true);
	            bossRayHead.GetComponent<bossRayShooter>().turretHitCounter = 0;
	            headLookingAtPlayer = false;
                bossPhase = 14;
	        }
	    }
        
        //rotate head to face other turret
	    else if (bossPhase == 14)
	    {
	        bossHeadSection.transform.Rotate(0, 180f * Time.deltaTime, 0);
	        if (bossRayHead.GetComponent<bossRayShooter>().turretHitCounter >= 2)
	        {
	            bossRayHead.SetActive(false);
	            headTarget = bossRayHead.GetComponent<bossRayShooter>().lastTurretHit;
	            headLookingAtPlayer = true;
	            timer1 = 0;

	            bossPhase = 15;
	        }
        }

	    //shoot laser at turret 2
        else if (bossPhase==15)

	    {
	        headTarget = null;
	        Instantiate(laser, laserEmitter.transform.position, laserEmitter
	            .transform.rotation);
	        timer1 = 0;
	        alpha = 0;
	        bossPhase = 16;
        }

        //wait for laser to shoot
	    else if (bossPhase == 16)

	    {
            timer1 += Time.deltaTime;
            if (timer1 > 13f)
            {
                
                headLookingAtPlayer = false;
                bossPhase = 17;
            }
        }

        //warnings for mirrors
        else if (bossPhase == 17)

	    {
	        alpha += (Time.deltaTime / 1.5f);
	        foreach (GameObject text in secondBattlePhaseWarnings)
	        {
	            text.GetComponent<TextMeshProUGUI>().color = new Color(1, 0, 0, alpha);
	            text.transform.parent.gameObject.GetComponent<Image>().color = new Color(1, 0, 0, alpha);
	        }

	        if (alpha >= 0.75f)
	        {
	            timer1 = 0;
	            bossPhase = 18;
	        }
        }

        //bring up mirrors
	    else if (bossPhase == 18)

	    {
	        secondBattlePhaseObjects.transform.Translate(0, 3f * Time.deltaTime, 0);
	        if (secondBattlePhaseObjects.transform.position.y >= 4.7f)
	        {
	            foreach (GameObject text in secondBattlePhaseWarnings)
	            {
	                text.GetComponent<TextMeshProUGUI>().color = new Color(1, 0, 0, 0);
	                text.transform.parent.gameObject.GetComponent<Image>().color = new Color(1, 0, 0, 0);
	            }
                bossPhase = 19;
	        }
        }

        //turn on boss forcefield
	    else if (bossPhase == 19)

	    {
	        timer1 += Time.deltaTime*2;
            bossForceFieldL.GetComponent<Renderer>().material.SetFloat("_FresnelWidth",timer1/2);
	        bossForceFieldR.GetComponent<Renderer>().material.SetFloat("_FresnelWidth", timer1 / 2);
	        bossForceFieldL.GetComponent<Renderer>().material.SetFloat("_Distort", timer1 *150);
	        bossForceFieldR.GetComponent<Renderer>().material.SetFloat("_Distort", timer1 *150);
            if (timer1>4)
            {
                bossRayHead.GetComponent<bossRayShooter>().playerHitCounter = 0;
                headLookingAtPlayer = false;
                bossRayHead.SetActive(true);
                bossPhase = 20;
            }
        }

        //rotate head to face player
        else if (bossPhase==20)
	    {
	        bossHeadSection.transform.Rotate(0, 180f * Time.deltaTime, 0);
	        if (bossRayHead.GetComponent<bossRayShooter>().playerHitCounter >= 1)
	        {
	            bossRayHead.SetActive(false);
	            alpha = 0;
	            timer2 = 0;
	            headTarget = Player;
	            headLookingAtPlayer = true;
	            bossPhase = 21;
	        }
        }

        //turn on boss targetting lines and wait a sec
        else if (bossPhase==21)
	    {
	        //bulletEmitter.GetComponentInChildren<LineRenderer>().enabled = true;
	        bulletEmitterL.GetComponentInChildren<LineRenderer>().enabled = true;
	        bulletEmitterR.GetComponentInChildren<LineRenderer>().enabled = true;
	        timer1 += Time.deltaTime;
	        if (timer1>1)
	        {
	            bossPhase = 22;
	        }
	    }

        //battle phase 3
	    else if (bossPhase == 22)
	    {
	        timer1 += Time.deltaTime;
            timer2 += Time.deltaTime;
	        if (timer2>0.5f)
	        {
	            Instantiate(bulletPrefab, bulletEmitterL.transform.position, bulletEmitterL.transform.rotation);
	            Instantiate(bulletPrefab, bulletEmitterR.transform.position, bulletEmitterR.transform.rotation);
                timer2 = 0;
	        }

	        if (timer1>4)
	        {
	            Instantiate(laser, laserEmitter.transform.position, laserEmitter
	                .transform.rotation);
	            timer1 = -12f;
	        }
	    }

	    else if (bossPhase == 23)
	    {

	    }
	    else if (bossPhase == 24)
	    {

	    }




        if (headLookingAtPlayer==true&&headTarget!=null)
	    {
	        //tempTransform.LookAt(Player.transform.position);
	        //bossHeadSection.transform.rotation = Quaternion.RotateTowards(bossHeadSection.transform.rotation,
	        //Player.transform.rotation, 1f * Time.deltaTime);

	        //Vector3 direction = Player.transform.position - bossHeadSection.transform.position;
	        //Quaternion toRotation = Quaternion.LookRotation(transform.up, direction);
	        //bossHeadSection.transform.rotation = Quaternion.Lerp(bossHeadSection.transform.rotation, toRotation, 50f * Time.time);
            bossHeadSection.transform.LookAt(new Vector3(headTarget.transform.position.x, transform.position.y, headTarget.transform.position.z));
        }

	    if (baseLookingAt=="Player")
	    {
	        bossBaseSection.transform.LookAt(new Vector3(Player.transform.position.x, transform.position.y, Player.transform.position.z));
        }
        else
	    if (baseLookingAt=="Back")
	    {
	        bossBaseSection.transform.LookAt(new Vector3(backFacingTarget.transform.position.x, transform.position.y, backFacingTarget.transform.position.z));
        }

	    if (closeWeakSpotCover==true)
	    {
	        timer2 += Time.deltaTime;
	        if (timer2<2)
	        {
	            WeakspotHolderLeft.transform.Rotate(0, 50f * Time.deltaTime, 0);
	            WeakspotHolderRight.transform.Rotate(0, -50f * Time.deltaTime, 0);
            }
	        
	        else
	        {
	            closeWeakSpotCover = false;
	        }
	            

	        
        }
	}

    public void TurnOnBoss()
    {
        bossPhase = 1;
    }
}
