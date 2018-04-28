using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[Serializable]
public class WorldRotationSerializer
{
    public double Radians;          // Just store the angle in radians

    public WorldRotationSerializer(WorldRotation rot)
    {
        Radians = rot.Radians;
    }

    public WorldRotation ToWorldRotation()
    {
        return new WorldRotation(Radians);
    }
}

[Serializable]
public class WorldPositionSerializer
{
    public WorldPosition Position;  // WorldPosition is serializable anyway

    public WorldPositionSerializer(WorldPosition pos)
    {
        this.Position = pos;
    }

    public WorldPosition ToWorldPosition()
    {
        return Position;
    }
}

