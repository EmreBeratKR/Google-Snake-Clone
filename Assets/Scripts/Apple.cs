using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : MonoBehaviour
{
    public int position;
    void Start()
    {
        StartCoroutine(appleBreathIn()); // starts apple breathing animation coroutine
    }

    IEnumerator appleBreathIn() // scales up the apple size
    {
        for (int i = 0; i < 60; i++)
        {
            transform.GetChild(0).localScale += new Vector3(0.005f, 0.005f, 0);
            yield return new WaitForSeconds(1f/120f);
        }
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(appleBreathOut());
    }

    IEnumerator appleBreathOut() // scales down the apple size
    {
        for (int i = 0; i < 60; i++)
        {
            transform.GetChild(0).localScale -= new Vector3(0.005f, 0.005f, 0);
            yield return new WaitForSeconds(1f/120f);
        }
        StartCoroutine(appleBreathIn());
    }
}
