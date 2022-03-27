using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPart : MonoBehaviour
{
    AppleSpawner appleSpawner;
    SnakeController snakeController;
    GameAreaManager gameAreaManager;
    SceneController sceneController;
    AudioController audioController;
    public string bodyType;

    void Start()
    {
        appleSpawner = GameObject.FindGameObjectWithTag("Apple Spawner").GetComponent<AppleSpawner>();
        snakeController = transform.parent.parent.parent.GetComponent<SnakeController>();
        gameAreaManager = GameObject.FindGameObjectWithTag("Game Area").GetComponent<GameAreaManager>();
        sceneController = GameObject.FindGameObjectWithTag("UI Canvas").GetComponent<SceneController>();
        audioController = GameObject.FindGameObjectWithTag("Audio System").GetComponent<AudioController>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Apple") // executes one time when snake ate an apple
        {
            // spawns 1 apple if there is an empty tile left
            Destroy(other.gameObject);
            appleSpawner.spawnedApples.Remove(other.GetComponent<Apple>().position);
            audioController.eatAppleSound.Play();
            if (appleSpawner.hasEmptyTile())
            {
                appleSpawner.chooseApplesPosition(1);
            }
            // checks if snake grew up to his max size
            if (snakeController.bodies.Count == gameAreaManager.column * gameAreaManager.row) // this is the win condition
            {
                snakeController.isWin = true;
                StartCoroutine(enterMain());
            }

        }
        else if (other.tag == "Body Part" || other.tag == "Border") // executes one time when snake hit a border or himself
        {
            snakeController.StopAllCoroutines();
            audioController.gameOverSound.Play();
            snakeController.isDead = true;
            StartCoroutine(enterMain());
        }
    }

    IEnumerator enterMain()
    {
        yield return new WaitForSeconds(1f);
        sceneController.openMenu(0);
    }
}
