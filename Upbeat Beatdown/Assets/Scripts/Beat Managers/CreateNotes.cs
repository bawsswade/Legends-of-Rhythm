using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Beatz;
public class CreateNotes : MonoBehaviour {
    public SongSO SongData;
    public AudioSource source;
    public int bpm;

    SongManager sm;

	// Use this for initialization
	void Start ()
    {
        sm = GetComponent<SongManager>();
        SongData.notes.OrderBy(note => note.x).ToList();
        InvokeRepeating("BpmUpdate", 0, 60/bpm);
	}

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            Vector3 note = new Vector3(sm.GetClosestBeat(), 1, 0);
            SongData.notes.Add(note);
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            Vector3 note = new Vector3(sm.GetClosestBeat(), 2, 0);
            SongData.notes.Add(note);
        }
    }

    private void BpmUpdate()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            Vector3 note = new Vector3(source.time, 1, 0);
            SongData.notes.Add(note);
            Debug.Log("hello");
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            Vector3 note = new Vector3(source.time, 2,0 );
            SongData.notes.Add(note);
        }
    }
}
