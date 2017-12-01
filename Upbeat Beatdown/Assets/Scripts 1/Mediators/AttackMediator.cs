using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;
using System;
using Beatz;

public class AttackMediator : Mediator {
    [Inject] public AttackView View { get; set; }

    public Weapon curWeapon;        // slot
    private bool hasAttack;
    private AudioSource hit;

	public override void OnRegister()
    {

	}

    private void Start()
    {
        //set current weapon
        curWeapon = new Guitar();       // set somehow
        // set the animator
        curWeapon.anim = View.anim;
        hit = GetComponent<AudioSource>();
    }

    private void Update()
    {
        // left attack
        if (Ins.InuptManager.GetControls(INPUTTYPE.BassAttack) && Beatz.beatsManager.IsOnBeat(NOTETYPE.BASS))
        {
            Attack(NOTETYPE.BASS);
        }

        // right attack
        if (Ins.InuptManager.GetControls(INPUTTYPE.SnareAttack) && Beatz.beatsManager.IsOnBeat(NOTETYPE.SNARE))
        {
            Attack(NOTETYPE.SNARE);
        }
    }

    private void Attack(NOTETYPE type)
    {
        if (!hasAttack)
        {
            //hit.Play();

            if (type == NOTETYPE.BASS)
            {
                Debug.Log("bass hit");  
                //curWeapon.BassAttack();
                GameObject go = Instantiate(View.bassAtk, transform.position, Quaternion.Euler(new Vector3(-90,0, 0)));
                Destroy(go, 2);
            }
            else if (type == NOTETYPE.SNARE)
            {
                Debug.Log("snare hit");
                GameObject go = Instantiate(View.snareAtk, transform.position, Quaternion.Euler(new Vector3(-90, View.model.transform.rotation.eulerAngles.y + 180, 0)));
                Destroy(go, 2);
                //curWeapon.SnareAttack();
            }
            else if (type == NOTETYPE.MELODY)
            {
                curWeapon.MelodyAttack();
            }
        }
    }
}
