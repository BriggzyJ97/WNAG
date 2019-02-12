using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.UIElements;
using UnityEngine;

public class linePulser : MonoBehaviour
{
    
    public int lengthOfLineRenderer = 20;

    public GameObject targetGameObject;
    private Vector3 directionOfTargetGameObject;
    public float scalingLengthBetweenPositions = 0;

    public enum StateOfLine
    {
        growing,
        stop
    }

    private List<float> widthList = new List<float>(new float[]
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

    private LineRenderer lineRenderer;
    public AnimationCurve widthCurve1;
    public float widthMultiplier =1;

	// Use this for initialization
	void Start ()
	{
        //widthList.Add(1);
	    lineRenderer = gameObject.GetComponent<LineRenderer>();
        //lineRenderer .material = new Material(Shader.Find("Particles/Additive"));
        //lineRenderer.startColor = c1;
	    //lineRenderer.endColor = c2;
	    lineRenderer.positionCount = lengthOfLineRenderer;

        
	}
	
	// Update is called once per frame
	void Update ()
	{
	    Quaternion tempQuaternion = (Quaternion.Euler((transform.parent.parent.rotation.eulerAngles/Mathf.Infinity)));
        Debug.Log(tempQuaternion.eulerAngles);
        transform.rotation = Quaternion.Inverse(tempQuaternion);
	    widthCurve1 = new AnimationCurve();
	    int i = 0;

	    if (targetGameObject != null)
	    {
	        directionOfTargetGameObject = (targetGameObject.transform.position - transform.position).normalized;
	    }

	    float distanceBetweenStartAndTarget =
	        Vector3.Distance(lineRenderer.GetPosition(0), targetGameObject.transform.localPosition);
        //Debug.Log("Start: "+lineRenderer.GetPosition(0)+"  Target: "+targetGameObject.transform.position+"   Difference: "+distanceBetweenStartAndTarget);

	    float segmentLength = (distanceBetweenStartAndTarget / lengthOfLineRenderer)+0.02f;
        //Debug.Log("segL "+segmentLength);

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

        if (lineTimer>0.001f)
        {
            float tempWidthFloat = widthList[lengthOfLineRenderer - 1];
            widthList.RemoveAt(lengthOfLineRenderer - 1);
            widthList.Insert(0, tempWidthFloat);
            lineTimer = 0;
        }
	    
        lineRenderer.widthCurve = widthCurve1;
        //lineRenderer.widthCurve = widthCurve;

        /*
        AnimationCurve widthCurve = new AnimationCurve();
        Gradient colorGradient = new Gradient();
        int i = 0;
        if (targetGameObject!=null)
        {
            directionOfTargetGameObject = (targetGameObject.transform.position - transform.position).normalized;
        }
        
        float distanceBetweenTargetAndLastPos = Vector3.Distance(
            transform.position + lineRenderer.GetPosition(lengthOfLineRenderer - 1),
            targetGameObject.transform.position);

        Vector3 directionFromLastPosToTarget = (targetGameObject.transform.position -
                                                (transform.position +
                                                 lineRenderer.GetPosition(lengthOfLineRenderer - 1))).normalized;
        if (currentLineState == StateOfLine.growing ||currentLineState == StateOfLine.waiting)
        {
            if (distanceBetweenTargetAndLastPos < 10f)
            {
                //Debug.Log("shrinkingline");
                currentLineState = StateOfLine.waiting;
            }
            if (distanceBetweenTargetAndLastPos > 0.1f)
            {

                scalingLengthBetweenPositions += 0.1f * Time.deltaTime;
            }
        }
        if (currentLineState == StateOfLine.shrinking)
        {
            scalingLengthBetweenPositions -= 0.6f * Time.deltaTime;
            if (scalingLengthBetweenPositions <= 0)
            {
                Destroy(this.gameObject);
            }
        }
        while (i< lengthOfLineRenderer)
        {
            
            

            
            Vector3 pos = new Vector3(lineRenderer.GetPosition(0).x + (i * scalingLengthBetweenPositions * directionOfTargetGameObject.x), lineRenderer.GetPosition(0).y + (i* scalingLengthBetweenPositions * directionOfTargetGameObject.y), lineRenderer.GetPosition(0).z + (i* scalingLengthBetweenPositions * directionOfTargetGameObject.z));
            //lineRenderer.SetPosition(i, pos);
            float width = ((((Mathf.Sin(i+ -Time.time*Time.deltaTime*2)+1) / 2)+0.5f)/2);
            widthCurve.AddKey((i / 20f), width);
            if (colourOfLine == "blue")
            {
                colorGradient.SetKeys(
                    new GradientColorKey[] { new GradientColorKey(blueColor, 0), new GradientColorKey(whiteColor, 1) },
                    new GradientAlphaKey[] { new GradientAlphaKey(1, i / 20f) }
                );
            }else if
                (colourOfLine == "yellow")
            {
                colorGradient.SetKeys(
                    new GradientColorKey[] { new GradientColorKey(yellowColor, 0), new GradientColorKey(whiteColor, 1) },
                    new GradientAlphaKey[] { new GradientAlphaKey(1, i / 20f) }
                );
            }
            else if
                (colourOfLine == "red")
            {
                colorGradient.SetKeys(
                    new GradientColorKey[] { new GradientColorKey(redColor, 0), new GradientColorKey(whiteColor, 1) },
                    new GradientAlphaKey[] { new GradientAlphaKey(1, i / 20f) }
                );
            }

            

            lineRenderer.widthCurve = widthCurve;
            //Debug.Log("End of line position: "+ transform.position + lineRenderer.GetPosition(lengthOfLineRenderer - 1));
            // Debug.Log("target Position: "+targetGameObject.transform.position);
            //Debug.Log("Distance Difference:" + Vector3.Distance(transform.position + lineRenderer.GetPosition(lengthOfLineRenderer - 1), targetGameObject.transform.position));
            lineRenderer.colorGradient = colorGradient;
            i++;
        }

        if (currentLineState == StateOfLine.waiting)
        {
            lineTimer -= Time.deltaTime;
            if (lineTimer<=0)
            {
                //currentLineState = StateOfLine.shrinking;
            }
        }

        
        lineRenderer.widthCurve = widthCurve;*/
    }
}
