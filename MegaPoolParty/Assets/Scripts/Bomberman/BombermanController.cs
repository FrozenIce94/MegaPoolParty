using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombermanController : MonoBehaviour
{

    public bool[,] chosenBlocks = new bool[7, 7];
    private int[] xValues = new int[13];
    private int[] yValues = new int[7];

    public int minBlockCountPerSide = 3;
    public int maxBlockCountPerSide = 5;

    public GameObject blockPrefab;


    private System.Random rnd = new System.Random();
    // Start is called before the first frame update
    void Start()
    {
        //Init Values
        int xMin = -72;
        for (int i = 0; i < 13; i++)
        {
            xValues[i] = xMin;
            xMin += 12;
        }

        int yMin = -39;
        for (int i = 0; i < 7; i++)
        {
            yValues[i] = yMin;
            yMin += 13;
        }

        PlaceBlocks();

    }

    private void PlaceBlocks()
    {
        var anzahlBloecke = rnd.Next(minBlockCountPerSide, maxBlockCountPerSide);
        for (int i = 1; i <= anzahlBloecke; i++)
        {
            var chosen = false;
            while(!chosen)
            {
                var posX = rnd.Next(0, 7);
                var posY = rnd.Next(0, 7);
                //if posX/Y is not starting position and is free, take it
                if(!chosenBlocks[posX, posY] && !(posX == 0 && posY == 0))
                {
                    //check for diagonal availability
                    if (posX + 1 < 7   && posY + 1 <  7 && chosenBlocks[posX + 1, posY + 1]) continue;
                    if (posX + 1 < 7   && posY - 1 >= 0 && chosenBlocks[posX + 1, posY - 1]) continue;
                    if (posX - 1 >= 0  && posY + 1 <  7 && chosenBlocks[posX - 1, posY + 1]) continue;
                    if (posX - 1 >= 0  && posY - 1 >= 0 && chosenBlocks[posX - 1, posY - 1]) continue;

                    chosenBlocks[posX, posY] = true;
                    //Block on Left Side
                    Instantiate(blockPrefab, 
                        new Vector3(xValues[posX], 0, yValues[posY]), 
                        Quaternion.Euler(Vector3.zero));

                    //Mirror Block on right side, when it's x <>  7 (middle point)
                    if(posX != 6)
                    { 
                        Instantiate(blockPrefab,
                                    new Vector3(xValues[xValues.Length - 1 - posX], 0, yValues[yValues.Length - 1 - posY]),
                                    Quaternion.Euler(Vector3.zero));
                    }

                    chosen = true;
                }
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
