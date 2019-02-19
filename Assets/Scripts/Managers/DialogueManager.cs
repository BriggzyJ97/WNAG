using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Assertions.Must;

public class DialogueManager : MonoBehaviour {
    //this script manages the dialogue in the dialogue levels

    #region Variables

    public enum DialogueState//different states of the console
    {
        preDialogue,
        zoomAndFade,
        menu,
        chat,
        quitDialogue,
    }

    public DialogueState CurrentDialogueState = DialogueState.preDialogue;

    private bool inDialogue = false;// if the player is in the dialogue

    private GameStateManager gameManager;

    [Header("Camera and UI variables")]
    private GameObject mainCamera;
    private float cameraSpeed = 10f;//speed that camera zooms in towards console
    private GameObject UICanvas;

    public Image fade;//fade to black when entering console
    private float alpha = 0;//alpha float to transition fade to black

    [Header("Console Variables")]

    public GameObject dialogueMenu;//consoles main menu
    public GameObject dialogueChat;//consoles chat section
    public TextMeshProUGUI newMessageText;//text box for new messages

    public Button OpenChatButton;// button for opening chat section from main menu
    public TextMeshProUGUI openChatText;//text for the open chat button

    private bool messagesRead = false; //bool if chat section has been opened
    private bool messagesLocked = false; //if the player is locked out of messages
    private float newMessageTimer = 0f;//timer used to change the new messages text
    private float newMessageYellowChange = 1f;//float used to flash new messages text in yellow
    private bool newMessageIncreasing = false;//whether to increase the new messages count

    public List<TextMeshProUGUI> chatTextList = new List<TextMeshProUGUI>();//list of chat section text boxes
    public List<string> chatTexts = new List<string>();//list of strings that comprise the possible messages that are sent 
    public int chatPosition;//how many lines have been typed

    private bool chatCursorBlinking = true;//whether the chat cursor should be blinking
    public GameObject typeCursor;//the chat cursor
    private float cursorBlinkTime = 0.25f;// how fast the chat Cursor blinks

    public float letterPause = 0.1f;// the pause between each letter is added to the textbox
    private bool hasTextChanged;

    public bool hasTriggered = true;//has the text been triggered to start typing
    public bool hasTextTyped = false;//has text been typed out

    private readonly Color greenText = new Color(0,1,0,1);//green text colour
    private readonly Color redText = new Color(1,0,0,1);//red text colour

    public GameObject TwoChatOptions;//chat option panel that has two options
    public GameObject ThreeChatOptions;// chat option panel that has three options
    public GameObject InputChatOptions;//chat option panel with a input-able text box
    public GameObject chatTextHolder;//holder for all the text boxes to move up when screen full

    private bool responseTrigger = false; // whether the player is expecting a response
    private int numberOfResponses = 0; //how many responses needed
    private int whichResponseNeeded; //which response is needed depending on what player has said

    private bool inputtingText = false;//whether text is being inputted
    public TextMeshProUGUI inputTextBox;//the text box used for inputting
    public GameObject inputTypeCursor;//the cursor for the input box
    private float inputCursorBlinkTime = 0.25f;//how quickly the input cursor blinks
    private bool inputCursorBlinking = true;//whether the input type cursor should be blinking
    private float blinkPause = 1f;//gap between blinks

    private CompletionKeeper completionTracker;

    public doorControl door;//the door in the room

    public string playerName = "";//what the player inputted their name as
    private bool chatHacked = false;//whether chat has been hacked and is shutting down
    private int chatHackPos = 0;//what line is currently being hacked
    private float chatHackTime = 0.2f;//how quickly the chat gets hacked
    private bool skipText = false;//whether the player is skipping the text typing out

    [Header("Sound Effects")]
    private AudioSource audioSource;
    public AudioSource morseCodeSoundSource;
    public AudioSource demonHackSoundSource;
    public AudioSource buttonSoundSource;

#endregion

