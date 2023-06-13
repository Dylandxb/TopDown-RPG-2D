using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{

    public Transform attachedCharacter;
    public float blendAmount = 0.05f;
    Vector3 curPos;
    Vector3 targetPos;
    float zVal;

    void Start()
    {
        curPos = transform.position;
        zVal = curPos.z;

        
    }

    
    void Update()
    {
        targetPos = attachedCharacter.transform.position;
        curPos = targetPos * blendAmount + curPos * (1.0f - blendAmount);
        curPos.z = zVal;
        transform.position = curPos;
    }
}
