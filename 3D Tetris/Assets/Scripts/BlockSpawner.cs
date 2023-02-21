using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpawner : MonoBehaviour
{
    public GameObject[] blocks;
    public Vector3 spawnPos = new Vector3(4, 10, 4);
    public float tickTime = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        FallingBlock currentBlock = FindObjectOfType<FallingBlock>();
        if (currentBlock == null) 
        {
            GameObject block = Instantiate(blocks[Random.Range(0, blocks.Length)], spawnPos, Quaternion.identity);
            block.GetComponent<FallingBlock>().tickTime = tickTime;
        }
    }
}
