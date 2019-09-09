using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

public class ConsistEditor
{
    [MenuItem("Assets/Create/Consist")]
    public static void CreateConsist()
    {
        Consist asset = ScriptableObject.CreateInstance<Consist>();

        AssetDatabase.CreateAsset(asset, "Assets/Resources/Consists/NewConsist.asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = asset;
    }

    [MenuItem("Assets/Create/Track Layout Demo File")]
    public static void CreateTrackLayout()
    {
        TrackCollection trackCollection = TrackCollection.GetInstance();

        /*
        TrackSection a = new TrackSection(new WorldPosition(0, 0, 20f, 20f), new WorldRotation(25, false), 750f, true, 90f);
        trackCollection.Add(a);
        TrackSection b = a.CreateNext(750f, true, 90f);
        b = b.CreateNext(750f, true, 90f);
        b = b.CreateNext(750f, true, 90f);

        b.NextSectionIndex = a.index;
        a.PreviousSectionIndex = b.index;
        */




        // Create start section
        TrackSection start = new TrackSection(new WorldPosition(0, 0, 20f, 10f), new WorldRotation(-100.0f, false), 200.0f);
        trackCollection.Add(start);
        TrackSection currentSection = start;

        currentSection = currentSection.CreateNext(100.0f, false, 0.0f);

        // Create a new junction
        TrackJunction junction = new TrackJunction(currentSection.GetPositionOnTrack(currentSection.length), currentSection.GetRotationOnTrack(currentSection.length));
        trackCollection.Add(junction);
        // Attach it to the previous section
        junction.PreviousSectionIndex = currentSection.index;
        currentSection.NextSectionIndex = junction.index;

        // Create the junction sections
        TrackSection switchSectionCurved = junction.CreateSection(200.0f, true, 90.0f);
        // 127.32395447351626861510701069801
        TrackSection switchSectionStraight = junction.CreateSection(2.0f * 127.32395f, false, 0.0f);
        // Create a loop
        currentSection = switchSectionStraight;
        // NOTE: It's split into two parts because junction sections from the same junction need at least 2 other section before they connect to eachother
        currentSection = currentSection.CreateNext(200.0f, true, 90.0f);
        currentSection = currentSection.CreateNext(400.0f, true, 180.0f);
        // Join it up
        currentSection.NextSectionIndex = switchSectionCurved.index;
        switchSectionCurved.NextSectionIndex = currentSection.index;

        // Create a new piece of track on the other end of the start piece
        currentSection = new TrackSection(start.position, start.rotation.Oppisite, 20.0f);
        currentSection.PreviousSectionIndex = start.index;
        start.PreviousSectionIndex = trackCollection.Add(currentSection);

        // Create a new junction
        junction = new TrackJunction(currentSection.GetPositionOnTrack(currentSection.length), currentSection.GetRotationOnTrack(currentSection.length));
        trackCollection.Add(junction);

        // Attach it to the previous section
        junction.PreviousSectionIndex = currentSection.index;
        currentSection.NextSectionIndex = junction.index;
        // Create the junction sections
        switchSectionCurved = junction.CreateSection(200.0f, true, 90.0f);
        //127.32395447351626861510701069801
        switchSectionStraight = junction.CreateSection(2.0f * 127.32395f, false, 0.0f);

        // Create a loop
        currentSection = switchSectionStraight;
        // NOTE: It's split into two parts because junction sections from the same junction need at least 2 other section before they connect to eachother
        // Add a junction and attach it
        junction = new TrackJunction(currentSection.GetPositionOnTrack(currentSection.length), currentSection.GetRotationOnTrack(currentSection.length));
        trackCollection.Add(junction);

        junction.PreviousSectionIndex = currentSection.index;
        currentSection.NextSectionIndex = junction.index;
        // Create the curved loop junction section
        currentSection = junction.CreateSection(200.0f, true, 90.0f);
        // Create the straight junction section
        TrackSection branch = junction.CreateSection(200.0f, false, 0.0f);
        // Finish the loop
        currentSection = currentSection.CreateNext(400.0f, true, 180.0f);
        // Join it up
        currentSection.NextSectionIndex = switchSectionCurved.index;
        switchSectionCurved.NextSectionIndex = currentSection.index;

        branch = branch.CreateNext(400, true, 10);
        branch = branch.CreateNext(100, false, 0);
        branch = branch.CreateNext(500, true, -20);
        branch = branch.CreateNext(1000, false, 0);







        //Save it

        TrackLayout trackLayout = TrackLayout.CreateFromTrackCollection(trackCollection);

        AssetDatabase.CreateAsset(trackLayout, "Assets/Resources/DemoTrackLayout.asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = trackLayout;
        
    }
}