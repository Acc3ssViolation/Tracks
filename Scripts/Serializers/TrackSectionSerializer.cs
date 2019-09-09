using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[Serializable]
public class TrackSectionSerializer
{
    public WorldPositionSerializer Position;
    public WorldRotationSerializer Rotation;

    public int Index;
    public int PrevIndex;
    public int NextIndex;

    public float Length;
    public bool Curved;
    public float Angle;

    public TrackSectionSerializer(TrackSection track)
    {
        Position = new WorldPositionSerializer(track.position);
        Rotation = new WorldRotationSerializer(track.rotation);

        Index = track.index;
        PrevIndex = track.PreviousSectionIndex;
        NextIndex = track.NextSectionIndex;

        Length = track.length;
        Curved = track.curved;
        Angle = track.angle;
    }

    public TrackSection ToTrackSection()
    {
        TrackSection track = new TrackSection(Position.ToWorldPosition(), Rotation.ToWorldRotation(), Length, Curved, Angle);
        track.index = Index;
        track.NextSectionIndex = NextIndex;
        track.PreviousSectionIndex = PrevIndex;
        return track;
    }
}

