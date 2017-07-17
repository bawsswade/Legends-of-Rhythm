using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using strange.extensions.mediation.impl;

public class BeatManagerView : View {
    // song stuff
    public float bpm;
    
    public GameObject p_beat;       // particles for metronome

    public GameObject beatExact;    // metronome immediate particles
    public GameObject melodyNote;   // metrenome melody notes
    public GameObject bassNote;     // metrenome bass notes

    public SongSO NoteData;
}
