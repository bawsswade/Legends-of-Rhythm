using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAtk : MonoBehaviour{

    public GameObject bassAtk, snareAtk, melodyAtk;

	public virtual void BassAtk() { Instantiate(bassAtk); }
    public virtual void SnareAtk() { Instantiate(snareAtk); }
    public virtual void MelodyAtk() { Instantiate(melodyAtk); }
}
