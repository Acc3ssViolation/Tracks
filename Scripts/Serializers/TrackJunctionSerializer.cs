using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[Serializable]
public class TrackJunctionSerializer
{
    public WorldPositionSerializer Position;
    public WorldRotationSerializer Rotation;

    public int Index;
    public int PrevIndex;
    public int[] SectionIndices;
    //public int ActiveSectionArrayIndex;

    public TrackJunctionSerializer(TrackJunction junction)
    {
        Position = new WorldPositionSerializer(junction.position);
        Rotation = new WorldRotationSerializer(junction.rotation);

        Index = junction.index;
        PrevIndex = junction.PreviousSectionIndex;
        SectionIndices = junction.Sections.ToArray();
    }

    public TrackJunction ToTrackJunction()
    {
        TrackJunction junction = new TrackJunction(Position.ToWorldPosition(), Rotation.ToWorldRotation(), SectionIndices);
        junction.index = Index;
        junction.PreviousSectionIndex = PrevIndex;
        return junction;
    }
}

