using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Track layout describes all tracks in the world. It can be loaded by a TrackManager.
/// </summary>
public class TrackLayout : ScriptableObject
{
    public TrackSectionSerializer[] trackSections;
    public TrackJunctionSerializer[] trackJunctions;

    private TrackLayout()
    {
    }

    public static void PopulateTrackCollection(TrackLayout layout, TrackCollection target)
    {
        target.Clear();
        foreach(TrackSectionSerializer serializedTrack in layout.trackSections)
        {
            TrackSection track = serializedTrack.ToTrackSection();
            if(!target.Add(track, track.index))
            {
                Debug.LogError("Could not add track section to collection at index " + track.index);
            }
        }

        foreach(TrackJunctionSerializer serializedJunction in layout.trackJunctions)
        {
            TrackJunction track = serializedJunction.ToTrackJunction();
            if(!target.Add(track, track.index))
            {
                Debug.LogError("Could not add track junction to collection at index " + track.index);
            }
        }
    }

    public static TrackLayout CreateFromTrackCollection(TrackCollection tc)
    {
        HashSet<TrackSectionSerializer> sectionList = new HashSet<TrackSectionSerializer>();
        HashSet<TrackJunctionSerializer> junctionList = new HashSet<TrackJunctionSerializer>();

        for(int i = 0; i < tc.sections.Length; i++)
        {
            if(tc.sections[i] != null)
            {
                TrackJunction j = tc.sections[i] as TrackJunction;
                if(j != null)
                {
                    junctionList.Add(new TrackJunctionSerializer(j));
                }
                else
                {
                    sectionList.Add(new TrackSectionSerializer(tc.sections[i]));
                }
            }
        }

        TrackLayout layout = ScriptableObject.CreateInstance<TrackLayout>();

        layout.trackSections = new TrackSectionSerializer[sectionList.Count];
        sectionList.CopyTo(layout.trackSections);
        layout.trackJunctions = new TrackJunctionSerializer[junctionList.Count];
        junctionList.CopyTo(layout.trackJunctions);

        return layout;
    }
}
