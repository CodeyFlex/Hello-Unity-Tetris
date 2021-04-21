using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Shape : MonoBehaviour
{
    public static double difficultySpeed = 1; //Drop speed that is increased with time currently

    public float lastMoveDown = 0; //Tracks the last time a shape was moved down

    public static double dropSpeed = 0; //I was trying to use this for a faster drop with space bar, but will abandon it for now.
    
    // Start is called before the first frame update
    void Start()
    {

        if (!IsInGrid()) //If a shape starts outside of the grid, game will end
        {
            BGMusicManager.Instance.PlayOneShot(BGMusicManager.Instance.gameOverSound); //Sound Effect

            Invoke("OpenGameOverScene", .5f); //Starts game over scene
        }

        InvokeRepeating("IncreaseSpeed", 5.0f, 5.0f); //Increases the drop speed repeatedly
        
    }

    //Changes to game over scene
    void OpenGameOverScene() 
    {
        Destroy(gameObject);
        SceneManager.LoadScene("GameOver");
    }

    //Increases drop speed on difficulty variable
    void IncreaseSpeed()
    {
        if (Shape.difficultySpeed >= 0.15) //To avoid the drop speed becoming too fast
        {
            Shape.difficultySpeed -= .005;
        }
        
        Debug.Log("Difficulty Speed: " + Shape.difficultySpeed); //Tracking drop speed (debug)

    }
    

    // Update is called once per frame
    void Update()
    {

        //Input to move tetris piece to the left
        if (Input.GetKeyDown("a") || Input.GetKeyDown("left"))
        {
            transform.position += new Vector3(-1, 0, 0); //Moves the object left once on the x axis

            Debug.Log(transform.position);

            if (!IsInGrid())
            {
                transform.position += new Vector3(1, 0, 0); //Reverts the move action
            } else
            {
                UpdateGameBoard();

                //Commented out until i find a more pleasing sound
                //SoundManager.Instance.PlayOneShot(SoundManager.Instance.shapeMoveSound); //Sound Effect
            }
        }

        //Input to move tetris piece to the right
        if (Input.GetKeyDown("d") || Input.GetKeyDown("right"))
        {
            transform.position += new Vector3(1, 0, 0); //Moves the object right once on the x axis

            Debug.Log(transform.position);

            if (!IsInGrid())
            {
                transform.position += new Vector3(-1, 0, 0); //Reverts the move action
            }
            else
            {
                UpdateGameBoard();

                //Commented out until i find a more pleasing sound
                //SoundManager.Instance.PlayOneShot(SoundManager.Instance.shapeMoveSound); //Sound Effect
            }
        }

        // Input to go down, and also automatic game drop
        if (Input.GetKeyDown("s") || (Input.GetKeyDown("down")) || Time.time - lastMoveDown >= Shape.difficultySpeed)
        {
            transform.position += new Vector3(0, -1, 0); //Moves the object down once on the y axis

            //Debug.Log(transform.position);

            if (!IsInGrid())
            {
                transform.position += new Vector3(0, 1, 0); //Reverts the move action

                bool rowDeleted = GameBoard.DeleteAllFullRows();

                if (rowDeleted)
                {
                    GameBoard.DeleteAllFullRows();

                    IncreaseTextUIScore();
                }

                enabled = false;

                FindObjectOfType<ShapeSpawner>().SpawnShape();

                SoundManager.Instance.PlayOneShot(SoundManager.Instance.shapeLandSound); //Sound Effect

            }
            else
            {
                UpdateGameBoard();
            }

            lastMoveDown = Time.time;
        }

        //Input to rotate tetris piece to the left
        if (Input.GetKeyDown("w") || Input.GetKeyDown("up"))
        {
            transform.Rotate(0, 0, 90);

            Debug.Log(transform.position);

            if (!IsInGrid())
            {
                transform.Rotate(0, 0, -90); //Reverts the rotate action
            }
            else
            {
                UpdateGameBoard();

                SoundManager.Instance.PlayOneShot(SoundManager.Instance.rotateLeftSound); //Sound Effect
            }
        }

        //Input to rotate tetris piece to the right
        if (Input.GetKeyDown("e"))
        {
            transform.Rotate(0, 0, -90);

            Debug.Log(transform.position);

            if (!IsInGrid())
            {
                transform.Rotate(0, 0, +90); //Reverts the rotate action
            }
            else
            {
                UpdateGameBoard();

                SoundManager.Instance.PlayOneShot(SoundManager.Instance.rotateRightSound); //Sound Effect
            }
        }

        //Drops the shape almost immediately currently
        if (Input.GetKey("space"))
        {
            transform.position += new Vector3(0, -1, 0);

            if (!IsInGrid())
            {
                transform.position += new Vector3(0, 1, 0);
            }
            else
            {
                UpdateGameBoard();
            }
        }

        /* //Speeds up the shape drop
        if (Input.GetKey("space"))
        {
            Shape.dropSpeed = 0.7;

            Debug.Log("Drop Speed: " + Shape.dropSpeed);
        }else
        {
            Shape.dropSpeed = 0;
        }
        */
    }

    public bool IsInGrid()
    {
        foreach (Transform childBlock in transform)
        {
            Vector2 vect = RoundVector(childBlock.position); //Rounding for movement accuracy

            if (!IsInBorder(vect))
            {
                return false;
            }

            //Checks if empty cells are available for the shape
            if(GameBoard.gameBoard[(int)vect.x, (int)vect.y] != null &&
                GameBoard.gameBoard[(int)vect.x, (int)vect.y].parent != transform)
            {
                return false;
            }
        }
        return true;
    }

    public Vector2 RoundVector(Vector2 vect) //Rounding for movement accuracy
    {
        return new Vector2(Mathf.Round(vect.x), Mathf.Round(vect.y)); //Rounding for movement accuracy
    }

    public static bool IsInBorder(Vector2 pos)
    {
        //Set in stone to work with rounding of game positions
        return ((int)pos.x >= 0 && //Right Wall
                (int)pos.x <= 9 && //Left Wall
                (int)pos.y >= 0); //Bottom Wall
    }

    //Method to update the board anytime a new piece is placed or removed
    public void UpdateGameBoard()
    {
        for(int y = 0; y < 20; ++y) //Goes through the board on the y axis
        {
            for(int x = 0; x < 10; ++x) //Goes through the board on the x axis
            {
                //If the 1st isn't null then a cube is in that position, so we will check if the shape passed in is already there
                if(GameBoard.gameBoard[x, y] != null &&
                    GameBoard.gameBoard[x, y].parent == transform)
                {
                    GameBoard.gameBoard[x, y] = null; //If a shape moves down it will be removed from the gameboard array
                }
            }
        }

        //Iterates all the spaces in the gameboard
        //And adds the new cubes from the shape
        foreach(Transform childBlock in transform)
        {
            Vector2 vect = RoundVector(childBlock.position); //Rounding for movement accuracy

            GameBoard.gameBoard[(int)vect.x, (int)vect.y] = childBlock; //Puts cubes in gameboard array
        }
    }

    //Updates the scoreboard anytime the player clears
    void IncreaseTextUIScore()
    {
        var textUIComp = GameObject.Find("Score").GetComponent<Text>(); //Finds score text

        int score = int.Parse(textUIComp.text);

        score++; //adds +1 to score

        textUIComp.text = score.ToString(); //Changes score to new value
    }

}
