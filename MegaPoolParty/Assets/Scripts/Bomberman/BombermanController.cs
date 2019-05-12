using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

public class BombermanController : MonoBehaviour
{

    private bool[,] chosenBlocks = new bool[7, 7];
    private int[] xValues = new int[13];
    private int[] yValues = new int[7];

    private List<Vector3> vectorList = new List<Vector3>();
    private Vector3 standardSize = new Vector3(12, 100, 12);


    public bool IsFinished = false;
    public bool generateRandomLevel = false;
    public TextAsset levelGenFile;

    bool endRequested = false;
    public bool timerRegistered = false;
    public void requestEnd()
    {
        endRequested = true;
    }

    public List<Vector3> VectorList
    {
        get
        {
            for (int x = 0; x < xValues.Length; x++)
            {
                for (int y = 0; y < yValues.Length; y++)
                {
                    vectorList.Add(new Vector3(xValues[x], 100, yValues[y]));
                }
            }
            return vectorList;
        }
    }




    public int minBlockCountPerSide = 3;
    public int maxBlockCountPerSide = 5;

    public GameObject blockPrefab;


    private List<GameObject> blocks = new List<GameObject>();

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

        if(generateRandomLevel)
        { PlaceBlocksRandom(); }
        else
        { PlaceBlocksWithFile();  }
    }

    private void PlaceBlocksWithFile()
    {
        var lineArray = Regex.Split(levelGenFile.text, "\r\n|\r|\n"); 
        var gamesCount = lineArray.Length / 8;
        var chosenLevel = rnd.Next(0, gamesCount);
        Debug.Log($"Ich habe mich für Level {chosenLevel} entschieden.");
        for (int lineIndex = chosenLevel * 8; lineIndex < chosenLevel * 8 + 7; lineIndex++)
        {
            var line = lineArray[lineIndex];
            var x = lineIndex - chosenLevel * 8;
            for (int y = 0; y < 7; y++)
            {
                if(line[y] == '1')
                {
                    AddBlock(x, y);
                }
            }

        }
    }

    //Baut zufälligerweise die Blöcke
    private void PlaceBlocksRandom()
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



                    AddBlock(posX, posY);

                    chosen = true;
                }
            }
        }

    }

    void AddBlock(int posX, int posY)
    {
        chosenBlocks[posX, posY] = true;
        //Block on Left Side
        blocks.Add(Instantiate(blockPrefab,
            new Vector3(xValues[posX], 0, yValues[posY]),
            Quaternion.Euler(Vector3.zero),transform));

        //Mirror Block on right side, when it's x <>  7 (middle point)
        if (posX != 6)
        {
            blocks.Add(Instantiate(blockPrefab,
                    new Vector3(xValues[xValues.Length - 1 - posX], 0, yValues[yValues.Length - 1 - posY]),
                    Quaternion.Euler(Vector3.zero), transform));
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (!timerRegistered)
        {
            timerRegistered = GetComponent<GameManager>().StartTimer(requestEnd, GameManager.Games.Swimming);
        }
        if (!timerRegistered)
            return;

        if (Input.GetKeyDown(KeyCode.C) && generateRandomLevel)
        {
            foreach (var block in blocks.ToList())
            {
                Destroy(block);
                blocks.Remove(block);
            }
            for (int x = 0; x < 7; x++)
            {
                for (int y = 0; y < 7; y++)
                {
                    chosenBlocks[x, y] = false;
                }
            }
            if(generateRandomLevel)
            { PlaceBlocksRandom(); } else { PlaceBlocksWithFile(); }
                
        } 
        if(Input.GetKeyDown(KeyCode.F) && generateRandomLevel)
        {
            var sb = new StringBuilder();
            for (int x = 0; x < 7; x++)
            {
                for (int y = 0; y < 7; y++)
                {
                    sb.Append((chosenBlocks[x,y] ? "1" : "0"));
                }
                sb.AppendLine();
            }
            var fileName = "lvl.gencmd";
            if (File.Exists(fileName)) File.AppendAllText(fileName, Environment.NewLine);
            File.AppendAllText(fileName, sb.ToString());
        }

        if (endRequested)
        {
            IsFinished = true;
            GetComponent<GameManager>().StopTimer();
            GetComponent<GameManager>().EndMinigame(null);
        }
    }

    internal Vector3 GetNearestPositionInGrid(Vector3 position)
    {
        var posBounds = new Bounds(position, standardSize);
        var intersects = from vector in VectorList
                         let vBounds = new Bounds(vector, standardSize)
                         let delta = Vector3.Distance(position, vector)
                         where vBounds.Intersects(posBounds)
                         select new { Delta = delta, Vector = vector };
        var result = intersects.FirstOrDefault((v) => v.Delta == intersects.Min((i) => i.Delta)).Vector;
        return result;
    }
}
