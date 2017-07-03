using System;
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.AccessControl;
using System.Text;
using Hashbang.Editor.Json;

public class Setup : Editor {

    [MenuItem("Window/Hashbang/CreateGame")]
    public static void CreateGame()
    {
        var json = (TextAsset)AssetDatabase.LoadAssetAtPath("Assets/Packages/GameCore/Editor/Templates/Classes.json", typeof(TextAsset));
        var jsonObj = JsonWrapper.DeserializeObject<ClassJsonObject>(json.text);
        var assetPath = Application.dataPath;

        //Create Folders for Files.
        foreach (var folder in jsonObj.Folders)
        {

            var assetFullPath = string.Format("{0}/{1}", assetPath, folder.Path);
            var path = folder.Path;
            var splitPaths = path.Split('/').ToList();
            var newFolder = splitPaths.Last();
            var baseSb = new StringBuilder();
            splitPaths.Remove(newFolder);
            baseSb.Append("Assets");
            foreach (var p in splitPaths)
            {
                baseSb.AppendFormat("/{0}", p);
            }

            //Debug.Log(newFolder);
            //Debug.Log(baseSb.ToString());
            if (!Directory.Exists(assetFullPath))
            {
                AssetDatabase.CreateFolder(baseSb.ToString(), newFolder);
            }
        }
        AssetDatabase.Refresh();

        foreach (var c in jsonObj.Classes)
        {
            if (!File.Exists(string.Format("{0}{1}", assetPath, c.Path)))
            {
                var sb = new StringBuilder();
                //Append Using statements
                foreach (var u in c.Using)
                {
                    sb.AppendLine(u);
                }
                sb.AppendLine("");
                //Build class opening
                sb.AppendLine(string.Format("public partial class {0} : {1} {{", c.Name, c.Extends));
                foreach (var p in c.Properties)
                {
                    if (p.IsProperty)
                    {
                        sb.AppendLine(string.Format("   {0} {1} {2} {{get; set;}}", p.Access, p.Type, p.Name));
                    }
                    else
                    {
                        sb.AppendLine(string.Format("   {0} {1} {2};", p.Access, p.Type, p.Name));
                    }
                }
                sb.AppendLine("");
                foreach (var m in c.Methods)
                {
                    var sbMethod = new StringBuilder();
                    sbMethod.AppendFormat("    {0} override {1} {2} (", m.Access, m.ReturnType, m.Name);
                    foreach (var arg in m.Arguments)
                    {
                        if (m.Arguments.Last() == arg)
                        {
                            sbMethod.AppendFormat("{0} {1}", arg.Type, arg.Name);
                        }
                        else
                        {
                            sbMethod.AppendFormat("{0} {1}, ", arg.Type, arg.Name);
                        }
                    }
                    sbMethod.Append(") {");
                    sb.AppendLine(sbMethod.ToString());

                    foreach (var content in m.Content)
                    {
                        sb.AppendLine(string.Format("        {0}", content));
                    }
                    sb.AppendLine("    }");
                    sb.AppendLine("");
                }

                //End Class
                sb.AppendLine("}");
                File.WriteAllText(string.Format("{0}{1}",assetPath,c.Path), sb.ToString());
            }
        }
        AssetDatabase.Refresh();

        SetupGame();
    }

    private static void SetupGame()
    {
        var mainContext = FindObjectOfType<MainContextView>();
        GameObject gameObject = null;
        if (mainContext == null)
        {
            gameObject = new GameObject { name = "GameCore" };
            mainContext = gameObject.AddComponent<MainContextView>();
        }
        else
        {
            gameObject = mainContext.gameObject;
        }

        gameObject.AddComponent<WindowManager>();

        var gameManager = FindObjectOfType<GameManager>();
        GameObject gmGameObject = null;
        if (gameManager == null)
        {
            gmGameObject = new GameObject() {name = "Game"};
            gameManager = gmGameObject.AddComponent<GameManager>();
            gmGameObject.transform.SetParent(mainContext.transform);
            var gameData = gmGameObject.AddComponent<GameData>();
            gameManager.GameData = gameData;
        }
    }
}
