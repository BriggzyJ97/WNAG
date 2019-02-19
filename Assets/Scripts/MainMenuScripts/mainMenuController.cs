using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.PostProcessing;


public class mainMenuController : MonoBehaviour//this script controls the main menu
{
    #region Variables
    [Header("Button Variables")] // these are all variables for the buttons 
    private bool pageMoving = false; // level select screen moving
    private string directionOfPageMove = ""; 
    private int currentPage = 1;
    public int pageMoveSpeed;
    private Vector3 currentPagePosition;
    private Vector3 targetPagePosition;
    public GameObject ButtonHolder;
    public GameObject upButton;
    public GameObject upButton2;
    public GameObject downButton;
    public GameObject downButton2;
    public GameObject mainMenuObjects;
    public GameObject optionsObjects;
    public GameObject levelSelectOptions;
    public GameObject storageLevelsObjects;
    public GameObject creditsObjects;
    public GameObject backButton;
    public GameObject startOrContinueText;
    public List<GameObject> levelButtons = new List<GameObject>();
    public AudioSource dingSound;
    public AudioSource noiseSound;

    private AudioSource buttonNoise;
    //Camera Variables
    public GameObject cameraHolder; //object that holds the camera
    public float cameraMoveSpeed;
    public Camera mainCamera;
    private string PPSetting; //post processing settings
    
    //States
    public enum pageChangeState //level select movement states
    {
        idle,
        bounceForward,
        move,
        bounceBack
    }

    private pageChangeState currentPageChangeState = pageChangeState.idle;

    private enum mainMenuState //main menu state
    {
        opening,
        main,
        levelSelect,
        options
    }

    private mainMenuState currentMainMenuState = mainMenuState.opening;
    private CompletionKeeper completionKeeper; //keeps track of how many levels completed
    //Noise static variables
    public GameObject noiseObject; //noise variables for the static when loading new scene
    public GameObject noiseBar;
    private float noiseTimer = 1f;
    public noiseSpriteData noiseSprites;
    private float barMoveSpeed = 10f;
    //loading variables
    private bool continuingToNextLevel = false; //for loading levels 
    private bool loadingLevel = false;
    private int whichLevelToLoad;

    #endregion

