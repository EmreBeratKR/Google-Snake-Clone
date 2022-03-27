using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouthAnimation : MonoBehaviour
{
    [SerializeField] GameObject snake;
    [SerializeField] SceneController sceneController;
    bool isMouthOpen = false;
    int nearByAppleCount = 0;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Apple")
        {
            nearByAppleCount++;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Apple")
        {
            nearByAppleCount--;
        }
    }

    void Update()
    {
        if (!sceneController.anyMenuOpened())
        {
            mouthControl();
        }
    }

    public void stayOnHead()
    {
        transform.position = snake.transform.GetChild(0).Find("Sprites").position;
        transform.rotation = snake.transform.GetChild(0).rotation;
    }

    void mouthControl()
    {
        if (nearByAppleCount > 0)
        {
            if (!isMouthOpen)
            {
                StartCoroutine(openMouth());
            }
        }
        else
        {
            if (isMouthOpen)
            {
                StartCoroutine(closeMouth());
            }
        }
    }

    IEnumerator openMouth()
    {
        isMouthOpen = true;
        for (int i = 0; i < 10; i++)
        {
            transform.GetChild(0).localScale += new Vector3(0, 0.1f, 0);
            transform.GetChild(0).localPosition += new Vector3(0, 0.025f, 0);
            yield return 0;
        }
    }
    IEnumerator closeMouth()
    {
        isMouthOpen = false;
        for (int i = 0; i < 10; i++)
        {
            transform.GetChild(0).localScale -= new Vector3(0, 0.1f, 0);
            transform.GetChild(0).localPosition -= new Vector3(0, 0.025f, 0);
            yield return 0;
        }
    }
}