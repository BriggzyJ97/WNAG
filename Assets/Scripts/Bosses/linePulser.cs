using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.UIElements;
using UnityEngine;

public class linePulser : MonoBehaviour //this script manages the line renderer of the laser 
{
    #region Variables

    public int lengthOfLineRenderer = 20;//how many segments in the line

    public GameObject targetGameObject;//the target of laser
    private Vector3 directionOfTargetGameObject;
    public float scalingLengthBetweenPositions = 0;//distance between each segment of line

    public enum StateOfLine//different states of line
    {
        growing,
        stop
    }

    private List<float> widthList = new List<float>(new float[]//list of widths for the line to create a width curve and cycle through, moving the line
    {
        1.7f,
        1.5f,
        1.3f,
        1.1f,
        0.7f,
        1.7f,
        1.5f,
        1.3f,
        1.1f,
        0.7f,
        1.7f,
        1.5f,
        1.3f,
        1.1f,
        0.7f,
        1.7f,
        1.5f,
        1.3f,
        1.1f,
        0.7f,


    });

    private float lineTimer = 0f;
    public StateOfLine currentLineState = StateOfLine.growing;

    public LineRenderer lineRenderer;//the line itself
    public AnimationCurve widthCurve1;//the width curve of line
    public float widthMultiplier =1;//used to change width of laser while still keeping curve

#endregion

    // assign variables
    void Start ()
	{
	    lineRenderer = gameObject.GetComponent<LineRenderer>();
	    lineRenderer.positionCount = lengthOfLineRenderer;

        
	}
	
	// Update is called once per frame
	void Update ()
	{
        //make sure rotation matches parent because it was unlinked
	    Quaternion tempQuaternion = (Quaternion.Euler((transform.parent.parent.rotation.eulerAngles/Mathf.Infinity)));
        Debug.Log(tempQuaternion.eulerAngles);
        transform.rotation = Quaternion.Inverse(tempQuaternion);
	    widthCurve1 = new AnimationCurve();


	    int i = 0;

        #region Setup dynamic Variables
        if (targetGameObject != null)
	    {
	        directionOfTargetGameObject = (targetGameObject.transform.position - transform.position).normalized;
	    }

	    float distanceBetweenStartAndTarget =
	        Vector3.Distance(lineRenderer.GetPosition(0), targetGameObject.transform.localPosition);
        

	    float segmentLength = (distanceBetweenStartAndTarget / lengthOfLineRenderer)+0.02f;
#endregion

        //go through each segment of line renderer setting width based on the list of widths
        while (i<lengthOfLineRenderer)
	    {
	        Vector3 pos = new Vector3(lineRenderer.GetPosition(0).x + (i * segmentLength  * directionOfTargetGameObject.x), lineRenderer.GetPosition(0).y + (i * segmentLength * directionOfTargetGameObject.y), lineRenderer.GetPosition(0).z + (i * segmentLength * directionOfTargetGameObject.z));
           // Debug.Log(i+ "   "+pos);
	        lineRenderer.SetPosition(i, pos);
            float width = (widthList[i]*widthMultiplier);
	        //float width = ((((Mathf.Sin(i + -Time.time * Time.deltaTime * 2) + 1) / 2) + 0.5f) / 2);

	        widthCurve1.AddKey((i / 20f), width);
	        i++;
            lineRenderer.widthCurve = widthCurve1;
            
	    }

	    lineTimer += Time.deltaTime;

        //cycle the width list, moving back to the front, creating movement
        if (lineTimer>0.001f)
        {
            float tempWidthFloat = widthList[lengthOfLineRenderer - 1];
            widthList.RemoveAt(lengthOfLineRenderer - 1);
            widthList.Insert(0, tempWidthFloat);
            lineTimer = 0;
        }
	    
        lineRenderer.widthCurve = widthCurve1;
        //lineRenderer.widthCurve = widthCurve;
        
    }
}
