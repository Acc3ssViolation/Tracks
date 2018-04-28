using UnityEngine;
using System.Collections;

public class TrackObjectMeta : MonoBehaviour
{
    public int trackId;

    public void Init(TrackSection section)
    {
        trackId = section.index;
    }
}
