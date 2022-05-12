//from video at https://youtu.be/6C1NPy321Nk
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GrappleWave : MonoBehaviour
{
    public LineRenderer myLineRenderer;
    public int points;
    public float amplitude = 1;
    public float frequency = 1;
    public Vector2 xLimits = new Vector2(0,1);
    public float movementSpeed = 1;
    [Range(0,2*Mathf.PI)]
    public CharacterControllerNonUnity characterController;
    public float radians;
    void Start()
    {
        myLineRenderer = GetComponent<LineRenderer>();
        characterController = GameObject.Find("DrawCharacter").GetComponent<CharacterControllerNonUnity>();
    }
    
    void Draw()
    {
        float xStart=0;
        float xFinish=0;
        if (transform.localScale.x > 0)
        {
            xStart = characterController.transform.position.x+0.2f;
            xFinish =transform.position.x-0.8f;
        }
        else if (transform.localScale.x < 0)
        {
           xStart = characterController.transform.position.x-0.2f;
           xFinish =transform.position.x+0.8f;
        }
        
        float Tau = 2* Mathf.PI;
        

        myLineRenderer.positionCount = points;
        for(int currentPoint = 0; currentPoint<points;currentPoint++)
        {
            float progress = (float)currentPoint/(points-1);
            float x = Mathf.Lerp(xStart,xFinish,progress);
            float y = amplitude*Mathf.Sin((Tau*frequency*x)+(Time.timeSinceLevelLoad*movementSpeed));
            myLineRenderer.SetPosition(currentPoint, new Vector3(x,y+transform.position.y,characterController.transform.position.z));
        }
    }

    void Update()
    {
        Draw();
    }
}