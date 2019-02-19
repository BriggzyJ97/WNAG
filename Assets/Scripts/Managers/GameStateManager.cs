﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.PostProcessing;

public class GameStateManager : MonoBehaviour { //This script manages the states of the level, as well as pause menus, loading scenes

    #region Variables

    public enum GameState //Level state
    {
        levelPlaying,
        levelWin,
        levelDialogue,
        levelLose,
        levelEnd
    }

    public GameState currentGameState = GameState.levelPlaying; 

    public GameObject door;
    private float timer = 2f; //timer used for giving the door enough time to open before loading the next level
    
    public GameObject fade; //black ui fadeout
    public float alpha = 1;// alpha variable for fadeout

    public TextMeshProUGUI timeText; //text object that shows the time in-world

    public TextMeshProUGUI winText; // text object used in demo to thank players for playing

    private CompletionKeeper completionKeeper; //singleton that keeps track of how many levels completed
    private Camera mainCamera; //camera for levels
    private string PPSetting; //which post-processing setting is being used

    [Header("Pause Menu Variables")]
    public bool isGamePaused = false; //if the game is paused
    public GameObject pauseMenu; 
    public GameObject optionsMenu;
    [Header("Game Ending Variables")]
    public GameObject grabPassObject; //object with grabpass shader used for distorting the level when the player dies
    private float grabPassIntensity = 0; // intesity of grab pass effect, which is rapidly changed for distortion effect

    public GameObject noiseObject; //static noise that shows when player dies
    public noiseSpriteData noiseSprites; //scriptable object that holds different noise sprites to create static
    private float noiseTimer = 1f; //timer for how long the noise stays on screen at end
    public GameObject noiseBar; //the black bar on the static noise
    private float barMoveSpeed = 10f; // movespeed of the static bar

    private AudioSource staticNoise;
    public AudioClip staticSound;

    public enum LoseState //states of when you lose
    {
       
        grabPass,
        tvNoise,
        load
    }

    public LoseState currentLoseState = LoseState.grabPass;

    public DialogueManager dialogueController;

    private bool loadingToMainMenu;

#endregion

    //assign variables and post processing settings
    void Start ()
	{
	    staticNoise = gameObject.GetComponent<AudioSource>();
        completionKeeper = GameObject.FindGameObjectWithTag("CompletionKeeper").GetComponent<CompletionKeeper>();
	    mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
	    PPSetting = PlayerPrefs.GetString("PPSetting", "max");
	    if (PPSetting == "max")
	    {
	        mainCamera.GetComponent<PostProcessingBehaviour>().profile = completionKeeper.cctvPPProfileMax;
	    }
	    else if (PPSetting == "min")
	    {
	        mainCamera.GetComponent<PostProcessingBehaviour>().profile = completionKeeper.cctvPPProfileMin;
	    }
	    else if (PPSetting == "none")
	    {
	        mainCamera.GetComponent<PostProcessingBehaviour>().profile = completionKeeper.cctvPPProfileNone;
	    }

	    
	}
	
