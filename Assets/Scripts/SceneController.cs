using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [SerializeField] GameObject[] menus;
    [SerializeField] ScrollviewController[] scrollViews;
    [SerializeField] Image[] appleImages;
    [SerializeField] Image[] gameModeImages;
    [SerializeField] Sprite[] gameModeSprites;
    [SerializeField] GameObject[] appleCountImages;
    [SerializeField] Image speedPreview;
    [SerializeField] Sprite[] speedSprites;
    [SerializeField] Image[] grassFieldPreviews;
    [SerializeField] Sprite[] grassFieldSprites;
    [SerializeField] Image snakePreview;
    [SerializeField] Sprite[] snakeSprites;
    [SerializeField] SnakeController snakeController;
    [SerializeField] AppleSpawner appleSpawner;
    [SerializeField] GameAreaManager gameAreaManager;
    [SerializeField] MouthAnimation mouthAnimation;
    [SerializeField] Transform border;
    public bool[] inMenus;

    void Start()
    {
        inMenus = new bool[menus.Length];
        openMenu(0);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }
    }

    public void openMenu(int menuIndex) // opens a menu by index
    {
        for (int i = 0; i < menus.Length; i++)
        {
            if (i == menuIndex)
            {
                menus[i].SetActive(true);
                inMenus[i] = true;
            }
            else
            {
                menus[i].SetActive(false);
                inMenus[i] = false;
            }
        }
    }

    public void backToMain()
    {
        saveSettings();
        openMenu(0);
    }

    public void startGame()
    {
        saveSettings();
        restartGame();
        openMenu(-1);
    }

    void saveSettings()
    {
        // save apple
        appleSpawner.selectedApple = scrollViews[0].selectedItem;
        foreach (var image in appleImages)
        {
            image.sprite = appleSpawner.appleSprites[appleSpawner.selectedApple];
        }
        // save game mode
        //gamemode = scrollViews[1].selectedItem;
        foreach (var image in gameModeImages)
        {
            image.sprite = gameModeSprites[scrollViews[1].selectedItem];
        }
        // save apple count
        if (scrollViews[2].selectedItem == 0)
        {
            appleSpawner.appleStartCount = 1;
        }
        else if (scrollViews[2].selectedItem == 1)
        {
            appleSpawner.appleStartCount = 3;
        }
        else if (scrollViews[2].selectedItem == 2)
        {
            appleSpawner.appleStartCount = 5;
        }
        for (int i = 0; i < appleCountImages.Length; i++)
        {
            if (i == scrollViews[2].selectedItem)
            {
                appleCountImages[i].SetActive(true);
            }
            else
            {
                appleCountImages[i].SetActive(false);
            }
        }
        // save snake speed
        if (scrollViews[3].selectedItem == 0)
        {
            speedPreview.enabled = false;
            snakeController.snakeSpeed = 120f;
        }
        else
        {
            speedPreview.enabled = true;
            speedPreview.sprite = speedSprites[scrollViews[3].selectedItem];
            if (scrollViews[3].selectedItem == 1)
            {
                snakeController.snakeSpeed = 500f;
            }
            else if (scrollViews[3].selectedItem == 2)
            {
                snakeController.snakeSpeed = 90f;
            }
        }
        // save grass field size
        if (scrollViews[4].selectedItem == 0)
        {
            grassFieldPreviews[0].sprite = grassFieldSprites[0];
            grassFieldPreviews[1].sprite = grassFieldSprites[1];
            gameAreaManager.row = 9;
            gameAreaManager.column = 10;
        }
        else if (scrollViews[4].selectedItem == 1)
        {
            grassFieldPreviews[0].sprite = grassFieldSprites[2];
            grassFieldPreviews[1].sprite = grassFieldSprites[3];
            gameAreaManager.row = 5;
            gameAreaManager.column = 6;
        }
        else if (scrollViews[4].selectedItem == 2)
        {
            grassFieldPreviews[0].sprite = grassFieldSprites[4];
            grassFieldPreviews[1].sprite = grassFieldSprites[5];
            gameAreaManager.row = 21;
            gameAreaManager.column = 22;
        }
        // save snake color
        snakePreview.sprite = snakeSprites[scrollViews[5].selectedItem];
        snakeController.skinColor = scrollViews[5].selectedItem;
    }

    public bool anyMenuOpened() // checks if any menu opened
    {
        foreach (var item in inMenus)
        {
            if (item)
            {
                return true;
            }
        }
        return false;
    }

    void restartGame()
    {
        // clears snake
        Transform snake = snakeController.gameObject.transform;
        for (int i = 0; i < snake.childCount; i++)
        {
            Destroy(snake.GetChild(i).gameObject);
        }
        snakeController.bodies.Clear();
        snakeController.isDead = false;
        snakeController.isWin = false;
        snakeController.direction = Vector3.zero;
        // clears game area
        Transform gameArea = gameAreaManager.gameObject.transform;
        for (int i = 0; i < gameArea.childCount; i++)
        {
            Destroy(gameArea.GetChild(i).gameObject);
        }
        // clears apples
        Transform apples = appleSpawner.gameObject.transform;
        for (int i = 0; i < apples.childCount; i++)
        {
            Destroy(apples.GetChild(i).gameObject);
        }
        appleSpawner.spawnedApples.Clear();
        // clears border
        for (int i = 0; i < border.childCount; i++)
        {
            Destroy(border.GetChild(i).gameObject);
        }
        // initialize game
        snakeController.init();
        gameAreaManager.init();
        appleSpawner.chooseApplesPosition(appleSpawner.appleStartCount);
        mouthAnimation.stayOnHead();
    }
}
