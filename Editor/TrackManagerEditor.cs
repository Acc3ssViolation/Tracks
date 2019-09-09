using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(TrackManager))]
public class TrackManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if(GUILayout.Button("Load demo in editor"))
        {
            ((TrackManager)target).LoadTrackLayout("DemoTrackLayout");
        }

        if(GUILayout.Button("Clear all tracks"))
        {
            ((TrackManager)target).ClearTrackLayout();
        }
    }
}
