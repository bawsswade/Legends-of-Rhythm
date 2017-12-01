using UnityEngine;
using UnityEditor;
using NUnit.Framework;

public class NotesEditor : EditorWindow {

    public SongSO songToEdit;
    public AudioSource song;

    //2427600
    static int numBeats = 500;

    Vector2 scrollPos = new Vector2(0,0);

    // toggle for displaying type of notes to edit
    bool displayReg, displayBass, displayInstant;
    // cylcing index of SO notes
    int regIndex = 0;
    int bassIndex = 0;
    int instIndex = 0;

    float bpm;  
    float beatTime = 0;

    // holds note toggles from SO
    bool[] regNoteList = new bool[numBeats];
    bool[] bassNoteList = new bool[numBeats];
    bool[] instNoteList = new bool[numBeats];

    //count for displaying whic beat on
    static int indexActive = 0;
    //bools for beat interations
    static bool[] beats = new bool[numBeats];

    // Add menu item 
    [MenuItem("Create/Edit Song notes")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(NotesEditor));
    }

    private void OnGUI()
    {
        // needed inputs
        songToEdit = (SongSO)EditorGUILayout.ObjectField("Scriptable Object to Edit:", songToEdit , typeof(SongSO), true);
        song = (AudioSource)EditorGUILayout.ObjectField("Song to Edit:", song, typeof(AudioSource), true);
        bpm = EditorGUILayout.FloatField("Beats per Minute ",bpm);

        GUILayout.Label("Type of Notes", EditorStyles.boldLabel);

        displayReg =GUILayout.Toggle(displayReg, "Regular Notes");
        displayBass =GUILayout.Toggle(displayBass, "Bass Notes");
        displayInstant = GUILayout.Toggle(displayInstant, "Instant Notes");

        GUILayout.Label("Edit Notes", EditorStyles.boldLabel);

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        // time text
        GUILayout.BeginHorizontal();
        beatTime = 0;
        regIndex = 0;
        bassIndex = 0;
        instIndex = 0;
        if (songToEdit != null)
        {
            for (int i = 0; i < regNoteList.Length; i++)
            {
                beatTime += 60 / bpm;
                // check SO value if matches beat time
                if (Mathf.Abs(songToEdit.regNotes[regIndex] - beatTime) < .1f)
                {
                    // set that toggle to true
                    regNoteList[i] = true;
                    regIndex++;
                }
                else
                {
                    regNoteList[i] = false;
                }
                if (Mathf.Abs(songToEdit.bassNotes[bassIndex] - beatTime) < .1f)
                {
                    // set that toggle to true
                    bassNoteList[i] = true;
                    bassIndex++;
                }
                else
                {
                    bassNoteList[i] = false;
                }
                if (Mathf.Abs(songToEdit.vocalNotes[instIndex] - beatTime) < .1f)
                {
                    // set that toggle to true
                    instNoteList[i] = true;
                    if (instIndex < songToEdit.vocalNotes.Count - 1)
                    {
                        instIndex++;
                    }
                }
                else
                {
                    instNoteList[i] = false;
                }

                GUILayout.Label(beatTime.ToString(), GUILayout.Width(40));
                GUILayout.Label("|", EditorStyles.label);
            }
        }
        GUILayout.EndHorizontal();
        // runtime current beat
        GUILayout.BeginHorizontal();
        for (int i = 0; i < regNoteList.Length; i++)
        {

            if (EditorApplication.isPlaying)
            {
                beatTime += 60 / bpm;
            }
            // set toggles
            GUILayout.Toggle(beats[i], "", GUILayout.Width(40));
            GUILayout.Label("|", EditorStyles.label);
        }
        GUILayout.EndHorizontal();

        // REGULAR NOTES DISPAY EDITOR
        if (displayReg && songToEdit != null)
        {
            GUILayout.Label("Regular", EditorStyles.label);

            // toggle switches
            GUILayout.BeginHorizontal();
            for(int i = 0; i < regNoteList.Length; i++)
            {
                // set toggles
                GUILayout.Toggle(regNoteList[i],"", GUILayout.Width(40));
                GUILayout.Label("|", EditorStyles.label);
            }
            GUILayout.EndHorizontal();
        }
        // BASS NOTES DISPAY EDITOR
        if (displayBass && songToEdit != null)
        {
            GUILayout.Label("Bass", EditorStyles.label);

            // toggle switches
            GUILayout.BeginHorizontal();
            for (int i = 0; i < bassNoteList.Length; i++)
            {
                // set toggles
                GUILayout.Toggle(bassNoteList[i], "", GUILayout.Width(40));
                GUILayout.Label("|", EditorStyles.label);
            }
            GUILayout.EndHorizontal();
        }
        // INSTANT NOTES DISPAY EDITOR
        if (displayInstant && songToEdit != null)
        {
            GUILayout.Label("Instant Hits", EditorStyles.label);

            // toggle switches
            GUILayout.BeginHorizontal();
            for (int i = 0; i < instNoteList.Length; i++)
            {
                // set toggles
                GUILayout.Toggle(instNoteList[i], "", GUILayout.Width(40));
                GUILayout.Label("|", EditorStyles.label);
            }
            GUILayout.EndHorizontal();
        }



        EditorGUILayout.EndScrollView();
    }

    public static void UpdateCurrentBeat()
    {
        beats[indexActive] = true;
            indexActive++;
    }

    private void OnInspectorUpdate()
    {
        Repaint();
    }
}
