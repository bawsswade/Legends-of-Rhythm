using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;

public class DanceFloor : MonoBehaviour {

    public List<GameObject> floorMat = new List<GameObject>();
    public Material m1, m2;
    bool isFirstMat = true;

    private void Start()
    {
        foreach (Transform child in gameObject.transform)
        {
            //Material m = child.GetComponent<Material>();
            floorMat.Add(child.gameObject);
        }

        for(int i = 0; i < floorMat.Count -1; i++)
        {
            if (isFirstMat)
            {
                floorMat[i].GetComponent<Renderer>().material = m1;
            }
            else
            {
                floorMat[i].GetComponent<Renderer>().material = m2;
            }
            isFirstMat = !isFirstMat;
        }
    }

    public void DrawTiles(int w, int l, GameObject g)
    {
        GameObject parent = new GameObject();
        parent.name = "Floor";

        for (int i = 0; i < w - 1; i++)
        {
            for (int j = 0; j < l - 1; j++)
            {
                GameObject go = (GameObject)Instantiate(g, new Vector3(i*5, 0, j*5), Quaternion.identity, parent.transform);
                Material m = go.GetComponent<Material>();
                m = m1;
            }
        }
    }
}
