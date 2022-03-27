using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleSpawner : MonoBehaviour
{
    System.Random rnd = new System.Random();
    [SerializeField] GameAreaManager gameAreaManager;
    [SerializeField] SnakeController snakeController;
    [SerializeField] GameObject applePrefab;
    public Sprite[] appleSprites;
    public List<int> spawnedApples = new List<int>();
    public int appleStartCount;
    public int selectedApple = 0;


    public void chooseApplesPosition(int appleCount) // chooses the apple position, as many as the given number
    {
        List<int> choosenApples = new List<int>();
        for (int i = 0; i < appleCount; i++)
        {
            choosenApples.Add(randomInt(choosenApples));
        }
        foreach (int apple in choosenApples) // spawns the apple
        {
            //int posX = apple % gameAreaManager.column;
            //int posY = (apple - posX)/gameAreaManager.column;
            applePrefab.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = appleSprites[selectedApple];
            GameObject newApple = Instantiate(applePrefab, getPosition(apple), Quaternion.identity);
            newApple.transform.SetParent(transform);
            newApple.GetComponent<Apple>().position = apple;
            spawnedApples.Add(apple);
        }
    }

    public Vector3 getPosition(int posID)
    {
        int posX = posID % gameAreaManager.column;
        int posY = (posID - posX)/gameAreaManager.column;
        return new Vector3(posX, posY, 0);
    }

    public bool isAppleOnSnake(int pos) // returns true if apple spawns on snake otherwise returns false
    {
        foreach (var body in snakeController.bodies)
        {
            if (pos == body.transform.position.x + body.transform.position.y * gameAreaManager.column)
            {
                return true;
            }
        }
        return false;
    }

    public bool hasEmptyTile()
    {
        return (snakeController.bodies.Count + spawnedApples.Count < gameAreaManager.column * gameAreaManager.row);
    }

    int randomInt(List<int> choosenApples) // returns an int
    {
        while (true)
        {
            int random = rnd.Next(gameAreaManager.row * gameAreaManager.column);
            if (!spawnedApples.Contains(random) && !choosenApples.Contains(random) && !isAppleOnSnake(random))
            {
                return random;
            }
        }
    }
}
