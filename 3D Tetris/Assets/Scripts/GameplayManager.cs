using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    [SerializeField] GameObject cubePrefab;
    [SerializeField] GameObject[,,] mCubes;
    [SerializeField] public Material[] colours;
    [SerializeField] int mGameWidth = 8;
    [SerializeField] int mGameHeight = 10; 
    [SerializeField] int mGameLenght = 8;
    [SerializeField] int[,,] mCubeMap = null;   //-1 for empty, otherwise the colour of the cube
    [SerializeField] int score = 0;
    [SerializeField] int scoreForLayer = 5;

    private int layerMultiplier = 1; //Keeps track of how many layers are cleared at once
    private bool hasEnded = false;
    private BlockSpawner spawner;

    // Start is called before the first frame update
    void Start()
    {
        spawner = FindObjectOfType<BlockSpawner>();
        mCubeMap = new int[mGameWidth, mGameHeight, mGameLenght];
        mCubes = new GameObject[mGameWidth, mGameHeight, mGameLenght];
        for (int i = 0; i < mGameWidth; i++)
        {
            for (int j = 0; j < mGameHeight; j++)
            {
                for (int k = 0; k < mGameLenght; k++)
                {
                    mCubes[i,j,k] = Instantiate(cubePrefab, new Vector3(i,j,k), Quaternion.identity);
                    mCubeMap[i, j, k] = -1;
                    mCubes[i, j, k].SetActive(false);
                }
            }
        }
    }

    void Restart()
    {
        score = 0;
        hasEnded = false;
        for (int i = 0; i < mGameWidth; i++)
        {
            for (int j = 0; j < mGameHeight; j++)
            {
                for (int k = 0; k < mGameLenght; k++)
                {
                    mCubeMap[i, j, k] = -1;
                    mCubes[i, j, k].SetActive(false);
                }
            }
        }
        FallingBlock currentBlock = FindObjectOfType<FallingBlock>();   //Clear out the block that triggered the end
        if (currentBlock != null)
        {
            Destroy(currentBlock.gameObject);
        }
        spawner.gameObject.SetActive(true);    //Restart spawning
        FindObjectOfType<TextMeshProUGUI>().text = "Score: 0";
    }

    private void FixedUpdate()
    {
        if (hasEnded)
            CheckForRestart();

        //SetRandomCube();  //Testing only
        layerMultiplier = 1;

        for (int j = 0; j < mGameHeight; j++)
        {
            bool isLayerComplete = true;
            for (int i = 0; i < mGameWidth; i++)
            {
                for (int k = 0; k < mGameLenght; k++)
                {
                    if (mCubeMap[i, j, k] != -1)
                    {
                        mCubes[i, j, k].SetActive(true);
                        mCubes[i, j, k].GetComponent<Renderer>().material = colours[mCubeMap[i, j, k]];
                    }
                    else
                    {
                        mCubes[i, j, k].SetActive(false);
                        isLayerComplete = false;
                    }
                }
            }
            if (isLayerComplete)
            {
                ClearLayer(j);
                j--;    //Redo the layer to show it correctly
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void CheckForRestart()
    {
        if (Input.GetKeyDown(KeyCode.R)) 
        {
            Restart();
        }
    }

    private void SetRandomCube()
    {
        //Testing only
        SetCube(Random.Range(0,mGameWidth),Random.Range(0,mGameHeight),Random.Range(0,mGameLenght), Random.Range(0, colours.Length));
    }

    public bool SetCube(int i, int j, int k, int colour)
    {
        if(j >= mGameHeight - 1)
        {
            if (!hasEnded)
            {
                Debug.Log("You lose!");
                FindObjectOfType<TextMeshProUGUI>().text = "Final score: " + score.ToString() + ", \n Press R to restart ";
                spawner.gameObject.SetActive(false);
                FallingBlock currentBlock = FindObjectOfType<FallingBlock>();
                if (currentBlock != null)
                {
                    currentBlock.enabled = false;
                }
                hasEnded = true;
            }
            return true;
        }
        else
        {
            mCubeMap[i, j, k] = colour;
            return false;
        }
    }

    public bool CheckIfCubeBelow(int i, int j, int k)
    {
        if (j == 0) 
            return true;
        else if (j >= mGameHeight) 
            return false;
        else if (mCubeMap[i, j - 1, k] != -1)
            return true;
        else
            return false;
    }

    public bool CheckifSpotValid(int i, int j, int k)
    {
        if (i < 0 || i >= mGameWidth) 
            return false;
        if (k < 0 || k >= mGameLenght)
            return false;
        if (i >= 0 && j >= 0 && k >= 0 && i < mGameWidth && j < mGameHeight && k < mGameLenght)    //Make sure we're in bounds
        {
            if (mCubeMap[i, j, k] != -1)
                return false;
        }
        return true;
    }

    private void ClearLayer(int layer)
    {
        score += scoreForLayer * layerMultiplier;
        layerMultiplier++;
        FindObjectOfType<TextMeshProUGUI>().text = "Score: " + score.ToString() + " ";
        for (int j = layer; j < mGameHeight - 1; j++)
        {
            for (int i = 0; i < mGameWidth; i++)
            {
                for (int k = 0; k < mGameLenght; k++)
                {
                    mCubeMap[i,j,k] = mCubeMap[i, j + 1, k];
                }
            }
        }
        for (int i = 0; i < mGameWidth; i++)
        {
            for (int k = 0; k < mGameLenght; k++)
            {
                mCubeMap[i, mGameHeight - 1, k] = -1;
            }
        }
    }
}
