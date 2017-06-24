using UnityEngine;
using System.Collections;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class beatsManager : MonoBehaviour {

    // song stuff
    public float bpm;
    private float secPerBeat;
    public GameObject p_beat;
    public GameObject player;
    public AudioSource audio;

    // tracking player hits
    float curBeatTime= 0;
    float lastBeat = 0;
    public float hitPadding;
    bool hasHitBeat = false;

    // boss attacks
    public SongSO songBeats;
    int songBeatIndex_v = 0;
    int songBeatIndex_b = 0;
    bool hasSpawnedAttack_v = false;
    bool hasSpawnedAttack_b = false;
    // events
    public UnityEvent groundAtkEvent;
    public UnityEvent bassAtkEvent;

    // Use this for initialization
    void Start () {
        // get time in seconds to spawn
        secPerBeat = 60f / bpm;
        InvokeRepeating("SpawnBeat", secPerBeat, secPerBeat);
        //audio.Pause();
	}
	
	// Update is called once per frame
	void Update () {
        secPerBeat = 60f / bpm;
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (IsOnBeat() && !hasHitBeat)
            {
                Debug.Log("hit");
                hasHitBeat = true;
            }
        }
        // Boss Attack: check if song beat matches audio.time
        if (Mathf.Abs(songBeats.vocalNotes[songBeatIndex_v] - audio.time) < .1f && !hasSpawnedAttack_v)
        {
            //Debug.Log("spawn!");
            groundAtkEvent.Invoke();
            hasSpawnedAttack_v = true;
        }
        if (Mathf.Abs(songBeats.bassNotes[songBeatIndex_b] - audio.time) < .1f && !hasSpawnedAttack_b)
        {
            //Debug.Log("spawn!");
            bassAtkEvent.Invoke();
            hasSpawnedAttack_b = true;
        }
    }


    void SpawnBeat()
    {
        //Debug.Log(audio.time);
        // Boss Attack:increase index of song beats to hit
        if (songBeats.vocalNotes[songBeatIndex_v] < audio.time && songBeatIndex_v < songBeats.vocalNotes.Length-1)
        {
            songBeatIndex_v++;
        }
        if (songBeats.bassNotes[songBeatIndex_b] < audio.time && songBeatIndex_b < songBeats.vocalNotes.Length - 1)
        {
            songBeatIndex_b++;
        }
        // enable spawning attacks again
        hasSpawnedAttack_v = false;
        hasSpawnedAttack_b = false;

        // Beat Tracker: instantiate beat ring prefab
        GameObject newBeat = Instantiate(p_beat, player.transform) as GameObject;
        newBeat.transform.localPosition = Vector3.zero;
        Destroy(newBeat, 1f);

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
        if(Time.fixedTime - curBeatTime < hitPadding )
        {
            //Debug.Log(Time.fixedTime - curBeatTime); 
            //Debug.Log("after");
            return true;
        }
        else if(nextBeat - Time.fixedTime < hitPadding) 
        {
            //Debug.Log(nextBeat - Time.fixedTime);
            //Debug.Log("before");
            return true;
        }
        return false;
    }
}
