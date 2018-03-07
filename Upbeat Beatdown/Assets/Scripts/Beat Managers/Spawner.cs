using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Beatz;

public class Spawner : MonoBehaviour {

    public GameObject cube;
    public GameObject sphere;
    public SongManager sm;
    public AudioSource source;

    private bool hasSpawned = false;

    private void Start()
    {
        sm.OnStartGame += StartGame;
    }

    private void StartGame()
    {
        InvokeRepeating("Increment", 0, 60f / SongManager.bpm);
        //Debug.Log("starting spawner");
    }

    private void Increment()
    {
        if (sm.GetShouldSpawnNote(NoteType.TLeft))
        {
            Vector3 pos = new Vector3(Random.Range(-10,10), Random.Range(0, 5), Random.Range(-10, 10));
            Instantiate(cube, pos, Quaternion.identity);
        }
        if (sm.GetShouldSpawnNote(NoteType.TRgiht))
        {
            Vector3 pos = new Vector3(Random.Range(-10, 10), Random.Range(0, 5), Random.Range(-10, 10));
            Instantiate(sphere, pos, Quaternion.identity);
        }
    }

    private void Spawn()
    {
        // instantiate

        // set behavior
    }
}
