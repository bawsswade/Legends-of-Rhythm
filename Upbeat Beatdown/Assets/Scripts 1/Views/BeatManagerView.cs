using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using strange.extensions.mediation.impl;

public class BeatManagerView : View {
    // song stuff
    public float bpm;
    
    public GameObject p_beat;       // particles for metronome

    public GameObject snareNote;    // metronome immediate particles
    public GameObject melodyNote;   // metrenome melody attack
    public GameObject bassNote;   

    public GameObject allMelodyNotes;
    public GameObject allBassNotes;
    public GameObject allSnareNotes;

    public SongSO NoteData;
}
