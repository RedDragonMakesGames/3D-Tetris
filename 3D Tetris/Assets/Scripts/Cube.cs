using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    public int widthPos;
    public int heightPos;
    public int lengthPos;
    public int colour;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetUpCube(int i, int j, int k, int col)
    {
        widthPos = i;
        heightPos = j;
        lengthPos = k;
        colour = col;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(widthPos, heightPos, lengthPos);
    }
}
