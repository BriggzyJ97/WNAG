﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TextTypingController : MonoBehaviour // this script controls typing text, both on the intro and on all levels
{
    #region Variables
    public float letterPause = 0.2f;// the pause between each letter is added to the textbox
    public float beginningPause = 10f;//pause at the beginning of new lines
    private string messageToType;//message to be typed out into text box
    public TextMeshProUGUI textField;//text box to be typed into

    public Image textTypingCursor;//white square used as typing cursor 
    private bool cursorBlinking = false;//is the cursor blinking
    public float cursorBlinkTime = 0.25f;//how quickly the cursor blinks 

    public bool hasTriggered = true; //trigger to start text typing 
    public bool hasTextTyped = false; //has the text been typed out

    [Header("Glitchy Text Variables")] //these variables make the text glitch around
    private Vector2 savedTextPos;
    public TextMeshProUGUI blueTextField;
    private float randomOffsetX;
    private float randomOffsetY;
    public float GlitchinessOfText = 1f;
    public float ChromaticAbberationOffset = 1f;
    public Color GlitchTextBaseColor;
    public Color RedTextBaseColor;
    public bool VerticlePositionGlitching = false;

    public GameObject textTyperToTrigger;//if the text should trigger another text typer
    private float loadTimer = 3; //used for loading scenes when the text typer triggers that (intro)
    private bool startToLoad = false; //is another scene loading
    public string sceneToLoad;
    public GameObject noiseObject;//these next couple variables are static noise for loading transitions
    public noiseSpriteData noiseSprites;
    public GameObject noiseBar;
    private float barMoveSpeed = 10f;
    public AudioSource staticNoise;//static noise effect

    public AudioClip thisTypingNoise;//typing sound effect

    

    public enum TextType//colour and text effects
    {
        whiteText,
        glitchyText,
        redText,
        greenText,
    }

    public TextType textType = TextType.whiteText;

    private AudioSource audioSource;//typing audio source

    private GameObject completionKeeper;//settings keeper

#endregion

    // assign variables and start text typing if triggered
    void Start ()
	{
	    savedTextPos = textField.gameObject.GetComponent<RectTransform>().anchoredPosition;
	    
	    messageToType = textField.text;
	    textField.text = "";
	    audioSource = gameObject.GetComponent<AudioSource>();
        blueTextField.text = "";
	    //redTextField.text = "";
	    if (textType == TextType.glitchyText)
	    {
	        blueTextField.enabled = true;
	        //redTextField.enabled = true;
            //GlitchImage.enabled = true;
	        textField.color = GlitchTextBaseColor;
	        //textTypingCursor.color = GlitchTextBaseColor;
	    }else if (textType == TextType.redText)
	    {
	        
	        //GlitchImage.enabled = true;
            blueTextField.enabled = true;
	        textField.color = RedTextBaseColor;
	        //textTypingCursor.color = RedTextBaseColor;
	    }
	    if (hasTriggered)
	    {
	        StartCoroutine(TypeText());
        }
        completionKeeper = GameObject.FindGameObjectWithTag("CompletionKeeper");
	    
	}

    void Update()
    {
        //if the cursor is blinking turn it off and on 
        if (cursorBlinking)
        {
            cursorBlinkTime -= Time.deltaTime;
            if (cursorBlinkTime<0)
            {
                if (textTypingCursor.enabled==false)
                {
                    textTypingCursor.enabled = true;
                }
                else
                {
                    textTypingCursor.enabled = false;
                }
                cursorBlinkTime = 0.25f;
            }
        }

        //if the text is glitchy text make it move around within constraints
        if (textType == TextType.glitchyText)
        {
            randomOffsetX = Random.Range(-GlitchinessOfText, GlitchinessOfText);
            if (VerticlePositionGlitching==true)
            {
                randomOffsetY = Random.Range(-GlitchinessOfText, GlitchinessOfText);
            }

            //ChromaticAbberationOffset = Random.Range(0.5f, 2f);
            //GlitchImage.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(glitchImagePos.x,glitchImagePos.y+Random.Range(-15f,15f));
            textField.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(savedTextPos.x+randomOffsetX,savedTextPos.y+randomOffsetY);
            blueTextField.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(savedTextPos.x + randomOffsetX - (ChromaticAbberationOffset/2), savedTextPos.y + randomOffsetY);
            //redTextField.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(savedTextPos.x + randomOffsetX + ChromaticAbberationOffset, savedTextPos.y + randomOffsetY);
        }

        //if the text is red, put the blue text behind it a bit
        else if (textType==TextType.redText)
        {
            //ChromaticAbberationOffset = Random.Range(0.5f, 2f);
            //GlitchImage.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(glitchImagePos.x+Random.Range(-3f,3f), glitchImagePos.y + Random.Range(-15f, 15f));
            //float ChromaticAbberationOffsetY = Random.Range(-1.5f, 1.5f);
            blueTextField.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(savedTextPos.x + randomOffsetX - (ChromaticAbberationOffset / 2), savedTextPos.y + randomOffsetY);
        }

        //if the text is triggered, start text typing coroutine
        if (hasTriggered && hasTextTyped==false)   
        {
            StartCoroutine(TypeText());
        }

        //start the next scene loading 
        if (startToLoad==true)
        {
            
            loadScene();
        }

        //if the player presses ESC on the intro level, skip to level 1
        if (SceneManager.GetActiveScene().name=="IntroLevel")
        {
            if (Input.GetKeyDown(KeyCode.Escape)&&startToLoad==false)
            {
                loadTimer = 2;      
                startToLoad = true;
            }
        }
    }


    //text typing coroutine
    IEnumerator TypeText()
    {
        hasTextTyped = true;
        //type out each character in the message one by one with some special characters performing diferent actions
        foreach (char letter in messageToType.ToCharArray())
        {
            //if a ~ character is in the message flash the cursor and pause the typing for a little bit
            if (letter.ToString() == "~")
            {
                textTypingCursor.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(textTypingCursor.gameObject.GetComponent<RectTransform>().anchoredPosition.x, textTypingCursor.gameObject.GetComponent<RectTransform>().anchoredPosition.y-30);
                cursorBlinking = true;
                yield return new WaitForSeconds(beginningPause);
            }

            //if a _ character is in the message, move the cursor down by 1 line
            else if (letter.ToString()=="_")
            {
                textTypingCursor.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(textTypingCursor.gameObject.GetComponent<RectTransform>().anchoredPosition.x, textTypingCursor.gameObject.GetComponent<RectTransform>().anchoredPosition.y - 30);
            }

            //if a { character is in the message, start typing sound
            else if (letter.ToString()=="{")
            {
                audioSource.clip = thisTypingNoise;
                if (thisTypingNoise.name=="morsecodeConsole1"|| thisTypingNoise.name == "morsecodeIntro")
                {
                    audioSource.volume = 0.015f;
                }
                audioSource.Play();
            }

            //if a { character is in the message, stop typing sound
            else if (letter.ToString() == "}")
            {
                audioSource.Stop();
                audioSource.volume = 0.1f;
            }

            //if a - character is in the message, make the text type a little slower
            else if (letter.ToString() == "-")
            {
                letterPause = letterPause + 0.1f;
            }

            //if a + character is in the message, make the text type a little faster
            else if (letter.ToString() == "+")
            {
                letterPause = letterPause - 0.1f;
            }

            //if a @ character is in the message, start loading the next scene
            else if (letter.ToString()=="@")
            {
                startToLoad = true;
            }

            //if a # character is in the message, trigger another text typer
            else if (letter.ToString()=="#")
            {
                textTyperToTrigger.GetComponent<TextTypingController>().hasTriggered = true;
            }

            //if theres no special characters, just type out the letters, waiting for a tiny amount of time between typing out the letters 
            else
            {
                cursorBlinking = false;
                textTypingCursor.enabled = false;
                textField.text += letter;
                if (textType == TextType.glitchyText)
                {
                    blueTextField.text += letter;
                    //redTextField.text += letter;
                }else if (textType == TextType.redText)
                {
                    blueTextField.text += letter;
                }
                yield return new WaitForSeconds(letterPause);
            }
            
        }
    }

    //load the next scene, with static transition
    void loadScene() 
    {
        loadTimer -= Time.deltaTime;
        if (loadTimer<2)
        {
            if (noiseObject.activeSelf == false)
            {
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
        }
        

        if (loadTimer<0)
        {
            completionKeeper.GetComponent<AudioSource>().volume = 0.06f;
            SceneManager.LoadScene(sceneToLoad);
        }

    }
}
