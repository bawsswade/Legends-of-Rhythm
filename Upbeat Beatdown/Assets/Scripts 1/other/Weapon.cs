using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

    public Animator anim;

    public virtual void MelodyAttack() { anim.SetTrigger("melody"); }
    public virtual void BassAttack() { anim.SetTrigger("bass"); }
    public virtual void SnareAttack() { anim.SetTrigger("snare"); }

    public virtual float AttackDistance() { return 0; }
}
