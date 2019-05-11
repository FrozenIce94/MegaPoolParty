using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Net.Mime.MediaTypeNames;

public class Hub : MonoBehaviour
{

    public Rigidbody player;
    public GameManager gm;

    private Dictionary<int, Vector3> positions = new Dictionary<int, Vector3>();

    // Start is called before the first frame update
    void Start()
    {
        positions.Add(1, new Vector3(-37.6f, -12.8f, 19f));
        positions.Add(2, new Vector3(-13.5f, -12.8f, 10.2f));
        positions.Add(3, new Vector3(0f, -12.8f, 5.8f));
        positions.Add(4, new Vector3(13.5f, -12.8f, 0f));
        positions.Add(5, new Vector3(26.63f, -12.8f, -4.19f));
        positions.Add(6, new Vector3(40.32f, -12.8f, -8.33f));
        positions.Add(7, new Vector3(53.97f, -12.8f, -12.83f));

        InitHub(gm.GetPositions());
    }

    private void Update()
    {
        if(Input.GetButtonDown("Fire1_S") || Input.GetButtonDown("Fire1_L"))
        {
            NextGame();
        }
    }

    public void InitHub(KeyValuePair<int, int> pos)
    {
        player.position = positions[pos.Key];
        player.MovePosition(positions[pos.Value]);
    }

    public void NextGame()
    {
        gm.EndHub();
    }

}
