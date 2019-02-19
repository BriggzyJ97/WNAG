using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class cameraModeSwap : MonoBehaviour
//this script manages changing the camera's post processing stack on the console dialogue levels
{
    #region Variables
    private bool inConsoleMode = false;//is the player in the console

    public PostProcessingProfile consolePPP;//the PP stack for in-console
    private PostProcessingProfile mainPPP;// the normal PP stack 

    private PostProcessingBehaviour PPBehaviour; //the cameras PP component
    #endregion

    // Use this for initialization
    void Start ()
	{
        //assign variables
	    PPBehaviour = gameObject.GetComponent<PostProcessingBehaviour>();
	    mainPPP = PPBehaviour.profile;

	}

    // change to the console PPStack
    public void SwapCameraMode()
    {
        if (inConsoleMode==true)
        {
            PPBehaviour.profile = mainPPP;
            gameObject.GetComponent<CRTDistortion>().enabled = false;
            inConsoleMode = false;

        }else if (inConsoleMode==false)
        {
            PPBehaviour.profile = consolePPP;
            gameObject.GetComponent<CRTDistortion>().enabled = true;
            inConsoleMode = true;
        }
    }
}
