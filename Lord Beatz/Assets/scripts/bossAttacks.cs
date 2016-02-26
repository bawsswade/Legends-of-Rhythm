using UnityEngine;
using System.Collections;

public class bossAttacks : MonoBehaviour {
    public GameObject groundAtk;
    public GameObject airAtk;
    public GameObject aoeAtk;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if(Input.GetKeyDown(KeyCode.Keypad1))
        {
            Vector3 v = new Vector3(-90.0f, Random.Range(0.0f, 360.0f), Random.Range(0.0f, 180.0f));
            Vector3 p = new Vector3(gameObject.transform.position.x, 0, gameObject.transform.position.z);
            GameObject g = Instantiate(groundAtk, p, Quaternion.Euler(v)) as GameObject;
            Destroy(g, 1f);
        }

        if(Input.GetKeyDown(KeyCode.Keypad2))
        {
            Vector3 v = new Vector3(Random.Range(0.0f, 180.0f), 0, Random.Range(0.0f, 360.0f));
            Vector3 p = new Vector3(gameObject.transform.position.x, 3.0f, gameObject.transform.position.z);
            GameObject g = Instantiate(airAtk, p, Quaternion.Euler(v)) as GameObject;
            //Destroy(g, 8f);
        }

        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            Vector3 v = new Vector3(-90.0f, 0, Random.Range(0.0f, 360.0f));
            Vector3 p = new Vector3(gameObject.transform.position.x, 0, gameObject.transform.position.z);
            GameObject g = Instantiate(aoeAtk, p, Quaternion.Euler(v)) as GameObject;
            Destroy(g, 3f);
        }
	}
}
