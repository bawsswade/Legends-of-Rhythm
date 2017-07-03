using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using strange.extensions.mediation.impl;

public class BeatManagerView : View {
    // song stuff
    public float bpm;
    
    public GameObject p_beat;
    public GameObject melodyNote;
    public GameObject bassNote;

    public GameObject player;
    public AudioSource audio;

    public SongSO songBeats;
}
