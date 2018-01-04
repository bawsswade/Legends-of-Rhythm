using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;
using Beatz;

[CreateAssetMenu(fileName = "Notes", menuName = "Song Data", order = 1)]
[System.Serializable]
public class SongSO : ScriptableObject
{
    [SerializeField]
    public List<float> regNotes = new List<float>();

    [SerializeField]
    public List<Vector2> notes;
}