	// Update is called once per frame
	void Update ()
	{
	    if (staticNoise==null)
	    {
	        staticNoise = gameObject.GetComponent<AudioSource>();
	    }
        //Todays date but 100 years in the future is put in timetext
	    DateTime tempDateTime = System.DateTime.Now.AddYears(100); 
	    timeText.text = tempDateTime.ToString();

        //Pause toggle
	    if (Input.GetKeyUp(KeyCode.Escape))
	    {
	        if (isGamePaused == false) 
	        {
                pauseMenu.SetActive(true);
	            isGamePaused = true;
	        }else if (isGamePaused==true)
	        {
                optionsMenu.SetActive(false);
                pauseMenu.SetActive(false);
	            isGamePaused = false;
	        }
	    }

        //get rid of fade (Not in currently)
	    if (currentGameState==GameState.levelPlaying)
	    {
	        if (fade.GetComponent<Image>().color.a>0.0f)
	        {
	            alpha -= Time.deltaTime / 2;
                fade.GetComponent<Image>().color = new Color(0,0,0,alpha);
	        }
	    }

	    //When the player wins 
        if (currentGameState==GameState.levelWin&&SceneManager.GetActiveScene().name!="StorageBoss") 
	    {
            //set all player doubles to disable
            if(TurretTurner.playerList!=null)
	        {
	            if (TurretTurner.playerList.Count > 0)
	            {
	                foreach (GameObject player in TurretTurner.playerList)
	                {
	                    if (player != null)
	                    {
	                        if (player.GetComponent<playerDouble>() != null)
	                        {
	                            player.GetComponent<playerDouble>().Disable();
	                        }
	                    }

	                }
	            }
            }
	        
	        
            //open the door
	        door.GetComponent<doorControl>().doorOpen = true;
	        timer -= Time.deltaTime;

	        //timer to load the next scene
            if (timer<0)
	        {
	            currentGameState = GameState.levelEnd;
	        }
	    }

	    // if the level ending
        if (currentGameState==GameState.levelEnd)
	    {
	        //fade to black
            if (fade.GetComponent<Image>().color.a < 0.99f)
	        {
	            if (fade.activeSelf==false)
	            {
	                fade.SetActive(true);
                    fade.GetComponent<Image>().color = new Color(0,0,0,0);
                }
                Debug.Log("darkening fade");
	            alpha += Time.deltaTime / 2;
	            fade.GetComponent<Image>().color = new Color(0, 0, 0, alpha);
            }

            //then show the noise and cycle through different sprites of it
            //after 1 sec load next levels, increment how many levels completed on keeper or go back to main menu if theres no more levels
            else
            {
                
	            if (noiseObject.activeSelf == false)
	            {
	                staticNoise.clip = staticSound;
	                staticNoise.volume = 0.015f;
	                staticNoise.Play();
                    noiseObject.SetActive(true);
	            }

	            if (noiseObject.GetComponent<Image>().sprite == noiseSprites.noise1)
	            {
	                noiseObject.GetComponent<Image>().sprite = noiseSprites.noise2;
	            }
	            else if (noiseObject.GetComponent<Image>().sprite == noiseSprites.noise2)
	            {
	                noiseObject.GetComponent<Image>().sprite = noiseSprites.noise3;
	            }
	            else if (noiseObject.GetComponent<Image>().sprite == noiseSprites.noise3)
	            {
	                noiseObject.GetComponent<Image>().sprite = noiseSprites.noise4;
	            }
	            else
	            {
	                noiseObject.GetComponent<Image>().sprite = noiseSprites.noise1;
	            }

	            noiseBar.GetComponent<RectTransform>().Translate(0, -barMoveSpeed * Time.deltaTime, 0);

	            noiseTimer -= Time.deltaTime;
                //after 1 sec load next levels, increment how many levels completed on keeper or go back to main menu if theres no more levels
                if (noiseTimer < 0)
	            {
	                if (winText != null)
	                {
	                    winText.text = "End of demo \nThanks For Playing! \nPlease Rate! < 3";
	                }

	                if (completionKeeper.howManyLevelsCompleted <= SceneManager.GetActiveScene().buildIndex-1)
	                {
	                    completionKeeper.howManyLevelsCompleted += 1;
	                   
	                    PlayerPrefs.SetInt("levelsCompleted", completionKeeper.howManyLevelsCompleted);
	                }else if (SceneManager.GetActiveScene().buildIndex == 1&&completionKeeper.howManyLevelsCompleted==0)
	                {
	                   // completionKeeper.howManyLevelsCompleted++;
	                   // PlayerPrefs.SetInt("levelsCompleted", completionKeeper.howManyLevelsCompleted);
                    }
                    //Debug.Log(SceneManager.GetActiveScene().buildIndex);
	                if (SceneManager.GetActiveScene().buildIndex==21)
	                {
	                    SceneManager.LoadScene("MenuScene");
                    }
                    else
	                if (SceneManager.sceneCountInBuildSettings != SceneManager.GetActiveScene().buildIndex + 1 )
	                {
	                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	                }
	                else
	                {
	                    SceneManager.LoadScene("MenuScene");
	                }
                }
                
                
	        }
        }

	    //if the player dies 
        if (currentGameState==GameState.levelLose) 
	    {
	        //distort using the grab pass object
            if (currentLoseState == LoseState.grabPass) 
	        {
	            if (grabPassObject.activeSelf==false)
	            {
	                grabPassObject.SetActive(true);
                }
	            else
	            {
                    grabPassObject.SetActive(false);
	            }
                grabPassIntensity += 3 * Time.deltaTime;
                grabPassObject.GetComponent<Renderer>().material.SetFloat("_Intensity", grabPassIntensity);
	            if (grabPassIntensity>=3)
	            {
	                currentLoseState = LoseState.tvNoise;
	            }
	        }

            //show the tv static noise and cycle through sprite variations for it to animate and reload scene
            else if (currentLoseState == LoseState.tvNoise) 
	        {
	            if (noiseObject.activeSelf == false)
	            {
	                staticNoise.clip = staticSound;
	                staticNoise.volume = 0.03f;
	                staticNoise.Play();
                    noiseObject.SetActive(true);
                    
	            }

	            if (noiseObject.GetComponent<Image>().sprite == noiseSprites.noise1)
	            {
	                noiseObject.GetComponent<Image>().sprite = noiseSprites.noise2;
	            }
	            else if (noiseObject.GetComponent<Image>().sprite == noiseSprites.noise2)
	            {
	                noiseObject.GetComponent<Image>().sprite = noiseSprites.noise3;
	            }
	            else if (noiseObject.GetComponent<Image>().sprite == noiseSprites.noise3)
	            {
	                noiseObject.GetComponent<Image>().sprite = noiseSprites.noise4;
	            }
	            else
	            {
	                noiseObject.GetComponent<Image>().sprite = noiseSprites.noise1;
	            }

	            noiseBar.GetComponent<RectTransform>().Translate(0, -barMoveSpeed * Time.deltaTime, 0);

	            noiseTimer -= Time.deltaTime;
	            if (noiseTimer < 0)
	            {
	                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);//reload scene
	            }
            }
	        
        }

