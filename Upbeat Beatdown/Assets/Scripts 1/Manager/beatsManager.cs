using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;

public enum NOTETYPE
{
    BASS,
    MELODY,
    SNARE
}

namespace Beatz
{
    public class beatsManager : MonoBehaviour
    {
        class NoteTypeData
        {
            public int indicatorIndex;     // spawns earlier
            public int curIndex;
            public List<float> dataList;

            public NoteTypeData(int  mi, int ind, List<float> list)
            {
                indicatorIndex = mi;
                curIndex = ind;
                dataList = list;
            }
        }

        // song stuff
        public static float hitPadding = .14f;            // deterimes how accurate you have to be
        public static float bpm = 140;
        public static int beatOffset = 7;               // num of beats pass before actual hit
        public SongSO NoteData;

        static Dictionary<NOTETYPE, NoteTypeData> NoteList = new Dictionary<NOTETYPE, NoteTypeData>();
        private AudioSource audio;
        private static float offsetSeconds;
        private static float lastBeat, curBeatTime;

        void Start()
        {
            // add note data
            NoteTypeData melody = new NoteTypeData(0, 0, NoteData.regNotes);
            NoteList.Add(NOTETYPE.MELODY, melody);
            NoteTypeData bass = new NoteTypeData(0,0, NoteData.bassNotes);
            NoteList.Add(NOTETYPE.BASS, bass);
            NoteTypeData snare = new NoteTypeData(0,0,NoteData.vocalNotes);
            NoteList.Add(NOTETYPE.SNARE, snare);

            // set audio
            audio = GetComponent<AudioSource>();
            offsetSeconds = beatOffset * (60f / bpm);

            // beat increments
            float secPerBeat = 60f / bpm;
            InvokeRepeating("Beat", secPerBeat, secPerBeat);
        }
        
        // called every beat
        void Beat()
        {
            // just update indexes
            for(int i = 0; i < NoteList.Count; i++)
            {
                // current notes
                NoteList[(NOTETYPE)i].curIndex = CheckToIncrement(NoteList[(NOTETYPE)i].dataList, NoteList[(NOTETYPE)i].curIndex);
                // metrenome note indicators
                NoteList[(NOTETYPE)i].indicatorIndex = CheckToIncrementIndicator(NoteList[(NOTETYPE)i].dataList, NoteList[(NOTETYPE)i].indicatorIndex);
            }
            // audio clip times on beat
            lastBeat = curBeatTime;
            curBeatTime = Time.fixedTime;
        }

        // updates cur index for onBeat checks to specified notelist
        private int CheckToIncrement(List<float> noteList, int index)
        {
            if (noteList.Count != 0 && noteList[index] < audio.time && index < noteList.Count - 1)
            {
                return index + 1;
            }
            return index;
        }

        // for metrenome note indicators
        private int CheckToIncrementIndicator(List<float> noteList, int index)
        {
            if (noteList.Count != 0 && noteList[index] - offsetSeconds < audio.time && index < noteList.Count - 1)
            {
                return index + 1;
            }
            return index;
        }

        // Check if on generic beat
        public static bool IsOnBeat()
        {
            float nextBeat = curBeatTime + curBeatTime - lastBeat;
            // after beat
            if (Time.fixedTime - curBeatTime < hitPadding)
            {
                return true;
            }
            // before beat
            else if (nextBeat - Time.fixedTime < hitPadding)
            {
                return true;
            }
            return false;
        }

        // Check if on specific beat
        public static bool IsOnBeat(NOTETYPE type)
        {
            float nextBeat = curBeatTime + curBeatTime - lastBeat;
            // check if current beat time is within note data time
            if (Mathf.Abs(curBeatTime - NoteList[type].dataList[NoteList[type].curIndex]) < .1f || Mathf.Abs(nextBeat - NoteList[type].dataList[NoteList[type].curIndex]) < .1f)
            {
                // after beat
                if (Time.fixedTime - curBeatTime < hitPadding)
                {
                    return true;
                }
                // before beat
                else if (nextBeat - Time.fixedTime < hitPadding)
                {
                    return true;
                }
                else
                {
                    Debug.Log("after beat: " + (Time.fixedTime - curBeatTime) + "/ " + "before beat: " + (nextBeat - Time.fixedTime));
                    return false;
                }
            }
            /*if (Mathf.Abs(Time.fixedTime - NoteList[type].dataList[NoteList[type].curIndex]) < hitPadding)
            {
                return true;
            }*/
            //Debug.Log(Time.time - NoteList[type].dataList[NoteList[type].curIndex]);
            return false;
        }

        public static bool IsIndicatorOnBeat(NOTETYPE type)
        {
            if (Mathf.Abs(Time.fixedTime - (NoteList[type].dataList[NoteList[type].indicatorIndex] - offsetSeconds)) < hitPadding)
            {
                return true;
            }
            return false;
        }

        public static bool IsOnBeat(NOTETYPE type, float offset)
        {
            if (Mathf.Abs(Time.fixedTime - NoteList[type].dataList[NoteList[type].curIndex]) < hitPadding)
            {
                return true;
            }
            return false;
        }

        public static void SetBPM(int num)
        {
            bpm = num;
        }
    }
}