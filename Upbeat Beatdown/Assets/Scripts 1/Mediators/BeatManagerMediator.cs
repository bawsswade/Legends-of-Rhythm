using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;
using System;
using System.Collections.Generic;

public enum NOTETYPE
{
    BASS,
    MELODY,
    SNARE
}


[RequireComponent(typeof(AudioSource))]
public class BeatManagerMediator : Mediator {

    [Inject] public BeatManagerView View { get; set; }
    
    // boss attack signals
    [Inject] public OnBassAttackSignal BassAtkSignal { get; set; }
    [Inject] public OnMelodyAttackSignal MelodyAtkSignal { get; set; }
    [Inject] public OnInstantAttackSignal InstantAtkSignal { get; set; }
    [Inject] public OnChangeNoteType NoteTypeSignal { get; set; }
    // set on beat bool for player
    //[Inject] public OnBassBeat BassBeatSignal { get; set; }
    //[Inject] public OnMelodyBeat MelodyBeatSignal { get; set; }

    private AudioSource audio;
    private float secPerBeat;
    private GameObject player;
    public NOTETYPE currentNoteType;

    // tracking player hits
    float curBeatTime = 0;
    float lastBeat = 0;
    float hitPadding = .1f;            // deterimes how accurate you have to be
    bool hasHitBeat = false;
    bool isMainBeat = true;

    // boss attacks
    public int songBeatIndex_r = 0;        // melody
    public int songBeatIndex_b = 0;
    public int songBeatIndex_v = 0;
    bool hasSpawnedAttack_v = false;
    bool hasSpawnedAttack_r = false;
    bool hasSpawnedAttack_b = false;

    // spawn notes to hit for metrenome indicator
    int songMelodyIndex = 0;
    int songBassIndex = 0;
    int songVocalIndex = 0;
    public List<float> melodyNotes = new List<float>();
    public List<float> bassNotes = new List<float>();
    public List<float> vocalNotes = new List<float>();

    public override void OnRegister()
    {
        NoteTypeSignal.AddListener(UpdateNoteType);
	}

    // Use this for initialization
    void Start()
    {
        currentNoteType = NOTETYPE.MELODY;      // start player on melody
        player = GameObject.FindGameObjectWithTag("Player");
        audio = GetComponent<AudioSource>();
        // get time in seconds to spawn
        secPerBeat = 60f / View.bpm;
        InvokeRepeating("SpawnBeat", secPerBeat, secPerBeat);
        //audio.Pause();

        foreach (float f in View.NoteData.regNotes)
        {
            melodyNotes.Add(f-((60/View.bpm)*7));           // seconds per beat * 7 beats off = time difference
        }
        foreach (float f in View.NoteData.bassNotes)
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
        if(SpawnNoteType(melodyNotes, songMelodyIndex))
        {
            // spawn melody beat
            GameObject b = Instantiate(View.melodyNote, player.transform) as GameObject;
            b.transform.parent = View.allMelodyNotes.transform;
            b.transform.localPosition = new Vector3(0, -1, 0);
            Destroy(b, 5f);
        }
        if(SpawnNoteType(bassNotes, songBassIndex))
        {
            // spawn melody beat
            GameObject b = Instantiate(View.bassNote, player.transform) as GameObject;
            b.transform.parent = View.allBassNotes.transform;
            b.transform.localPosition = new Vector3(0, -1, 0);
            Destroy(b, 5f);
        }


        // Boss Attack: check if song beat matches audio.time
        if (View.NoteData.regNotes.Count != 0 && Mathf.Abs(View.NoteData.regNotes[songBeatIndex_r] - audio.time) < .1f && !hasSpawnedAttack_r && currentNoteType!= NOTETYPE.MELODY)
        {
            //Debug.Log("spawn!");
            //groundAtkEvent.Invoke();    // call singal
            MelodyAtkSignal.Dispatch();
            hasSpawnedAttack_r = true;
        }
        if (View.NoteData.bassNotes.Count != 0 && Mathf.Abs(View.NoteData.bassNotes[songBeatIndex_b] - audio.time) < .1f && !hasSpawnedAttack_b && currentNoteType != NOTETYPE.BASS)
        {
            //Debug.Log("spawn!");
            //bassAtkEvent.Invoke();   // call singal
            BassAtkSignal.Dispatch();
            hasSpawnedAttack_b = true;
        }
        if (View.NoteData.vocalNotes.Count != 0 && Mathf.Abs(View.NoteData.vocalNotes[songBeatIndex_v] - audio.time) < .1f && !hasSpawnedAttack_v && currentNoteType != NOTETYPE.SNARE)
        {
            // replace ground attack with different kind later
            //groundAtkEvent.Invoke();     // call singal
            InstantAtkSignal.Dispatch();
            hasSpawnedAttack_v = true;
        }

        // Boss Attack:increase index of song beats to hit
        songBeatIndex_r = CheckToIncrement(View.NoteData.regNotes, songBeatIndex_r);
        songBeatIndex_b = CheckToIncrement(View.NoteData.bassNotes, songBeatIndex_b);
        songBeatIndex_v = CheckToIncrement(View.NoteData.vocalNotes, songBeatIndex_v);

        // increase for note hits for metrenome
        songMelodyIndex = UpdateMetrenomeVars(melodyNotes, songMelodyIndex);
        songBassIndex = UpdateMetrenomeVars(bassNotes, songBassIndex);
        songVocalIndex =  UpdateMetrenomeVars(vocalNotes, songVocalIndex);

        /* (melodyNotes.Count != 0 && melodyNotes[songMelodyIndex] < audio.time && songMelodyIndex < melodyNotes.Count - 1 )
        {
            songMelodyIndex++;
        }
        if (bassNotes.Count != 0 && bassNotes[songBassIndex] < audio.time && songBassIndex < bassNotes.Count - 1)
        {
            songBassIndex++;
        }
        if (vocalNotes.Count != 0 && vocalNotes[songVocalIndex] < audio.time && songVocalIndex < vocalNotes.Count - 1)
        {
            songVocalIndex++;
        }*/


        UpdateBeatVars();

        // Beat Tracker / Metrenome
        SpawnBeatTracker();

        //update editor window for current beat
        //NotesEditor.UpdateCurrentBeat();
    }

