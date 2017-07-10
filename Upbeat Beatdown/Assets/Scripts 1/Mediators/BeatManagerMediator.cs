using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;
using System;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class BeatManagerMediator : Mediator {

    [Inject] public BeatManagerView View { get; set; }
    
    // boss attack signals
    [Inject] public OnBassAttackSignal BassAtkSignal { get; set; }
    [Inject] public OnMelodyAttackSignal MelodyAtkSignal { get; set; }
    [Inject] public OnInstantAttackSignal InstantAtkSignal { get; set; }
    // set on beat bool for player
    //[Inject] public OnBassBeat BassBeatSignal { get; set; }
    //[Inject] public OnMelodyBeat MelodyBeatSignal { get; set; }

    private float secPerBeat;

    // tracking player hits
    float curBeatTime = 0;
    float lastBeat = 0;
    float hitPadding = .15f;            // deterimes how accurate you have to be
    bool hasHitBeat = false;

    // boss attacks
    int songBeatIndex_r = 0;        // melody
    int songBeatIndex_b = 0;
    int songBeatIndex_v = 0;
    bool hasSpawnedAttack_v = false;
    bool hasSpawnedAttack_r = false;
    bool hasSpawnedAttack_b = false;

    // spawn notes to hit
    int songMelodyIndex = 0;
    int songBassIndex = 0;
    int songVocalIndex = 0;
    public List<float> melodyNotes = new List<float>();
    public List<float> bassNotes = new List<float>();
    public List<float> vocalNotes = new List<float>();

    public override void OnRegister()
    {

	}

    // Use this for initialization
    void Start()
    {
        // get time in seconds to spawn
        secPerBeat = 60f / View.bpm;
        InvokeRepeating("SpawnBeat", secPerBeat, secPerBeat);
        //audio.Pause();

        foreach (float f in View.songBeats.regNotes)
        {
            melodyNotes.Add(f-((60/View.bpm)*7));           // seconds per beat * 7 beats off = time difference
        }
        foreach (float f in View.songBeats.bassNotes)
        {
            bassNotes.Add(f - ((60 / View.bpm) * 7));           // seconds per beat * 7 beats off = time difference
        }
    }

    // Update is called once per frame
    void Update()
    {
        secPerBeat = 60f / View.bpm;

        //CheckIsOnBeat();
    }


    void SpawnBeat()
    {
        //Debug.Log(Mathf.Abs(melodyNotes[songMelodyIndex] - View.audio.time));
        // spawn notes to match metrenome
        /*if (melodyNotes.Count != 0 && Mathf.Abs(melodyNotes[songMelodyIndex] - View.audio.time) < .1f)      // check if to spawn this beat or next beat
        {
            // spawn melody beat
            GameObject b = Instantiate(View.melodyNote, View.player.transform) as GameObject;
            b.transform.localPosition = new Vector3(0, -1, 0);
            Destroy(b, 5f);
        }
        if (bassNotes.Count != 0 && Mathf.Abs(bassNotes[songBassIndex] - View.audio.time) < .1f)
        {
            // spawn melody beat
            GameObject b = Instantiate(View.bassNote, View.player.transform) as GameObject;
            b.transform.localPosition = new Vector3(0, -1, 0);
            Destroy(b, 5f);
        }*/


        // Boss Attack: check if song beat matches audio.time
        if (View.songBeats.regNotes.Count != 0 && Mathf.Abs(View.songBeats.regNotes[songBeatIndex_r] - View.audio.time) < .1f && !hasSpawnedAttack_r)
        {
            //Debug.Log("spawn!");
            //groundAtkEvent.Invoke();    // call singal
            MelodyAtkSignal.Dispatch();
            hasSpawnedAttack_r = true;
        }
        if (View.songBeats.bassNotes.Count != 0 && Mathf.Abs(View.songBeats.bassNotes[songBeatIndex_b] - View.audio.time) < .1f && !hasSpawnedAttack_b)
        {
            //Debug.Log("spawn!");
            //bassAtkEvent.Invoke();   // call singal
            BassAtkSignal.Dispatch();
            hasSpawnedAttack_b = true;
        }
        if (View.songBeats.vocalNotes.Count != 0 && Mathf.Abs(View.songBeats.vocalNotes[songBeatIndex_v] - View.audio.time) < .1f && !hasSpawnedAttack_v)
        {
            // replace ground attack with different kind later
            //groundAtkEvent.Invoke();     // call singal
            InstantAtkSignal.Dispatch();
            hasSpawnedAttack_v = true;
        }

        // Boss Attack:increase index of song beats to hit
        if (View.songBeats.regNotes.Count != 0 && View.songBeats.regNotes[songBeatIndex_r] < View.audio.time && songBeatIndex_r < View.songBeats.regNotes.Count - 1)
        {
            songBeatIndex_r++;
        }
        if (View.songBeats.bassNotes.Count != 0 && View.songBeats.bassNotes[songBeatIndex_b] < View.audio.time && songBeatIndex_b < View.songBeats.bassNotes.Count - 1)
        {
            songBeatIndex_b++;
        }
        if (View.songBeats.vocalNotes.Count != 0 && View.songBeats.vocalNotes[songBeatIndex_v] < View.audio.time && songBeatIndex_v < View.songBeats.vocalNotes.Count - 1)
        {
            songBeatIndex_v++;
        }

        // increase for note hits
        if (melodyNotes.Count != 0 && melodyNotes[songMelodyIndex] < View.audio.time && songMelodyIndex < melodyNotes.Count - 1 )
        {
            songMelodyIndex++;
        }
        if (bassNotes.Count != 0 && bassNotes[songBassIndex] < View.audio.time && songBassIndex < bassNotes.Count - 1)
        {
            songBassIndex++;
        }
        if (vocalNotes.Count != 0 && vocalNotes[songVocalIndex] < View.audio.time && songVocalIndex < vocalNotes.Count - 1)
        {
            songVocalIndex++;
        }

        // enable spawning attacks again
        hasSpawnedAttack_r = false;
        hasSpawnedAttack_b = false;
        hasSpawnedAttack_v = false;

        // Beat Tracker: instantiate beat ring prefab
        GameObject newBeat = Instantiate(View.p_beat, View.player.transform) as GameObject;
        newBeat.transform.localPosition = new Vector3(0, -1, 0);
        Destroy(newBeat, 5f);
        // eaxact beat
        GameObject b = Instantiate(View.beatExact, View.player.transform) as GameObject;
        b.transform.localPosition = new Vector3(0, -1.2f, 0);
        Destroy(b, .5f);

        // Player: update current beat for player
        hasHitBeat = false;
        lastBeat = curBeatTime;
        curBeatTime = Time.fixedTime;

        //update editor window for current beat
        NotesEditor.UpdateCurrentBeat();
    }

    // Player: check for attacking on beat
    public bool CheckIsOnBeat()
    {
        float nextBeat = curBeatTime + curBeatTime - lastBeat;

        // after beat || before beat
        if (Time.fixedTime - curBeatTime < hitPadding)
        {
            return true;
        }
        else if (nextBeat - Time.fixedTime < hitPadding)
        {
            return true;
        }
        else
        {
            //Debug.Log("missed");
            return false;
            //MelodyBeatSignal.Dispatch(false);
        }
    }

    // for creating beat hits for song
    public float SaveBeat()
    {
        float nextBeat = curBeatTime + curBeatTime - lastBeat;

        // after beat 
        if (Time.fixedTime - curBeatTime < nextBeat - Time.fixedTime)
        {
            return curBeatTime;
        }
        // before beat
        else if (nextBeat - Time.fixedTime < Time.fixedTime - curBeatTime)
        {
            return nextBeat;
        }
        return 0;
    }
}
