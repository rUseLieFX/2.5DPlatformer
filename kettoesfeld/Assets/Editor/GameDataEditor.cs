using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GameDataEditor : EditorWindow
{
    [MenuItem("Window/GameData")]
    
    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        GetWindow(typeof(GameDataEditor));
        // Igen, dokument�ci�b�l loptam. Kell ahogy, hogy megjelenhessen a Window r�szn�l.
    }
    
    private void OnGUI()
    {
        for (int i = 0; i < GameData.maps.Length; i++)
        {
            GUILayout.Label($"EP{i+1}: {GameData.maps[i]}");
        }
    }
}
