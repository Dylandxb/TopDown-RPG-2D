using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stat : MonoBehaviour
{
    private Image content;

    [SerializeField] private Text statValue;

    [SerializeField] private float lerpSpeed;

    private float currentFill;

    public float MyMaxValue { get; set; }

    private float currentValue;

    public float MyCurrentValue
    {
        get
        {
            return currentValue;
        }
        set
        {
            if(value > MyMaxValue)
            {
                currentValue = MyMaxValue;
            }
            else if (value < 0)
            {
                currentValue = 0;
            }
            else
            {
                currentValue = value;
            }

            currentFill = currentValue / MyMaxValue;

            statValue.text = currentValue.ToString("F0") + "/" + MyMaxValue; //Sets the currentValue to the rounded version of the float out of the maxvalue of the variable
        }


    }

    

    void Start()
    {
       
        content = GetComponent<Image>(); //references the actual image and stores it in variable content
        
    }

    
    public void Update()
    {
        if (currentFill != content.fillAmount)
        {
            content.fillAmount = Mathf.Lerp(content.fillAmount, currentFill, Time.deltaTime * lerpSpeed);
        }
        content.fillAmount = currentFill;
    }

    public void Initialize(float currentValue, float maxValue)
    {
        MyMaxValue = maxValue;
        MyCurrentValue = currentValue;
    }
}