    //assign dynamic variables and setting post processing settings
    void Start ()
	{
        
	    buttonNoise = gameObject.GetComponent<AudioSource>();
	    completionKeeper = GameObject.FindGameObjectWithTag("CompletionKeeper").GetComponent<CompletionKeeper>();
	    PPSetting = PlayerPrefs.GetString("PPSetting", "max");
	    if (PPSetting=="max")
	    {
	        completionKeeper.currentPostProcessingSettings = CompletionKeeper.PostProcessingSettings.max;
	        mainCamera.GetComponent<PostProcessingBehaviour>().profile = completionKeeper.mainMenuPPProfileMax;
        }
        else if (PPSetting=="min")
	    {
	        completionKeeper.currentPostProcessingSettings = CompletionKeeper.PostProcessingSettings.min;
	        mainCamera.GetComponent<PostProcessingBehaviour>().profile = completionKeeper.mainMenuPPProfileMin;
        }
        else if (PPSetting=="none")
	    {
	        completionKeeper.currentPostProcessingSettings = CompletionKeeper.PostProcessingSettings.none;
	        mainCamera.GetComponent<PostProcessingBehaviour>().profile = completionKeeper.mainMenuPPProfileNone;
        }
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetKeyDown(KeyCode.L))
	    {
            unlockAllLevels();
	    }
	    // if the game isnt loading into another scene
        if (continuingToNextLevel!=true&&loadingLevel!=true)
	    {
            if (currentMainMenuState == mainMenuState.opening) 
            {
                //if the player hasnt completed any levels, change the top left button from continue to start
                if (completionKeeper.howManyLevelsCompleted == 0)
                {
                    startOrContinueText.GetComponent<TextMeshProUGUI>().text = "Start";
                }
            }
            else if (currentMainMenuState == mainMenuState.main)
            {

            }
            // level select screen
            else if (currentMainMenuState == mainMenuState.levelSelect)
            {
                
                //moving the level buttons
                if (pageMoving == true)
                {
                    //moving up
                    if (directionOfPageMove == "up") 
                    {
                        // move page change onto the first moving state
                        if (currentPageChangeState == pageChangeState.idle)
                        {
                            currentPageChangeState = pageChangeState.bounceForward;
                        }

                        //move the page down slightly before moving up to give it a little bounce
                        else if (currentPageChangeState == pageChangeState.bounceForward)
                        {
                            ButtonHolder.GetComponent<RectTransform>().Translate(0, pageMoveSpeed * Time.deltaTime, 0);
                            if (ButtonHolder.GetComponent<RectTransform>().transform.localPosition.y > targetPagePosition.y)
                            {
                                targetPagePosition.y = currentPagePosition.y - 650;
                                currentPageChangeState = pageChangeState.move;
                            }
                        }

                        //move the page up
                        else if (currentPageChangeState == pageChangeState.move)
                        {
                            cameraHolder.transform.Translate(0, cameraMoveSpeed * Time.deltaTime, 0);
                            ButtonHolder.GetComponent<RectTransform>().Translate(0, -pageMoveSpeed * Time.deltaTime, 0);
                            if (ButtonHolder.GetComponent<RectTransform>().transform.localPosition.y < targetPagePosition.y)
                            {
                                targetPagePosition.y = currentPagePosition.y - 600;
                                currentPageChangeState = pageChangeState.bounceBack;
                            }
                        }

                        //move it down a bit after moving to give more bounce
                        else if (currentPageChangeState == pageChangeState.bounceBack)
                        {
                            ButtonHolder.GetComponent<RectTransform>().Translate(0, pageMoveSpeed * Time.deltaTime, 0);
                            if (ButtonHolder.GetComponent<RectTransform>().transform.localPosition.y > targetPagePosition.y)
                            {
                                currentPage += 1;
                                directionOfPageMove = "";
                                pageMoving = false;
                                ButtonHolder.GetComponent<RectTransform>().localPosition = new Vector3(currentPagePosition.x, currentPagePosition.y - 600, currentPagePosition.z);

                                downButton.GetComponent<Button>().interactable = true;
                                downButton.GetComponent<Image>().enabled = true;
                                downButton2.GetComponent<Image>().enabled = true;
                                //make sure the player cant move up further then there is pages
                                if (currentPage < 4)
                                {
                                    upButton.GetComponent<Button>().interactable = true;
                                    upButton.GetComponent<Image>().enabled = true;
                                    upButton2.GetComponent<Image>().enabled = true;
                                }
                                currentPageChangeState = pageChangeState.idle;
                            }
                        }
                    }

                    //moving down
                    else if (directionOfPageMove == "down")
                    {
                        // move page change onto the first moving state
                        if (currentPageChangeState == pageChangeState.idle)
                        {
                            currentPageChangeState = pageChangeState.bounceForward;
                        }

                        //move the page up slightly before moving down to give it a little bounce
                        else if (currentPageChangeState == pageChangeState.bounceForward)
                        {
                            ButtonHolder.GetComponent<RectTransform>().Translate(0, -pageMoveSpeed * Time.deltaTime, 0);
                            if (ButtonHolder.GetComponent<RectTransform>().transform.localPosition.y < targetPagePosition.y)
                            {
                                targetPagePosition.y = currentPagePosition.y + 650;
                                currentPageChangeState = pageChangeState.move;
                            }
                        }

                        //move down
                        else if (currentPageChangeState == pageChangeState.move)
                        {
                            cameraHolder.transform.Translate(0, -cameraMoveSpeed * Time.deltaTime, 0);
                            ButtonHolder.GetComponent<RectTransform>().Translate(0, pageMoveSpeed * Time.deltaTime, 0);
                            if (ButtonHolder.GetComponent<RectTransform>().transform.localPosition.y > targetPagePosition.y)
                            {
                                targetPagePosition.y = currentPagePosition.y + 600;
                                currentPageChangeState = pageChangeState.bounceBack;
                            }
                        }

                        //move the page up slightly after moving down to give it a little bounce
                        else if (currentPageChangeState == pageChangeState.bounceBack)
                        {
                            ButtonHolder.GetComponent<RectTransform>().Translate(0, -pageMoveSpeed * Time.deltaTime, 0);
                            if (ButtonHolder.GetComponent<RectTransform>().transform.localPosition.y < targetPagePosition.y)
                            {
                                currentPage -= 1;
                                directionOfPageMove = "";
                                pageMoving = false;
                                ButtonHolder.GetComponent<RectTransform>().localPosition = new Vector3(currentPagePosition.x, currentPagePosition.y + 600, currentPagePosition.z);
                                upButton.GetComponent<Button>().interactable = true;
                                upButton.GetComponent<Image>().enabled = true;
                                upButton2.GetComponent<Image>().enabled = true;
                                //make sure the player can't move down if they on bottom page
                                if (currentPage > 1)
                                {
                                    downButton.GetComponent<Button>().interactable = true;
                                    downButton.GetComponent<Image>().enabled = true;
                                    downButton2.GetComponent<Image>().enabled = true;
                                }
                                currentPageChangeState = pageChangeState.idle;
                            }
                        }
                    }
                }
            }
            else if (currentMainMenuState == mainMenuState.options)
            {

            }
        }

