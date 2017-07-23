using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using strange.extensions.mediation.impl;

public class BeatIndicatorView : View {
    public GameObject beatIndicator;
    public GameObject melodyIndicator;
    public GameObject bassIndicator;
    public GameObject parentIndicator;

    public Transform melodyParent;
    public Transform bassParent;

    public Transform boss;
}
