using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class circularRotation : MonoBehaviour
{
    [SerializeField]
    Transform rotationCenter;
    [SerializeField]
    float rotationRadius=2f, angularspeed=2f;

    float posx,posy,angle =0f;
    
    void Update()
    {
        posx =  rotationCenter.position.x + Mathf.Cos(angle) * rotationRadius;
        posy =  rotationCenter.position.y + Mathf.Sin(angle) * rotationRadius;
        transform.position = new Vector2 (posx,posy);
        angle = angle + Time.deltaTime * angularspeed;
        if(angle>=360)
            angle=0f;
    }
}
