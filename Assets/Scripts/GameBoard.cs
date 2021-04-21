using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameBoard : MonoBehaviour
{
    public static Transform[,] gameBoard = new Transform[10, 20];

    public static bool DeleteAllFullRows()
    {
        for(int row = 0; row < 20; ++row) //Cycles through all the rows
        {
            if(IsRowFull(row)) //Checks for rows that are full
            {
                DeleteGBRow(row); //Deletes any full rows

                SoundManager.Instance.PlayOneShot(SoundManager.Instance.tetrisClearSound); //Sound Effect

                return true; //Returns a true if the row was full
            }
        }

        return false; //Returns a false if the row wasn't full
    }

    //Checks if a row is full
    public static bool IsRowFull(int row)
    {
        for(int colColumn = 0; colColumn < 10; ++colColumn) //Cycles through each column in the row
        {
            //Returns false if any nulls are found
            if (gameBoard[colColumn, row] == null)
            {
                return false;
            }
        }

        return true;
    }

    //Deletes full rows on the GameBoard and in the array
    public static void DeleteGBRow(int row)
    {
        for(int colColumn = 0; colColumn < 10; ++colColumn)
        {
            Destroy(gameBoard[colColumn, row].gameObject); //Destroys each cube of the shapes in the scene
            gameBoard[colColumn, row] = null; //Destroys the cubes in the array
        }

        row++; //Increments a row up to move above cubes down

        for(int j = row; j <20; ++j) //Cycles through all the rows
        {
            for(int colColumn = 0; colColumn < 10; ++colColumn) //Cycles through all the columns
            {
                if (gameBoard[colColumn, j] != null) //Checks if there is a cube in the cell
                {
                    gameBoard[colColumn, j - 1] = gameBoard[colColumn, j]; //Moves cubes above down

                    gameBoard[colColumn, j] = null; //Delete the cube that was moved down

                    gameBoard[colColumn, j - 1].position += new Vector3(0, -1, 0); //Moves the cube in the scene as well
                }
            }
        }
    }
}
