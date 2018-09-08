using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Beatz;

public class Split : MonoBehaviour {

    public GameObject prefab;
    public int numSplit;
    public int numBeatLifetime;

    public int moveSpeed;

    private List<GameObject> prefabList = new List<GameObject>();

	// Use this for initialization
	void Start () {

        float angleIncrement = 360f / numSplit;
        float angle = 0;
        // instantiate
        for (int i = 0; i < numSplit; i++)
        {
            GameObject g = Instantiate(prefab, Vector3.zero, Quaternion.Euler(0,angle,0));
            g.transform.localRotation = Quaternion.Euler(0, angle, 0);
            prefabList.Add(g);
            angle += angleIncrement;
        }

        //set the destroy
        float spb = 60f / SongManager.bpm;
        Invoke("DestroyAll", spb * numBeatLifetime);
	}
	
	// Update is called once per frame
	void Update () {
        if (prefabList.Count > 0)
        {
            foreach (GameObject g in prefabList)
            {
                g.transform.position += g.transform.forward * moveSpeed * .1f;
            }
        }
    }

    private void DestroyAll()
    {
        foreach (GameObject g in prefabList)
        {
            Destroy(g);
        }
        prefabList.Clear();
    }
}
