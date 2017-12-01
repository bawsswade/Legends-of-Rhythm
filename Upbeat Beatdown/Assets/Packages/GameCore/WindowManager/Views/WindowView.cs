using UnityEngine;
using System.Collections.Generic;
using strange.extensions.mediation.impl;

public class WindowView : View
{
    public int Index = 0;
    public bool DestroyOnClose = true;
    public List<GameObject> ItemsToShow;
    public List<GameObject> ItemsToHide;
    public GameObject InnerContent;
}
