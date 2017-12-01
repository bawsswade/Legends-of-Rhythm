using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DanceFloorEditor : EditorWindow {

    public static DanceFloor instance;

    int x, y;
    GameObject g;
    Material m1, m2;

    [MenuItem("Create/Make Floor")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(DanceFloorEditor));
    }
    

    void OnGUI()
    {
        x = EditorGUILayout.IntField("Num Y Tiles", x);
        y = EditorGUILayout.IntField("Num Y Tiles", y);
        g = (GameObject) EditorGUILayout.ObjectField((Object)g, typeof(GameObject), true);

        //instance.tile = (GameObject) EditorGUILayout.ObjectField("Tile", instance.tile, false);

        if (GUILayout.Button("Create"))
        {
            Debug.Log("drawing");
            instance = new DanceFloor();
            instance.DrawTiles(x, y, g);
        }
    }
}
