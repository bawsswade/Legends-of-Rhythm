using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Beatz
{
    public class SongManager : MonoBehaviour
    {
        public static int bpm = 140;
        public SongSO songData;
        public Action OnStartGame;      // used to sync all classes reliant on beat

        //required
        private AudioSource song;
        private List<NoteData> PlayerNotes = new List<NoteData>();
        private List<NoteData> EnemyNotes = new List<NoteData>();

        private int beatIndex = 0;  // constant beat
        private int playerIndex = 0;
        private int enemyIndex = 0;
        private float secPerBeat = 0;
        private static float songTime = 0;     // used for getting song time on beat

        // the leniency of hits
        public float hitPadding = .1f;

        private void Start()
        {
            SetSong(bpm, GetComponent<AudioSource>(), songData);
            OnStartGame += StartSong;

            //songService.SetSong(140f, GetComponent<AudioSource>());
            //songService.StartSong();

            OnStartGame.Invoke();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                GetIsOnBeat();
            }
        }

        // Always Set Song Data
        private void SetSong(float bpm, AudioSource source, SongSO data)
        {
            secPerBeat = 60f / bpm;

            // set source to reference time
            song = source;

            foreach (Vector2 note in data.notes)
            {
                if (PlayerNotes.Find(n => n.time == note.x) != null)
                {
                    PlayerNotes.Find(n => n.time == note.x).noteType.Add((NoteType)note.y);
                }
                else
                {
                    NoteData hit = new NoteData();
                    hit.time = note.x;
                    hit.noteType = new List<NoteType>();
                    hit.noteType.Add((NoteType)note.y);
                    PlayerNotes.Add(hit);
                }
            }
        }

        public void StartSong()
        {
            if (secPerBeat != 0)
            {
                song.Play();
                // increment indexes and checks (hitPadding/2) to check padding before and after exact note hit
                InvokeRepeating("IncrementBeat", secPerBeat - (hitPadding / 2.0f), secPerBeat);
            }
            else
            {
                Debug.Log("song data not set");
            }
        }

        private void IncrementBeat()
        {
            beatIndex++;
            // increment player checks
            if ((playerIndex < PlayerNotes.Count && song.time > PlayerNotes[playerIndex].time) || PlayerNotes[playerIndex].hasHitNote)
            {
                if (playerIndex < PlayerNotes.Count)
                {
                    playerIndex++;
                }
            }

            // increment boss checks
            if (enemyIndex < PlayerNotes.Count && song.time > PlayerNotes[enemyIndex].time)
            {
                enemyIndex++;
            }
        }

        /// <summary>
        /// check for player to hit certain beat
        /// - noteLength: use to check half beats, quarter beats, ect.
        /// - only used by player (for now)
        /// </summary>
        public bool GetHasHitNote(NoteType noteType)
        {
            // check song.time to current beats time
            if (Mathf.Abs(song.time - PlayerNotes[enemyIndex].time) < (hitPadding) && PlayerNotes[enemyIndex].noteType.Contains(noteType))
            {
                return true;
            }
            //Debug.Log(song.time + " - " + PlayerNotes[enemyIndex].time + " = " + (song.time - PlayerNotes[playerIndex].time));
            return false;
        }

        /// <summary>
        /// check for ai to spawn a note/attack
        /// - only checks if song time passed next note
        /// </summary>
        public bool GetShouldSpawnNote(NoteType noteType, int beatOffset = 0)
        {
            float timeOffest = beatOffset * (60f / bpm);
            // check song.time to current beats time
            if (playerIndex < PlayerNotes.Count &&  Mathf.Abs(song.time - (PlayerNotes[playerIndex].time - timeOffest)) < (60f/bpm /2) && PlayerNotes[playerIndex].noteType.Contains(noteType))
            {
                PlayerNotes[playerIndex].hasHitNote = true;
                return true;
            }
            return false;
        }

        /// <summary>
        /// check if on constant beat
        /// - noteLength: use to check half beats, quarter beats, ect.
        /// </summary>
        public bool GetIsOnBeat(float noteLength = 1)
        {
            // (hitTime - half padding) -index*secPerBeat < hitpadding
            if (Mathf.Abs(((song.time - hitPadding/2) - ((secPerBeat * noteLength) * (beatIndex)))) < hitPadding)
            {
                //Debug.Log("hit");
                return true;
            }
            //Debug.Log(Mathf.Abs(song.time - ((secPerBeat * noteLength) * (beatIndex * (1 / noteLength)))));
            return false;
        }

        public float GetClosestBeat()
        {
            Debug.Log(beatIndex);
            return beatIndex * (60f / bpm);
        }

        /// <summary>
        /// get beatIndex by song time
        /// - can use for when starting at a certain point in the song
        /// </summary>
        public int GetClosestIndex(float songTime)
        {
            int index = beatIndex;
            // search 


            return index;
        }
    }

    public class NoteData
    {
        public float time;      // should be exact
        public List<NoteType> noteType;
        public bool hasHitNote = false;

        // duration of note holds
        public int duration;
    }

    public enum NoteType
    {
        Absorb, TLeft, TRgiht, BLeft, BRight, TLeftCharged, TRightCharged, BLeftCharged, BRightCharged
    }
}