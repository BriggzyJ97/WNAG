using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ToggleScript : MonoBehaviour {//this script toggles between various settings used in options menu
    
    public enum ToggleType
    {
        lasersOnOff,
        musicOnOff,
        noneMinMax,

    }

    public ToggleType ThisToggleType = ToggleType.lasersOnOff;

    private int toggleValue = 2;


    //initialize toggle buttons
    void Start () {
        
	    if (ThisToggleType == ToggleType.lasersOnOff)
	    {
	        if (PlayerPrefs.GetInt("lasersOn") == 1)
	        {
	            toggleValue = 1;
	            gameObject.GetComponent<TextMeshProUGUI>().text = "<on>";
            }else if (PlayerPrefs.GetInt("lasersOn") == 1)
	        {
	            toggleValue = 2;
                gameObject.GetComponent<TextMeshProUGUI>().text = "<off>";
            }
        }

       
	    else if (ThisToggleType == ToggleType.musicOnOff)
	    {
	        if (PlayerPrefs.GetInt("musicMuted") == 1)
	        {
	            toggleValue = 1;
	            gameObject.GetComponent<TextMeshProUGUI>().text = "<off>";
	        }
	        else if (PlayerPrefs.GetInt("musicMuted") == 1)
	        {
	            toggleValue = 2;
	            gameObject.GetComponent<TextMeshProUGUI>().text = "<on>";
	        }
        }

       
	    else if (ThisToggleType == ToggleType.noneMinMax)
	    {
	        if (PlayerPrefs.GetString("PPSetting") == "none")
	        {
	            toggleValue = 1;
	            gameObject.GetComponent<TextMeshProUGUI>().text = "<none>";
            }
            else
	        if (PlayerPrefs.GetString("PPSetting") == "min")
	        {
	            toggleValue = 2;
	            gameObject.GetComponent<TextMeshProUGUI>().text = "<min>";
            }
	        else
	        if (PlayerPrefs.GetString("PPSetting") == "max")
	        {
	            toggleValue = 3;
	            gameObject.GetComponent<TextMeshProUGUI>().text = "<max>";
            }

        }
	    
        
	}
	
    public void Toggle()
    {
        //toggle targetting lasers on/off
        if (ThisToggleType == ToggleType.lasersOnOff)
        {
            if (toggleValue==1)
            {
                toggleValue = 2;
                gameObject.GetComponent<TextMeshProUGUI>().text = "<off>";
            }
            else
            {
                toggleValue = 1;
                gameObject.GetComponent<TextMeshProUGUI>().text = "<on>";
            }
        }

        //toggle music on/off
        else
        if (ThisToggleType == ToggleType.musicOnOff)
        {
            if (toggleValue == 1)
            {
                toggleValue = 2;
                gameObject.GetComponent<TextMeshProUGUI>().text = "<on>";
            }
            else
            {
                toggleValue = 1;
                gameObject.GetComponent<TextMeshProUGUI>().text = "<off>";
            }
        }

        //toggle post processing stack none/max/min
        else if (ThisToggleType == ToggleType.noneMinMax)
        {
            if (toggleValue==1)
            {
                toggleValue = 2;
                gameObject.GetComponent<TextMeshProUGUI>().text = "min";
                GameObject.FindGameObjectWithTag("mainMenuController").GetComponent<mainMenuController>().setPPtoMin();
            }else if (toggleValue==2)
            {
                toggleValue = 3;
                gameObject.GetComponent<TextMeshProUGUI>().text = "max";
                GameObject.FindGameObjectWithTag("mainMenuController").GetComponent<mainMenuController>().setPPtoMax();
            }
            else if (toggleValue==3)
            {
                toggleValue = 1;
                gameObject.GetComponent<TextMeshProUGUI>().text = "none";
                GameObject.FindGameObjectWithTag("mainMenuController").GetComponent<mainMenuController>().setPPtoNone();
            }
        }
    }
}