	    //if level is loading trigger noise and then load whichever level button was clicked
        if (loadingLevel==true)
	    {
	        if (noiseObject.activeSelf == false)
	        {
                noiseSound.Play();
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
	            if (whichLevelToLoad==1)
	            {
	                SceneManager.LoadScene("IntroLevel");
                }
	            SceneManager.LoadScene(whichLevelToLoad);
	        }
        }

        //if player presses continue button, trigger noise, then load the level that the player has gotten up to    
	    if (continuingToNextLevel == true) 
	    {
	        if (noiseObject.activeSelf == false)
	        {
                noiseSound.Play();
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
	            if (completionKeeper.howManyLevelsCompleted+1==1)
	            {
                    SceneManager.LoadScene("IntroLevel");
                }
	            else
	            {
	                SceneManager.LoadScene(completionKeeper.howManyLevelsCompleted + 1);
                }
	            
	        }
        }
	    
	    
	}

    //load level from level select buttons
    public void LoadLevel(int levelToLoad) 
    {
        loadingLevel = true;
        dingSound.Play();
        whichLevelToLoad = levelToLoad;
        foreach (GameObject button in levelButtons)//make the buttons un-interactable when loading
        {
            button.GetComponent<Button>().interactable = false;
        }
    }

    //move the level select page up
    public void MoveUpPage() 
    {
        if (pageMoving==false)
        {
            upButton.GetComponent<Button>().interactable = false;
            upButton.GetComponent<Image>().enabled = false;
            upButton2.GetComponent<Image>().enabled = false;
            downButton.GetComponent<Button>().interactable = false;
            downButton.GetComponent<Image>().enabled = false;
            downButton2.GetComponent<Image>().enabled = false;
            directionOfPageMove = "up";
            currentPagePosition = ButtonHolder.GetComponent<RectTransform>().transform.localPosition;
            targetPagePosition = new Vector3(currentPagePosition.x, currentPagePosition.y+50, currentPagePosition.z);
            pageMoving = true;
        }
        
    }

    //move the level select page down
    public void MoveDownPage() 
    {
        if (pageMoving == false)
        {
            upButton.GetComponent<Button>().interactable = false;
            upButton.GetComponent<Image>().enabled = false;
            upButton2.GetComponent<Image>().enabled = false;
            downButton.GetComponent<Button>().interactable = false;
            downButton.GetComponent<Image>().enabled = false;
            downButton2.GetComponent<Image>().enabled = false;
            directionOfPageMove = "down";
            
            currentPagePosition = ButtonHolder.GetComponent<RectTransform>().transform.localPosition;
            targetPagePosition = new Vector3(currentPagePosition.x, currentPagePosition.y-50 , currentPagePosition.z);
            pageMoving = true;
        }
    }

    // open level select menu
    public void changeToLevelSelect() 
    {
        completionKeeper.RestoreDataFromPlayerPrefBackup();
        mainMenuObjects.SetActive(false);
        levelSelectOptions.SetActive(true);
        for (int i = levelButtons.Count - 1; i > (completionKeeper.howManyLevelsCompleted); i--)
        {
            Debug.Log(i);
            
            levelButtons[i].GetComponent<Button>().interactable = false;
        }
        currentMainMenuState = mainMenuState.levelSelect;
    }

    //open different department menus in level select
    public void openStorageLevels()
    {
        storageLevelsObjects.SetActive(true);
        levelSelectOptions.SetActive(false);
    }
    public void openSecurityLevels()
    {
        storageLevelsObjects.SetActive(true);
        levelSelectOptions.SetActive(false);
    }
    public void openOfficeLevels()
    {
        storageLevelsObjects.SetActive(true);
        levelSelectOptions.SetActive(false);
    }
    public void openRnDLevels()
    {
        storageLevelsObjects.SetActive(true);
        levelSelectOptions.SetActive(false);
    }
    public void openProductionLevels()
    {
        storageLevelsObjects.SetActive(true);
        levelSelectOptions.SetActive(false);
    }
    public void openHeadOfficeLevels()
    {
        storageLevelsObjects.SetActive(true);
        levelSelectOptions.SetActive(false);
    }

    //go back to department select
    public void BackToAreaSelect()
    {
        storageLevelsObjects.SetActive(false);
        levelSelectOptions.SetActive(true);
    }

    //go back to main menu from other menus
    public void backToMainMenu() 
    {
        mainMenuObjects.SetActive(true);
        if (completionKeeper.howManyLevelsCompleted == 0)
        {
            startOrContinueText.GetComponent<TextMeshProUGUI>().text = "Start";
        }
        levelSelectOptions.SetActive(false);
        optionsObjects.SetActive(false);
        backButton.SetActive(false);
        downButton.SetActive(false);
        currentMainMenuState = mainMenuState.main;
    }

    //load the next level that the player should play
    public void continueToNextLevel() 
    {
        dingSound.Play();
        continuingToNextLevel = true;
        foreach (GameObject button in levelButtons)
        {
            button.GetComponent<Button>().interactable = false;
        }
    }

    //quit the game
    public void quit() 
    {
        PlayerPrefs.SetInt("levelsCompleted",completionKeeper.howManyLevelsCompleted);
        Application.Quit();
    }

    //open the options menu
    public void optionsOpen() 
    {
        mainMenuObjects.SetActive(false);
        optionsObjects.SetActive(true);
        currentMainMenuState = mainMenuState.options;
    }

    //delete all saved data
    public void deleteData() 
    {
        PlayerPrefs.SetInt("levelsCompleted",0);
        PlayerPrefs.SetString("playerName","");
        completionKeeper.howManyLevelsCompleted = 0;
    }

    // unlock all levels for player
    public void unlockAllLevels() 
    {
        PlayerPrefs.SetInt("levelsCompleted", 60);
        completionKeeper.howManyLevelsCompleted = 60;
        
    }

    // open the credits page
    public void openCredits() 
    {
        optionsObjects.SetActive(false);
        creditsObjects.SetActive(true);
    }

    //go back to the options menu from the credits menu
    public void backToOptions() 
    {
        optionsObjects.SetActive(true);
        creditsObjects.SetActive(false);
    }

    //change the post processing settings

    public void setPPtoMax()
    {
        completionKeeper.currentPostProcessingSettings = CompletionKeeper.PostProcessingSettings.max;
        mainCamera.GetComponent<PostProcessingBehaviour>().profile = completionKeeper.mainMenuPPProfileMax;
        PlayerPrefs.SetString("PPSetting","max");
    }

    public void setPPtoMin()
    {
        completionKeeper.currentPostProcessingSettings = CompletionKeeper.PostProcessingSettings.min;
        mainCamera.GetComponent<PostProcessingBehaviour>().profile = completionKeeper.mainMenuPPProfileMin;
        PlayerPrefs.SetString("PPSetting", "min");
    }

    public void setPPtoNone()
    {
        completionKeeper.currentPostProcessingSettings = CompletionKeeper.PostProcessingSettings.none;
        mainCamera.GetComponent<PostProcessingBehaviour>().profile = completionKeeper.mainMenuPPProfileNone;
        PlayerPrefs.SetString("PPSetting", "none");
    }

    //mute music toggle
    public void muteMusic()
    {
        if (completionKeeper.gameObject.GetComponent<AudioSource>().mute == true)
        {
            completionKeeper.gameObject.GetComponent<AudioSource>().mute = false;
            completionKeeper.mutedMusic = false;
        }
        else
        {
            completionKeeper.gameObject.GetComponent<AudioSource>().mute = true;
            completionKeeper.mutedMusic = true;
        }
        completionKeeper.BackupDataToPlayerPrefs();
        

    }

    //toggle turret targetting lasers
    public void toggleLasers()
    {
        if (completionKeeper.toggleLasers==true)
        {
            completionKeeper.toggleLasers = false;
        }
        else
        {
            completionKeeper.toggleLasers = true;
        }
        completionKeeper.BackupDataToPlayerPrefs();
    }

    //sound for all buttons
    public void ButtonSound()
    {
        buttonNoise.Play();
    }
}
