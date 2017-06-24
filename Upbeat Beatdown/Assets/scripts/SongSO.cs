using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Notes", menuName = "Song Data", order = 1)]
public class SongSO : ScriptableObject
{
    public float [] bassNotes;
    public float [] snareNotes;
    public float [] vocalNotes;
}
