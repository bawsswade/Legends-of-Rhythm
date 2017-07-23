using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;
using System;

public class BeatIndicatorMediator : Mediator {

    [Inject] public BeatIndicatorView View { get; set; }
    [Inject] public OnChangeNoteType NoteChangeSignal { get; set; }

    private GameObject player;
    private bool isMainBeat = true;
    private NOTETYPE curNoteType;

	public override void OnRegister()
    {
        NoteChangeSignal.AddListener(ChangeNoteType);
	}

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        float secPerBeat = 60f / Beatz.beatsManager.bpm;
        InvokeRepeating("SpawnBeat", secPerBeat, secPerBeat);
    }

    private void Update()
    {
        View.parentIndicator.transform.LookAt(View.boss.transform);
        //View.parentIndicator.transform.rotation = Quaternion.AngleAxis(View.camera.transform.rotation.y, Vector3.up);
    }

    private void SpawnBeat()
    {
        SpawnMainBeat();

        if (curNoteType == NOTETYPE.MELODY)
        {
            CheckSpawnIndicator(NOTETYPE.MELODY, View.melodyIndicator, View.melodyParent);
        }
        else if (curNoteType == NOTETYPE.BASS)
        {
            CheckSpawnIndicator(NOTETYPE.BASS, View.bassIndicator, View.bassParent);
        }
    }

    private void SpawnMainBeat()
    {
        if (isMainBeat)
        {
            GameObject newBeat = Instantiate(View.beatIndicator, View.parentIndicator.transform) as GameObject;
            newBeat.transform.localPosition = new Vector3(0, -1, 0);
            Destroy(newBeat, 5f);
        }
        //isMainBeat = !isMainBeat;
    }

    private void CheckSpawnIndicator(NOTETYPE type, GameObject g, Transform t)
    {
        if (Beatz.beatsManager.IsIndicatorOnBeat(type))
        {
            GameObject newBeat = Instantiate(g, t) as GameObject;
            newBeat.transform.localPosition = new Vector3(0, -1, 0);
            Destroy(newBeat, 5f);
        }
    }

    private void ChangeNoteType(NOTETYPE n)
    {
        curNoteType = n;
    }
}
