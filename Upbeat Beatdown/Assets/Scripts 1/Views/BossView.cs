﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using strange.extensions.mediation.impl;

public class BossView : View {

    public BossAtk Attacks;

    // atk prefabs
    //public GameObject groundAtk;    // orb
    public GameObject instantAOE;   // cicle instnat
    public GameObject instantLine;  // line instant
    public GameObject aoeAtk;       // bass

    // UI
    public float health;
    public GameObject healthDisplay;

    // container
    public GameObject bassBossAtks;
    public GameObject melodyBossAtks;
    public GameObject snareBossAtks;
}
