using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantAtk : MonoBehaviour {
    public GameObject Beam;
    public GameObject AoeCircle;
    public GameObject AoeCircleBurst;

    public bool isBeam, isCircle, isCircleBurst;
    GameObject player;
	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        if(isBeam)SpawnInstantLine();
        if (isCircle) SpawnInstant();
        if (isCircleBurst) SpawnAOE();
	}

    public void SpawnInstant()
    {
        Vector3 p = new Vector3(player.transform.position.x + UnityEngine.Random.Range(-5, 5), -1.2f, player.transform.position.z + UnityEngine.Random.Range(-5, 5));
        GameObject g = Instantiate(AoeCircle, p, Quaternion.Euler(90, 0, 0)) as GameObject; 
        Destroy(g, 2f);
    }

    public void SpawnInstantLine()
    {
        Vector3 p = new Vector3(player.transform.position.x + UnityEngine.Random.Range(-5, 5), -1.2f, player.transform.position.z + UnityEngine.Random.Range(-5, 5));
        GameObject g = Instantiate(Beam, transform.position, Quaternion.LookRotation(new Vector3(p.x, 0, p.z))) as GameObject;
        Destroy(g, 2f);
    }

    public void SpawnAOE()
    {
        //Vector3 v = new Vector3(-90.0f, 0, Random.Range(0.0f, 360.0f));
        Vector3 p = new Vector3(transform.position.x, 0, transform.position.z);
        GameObject g = Instantiate(AoeCircleBurst, transform.position, Quaternion.Euler(p)) as GameObject;
        Destroy(g, 1f);
    }
}
