using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAreaManager : MonoBehaviour
{
    [SerializeField] Transform borders;
    [SerializeField] GameObject borderPrefab;
    [SerializeField] Color borderColor;
    [SerializeField] GameObject tilePrefab;
    [SerializeField] Color[] tileColors;
    public int row = 10;
    public int column = 10;

    public void init()
    {
        createGameArea();
        createBorders();
    }

    void createGameArea() // creates game area and adjusts the camera to the game area
    {
        for (int c = 0; c < column; c++)
        {
            for (int r = 0; r < row; r++)
            {
                GameObject newTile = Instantiate(tilePrefab, new Vector3(c, r, 1), Quaternion.identity);
                newTile.transform.SetParent(transform);
                if ((c % 2 == 0 && r % 2 == 0) || (c % 2 == 1 && r % 2 == 1))
                {
                    newTile.GetComponent<SpriteRenderer>().color = tileColors[0];
                }
                else
                {
                    newTile.GetComponent<SpriteRenderer>().color = tileColors[1];
                }
            }
        }
        Camera.main.transform.position = new Vector3((column-1)/2f, row/2f, -10);
        Camera.main.orthographicSize = (row+3) / 2f;
    }

    void createBorders()
    {
        Transform leftBorder = Instantiate(borderPrefab, new Vector3(-1, (row-1)/2f, 1), Quaternion.identity).transform;
        leftBorder.localScale = new Vector3(1, row + 1, 1);
        leftBorder.SetParent(borders);

        Transform rightBorder = Instantiate(borderPrefab, new Vector3(column, (row-1)/2f, 1), Quaternion.identity).transform;
        rightBorder.localScale = new Vector3(1, row + 1, 1);
        rightBorder.SetParent(borders);

        Transform upBorder = Instantiate(borderPrefab, new Vector3((column-1)/2f, row, 1), Quaternion.identity).transform;
        upBorder.localScale = new Vector3(column + 2, 1, 1);
        upBorder.SetParent(borders);

        Transform downBorder = Instantiate(borderPrefab, new Vector3((column-1)/2f, -1, 1), Quaternion.identity).transform;
        downBorder.localScale = new Vector3(column + 2, 1, 1);
        downBorder.SetParent(borders);
    }
}
