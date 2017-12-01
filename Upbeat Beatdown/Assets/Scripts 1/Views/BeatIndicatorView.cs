using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using strange.extensions.mediation.impl;

public class BeatIndicatorView : View {
    public GameObject beatIndicator;
    public GameObject melodyIndicator;
    public GameObject bassIndicator;
    public GameObject snareIndicator;
    public GameObject parentIndicator;

    //public Animator [] IndAnim;
    public GameObject melodyInd;
    public GameObject bassInd;
    public GameObject snareInd;
    public Transform indLoc;

    // particles
    public GameObject melodyParent;
    public GameObject bassParent;
    public GameObject snareParent;
    // HUD
    public GameObject melodyHUDParent;
    public GameObject bassHUDParent;
    public GameObject snareHUDParent;

    public Transform boss;
}
