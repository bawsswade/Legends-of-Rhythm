using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongData : MonoBehaviour {

    public List<float> vocalNotes = new List<float>();
    public List<float> bassNotes = new List<float>();
    public List<float> regNotes = new List<float>();

    BeatManagerMediator beatMan;

    private void Start()
    {
        beatMan = GameObject.FindObjectOfType<BeatManagerMediator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            beatMan = GameObject.FindObjectOfType<BeatManagerMediator>();
            regNotes.Add(beatMan.SaveBeat());
            //bassNotes.Add(beatMan.SaveBeat());

        }
    }
}
