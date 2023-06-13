using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class MessageDisplay : MonoBehaviour
{

    public Transform messageUI;
    Text textObject;

    void Start()
    {
        textObject = messageUI.GetChild(1).GetComponent<Text>();
    }


    IEnumerator DoMessage(string message, float seconds)
    {
        messageUI.gameObject.SetActive(true);
        textObject.text = message;
        yield return new WaitForSeconds(seconds);
        messageUI.gameObject.SetActive(false);
    }    

    public void ShowMessage(string message, float seconds)
    {
        StartCoroutine(DoMessage(message, seconds));
    }

    IEnumerator DoMultilineMessage(string message)
    {
        messageUI.gameObject.SetActive(true);
        string[] lines = message.Split('\n');
        foreach (string line in lines)
        {
            textObject.text = line;
            yield return null;
            while (true)
            {
                if (Input.GetButtonDown("Fire1"))
                    break;
                yield return null;
            }
        }
        messageUI.gameObject.SetActive(false);
    }

    public void ShowMultilineMessage(string message)
    {
        StartCoroutine(DoMultilineMessage(message));
    }

    IEnumerator DoYesNo(string message, Action<bool>callback)
    {
        message += "\n(Y/N)";
        messageUI.gameObject.SetActive(true);
        textObject.text = message;
        bool answer = false;
        while (true)
        {
            if (Input.GetKeyDown("n"))
            {
                answer = false;
                break;
            }
            if (Input.GetKeyDown("y"))
            {
                answer = true;
                break;
            }
            yield return null;
        }
        messageUI.gameObject.SetActive(false);
        callback(answer);
    }

    public void YesNoMessage(string message, Action<bool>answerFunc)
    {
        StartCoroutine(DoYesNo(message, answerFunc));
    }
}
