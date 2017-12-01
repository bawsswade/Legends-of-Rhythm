using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guitar : Weapon {

    //somthing that has all data needed
    public GameObject guitar;
    

    private float distance;

    private void Start()
    {
        //load data (attack prefabs needed)
    }

    // ground slam
    public override void BassAttack()
    {
        base.BassAttack();

        distance = 4;
    }

    public override void MelodyAttack()
    {
        base.MelodyAttack();

        distance = 0;
    }

    public override void SnareAttack()
    {
        base.SnareAttack();

        distance = 0;
    }

    public override float AttackDistance()
    {
        return distance;
    }
}