    // Player: check for attacking on beat
    public bool CheckIsOnBeat()
    {
        float nextBeat = curBeatTime + curBeatTime - lastBeat;

        // after beat
        if (Time.fixedTime - curBeatTime < hitPadding)      // also check to cureNoteType
        {
            return true;
        }
        // before beat
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

    public bool SpawnNoteType(List<float> noteList, int index)
    {
        if (noteList.Count != 0 && Mathf.Abs(noteList[index] - audio.time) < .1f)      // disable all others except this note typs
        {
            return true;
        }
        return false;
    }

    public bool CheckHitBeat()
    {
        if (currentNoteType == NOTETYPE.MELODY)
        {
            if(Mathf.Abs(View.NoteData.regNotes[songBeatIndex_r] - audio.time) < .1f)
            {
                return true;
            }
        }
        else if (currentNoteType == NOTETYPE.BASS)
        {
            if(Mathf.Abs(View.NoteData.bassNotes[songBeatIndex_r] - audio.time) < .1f)
            {
                return true;
            }
        }
        else if (currentNoteType == NOTETYPE.SNARE)
        {
            if(Mathf.Abs(View.NoteData.vocalNotes[songBeatIndex_r] - audio.time) < .1f)
            {
                return true;
            }
        }
        return false;
    }

    public int UpdateMetrenomeVars(List<float> noteList, int index)
    {
        if (noteList.Count != 0 && noteList[index] < audio.time && index < noteList.Count - 1)
        {
            return index + 1;
        }
        return index;
    }

    private void UpdateBeatVars()
    {
        // enable spawning attacks again for boss
        hasSpawnedAttack_r = false;
        hasSpawnedAttack_b = false;
        hasSpawnedAttack_v = false;

        // Player: update current beat for player
        hasHitBeat = false;
        lastBeat = curBeatTime;
        curBeatTime = Time.fixedTime;
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

    // returns index
    private int CheckToIncrement(List<float> dataList, int index)
    {
        if (dataList.Count != 0 && dataList[index] < audio.time && index < dataList.Count - 1)
        {
            return index+1;
        }
        return index;
    }

    private void SpawnBeatTracker()
    {
        // Beat Tracker: instantiate beat ring prefab  (every other) 
        if (isMainBeat)
        {
            GameObject newBeat = Instantiate(View.p_beat, player.transform) as GameObject;
            newBeat.transform.localPosition = new Vector3(0, -1, 0);
            Destroy(newBeat, 5f);

            // eaxact beat
            /*GameObject b = Instantiate(View.snareNote, player.transform) as GameObject;
            b.transform.localPosition = new Vector3(0, -1.2f, 0);
            Destroy(b, .5f);*/
        }
        // to sawn every other
        //isMainBeat = !isMainBeat;
    }

    private void UpdateNoteType(NOTETYPE nt)
    {
        currentNoteType = nt;
    }
}
