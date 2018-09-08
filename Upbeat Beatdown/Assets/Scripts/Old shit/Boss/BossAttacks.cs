using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Beatz;

public class BossAttacks : MonoBehaviour {

    private SongManager sm;

    private player_abilities abilities;
    public bool isNoteSyncActive = false;

    public Transform IndicatorContainer;
    public GameObject Atk1, Atk2;

	// Use this for initialization
	void Start () {
        abilities = FindObjectOfType<player_abilities>();
        abilities.OnEnterNoteSync += EnableBossAttacks;
        abilities.OnExitNoteSync += DisableBossAttacks;

        sm = FindObjectOfType<SongManager>();

        InvokeRepeating("Increment", 0, 60f / SongManager.bpm);
	}

    private void Increment()
    {
        if (isNoteSyncActive)
        {
            if (sm.GetShouldSpawnNote(NoteType.TLeft, 4))
            {
                GameObject g =  Instantiate(Atk1, IndicatorContainer);
                g.GetComponent<Image>().color = Color.red;
            }
            if (sm.GetShouldSpawnNote(NoteType.TRgiht, 4))
            {
                GameObject g = Instantiate(Atk2, IndicatorContainer);
                g.GetComponent<Image>().color = Color.blue;
            }
        }
    }

    private void EnableBossAttacks()
    {
        isNoteSyncActive = true;
    }

    private void DisableBossAttacks()
    {
        isNoteSyncActive = false;
    }
}
