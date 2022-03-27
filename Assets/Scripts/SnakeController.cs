using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeController : MonoBehaviour
{
    public List<GameObject> bodies;
    [SerializeField] AppleSpawner appleSpawner;
    [SerializeField] GameAreaManager gameAreaManager;
    [SerializeField] SceneController sceneController;
    [SerializeField] AudioController audioController;
    [SerializeField] MouthAnimation mouthAnimation;
    [SerializeField] GameObject bodyPrefab;
    [SerializeField] Sprite[] sprites;
    [SerializeField] Color[] skinColors;
    public Vector3 direction = Vector3.zero;
    public int skinColor = 0;
    public float snakeSpeed;
    bool canChangeDir;
    public bool isDead;
    public bool isWin;

    void Update()
    {
        if (!sceneController.anyMenuOpened() && canChangeDir && !isDead && !isWin)
        {
            checkUserInput();
        }
        if (Input.GetKeyDown(KeyCode.Space) && (isDead || isWin || sceneController.anyMenuOpened()))
        {
            sceneController.startGame();
        }
    }

    public void init()
    {
        canChangeDir = true;
        isDead = false;
        isWin = false;
        setSnakeColor();
        spawnBody(Vector3.zero, false);
        spawnBody(Vector3.right, false);
        spawnBody(Vector3.right * 2, false);
        updateSnakeSprites();
        StartCoroutine(moveSnake());
    }

    void checkUserInput() // checks user input
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) && (Vector3.left != direction) && (Vector3.left != illegalMove()))
        {
            direction = Vector3.left;
            canChangeDir = false;
            audioController.turnSounds[1].Play();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) && (Vector3.right != direction) && (Vector3.right != illegalMove()))
        {
            direction = Vector3.right;
            canChangeDir = false;
            audioController.turnSounds[2].Play();
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) && (Vector3.up != direction) && (Vector3.up != illegalMove()))
        {
            direction = Vector3.up;
            canChangeDir = false;
            audioController.turnSounds[3].Play();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) && (Vector3.down != direction) && (Vector3.down != illegalMove()))
        {
            direction = Vector3.down;
            canChangeDir = false;
            audioController.turnSounds[0].Play();
        }
    }

    void setSnakeColor() // changes color of the snake
    {
        Color primary = skinColors[2 * skinColor];
        Color secondary = skinColors[2 * skinColor + 1];

        bodyPrefab.transform.Find("Sprites").Find("Body").gameObject.GetComponent<SpriteRenderer>().color = primary;

        //bodyPrefab.transform.Find("Sprites").Find("Mouth").gameObject.GetComponent<SpriteRenderer>().color = primary;
        mouthAnimation.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = primary;
        //bodyPrefab.transform.Find("Sprites").Find("Mouth").Find("Inner").gameObject.GetComponent<SpriteRenderer>().color = secondary;
        mouthAnimation.transform.GetChild(0).Find("Inner").gameObject.GetComponent<SpriteRenderer>().color = secondary;

        Transform eyes = bodyPrefab.transform.Find("Sprites").Find("Face").Find("Eyes");

        eyes.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = primary;
        eyes.GetChild(0).GetChild(0).GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = secondary;

        eyes.GetChild(1).gameObject.GetComponent<SpriteRenderer>().color = primary;
        eyes.GetChild(1).GetChild(0).GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = secondary;

        bodyPrefab.transform.Find("Sprites").Find("Face").Find("Nose Holes").GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = secondary;
        bodyPrefab.transform.Find("Sprites").Find("Face").Find("Nose Holes").GetChild(1).gameObject.GetComponent<SpriteRenderer>().color = secondary;
    }

    public void updateSnakeSprites() // updates first 2 and last body parts' sprite
    {
        // updates head sprite
        bodies[0].transform.Find("Sprites").Find("Body").gameObject.GetComponent<BodyPart>().bodyType = "head";
        bodies[0].transform.Find("Sprites").Find("Body").gameObject.GetComponent<SpriteRenderer>().sprite = sprites[0];
        bodies[0].transform.Find("Sprites").Find("Face").gameObject.SetActive(true);
        //bodies[0].transform.Find("Sprites").Find("Mouth").gameObject.SetActive(true);
        Vector3 headDir = bodies[0].transform.position - bodies[1].transform.position;
        if (headDir == Vector3.up)
        {
            bodies[0].transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (headDir == Vector3.left)
        {
            bodies[0].transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        else if (headDir == Vector3.down)
        {
            bodies[0].transform.rotation = Quaternion.Euler(0, 0, 180);
        }
        else if (headDir == Vector3.right)
        {
            bodies[0].transform.rotation = Quaternion.Euler(0, 0, 270);
        }
        // updates tail sprite
        bodies[bodies.Count-1].transform.Find("Sprites").Find("Body").gameObject.GetComponent<BodyPart>().bodyType = "tail";
        bodies[bodies.Count-1].transform.Find("Sprites").Find("Body").gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
        Destroy(bodies[bodies.Count-1].transform.Find("Sprites").Find("Body").gameObject.GetComponent<Rigidbody2D>());
        bodies[bodies.Count-1].transform.Find("Sprites").Find("Body").gameObject.GetComponent<SpriteRenderer>().sprite = sprites[3];
        Vector3 tailDir = bodies[bodies.Count-1].transform.position - bodies[bodies.Count-2].transform.position;
        if (tailDir == Vector3.up)
        {
            bodies[bodies.Count-1].transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (tailDir == Vector3.left)
        {
            bodies[bodies.Count-1].transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        else if (tailDir == Vector3.down)
        {
            bodies[bodies.Count-1].transform.rotation = Quaternion.Euler(0, 0, 180);
        }
        else if (tailDir == Vector3.right)
        {
            bodies[bodies.Count-1].transform.rotation = Quaternion.Euler(0, 0, 270);
        }
        bodies[1].transform.Find("Sprites").Find("Body").gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
        bodies[1].transform.Find("Sprites").Find("Body").gameObject.GetComponent<BoxCollider2D>().offset = Vector2.zero;
        bodies[1].transform.Find("Sprites").Find("Body").gameObject.GetComponent<BoxCollider2D>().size = new Vector2(0.9f, 0.9f);
        bodies[1].transform.Find("Sprites").Find("Face").gameObject.SetActive(false);
        //bodies[1].transform.Find("Sprites").Find("Mouth").gameObject.SetActive(false);
        Destroy(bodies[1].transform.Find("Sprites").Find("Body").gameObject.GetComponent<Rigidbody2D>());
        Vector3 neighborDif = bodies[0].transform.position - bodies[2].transform.position;
        if (neighborDif.x == 0) // updates to normal sprite
        {
            bodies[1].transform.Find("Sprites").Find("Body").gameObject.GetComponent<BodyPart>().bodyType = "normal";
            bodies[1].transform.Find("Sprites").Find("Body").gameObject.GetComponent<SpriteRenderer>().sprite = sprites[2];
            bodies[1].transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (neighborDif.y == 0)
        {
            bodies[1].transform.Find("Sprites").Find("Body").gameObject.GetComponent<BodyPart>().bodyType = "normal";
            bodies[1].transform.Find("Sprites").Find("Body").gameObject.GetComponent<SpriteRenderer>().sprite = sprites[2];
            bodies[1].transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        else // updates to corner sprite
        {
            bodies[1].transform.Find("Sprites").Find("Body").gameObject.GetComponent<BodyPart>().bodyType = "corner";
            bodies[1].transform.Find("Sprites").Find("Body").gameObject.GetComponent<SpriteRenderer>().sprite = sprites[1];
            Vector3 frontNeighborDif = bodies[0].transform.position - bodies[1].transform.position;
            Vector3 backNeighborDif = bodies[2].transform.position - bodies[1].transform.position;
            if ((frontNeighborDif == Vector3.right && backNeighborDif == Vector3.down) || (frontNeighborDif == Vector3.down && backNeighborDif == Vector3.right))
            {
                bodies[1].transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else if ((frontNeighborDif == Vector3.right && backNeighborDif == Vector3.up) || (frontNeighborDif == Vector3.up && backNeighborDif == Vector3.right))
            {
                bodies[1].transform.rotation = Quaternion.Euler(0, 0, 90);
            }
            else if ((frontNeighborDif == Vector3.left && backNeighborDif == Vector3.up) || (frontNeighborDif == Vector3.up && backNeighborDif == Vector3.left))
            {
                bodies[1].transform.rotation = Quaternion.Euler(0, 0, 180);
            }
            else if ((frontNeighborDif == Vector3.left && backNeighborDif == Vector3.down) || (frontNeighborDif == Vector3.down && backNeighborDif == Vector3.left))
            {
                bodies[1].transform.rotation = Quaternion.Euler(0, 0, 270);
            }
        }
    }

    public GameObject spawnBody(Vector3 pos, bool isMoving) // adds a new body part to the beginning of the snake
    {
        GameObject newBody = Instantiate(bodyPrefab, pos, Quaternion.identity);
        if (isMoving)
        {
            newBody.transform.Find("Sprites").localPosition = new Vector3(0, -1f, 0);
        }
        bodies.Insert(0, newBody);
        newBody.transform.SetParent(transform);
        newBody.transform.SetSiblingIndex(0);
        return newBody;
    }

    void removeTail() // removes last body part of the snake
    {
        GameObject tail = bodies[bodies.Count-1];
        bodies.RemoveAt(bodies.Count-1);
        Destroy(tail);
    }

    Vector3 illegalMove() // returns the illegal move which snake cannot direct to
    {
        return bodies[1].transform.position - bodies[0].transform.position;
    }

    IEnumerator moveSnake() // snake move coroutine
    {
        while (!isDead && !isWin)
        {
            if (direction != Vector3.zero) // waits until player give a direction to the snake
            {
                bool appleEaten = false;
                spawnBody(bodies[0].transform.position + direction, true);
                bodies[0].transform.Find("Sprite Mask").gameObject.SetActive(true);
                bodies[0].transform.Find("Sprites").Find("Body").GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
                bodies[0].transform.Find("Mouth Masks").gameObject.SetActive(true);
                // checks if snake turned right
                Vector3 firstNeighbor = bodies[2].transform.position - bodies[1].transform.position;
                if ((direction == Vector3.right && firstNeighbor == Vector3.down) || (direction == Vector3.down && firstNeighbor == Vector3.left)
                || (direction == Vector3.up && firstNeighbor == Vector3.right) || (direction == Vector3.left && firstNeighbor == Vector3.up))
                {
                    bodies[0].transform.Find("Mouth Masks").Find("Right Mouth Mask").gameObject.SetActive(false);
                }
                // checks if snake turned left
                else if ((direction == Vector3.right && firstNeighbor == Vector3.up) || (direction == Vector3.down && firstNeighbor == Vector3.right)
                || (direction == Vector3.up && firstNeighbor == Vector3.left) || (direction == Vector3.left && firstNeighbor == Vector3.down))
                {
                    bodies[0].transform.Find("Mouth Masks").Find("Left Mouth Mask").gameObject.SetActive(false);
                }
                bodies[1].transform.Find("Sprites").Find("Body").GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
                // checks if apple ate an apple
                if (isHeadOnApple())
                {
                    appleEaten = true;
                }
                firstNeighbor = bodies[bodies.Count-1].transform.position - bodies[bodies.Count-2].transform.position;
                Vector3 secondNeighbor = bodies[bodies.Count-3].transform.position - bodies[bodies.Count-2].transform.position;
                // checks if tail turning right
                if ((secondNeighbor == Vector3.right && firstNeighbor == Vector3.down) || (secondNeighbor == Vector3.down && firstNeighbor == Vector3.left)
                || (secondNeighbor == Vector3.up && firstNeighbor == Vector3.right) || (secondNeighbor == Vector3.left && firstNeighbor == Vector3.up))
                {
                    bodies[bodies.Count-1].transform.Find("Mouth Masks").Find("Left Mouth Mask").gameObject.SetActive(false);
                }
                // checks if tail turning left
                else if ((secondNeighbor == Vector3.right && firstNeighbor == Vector3.up) || (secondNeighbor == Vector3.down && firstNeighbor == Vector3.right)
                || (secondNeighbor == Vector3.up && firstNeighbor == Vector3.left) || (secondNeighbor == Vector3.left && firstNeighbor == Vector3.down))
                {
                    bodies[bodies.Count-1].transform.Find("Mouth Masks").Find("Right Mouth Mask").gameObject.SetActive(false);
                }
                updateSnakeSprites();
                canChangeDir = true;
                if (bodies[1].transform.Find("Sprites").Find("Body").gameObject.GetComponent<BodyPart>().bodyType == "corner")
                {
                    bodies[0].transform.Find("Corner Masks").gameObject.SetActive(true);
                }
                if (!appleEaten)
                {
                    bodies[bodies.Count-1].transform.Find("Sprite Mask").gameObject.SetActive(true);
                    bodies[bodies.Count-1].transform.Find("Sprites").Find("Body").GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
                    bodies[bodies.Count-1].transform.Find("Mouth Masks").gameObject.SetActive(true);
                    bodies[bodies.Count-1].transform.Find("Corner Masks").gameObject.SetActive(true);
                }
                bodies[bodies.Count-2].transform.Find("Sprites").Find("Body").GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.None;
                for (int t = 0; t < 10; t++)
                {
                    mouthAnimation.stayOnHead();
                    bodies[0].transform.Find("Sprites").localPosition += new Vector3(0, 0.1f, 0);
                    if (!appleEaten)
                    {
                        bodies[bodies.Count-1].transform.Find("Sprites").localPosition -= new Vector3(0, 0.1f, 0);
                    }
                    if (t == 4)
                    {
                        bodies[1].transform.Find("Sprites").Find("Body").GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.None;
                        bodies[bodies.Count-2].transform.Find("Sprites").Find("Body").GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
                    }
                    if (snakeSpeed == 500f)
                    {
                        yield return 0;
                    }
                    else
                    {
                        yield return new WaitForSeconds(1f / snakeSpeed);   
                    }
                }
                bodies[0].transform.Find("Sprites").Find("Body").GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.None;
                bodies[0].transform.Find("Sprite Mask").gameObject.SetActive(false);
                bodies[0].transform.Find("Mouth Masks").gameObject.SetActive(false);
                bodies[0].transform.Find("Corner Masks").gameObject.SetActive(false);
                bodies[0].transform.Find("Mouth Masks").Find("Right Mouth Mask").gameObject.SetActive(true);
                bodies[0].transform.Find("Mouth Masks").Find("Left Mouth Mask").gameObject.SetActive(true);
                if (!appleEaten)
                {
                    removeTail();
                }
            }
            else
            {
                yield return 0;
            }
        }
        if (isWin) // if motion stopped by the win condition
        {
            audioController.winSound.Play();
            Debug.Log("Kazandın!");
        }
        else if (isDead) // if motion stopped by a die condition
        {
            Debug.Log("Kaybettin!");
        }
    }

    bool isHeadOnApple() // returns true if snake is about to eat an apple otherwise it returns false
    {
        return(appleSpawner.spawnedApples.Contains(System.Convert.ToInt32(bodies[0].transform.position.x + bodies[0].transform.position.y * gameAreaManager.column)));
    }
}
