using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeSpawner : MonoBehaviour
{
    public GameObject[] shapes; //Array with shapes

    public GameObject[] nextShapes; //Array with predicted shapes

    private GameObject upNextObject = null; //Shows the next shape under the "Next" text box (GameObject Reference)

    public int shapeIndex = 0;
    public int nextShapeIndex = 0;

    public void SpawnShape()
    {
        //Generate a random index of shapes, from the selection i have provided, in this case 7 shapes.
        int shapeIndex = nextShapeIndex;

        //Creates the shape from the index, in the spawn location on the GameBoard
        Instantiate(shapes[shapeIndex],
            transform.position,
            Quaternion.identity);

        nextShapeIndex = Random.Range(0, 6);

        Vector3 nextShapePos = new Vector3(-8, 14, 1); //The position of the "next shape" game object

        if (upNextObject != null) //Destroys the next shape object if it already exists
        {
            Destroy(upNextObject);
        }

        //Gets the next shape and displays it on the gameboard
        upNextObject = Instantiate(nextShapes[nextShapeIndex],
            nextShapePos,
            Quaternion.identity);
    }


    // Initializing shape spawn
    void Start()
    {
        nextShapeIndex = Random.Range(0, 6);


        SpawnShape();
    }
}
