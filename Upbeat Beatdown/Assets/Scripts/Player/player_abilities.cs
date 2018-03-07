using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Beatz;
using UnityEngine.UI;

public class player_abilities : MonoBehaviour {

    private SphereCollider dashCol;
    private player_input input;
    private player_motor motor;
    private int noteSyncDuration = 20;

    public Slider meter;
    public int noteIncrement = 0;

    private AudioSource source;
    private SongManager sm;

    public GameObject model;
    private SkinnedMeshRenderer modelRenderer;
    public GameObject DashParts;
    public Material onBeatMaterial;
    private Material startMaterial;

    public Action OnEnterNoteSync;
    public Action OnExitNoteSync;

	void Start ()
    {
        if (GetComponent<SphereCollider>() == null)
        {
            SphereCollider col =  gameObject.AddComponent<SphereCollider>();
            col.center = new Vector3(0,.1f,0);
            col.radius = 2.25f;
        }
        else
        {
            dashCol = GetComponent<SphereCollider>();
        }

        motor = GetComponent<player_motor>();

        dashCol.enabled = false;
        input = GetComponent<player_input>();
        modelRenderer = model.GetComponent<SkinnedMeshRenderer>();
        startMaterial = modelRenderer.material;

        //audio
        source = GetComponent<AudioSource>();

        sm = GameObject.FindObjectOfType<Beatz.SongManager>();

        // Actions
        input.OnDash += Dash;
	}
	
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Note")
        {
            // note actions
            other.GetComponent<P1Note>().HasHitNote();

            // play sound
            source.Play();

            //check meter
            meter.value += noteIncrement;
            if(meter.value >= 100)
            {
                OnEnterNoteSync();
                StartCoroutine(StartNoteSyncTimer());
            }
        }
    }

    private void Dash(INPUTTYPE type)
    {
        dashCol.enabled = true;

        //set parts
        DashParts.SetActive(true);
        var ps = DashParts.GetComponent<ParticleSystem>().main;
        Color temp = type == INPUTTYPE.Atk1 ? Color.red : Color.blue;
        temp.a = .15f;
        ps.startColor = temp;
        // dash material
        onBeatMaterial.SetColor("_EmissionColor", temp);
        modelRenderer.material = onBeatMaterial;

        StartCoroutine("WaitForEndDash");
    }

    IEnumerator WaitForEndDash()
    {
        yield return new WaitForSeconds(motor.dashTimer - sm.hitPadding);
        dashCol.enabled = false;
        DashParts.SetActive(false);
        modelRenderer.material = startMaterial;
    }

    IEnumerator StartNoteSyncTimer()
    {
        float count = 0;
        while (count < noteSyncDuration)
        {
            count += Time.deltaTime;
            meter.value -= (Time.deltaTime / noteSyncDuration) * (meter.maxValue);        // this might be wrong
            yield return null;
        }
        
        OnExitNoteSync();
    }
}
