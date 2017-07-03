using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Notes", menuName = "Song Data", order = 1)]
[System.Serializable]
public class SongSO : ScriptableObject
{
    [SerializeField]
    public List<float> vocalNotes = new List<float>();
    [SerializeField]
    public List<float> bassNotes = new List<float>();
    [SerializeField]
    public List<float> regNotes = new List<float>();

    public float GetRegNotes(int index)
    {
        return regNotes[index];
    }

    public float GetBassNotes(int index)
    {
        return bassNotes[index];
    }

    public int GetRegNotesCount()
    {
        return regNotes.Count;
    }

    public int getBassNotesCount()
    {
        return bassNotes.Count;
    }

    public void AddBassNote(float f)
    {
        bassNotes.Add(f);
    }

    public void AddMelodyNote(float f)
    {
        regNotes.Add(f);
    }
}
