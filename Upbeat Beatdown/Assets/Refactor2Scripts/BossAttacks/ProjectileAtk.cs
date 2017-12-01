using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAtk : MonoBehaviour {

    public GameObject groundAtk;
    public bool Four, Circle, One, Arc;

    /*public override void BassAtk()
    {
        Debug.Log("projectile bass attack");
        base.BassAtk();
    }

    public override void SnareAtk()
    {
        Debug.Log("projectile snare attack");
        base.SnareAtk();
    }

    public override void MelodyAtk()
    {
        Debug.Log("projectile melody attack");
        base.MelodyAtk();
    }*/

    private void Start()
    {
        if (Four) { Spawn4Ground(); }
        if (Circle) { SpawnCircle(); }
        if (One) { SpawnGround(); }
        if (Arc) { SpawnArcGround(); }
    }

    public void Spawn4Ground()
    {
        Vector3 v = new Vector3(0, UnityEngine.Random.Range(0.0f, 360.0f), 0);
        StartCoroutine(SpawnMultGround(4, v));
    }

    IEnumerator SpawnMultGround(int numTimes, Vector3 v)
    {
        int count = 0;
        while (count < numTimes)
        {
            Vector3 p = new Vector3(gameObject.transform.position.x, 0, gameObject.transform.position.z);
            GameObject g = Instantiate(groundAtk, p, Quaternion.Euler(v)) as GameObject;
            count++;
            Destroy(g,10);
            yield return new WaitForSeconds(.42f);
        }
    }

    public void SpawnCircle()
    {
        float increment = 0;
        float num = 8;
        for (int i = 0; i < num; i++)
        {
            Vector3 v = new Vector3(0, increment, 0);
            Vector3 p = new Vector3(gameObject.transform.position.x, 0, gameObject.transform.position.z);
            GameObject g = Instantiate(groundAtk, p, Quaternion.Euler(v)) as GameObject;
            Destroy(g, 10);
            increment += 360 / num;
        }
    }

    public void SpawnGround()
    {
        Vector3 v = new Vector3(0, UnityEngine.Random.Range(0.0f, 360.0f), 0);

        Vector3 p = new Vector3(gameObject.transform.position.x, 0, gameObject.transform.position.z);
        GameObject g = Instantiate(groundAtk, p, Quaternion.Euler(v)) as GameObject;
        Destroy(g, 10);
        //Destroy(g, 3f);
    }

    public void SpawnArcGround()
    {
        Vector3 v = new Vector3(0, UnityEngine.Random.Range(0.0f, 360.0f), 0);
        Vector3 p = new Vector3(gameObject.transform.position.x, 0, gameObject.transform.position.z);
        GameObject g1 = Instantiate(groundAtk, p, Quaternion.Euler(v));
        GameObject g2 = Instantiate(groundAtk, p, Quaternion.Euler(0, v.y - 15, 0));
        GameObject g3 = Instantiate(groundAtk, p, Quaternion.Euler(0, v.y + 15, 0));

        Destroy(g1, 10);
        Destroy(g2, 10);
        Destroy(g3, 10);
    }
}
