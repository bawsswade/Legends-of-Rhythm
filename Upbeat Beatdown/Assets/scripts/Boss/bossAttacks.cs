using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class bossAttacks : MonoBehaviour {
    public GameObject groundAtk;
    public GameObject airAtk;
    public GameObject aoeAtk;

    //public UnityEvent groundAtkEvent = new UnityEvent();

    // Use this for initialization
    void Start ()
    {
        //groundAtkEvent.AddListener(SpawnGround);
    }
	
	// Update is called once per frame
	void Update ()
    {
	    if(Input.GetKeyDown(KeyCode.Keypad1))
        {
            SpawnGround();
        }

        if(Input.GetKeyDown(KeyCode.Keypad2))
        {
            SpawnAir();
        }

        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            SpawnAOE();
        }

        
	}

    public void SpawnGround()
    {
        Vector3 v = new Vector3(0, Random.Range(0.0f, 360.0f), 0);

        Vector3 p = new Vector3(gameObject.transform.position.x, 0, gameObject.transform.position.z);
        GameObject g = Instantiate(groundAtk, p, Quaternion.Euler(v)) as GameObject;
        //Destroy(g, 3f);
    }

    public void SpawnAir()
    {
        Vector3 v = new Vector3(Random.Range(0.0f, 180.0f), 0, Random.Range(0.0f, 360.0f));
        Vector3 p = new Vector3(gameObject.transform.position.x, 3.0f, gameObject.transform.position.z);
        GameObject g = Instantiate(airAtk, p, Quaternion.Euler(v)) as GameObject;
        Destroy(g, 15f);
    }

    public void SpawnAOE()
    {
        //Vector3 v = new Vector3(-90.0f, 0, Random.Range(0.0f, 360.0f));
        Vector3 p = new Vector3(gameObject.transform.position.x, 0, gameObject.transform.position.z);
        GameObject g = Instantiate(aoeAtk, p, Quaternion.Euler(Vector3.zero)) as GameObject;
        Destroy(g, 5f);
    }
}
