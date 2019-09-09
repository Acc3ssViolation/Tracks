using UnityEngine;
using UnityEditor;
using System.Collections;

public class TrackEditorWindow : EditorWindow
{
    TrackLayout trackLayout;


    [MenuItem("Window/Track Editor")]

    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(TrackEditorWindow));
    }

    void OnGUI()
    {
        // The actual window code goes here
        //string[] layouts = AssetDatabase.FindAssets(string.Format("t:{0}", typeof(TrackLayout)));
    }
}