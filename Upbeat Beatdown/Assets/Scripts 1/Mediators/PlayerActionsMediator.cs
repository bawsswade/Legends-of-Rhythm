using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;
using System;

public class PlayerActionsMediator : Mediator {

    [Inject] public PlayerActionsView View { get; set; }
    [Inject] public OnChangeNoteType NoteChangeSignal { get; set; }


    public NOTETYPE curNoteType;

	public override void OnRegister()
    {
        NoteChangeSignal.AddListener(ChangeNoteType);
	}

    private void Start()
    {
        NoteChangeSignal.Dispatch(NOTETYPE.MELODY);
    }

    private void Update()
    {
        if (Ins.InuptManager.GetControls(INPUTTYPE.Block) && Beatz.beatsManager.IsOnBeat())
        {
            Block();
            HitBeatIndicator();
        }

        else if (Ins.InuptManager.GetControls(INPUTTYPE.Attack) && Beatz.beatsManager.IsOnBeat(curNoteType))
        {
            SpawnAttack();
            HitBeatIndicator();
        }

        // changing beats
        if (Ins.InuptManager.GetControls(INPUTTYPE.SwitchLeft))
        {
            curNoteType -= 1;
            NoteChangeSignal.Dispatch(curNoteType);
        }
        else if (Ins.InuptManager.GetControls(INPUTTYPE.SwitchRight))
        {
            curNoteType++;
            NoteChangeSignal.Dispatch(curNoteType);
        }
    }

    private void HitBeatIndicator()
    {
        View.platformAnim.Play("HitBeat");
    }

    private void Block()
    {
        View.shieldAnim.Play("ShieldActive");
    }

    private void SpawnAttack()
    {
        GameObject d = Instantiate(View.projectile, transform.position ,Quaternion.Euler(0,180,0));
        //d.transform.localPosition = Vector3.zero;
        //d.transform.localRotation = Quaternion.Euler(Vector3.zero);
    }

    private void ChangeNoteType(NOTETYPE n)
    {
        curNoteType = n;
    }
}
