using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeMovements : MonoBehaviour
{

AppleSpawner appleSpawner;
Transform snake;
float eyeRadius = 0.07f;

void Start()
{
    appleSpawner = GameObject.FindWithTag("Apple Spawner").GetComponent<AppleSpawner>();
    snake = GameObject.FindWithTag("Snake").transform;
    StartCoroutine(movePupil(transform.GetChild(0)));
    StartCoroutine(movePupil(transform.GetChild(1)));
}


IEnumerator movePupil(Transform eye) // updates pupil position every frame
{
    eye.GetChild(0).GetChild(0).localPosition = new Vector3(0, 0, eye.GetChild(0).GetChild(0).position.z);
    eye.GetChild(0).GetChild(0).position += (eyeRadius / (applePos() - eye.position).magnitude) * (applePos() - eye.position);
    yield return 0;
}

Vector3 applePos() // returns nearest apple's position
{
    Vector3 currentNearest = appleSpawner.getPosition(appleSpawner.spawnedApples[0]);
    foreach (var apple in appleSpawner.spawnedApples)
    {
        if ((appleSpawner.getPosition(apple) - snake.GetChild(0).position).magnitude < (currentNearest - snake.GetChild(0).position).magnitude)
        {
            currentNearest = appleSpawner.getPosition(apple);
        }
    }
    return currentNearest;
}




}
