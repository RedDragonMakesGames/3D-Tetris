using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingBlock : MonoBehaviour
{
    private Cube[] cubes;
    public float tickTime = 1.0f;

    private float timeSinceLastTick;
    private GameplayManager manager;
    public int colourIndex;

    public bool canRotate = false;
    public Vector2Int rotationAxis = Vector2Int.zero;

    // Start is called before the first frame update
    void Start()
    {
        manager = FindObjectOfType<GameplayManager>();
        cubes = GetComponentsInChildren<Cube>();
        timeSinceLastTick = Time.time;
        foreach (Cube cube in cubes)
        {
            cube.GetComponent<Renderer>().material = manager.colours[colourIndex];
            cube.colour = colourIndex;
        }
    }

    // Update is called once per frame
    void Update()
    {
        MoveBlock();
        if (canRotate)
            RotateBlock();

        if (Time.time > timeSinceLastTick + tickTime || Input.GetKeyDown(KeyCode.Space))
        {
            timeSinceLastTick = Time.time;
            bool willStop = false;
            foreach(Cube cube in cubes)
            {
                if (manager.CheckIfCubeBelow(cube.widthPos, cube.heightPos, cube.lengthPos))
                {
                    willStop = true;
                }
            }
            if(willStop)
            {
                bool haveLost = false;
                foreach(Cube cube in cubes)
                {
                    if (manager.SetCube(cube.widthPos, cube.heightPos, cube.lengthPos, cube.colour))
                        haveLost = true;
                }
                if (!haveLost)              //If we've lost, leave the block here to show it
                    Destroy(gameObject);
            }
            else
            {
                foreach (Cube cube in cubes)
                {
                    cube.heightPos -= 1;
                }
            }
        }
    }

    private void RotateBlock()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            bool canMove = true;
            foreach (Cube cube in cubes)
            {
                if (new Vector2Int(cube.widthPos, cube.lengthPos) != rotationAxis)
                {
                    int xdif = cube.widthPos - rotationAxis.x;
                    int ydif = cube.lengthPos - rotationAxis.y;
                    Vector2Int beforeRot = new Vector2Int(xdif, ydif);
                    Vector2Int afterRot = new Vector2Int(-ydif, xdif);
                    Vector2Int added = rotationAxis + afterRot;
                    if (!manager.CheckifSpotValid(added.x, cube.heightPos, added.y))
                        canMove = false;
                }
            }
            if (canMove)
            {
                foreach (Cube cube in cubes)
                {
                    if (new Vector2Int(cube.widthPos, cube.lengthPos) != rotationAxis)
                    {
                        int xdif = cube.widthPos - rotationAxis.x;
                        int ydif = cube.lengthPos - rotationAxis.y;
                        Vector2Int beforeRot = new Vector2Int(xdif, ydif);
                        Vector2Int afterRot = new Vector2Int(-ydif, xdif);
                        Vector2Int added = rotationAxis + afterRot;
                        cube.widthPos = added.x;
                        cube.lengthPos = added.y;
                    }
                }
            }
        }
    }

    private void MoveBlock()
    {
        //Move block
        if (Input.GetKeyDown(KeyCode.W))
        {
            bool canMove = true;
            foreach (Cube cube in cubes)
            {
                if (!manager.CheckifSpotValid(cube.widthPos - 1, cube.heightPos, cube.lengthPos))
                    canMove = false;
            }
            if (canMove)
            {
                foreach (Cube cube in cubes)
                {
                    cube.widthPos -= 1;
                }
                rotationAxis.x -= 1;
            }
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            bool canMove = true;
            foreach (Cube cube in cubes)
            {
                if (!manager.CheckifSpotValid(cube.widthPos, cube.heightPos, cube.lengthPos + 1))
                    canMove = false;
            }
            if (canMove)
            {
                foreach (Cube cube in cubes)
                {
                    cube.lengthPos += 1;
                }
                rotationAxis.y += 1;
            }
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            bool canMove = true;
            foreach (Cube cube in cubes)
            {
                if (!manager.CheckifSpotValid(cube.widthPos + 1, cube.heightPos, cube.lengthPos))
                    canMove = false;
            }
            if (canMove)
            {
                foreach (Cube cube in cubes)
                {
                    cube.widthPos += 1;
                }
                rotationAxis.x += 1;
            }
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            bool canMove = true;
            foreach (Cube cube in cubes)
            {
                if (!manager.CheckifSpotValid(cube.widthPos, cube.heightPos, cube.lengthPos - 1))
                    canMove = false;
            }
            if (canMove)
            {
                foreach (Cube cube in cubes)
                {
                    cube.lengthPos -= 1;
                }
                rotationAxis.y -= 1;
            }
        }
    }
}