        //if the game state moves into dialogue, trigger the dialogue controller
	    if (currentGameState == GameState.levelDialogue)
	    {
	        dialogueController.Trigger();
	    }

        //if the player is loading to the main menu, do the static transition and then load main menu
	    if (loadingToMainMenu==true)
	    {
            //static animation
	        if (noiseObject.activeSelf == false)
	        {
	            staticNoise.clip = staticSound;
	            staticNoise.volume = 0.015f;
	            staticNoise.Play();
                noiseObject.SetActive(true);
	        }

	        if (noiseObject.GetComponent<Image>().sprite == noiseSprites.noise1)
	        {
	            noiseObject.GetComponent<Image>().sprite = noiseSprites.noise2;
	        }
	        else if (noiseObject.GetComponent<Image>().sprite == noiseSprites.noise2)
	        {
	            noiseObject.GetComponent<Image>().sprite = noiseSprites.noise3;
	        }
	        else if (noiseObject.GetComponent<Image>().sprite == noiseSprites.noise3)
	        {
	            noiseObject.GetComponent<Image>().sprite = noiseSprites.noise4;
	        }
	        else
	        {
	            noiseObject.GetComponent<Image>().sprite = noiseSprites.noise1;
	        }

	        noiseBar.GetComponent<RectTransform>().Translate(0, -barMoveSpeed * Time.deltaTime, 0);

	        noiseTimer -= Time.deltaTime;
	        if (noiseTimer < 0)
	        {
	            SceneManager.LoadScene("MenuScene");
            }
	    }

	    if (currentGameState==GameState.levelDialogue)
	    {

	    }
	}


    //start load to main menu
    public void mainMenu() 
    {
        loadingToMainMenu = true;
    }

    //get rid of pause menu
    public void Resume() 
    {
        pauseMenu.SetActive(false);
        isGamePaused = false;
    }

    //quit game
    public void Quit()
    {
        Application.Quit();
    }

    // open the options menu
    public void OpenOptions()
    {
        optionsMenu.SetActive(true);
        pauseMenu.SetActive(false);
    }

    //go from options menu to pause menu
    public void closeToPause() 
    {
        optionsMenu.SetActive(false);
        pauseMenu.SetActive(true);
    }


    // change post processing settings
    public void setPPtoMax() 
    {
        completionKeeper.currentPostProcessingSettings = CompletionKeeper.PostProcessingSettings.max;
        mainCamera.GetComponent<PostProcessingBehaviour>().profile = completionKeeper.cctvPPProfileMax;
        PlayerPrefs.SetString("PPSetting", "max");
    }

    public void setPPtoMin()
    {
        completionKeeper.currentPostProcessingSettings = CompletionKeeper.PostProcessingSettings.min;
        mainCamera.GetComponent<PostProcessingBehaviour>().profile = completionKeeper.cctvPPProfileMin;
        PlayerPrefs.SetString("PPSetting", "min");
    }

    public void setPPtoNone()
    {
        completionKeeper.currentPostProcessingSettings = CompletionKeeper.PostProcessingSettings.none;
        mainCamera.GetComponent<PostProcessingBehaviour>().profile = completionKeeper.cctvPPProfileNone;
        PlayerPrefs.SetString("PPSetting", "none");
    }

    //toggle the targetting lines on all turrets
    public void ToggleLasers()
    {
        if (completionKeeper.toggleLasers==true)
        {
            completionKeeper.toggleLasers = false;
            foreach (GameObject turret in gameObject.GetComponent<TurretManager>().turretList)
            {
                turret.GetComponent<TurretShooter>().isTargettingLaserOn = false;
            }
        }
        else
        {
            completionKeeper.toggleLasers = true;
            foreach (GameObject turret in gameObject.GetComponent<TurretManager>().turretList)
            {
                turret.GetComponent<TurretShooter>().isTargettingLaserOn = true;
            }
        }
        completionKeeper.BackupDataToPlayerPrefs();
    }
}
