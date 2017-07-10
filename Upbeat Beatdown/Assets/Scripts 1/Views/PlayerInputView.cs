using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using strange.extensions.mediation.impl;

public class PlayerInputView : View {
    //public beatsManager beatMan;
    public GameObject boss;

    // change to use events
    public GameObject l_attack, r_attack;   // enable mesh renerer for visual animation
    public GameObject hit_part;
    public GameObject missParticles;
    public Transform l_hitPos, r_hitPos;
    public Collider l_boxCol, r_boxCol;  // enable collider

    public bool noteHit;            // used by right and left attack scripts
    public GameObject beatMan;

    // deflected projectile
    public GameObject deflectProjectile;

    public Text specialAtkText;
    public GameObject specialAtk;
    public GameObject spAtkIndicator;

    public bool isDashing;
    public GameObject dashParticles;

    public float health;
    public GameObject healthBar;
}
