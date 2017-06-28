using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Notes", menuName = "Song Data", order = 1)]
public class SongSO : ScriptableObject
{
    public List<float> vocalNotes = new List<float>();
    public List<float> bassNotes = new List<float>();
    public List<float> regNotes = new List<float>();
}
