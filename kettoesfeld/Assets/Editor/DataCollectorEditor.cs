using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[CustomEditor(typeof(DataCollector))]
public class DataCollectorEditor : Editor
{
    int ep;
    int map;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();


        GUILayout.Space(15);

        ep = EditorGUILayout.IntField(new GUIContent("Episode","Hanyadik epiz�dhoz akarsz �rni? (-1)"),ep);
        map = EditorGUILayout.IntField(new GUIContent("Map", "Hanyadik mapig akarod a felold�st?"), map);
        GUILayout.Label(new GUIContent($"e{ep+1}_m{map}", $"Az {ep+1}. epiz�dban, a {map} sz�m� maphoz ad hozz�f�r�st."));
        
        if (GUILayout.Button(new GUIContent("Savefile fel�l�r�sa.")))
        {
            GameData.maps[ep] = map;
            string destination = Application.persistentDataPath + "/save.dat";
            FileStream file;

            if (File.Exists(destination)) file = File.OpenWrite(destination);
            else file = File.Create(destination);

            int[] data = GameData.maps;
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(file, data);
            file.Close();
        }



        if (GUILayout.Button(new GUIContent("Savefile t�rl�se.", "bruh."))){
            string destination = Application.persistentDataPath + "/save.dat";
            if (File.Exists(destination))
            {
                File.Delete(destination);
                Debug.Log("F�jl t�r�lve!");
            }
            else Debug.Log("Nem tal�ltam meg a f�jlt.");
        }
    }
}