    // assign variables
    void Start ()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameStateManager>();
		mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        UICanvas = GameObject.FindGameObjectWithTag("UICanvas");
        completionTracker = GameObject.FindGameObjectWithTag("CompletionKeeper").GetComponent<CompletionKeeper>();
        
	}
	
	
	void Update () {
	    if (inDialogue == true)
	    {
	        
	        if (CurrentDialogueState==DialogueState.preDialogue)
	        {
	            CurrentDialogueState = DialogueState.zoomAndFade;
	        }
            //move the camera down towards the console while shifting fov out to give dolly zoom effect, fade to black, swap canvas mode and post processing stack, bring up main menu of console
	        else 
	        if (CurrentDialogueState == DialogueState.zoomAndFade)
	        {
                mainCamera.transform.Translate(0,0, cameraSpeed * Time.deltaTime);
	            mainCamera.GetComponent<Camera>().fieldOfView += cameraSpeed*2 * Time.deltaTime;
	            alpha += Time.deltaTime / 2;
                fade.color = new Color(0,0,0,alpha);
                mainCamera.transform.GetChild(0).GetComponent<Camera>().fieldOfView += cameraSpeed * 2 * Time.deltaTime;
                if (fade.color.a >0.97f)
                {
                    UICanvas.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceCamera;
                    mainCamera.GetComponent<cameraModeSwap>().SwapCameraMode();
                    fade.color = new Color(0, 0, 0, 1);
                    dialogueMenu.SetActive(true);
	                CurrentDialogueState = DialogueState.menu;
	            }
	        }

            //main menu of the console
	        else if (CurrentDialogueState == DialogueState.menu)
	        {
	            if (completionTracker != null)
	            {
	                completionTracker.GetComponent<AudioSource>().volume = 0.02f;
	            }

                //have new messages pop up if they are unread, or lock the chat section of console if its been hacked
                if (messagesLocked==false)
	            {
	                if (messagesRead == false)
	                {
	                    newMessageTimer += Time.deltaTime;
	                    if (newMessageTimer > 0.5f && newMessageTimer < 1.2f)
	                    {
	                        newMessageText.text = "1 new message";
	                    }
	                    if (newMessageTimer > 1.2f)
	                    {
	                        newMessageText.text = "2 new message";
	                    }

	                    if (newMessageIncreasing == false)
	                    {
	                        newMessageYellowChange -= Time.deltaTime * 2;
	                        if (newMessageYellowChange < 0.1f)
	                        {
	                            newMessageIncreasing = true;
	                        }
	                    }
	                    else
	                    {
	                        newMessageYellowChange += Time.deltaTime * 2;
	                        if (newMessageYellowChange > 0.95f)
	                        {
	                            newMessageIncreasing = false;
	                        }
	                    }

	                    newMessageText.color = new Color(1, 1, newMessageYellowChange);

	                }
	                else
	                {
	                    newMessageText.text = "";
	                }
                }
	            else
	            {
                    //Debug.Log("messages locked");
	                if (OpenChatButton.interactable==true)
	                {
	                    openChatText.color = redText;
	                    OpenChatButton.interactable = false;
	                    newMessageText.text = "";
                    }
	            }
	            
	            
	        }
            //chat section of console
	        else if (CurrentDialogueState == DialogueState.chat)
	        {
                //skip forward text if mouse pressed
	            if (Input.GetKeyDown(KeyCode.Mouse0))
	            {
	                Tap();
                }

                //blink chat cursor
	            if (chatCursorBlinking==true)
	            {
	                cursorBlinkTime -= Time.deltaTime;
	                if (cursorBlinkTime < 0)
	                {
	                    if (typeCursor.activeSelf == false)
	                    {
	                        typeCursor.SetActive(true);
	                    }
	                    else
	                    {
	                        typeCursor.SetActive(false);
	                    }
	                    cursorBlinkTime = 0.25f;
	                }
                }

                //if the robot should give response, make the correct response, including chaining responses if necessary
	            if (responseTrigger==true)
	            {
                    //Debug.Log("makeResponse");
	                if (chatPosition >= 12)
	                {
	                    chatTextHolder.GetComponent<RectTransform>().localPosition = new Vector2(chatTextHolder.GetComponent<RectTransform>().localPosition.x, chatTextHolder.GetComponent<RectTransform>().localPosition.y + 35);
	                }

	                
                    StartCoroutine(TypeText(chatTexts[whichResponseNeeded],chatTextList[chatPosition]));
	                if (whichResponseNeeded == 23)
	                {
                        demonHackSoundSource.Play();
	                    chatHacked = true;
	                }
                    if (whichResponseNeeded==5)
	                {
	                    whichResponseNeeded = 6;
	                }else if (whichResponseNeeded==6)
	                {
	                    whichResponseNeeded = 7;
	                }else if (whichResponseNeeded==13)
	                {
	                    whichResponseNeeded = 14;
	                }else if (whichResponseNeeded==14)
	                {
	                    whichResponseNeeded = 15;
	                }else if (whichResponseNeeded==11)
	                {
	                    whichResponseNeeded = 15;
	                }else if (whichResponseNeeded==12)
	                {
	                    whichResponseNeeded = 15;
	                }else if (whichResponseNeeded == 16)
	                {
	                    whichResponseNeeded = 19;
                    }
	                else if (whichResponseNeeded == 19)
	                {
	                    whichResponseNeeded = 17;
	                }
	                else if (whichResponseNeeded == 17)
	                {
	                    whichResponseNeeded = 18;
	                }else if (whichResponseNeeded == 18)
	                {
	                    whichResponseNeeded = 21;
	                }
	                else if (whichResponseNeeded == 21)
	                {
	                    whichResponseNeeded = 22;
	                }
	                else if (whichResponseNeeded == 22)
	                {
	                    whichResponseNeeded = 23;
	                }


                    responseTrigger = false;
	            }

                //let the player input text if the input text box is up
	            if (inputtingText==true)
	            {
	                foreach (char c in Input.inputString)
	                {
	                    if (c == '\b') // has backspace/delete been pressed?
	                    {

	                        if (inputTextBox.text.Length != 0)
	                        {
	                            inputTextBox.text = inputTextBox.text.Substring(0, inputTextBox.text.Length - 1);
	                        }
	                    }
	                    else if ((c == '\n') || (c == '\r')) // enter/return
	                    {
	                        print("User entered their name: " + inputTextBox.text);
                            Submit();
	                    }
	                    else if (inputTextBox.text.Length<10)
	                    {
	                        inputCursorBlinking = false;
	                        inputTextBox.text += c;
	                    }
	                }
                    //blink the input typing cursor
	                if (inputCursorBlinking==true)
	                {
	                    inputCursorBlinkTime -= Time.deltaTime;
	                    if (inputCursorBlinkTime < 0)
	                    {
	                        if (inputTypeCursor.activeSelf == false)
	                        {
	                            inputTypeCursor.SetActive(true);
	                        }
	                        else
	                        {
	                            inputTypeCursor.SetActive(false);
	                        }
	                        inputCursorBlinkTime = 0.25f;
	                    }
                    }
	                else
	                {
                        inputTypeCursor.SetActive(false);
	                }
                }

                //if the chat is hacked, go through line by line "hacking it"
	            if (chatHacked == true)
	            {
                    if(chatHackPos<=chatTextList.Count)
	                {
	                    if (chatTextList[chatHackPos].text == "")
	                    {
	                        messagesLocked = true;
                            demonHackSoundSource.Stop();
                            BackToConsole();
	                    }
	                    chatTextList[chatHackPos].color = redText;
	                    chatTextList[chatHackPos].text = "STOPSTOPSTOPSTOPSTOPSTOPSTOPSTOPSTOPSTOPSTOPSTOPSTOPSTOPSTOPS";
                    }
	                chatHackTime -= Time.deltaTime;
	                if (chatHackTime<=0)
	                {

	                    chatHackPos++;
	                    chatHackTime = 0.2f;
	                }

	            }

	        }
            //if the player quits the console reverse earlier changes to camera and ui and fade from black
	        else if (CurrentDialogueState==DialogueState.quitDialogue)
	        {
	            dialogueMenu.SetActive(false);
                mainCamera.transform.Translate(0, 0, -cameraSpeed * Time.deltaTime);
	            mainCamera.GetComponent<Camera>().fieldOfView -= cameraSpeed * 2 * Time.deltaTime;
	            alpha -= Time.deltaTime / 2;
	            fade.color = new Color(0, 0, 0, alpha);
	            mainCamera.transform.GetChild(0).GetComponent<Camera>().fieldOfView -= cameraSpeed * 2 * Time.deltaTime;
	            if (fade.color.a < 0.03f)
	            {
	                
	                fade.color = new Color(0, 0, 0, 0);
	                gameManager.currentGameState = GameStateManager.GameState.levelPlaying;
	                CurrentDialogueState = DialogueState.preDialogue;
	                inDialogue = false;
	            }
            }
        }
	    else
	    {
	        completionTracker.GetComponent<AudioSource>().volume = 0.06f;
        }
	}
    //when the player steps on console pad, triggered from external collision script
    public void Trigger()
    {
        inDialogue = true;
    }

    //when chat button pressed
    public void OpenChat()
    {
        messagesRead = true;
        dialogueMenu.SetActive(false);
        dialogueChat.SetActive(true);
        CurrentDialogueState = DialogueState.chat;

    }

    //when open door button pressed
    public void OpenDoor()
    {
        door.doorOpen = true;
        
    }

    //when any button pressed, make sound
    public void ButtonSound()
    {
        buttonSoundSource.Play();
    }

    //when the console is quit from
    public void QuitConsole()
    {
        mainCamera.GetComponent<cameraModeSwap>().SwapCameraMode();
        UICanvas.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
        dialogueMenu.SetActive(false);
        CurrentDialogueState = DialogueState.quitDialogue;
    }

    //when the player goes from the chat menu to the main menu of the console
    public void BackToConsole()
    {
        dialogueMenu.SetActive(true);
        dialogueChat.SetActive(false);
        CurrentDialogueState = DialogueState.menu;
    }

    //when option 1 of the two option selection is pressed while chatting
    public void TwoChatOption1()
    {
        skipText = false;
        chatTextList[chatPosition].color = greenText;
        StartCoroutine(TypeText(chatTexts[0], chatTextList[chatPosition]));
        whichResponseNeeded = 4;
        TwoChatOptions.SetActive(false);
    }

    //when option 2 of the two option selection is pressed while chatting
    public void TwoChatOption2()
    {
        skipText = false;
        chatTextList[chatPosition].color = greenText;
        StartCoroutine(TypeText(chatTexts[1], chatTextList[chatPosition]));
        whichResponseNeeded = 5;
        TwoChatOptions.SetActive(false);
    }

    //when option 1 of the three option selection is pressed while chatting
    public void ThreeChatOption1()
    {
        skipText = false;
        chatTextList[chatPosition].color = greenText;
        StartCoroutine(TypeText(chatTexts[8], chatTextList[chatPosition]));
        whichResponseNeeded = 11;
        ThreeChatOptions.SetActive(false);
    }

    //when option 2 of the three option selection is pressed while chatting
    public void ThreeChatOption2()
    {
        skipText = false;
        chatTextList[chatPosition].color = greenText;
        StartCoroutine(TypeText(chatTexts[9], chatTextList[chatPosition]));
        whichResponseNeeded = 12;
        ThreeChatOptions.SetActive(false);
    }

    //when option 3 of the three option selection is pressed while chatting
    public void ThreeChatOption3()
    {
        skipText = false;
        chatTextList[chatPosition].color = greenText;
        StartCoroutine(TypeText(chatTexts[10], chatTextList[chatPosition]));
        whichResponseNeeded = 13;
        ThreeChatOptions.SetActive(false);
    }

    //when the player submits their inputted text
    public void Submit()
    {

        playerName = inputTextBox.text;
        InputChatOptions.SetActive(false);
        whichResponseNeeded = 16;
        //responseTrigger = true;
        if (completionTracker!=null)
        {
            completionTracker.playername = playerName;
            completionTracker.UpdatePlayerName();
        }
        chatTexts[20] = "{"+playerName + "}#@";
        chatTexts[19] = "~£"+playerName+"$#@";
        chatTextList[chatPosition].color = greenText;
        StartCoroutine(TypeText(chatTexts[20], chatTextList[chatPosition]));

    }


    //text typing corouting
    IEnumerator TypeText(string messageToType, TextMeshProUGUI textField)
    {
        hasTextTyped = true;
        //type out each character in the message one by one with some special characters performing diferent actions
        foreach (char letter in messageToType.ToCharArray())
        {
            if (skipText==false)
            {

                //if a ~ character is in the message flash the cursor and pause the typing for a little bit
                if (letter.ToString() == "~")
                {
                    typeCursor.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(
                        typeCursor.gameObject.GetComponent<RectTransform>().anchoredPosition.x,
                        typeCursor.gameObject.GetComponent<RectTransform>().anchoredPosition.y - 30);
                    chatCursorBlinking = true;
                    yield return WaitForSecondsOrTap(1f);



                }

                //if a _ character is in the message, move the cursor down by 1 line
                else if (letter.ToString() == "_")
                {
                    chatCursorBlinking = false;
                    typeCursor.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(
                        typeCursor.gameObject.GetComponent<RectTransform>().anchoredPosition.x,
                        typeCursor.gameObject.GetComponent<RectTransform>().anchoredPosition.y - 30);
                }

                //if a { character is in the message, start typing sound
                else if (letter.ToString() == "{")
                {
                    audioSource.Play();
                }

                //if a } character is in the message, stop typing sound
                else if (letter.ToString() == "}")
                {
                    audioSource.Stop();
                }

                //if a £ character is in the message, start morse code sound
                else if (letter.ToString() == "£")
                {
                    morseCodeSoundSource.Play();
                }

                //if a $ character is in the message, start morse code sound
                else if (letter.ToString() == "$")
                {
                    morseCodeSoundSource.Stop();
                }

                //if a _ character is in the message, move the cursor down by 1 line (same as _ fix)
                else if (letter.ToString() == ":")
                {
                    chatCursorBlinking = false;
                    typeCursor.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(
                        typeCursor.gameObject.GetComponent<RectTransform>().anchoredPosition.x,
                        typeCursor.gameObject.GetComponent<RectTransform>().anchoredPosition.y - 30);
                }

                //if a ; character is in the message, bring up three chat options
                else if (letter.ToString() == ";")
                {
                    ThreeChatOptions.SetActive(true);
                }

                //if a ] character is in the message, bring up input chat options
                else if (letter.ToString() == "]")
                {
                    InputChatOptions.SetActive(true);
                    inputtingText = true;

                }

                //if a [ character is in the message, hack the chat
                else if (letter.ToString() == "[")
                {
                    chatHacked = true;
                }

                //if a + character is in the message, blink the chat cursor
                else if (letter.ToString() == "+")
                {
                    //letterPause = letterPause - 0.1f;
                    chatCursorBlinking = true;
                }

                //if a @ character is in the message, add another response to be said
                else if (letter.ToString() == "@")
                {
                    numberOfResponses += 1;
                    responseTrigger = true;
                }

                //if a # character is in the message, move down one line of text boxes
                else if (letter.ToString() == "#")
                {
                    //textTyperToTrigger.GetComponent<TextTypingController>().hasTriggered = true;
                    chatPosition++;
                }

                //otherwise input into the text box
                else
                {
                    chatCursorBlinking = false;
                    typeCursor.SetActive(false);
                    textField.text += letter;

                    yield return new WaitForSeconds(letterPause);

                }
            }
            //if the chat is being skipped instantly type out the line
            else
            {
                if (letter.ToString() == "~")
                {



                }
                else if (letter.ToString() == "_")
                {
                    chatCursorBlinking = false;
                    typeCursor.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(
                        typeCursor.gameObject.GetComponent<RectTransform>().anchoredPosition.x,
                        typeCursor.gameObject.GetComponent<RectTransform>().anchoredPosition.y - 30);
                }
                else if (letter.ToString() == "{")
                {
                    audioSource.Play();
                }
                else if (letter.ToString() == "}")
                {
                    audioSource.Stop();
                }
                else if (letter.ToString() == "£")
                {
                    morseCodeSoundSource.Play();
                }
                else if (letter.ToString() == "$")
                {
                    morseCodeSoundSource.Stop();
                }
                else if (letter.ToString() == ":")
                {
                    chatCursorBlinking = false;
                    typeCursor.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(
                        typeCursor.gameObject.GetComponent<RectTransform>().anchoredPosition.x,
                        typeCursor.gameObject.GetComponent<RectTransform>().anchoredPosition.y - 30);
                }
                else if (letter.ToString() == ";")
                {
                    ThreeChatOptions.SetActive(true);
                }
                else if (letter.ToString() == "]")
                {
                    InputChatOptions.SetActive(true);
                    inputtingText = true;

                }
                else if (letter.ToString() == "[")
                {
                    chatHacked = true;
                }
                else if (letter.ToString() == "+")
                {
                    //letterPause = letterPause - 0.1f;
                    chatCursorBlinking = true;
                }
                else if (letter.ToString() == "@")
                {
                    numberOfResponses += 1;
                    responseTrigger = true;
                }
                else if (letter.ToString() == "#")
                {
                    //textTyperToTrigger.GetComponent<TextTypingController>().hasTriggered = true;
                    chatPosition++;
                }
                else
                {
                    chatCursorBlinking = false;
                    typeCursor.SetActive(false);
                    textField.text += letter;

                }
            }
        }

        skipText = false;
    }

    //coroutine that waits for a few seconds unless the player taps, which returns it immediately
    IEnumerator WaitForSecondsOrTap(float seconds)
    {
        blinkPause = seconds;
        while (blinkPause>0f)
        {
            blinkPause -= Time.deltaTime;
            yield return 0f;
        }
    }

    //tap to end above coroutine
    private void Tap()
    {
        blinkPause = 0;
        skipText = true;
    }
}
