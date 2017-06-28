using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;

public class CreateHits : MonoBehaviour {

    public SongSO SOtoWriteTo;
    public beatsManager beatMan;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //SOtoWriteTo.bassNotes.Add(beatMan.SaveBeat());
            SOtoWriteTo.regNotes.Add(beatMan.SaveBeat());
            //SOtoWriteTo.vocalNotes.Add(beatMan.SaveBeat());
        }
	}
}
