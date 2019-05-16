using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Net.Mime.MediaTypeNames;

public class Hub : MonoBehaviour
{

    public GameObject player;
    public GameObject playerModel;
    public GameManager gm;
    public ParticleSystem ps;

    private Dictionary<int, Vector3> positions = new Dictionary<int, Vector3>();

    public UnityEngine.UI.Text nextGameLabel;
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
        
        if (GameManager.isFirstGame)
        {
            //erst das äußere Model zentrieren
            player.transform.position = positions[pos.Value];
            //dann das innere
            playerModel.transform.localPosition = Vector3.zero;
            playerModel.GetComponent<Animator>().enabled = false;
            GameManager.isFirstGame = false;
            nextGameLabel.text = "Spiel starten";
        }
        else
        {
            player.transform.position = positions[pos.Key];
            nextGameLabel.text = "Nächstes Spiel";
            if (positions[pos.Key] == positions[pos.Value]) return;
            StartCoroutine(FallDown(positions[pos.Value]));
        }
    }

    private IEnumerator FallDown(Vector3 newPosition)
    {
        yield return new WaitForSeconds(1f);
        player.transform.position = newPosition;
        ps.Play();
    }

    public void NextGame()
    {
        gm.EndHub();
    }

}
