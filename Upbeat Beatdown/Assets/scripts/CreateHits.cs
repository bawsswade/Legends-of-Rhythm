using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;

public class CreateHits : MonoBehaviour {

    public SongSO SOtoWriteTo;
    //public beatsManager beatMan;
    BeatManagerMediator beatMan;

	// Use this for initialization
	void Start () {
        beatMan = GameObject.FindObjectOfType<BeatManagerMediator>();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            beatMan = GameObject.FindObjectOfType<BeatManagerMediator>();
            //SOtoWriteTo.AddBassNote(beatMan.SaveBeat());
            //SOtoWriteTo.AddMelodyNote(beatMan.SaveBeat());
            SOtoWriteTo.vocalNotes.Add(beatMan.SaveBeat());
            
        }
	}
}
