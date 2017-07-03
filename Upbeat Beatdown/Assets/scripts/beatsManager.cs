using UnityEngine;
using System.Collections;
using UnityEngine.Events;


//[RequireComponent(typeof(AudioSource))]
public class beatsManager : MonoBehaviour
{
    // song stuff
    public float bpm;
    private float secPerBeat;
    public GameObject p_beat;
    public GameObject player;
    public AudioSource audio;

    // tracking player hits
    float curBeatTime = 0;
    float lastBeat = 0;
    public float hitPadding;
    bool hasHitBeat = false;

    // boss attacks
    public SongSO songBeats;
    int songBeatIndex_r = 0;
    int songBeatIndex_b = 0;
    int songBeatIndex_v = 0;
    bool hasSpawnedAttack_v = false;
    public bool hasSpawnedAttack_r = false;
    bool hasSpawnedAttack_b = false;
    // events
    public UnityEvent groundAtkEvent;
    public UnityEvent bassAtkEvent;

    // Use this for initialization
    void Start()
    {
        // get time in seconds to spawn
        secPerBeat = 60f / bpm;
        InvokeRepeating("SpawnBeat", secPerBeat, secPerBeat);
        //audio.Pause();
    }

    // Update is called once per frame
    void Update()
    {
        secPerBeat = 60f / bpm;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (IsOnBeat() && !hasHitBeat)
            {
                Debug.Log("hit");
                hasHitBeat = true;
            }
        }

    }


    void SpawnBeat()
    {
        // Boss Attack: check if song beat matches audio.time
        if (songBeats.GetRegNotesCount() != 0 && Mathf.Abs(songBeats.GetRegNotes(songBeatIndex_r) - audio.time) < .1f && !hasSpawnedAttack_r)
        {
            //Debug.Log("spawn!");
            groundAtkEvent.Invoke();
            hasSpawnedAttack_r = true;
        }
        if (songBeats.getBassNotesCount() != 0 && Mathf.Abs(songBeats.GetBassNotes(songBeatIndex_b) - audio.time) < .1f && !hasSpawnedAttack_b)
        {
            //Debug.Log("spawn!");
            bassAtkEvent.Invoke();
            hasSpawnedAttack_b = true;
        }
        /*if (songBeats.vocalNotes.Count != 0 && Mathf.Abs(songBeats.vocalNotes[songBeatIndex_v] - audio.time) < .1f && !hasSpawnedAttack_v)
        {
            // replace ground attack with different kind later
            groundAtkEvent.Invoke();
            hasSpawnedAttack_v = true;
        }*/

        //Debug.Log(audio.time);
        // Boss Attack:increase index of song beats to hit
        if (songBeats.GetRegNotesCount() != 0 && songBeats.GetRegNotes(songBeatIndex_r) < audio.time && songBeatIndex_r < songBeats.GetRegNotesCount() - 1)
        {
            songBeatIndex_r++;
        }
        if (songBeats.getBassNotesCount() != 0 && songBeats.GetBassNotes(songBeatIndex_b) < audio.time && songBeatIndex_b < songBeats.getBassNotesCount() - 1)
        {
            songBeatIndex_b++;
        }
        /*if (songBeats.vocalNotes.Count != 0 && songBeats.vocalNotes[songBeatIndex_v] < audio.time && songBeatIndex_v < songBeats.vocalNotes.Count - 1)
        {
            songBeatIndex_v++;
        }*/
        // enable spawning attacks again
        hasSpawnedAttack_r = false;
        hasSpawnedAttack_b = false;
        hasSpawnedAttack_v = false;

        // Beat Tracker: instantiate beat ring prefab
        GameObject newBeat = Instantiate(p_beat, player.transform) as GameObject;
        newBeat.transform.localPosition = new Vector3(0, 0, 0);
        Destroy(newBeat, 5f);

        // Player: update current beat for player
        hasHitBeat = false;
        lastBeat = curBeatTime;
        curBeatTime = Time.fixedTime;


    }

    // Player: check for attacking on beat
    public bool IsOnBeat()
    {
        float nextBeat = curBeatTime + curBeatTime - lastBeat;

        // after beat || before beat
        if (Time.fixedTime - curBeatTime < hitPadding)
        {
            //Debug.Log(Time.fixedTime - curBeatTime); 
            //Debug.Log("after");
            return true;
        }
        else if (nextBeat - Time.fixedTime < hitPadding)
        {
            //Debug.Log(nextBeat - Time.fixedTime);
            //Debug.Log("before");
            return true;
        }
        return false;
    }

    // for creating beat hits for song
    public float SaveBeat()
    {
        float nextBeat = curBeatTime + curBeatTime - lastBeat;

        // after beat 
        if (Time.fixedTime - curBeatTime < nextBeat - Time.fixedTime)
        {
            return curBeatTime;
        }
        // before beat
        else if (nextBeat - Time.fixedTime < Time.fixedTime - curBeatTime)
        {
            return nextBeat;
        }
        return 0;
    }
}
