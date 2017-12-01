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
    public int indIndex = 0;

    private UnityEngine.UI.Image mel, bass, snare;

	public override void OnRegister()
    {
        NoteChangeSignal.AddListener(ChangeNoteType);
	}

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        float secPerBeat = 60f / Beatz.beatsManager.bpm;
        InvokeRepeating("SpawnBeat", secPerBeat, secPerBeat);

        //set images
        mel = View.melodyInd.GetComponent<UnityEngine.UI.Image>();
        bass = View.bassInd.GetComponent<UnityEngine.UI.Image>();
        snare = View.snareInd.GetComponent<UnityEngine.UI.Image>();
    }

    private void Update()
    {
        View.parentIndicator.transform.LookAt(View.boss.transform);
        //View.parentIndicator.transform.rotation = Quaternion.AngleAxis(View.camera.transform.rotation.y, Vector3.up);
    }

    private void SpawnBeat()
    {
        //SpawnMainBeat();
        //CheckSpawnIndicator(NOTETYPE.MELODY, View.melodyIndicator, mel, View.melodyParent.transform, View.melodyHUDParent.transform);
        CheckSpawnIndicator(NOTETYPE.BASS, View.bassIndicator, bass, View.bassParent.transform, View.bassHUDParent.transform);
        CheckSpawnIndicator(NOTETYPE.SNARE, View.snareIndicator, snare, View.snareParent.transform, View.snareHUDParent.transform);
    }

    private void SpawnMainBeat()
    {
        if (isMainBeat)
        {
            GameObject newBeat = Instantiate(View.beatIndicator, View.parentIndicator.transform) as GameObject;
            newBeat.transform.localPosition = new Vector3(0, -1, 0);
            Destroy(newBeat, 3f);
        }
        //isMainBeat = !isMainBeat;
    }
    
    private void CheckSpawnIndicator(NOTETYPE type, GameObject g, UnityEngine.UI.Image i, Transform t, Transform HUDtrans)
    {
        if (Beatz.beatsManager.IsIndicatorOnBeat(type))
        {
            GameObject newBeat = Instantiate(g, t) as GameObject;
            newBeat.transform.localPosition = new Vector3(0, -1, 0);

            // hud indicator
            /*if (type == NOTETYPE.MELODY)
            {
                View.IndAnim[indIndex].SetTrigger("spawn");
                indIndex++;
                if (indIndex > View.IndAnim.Length - 1)
                {
                    indIndex = 0;
                }
            }*/
            // instantiate instead
            UnityEngine.UI.Image im = Instantiate(i, HUDtrans);
            im.transform.position = View.indLoc.transform.position;
            im.gameObject.GetComponent<Animator>().SetTrigger("spawn");
            Destroy(im.gameObject, 2);

            // disable renderer if not cur type
            if (type != curNoteType)
            {
                //SetAllParticles(newBeat, im.gameObject, false);
            }

            Destroy(newBeat, 3f);
        }
    }

    private void ChangeNoteType(NOTETYPE n)
    {
        DisableAllIndicators(curNoteType);
        curNoteType = n;
        EnableAllIndicators(curNoteType);
    }

    private void DisableAllIndicators(NOTETYPE type)
    {
        if (type == NOTETYPE.MELODY)
        {
            SetAllParticles(View.melodyParent, View.melodyHUDParent, false);
        }
        else if (type == NOTETYPE.BASS)
        {
            SetAllParticles(View.bassParent, View.bassHUDParent, false);
        }
        else if (type == NOTETYPE.SNARE)
        {
            SetAllParticles(View.snareParent, View.snareHUDParent, false);
        }
    }

    private void EnableAllIndicators(NOTETYPE type)
    {
        if (type == NOTETYPE.MELODY)
        {
            SetAllParticles(View.melodyParent, View.melodyHUDParent, true);
        }
        else if (type == NOTETYPE.BASS)
        {
            SetAllParticles(View.bassParent, View.bassHUDParent, true);
        }
        else if (type == NOTETYPE.SNARE)
        {
            SetAllParticles(View.snareParent, View.snareHUDParent, true);
        }
    }

    private void SetAllParticles(GameObject g , GameObject HUDg, bool b)
    {
        foreach(ParticleSystemRenderer p in g.GetComponentsInChildren<ParticleSystemRenderer>())
        {
            p.enabled = b;
        }

        foreach(UnityEngine.UI.Image i in HUDg.GetComponentsInChildren<UnityEngine.UI.Image>())
        {
            i.enabled = b;
        }
    }
}
