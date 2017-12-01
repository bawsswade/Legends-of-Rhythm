using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using strange.extensions.mediation.impl;

public class BossView : View {
<<<<<<< HEAD

    public BossAtk Attacks;

    // atk prefabs
    //public GameObject groundAtk;    // orb
=======
    // atk prefabs
    public GameObject groundAtk;    // orb
>>>>>>> ae8663e27890825f99da0550b67eec12000e619c
    public GameObject instantAOE;   // cicle instnat
    public GameObject instantLine;  // line instant
    public GameObject aoeAtk;       // bass

    // UI
    public float health;
    public GameObject healthDisplay;

<<<<<<< HEAD
    // container
=======
    // where attacks are stored
>>>>>>> ae8663e27890825f99da0550b67eec12000e619c
    public GameObject bassBossAtks;
    public GameObject melodyBossAtks;
    public GameObject snareBossAtks;
}
