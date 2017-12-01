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
<<<<<<< HEAD
        public static float hitPadding = .15f;            // deterimes how accurate you have to be
        public static float bpm = 140;
        public static float secPerBeat;
        public static int beatOffset = 4;               // num of beats pass before actual hit
=======
        public static float hitPadding = .14f;            // deterimes how accurate you have to be
        public static float bpm = 140;
        public static int beatOffset = 7;               // num of beats pass before actual hit
>>>>>>> ae8663e27890825f99da0550b67eec12000e619c
        public SongSO NoteData;

        static Dictionary<NOTETYPE, NoteTypeData> NoteList = new Dictionary<NOTETYPE, NoteTypeData>();
        private AudioSource audio;
        private static float offsetSeconds;
        private static float lastBeat, curBeatTime;
<<<<<<< HEAD
        private static bool hasChecked;
=======
>>>>>>> ae8663e27890825f99da0550b67eec12000e619c

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
<<<<<<< HEAD
            secPerBeat = 60f / bpm;
=======
            float secPerBeat = 60f / bpm;
>>>>>>> ae8663e27890825f99da0550b67eec12000e619c
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
<<<<<<< HEAD
            if (noteList.Count != 0 && (noteList[index]+.1f) < audio.time && index < noteList.Count - 1)
=======
            if (noteList.Count != 0 && noteList[index] < audio.time && index < noteList.Count - 1)
>>>>>>> ae8663e27890825f99da0550b67eec12000e619c
            {
                return index + 1;
            }
            return index;
        }

        // for metrenome note indicators
        private int CheckToIncrementIndicator(List<float> noteList, int index)
        {
<<<<<<< HEAD
            if (noteList.Count != 0 && (noteList[index] - offsetSeconds + .1f) < audio.time && index < noteList.Count - 1)
=======
            if (noteList.Count != 0 && noteList[index] - offsetSeconds < audio.time && index < noteList.Count - 1)
>>>>>>> ae8663e27890825f99da0550b67eec12000e619c
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
<<<<<<< HEAD
            float nextBeat = curBeatTime + (60/bpm);
            float time = Time.fixedTime;
            
            //after current beat
            if (Mathf.Abs(curBeatTime - NoteList[type].dataList[NoteList[type].curIndex]) < secPerBeat/2 && time - curBeatTime < hitPadding)
            {
                hasChecked = true;
                return true;
            }
            // before next beat
            if (Mathf.Abs(nextBeat - NoteList[type].dataList[NoteList[type].curIndex]) < secPerBeat/2 && nextBeat - time < hitPadding)
            {
                hasChecked = true;
                return true;
            }
            hasChecked = false;
            return false;
        }

        public static bool IsBossOnBeat(NOTETYPE type)
        {
            float nextBeat = curBeatTime + (60 / bpm);
            if (Mathf.Abs(curBeatTime - NoteList[type].dataList[NoteList[type].curIndex]) < .1f && Time.fixedTime - curBeatTime < hitPadding)
            {
                return true;
            }
            if(Mathf.Abs(nextBeat - NoteList[type].dataList[NoteList[type].curIndex]) < .1f && nextBeat - Time.fixedTime < hitPadding)
            {
                return true;
            }
=======
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
>>>>>>> ae8663e27890825f99da0550b67eec12000e619c
            return false;
        }

        public static bool IsIndicatorOnBeat(NOTETYPE type)
        {
<<<<<<< HEAD
            // type's note time [type index] - offset
            if (Mathf.Abs(Time.time - (NoteList[type].dataList[NoteList[type].indicatorIndex] - offsetSeconds)) < (60/bpm)/2)
            {
                return true;
            }
            else
            {
                //Mathf.Abs(Time.time - (NoteList[type].dataList[NoteList[type].indicatorIndex] - offsetSeconds));
                return false;
            }
            
            
=======
            if (Mathf.Abs(Time.fixedTime - (NoteList[type].dataList[NoteList[type].indicatorIndex] - offsetSeconds)) < hitPadding)
            {
                return true;
            }
            return false;
>>>>>>> ae8663e27890825f99da0550b67eec12000e619c
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