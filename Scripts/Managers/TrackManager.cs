using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

public class TrackManager : MonoBehaviour
{
    // Data
    public TrackCollection trackCollection;

    // Runtime
    List<GameObject> trackObjects;
    List<GameObject> junctionObjects;

    TrackRenderer renderer;

    // Prefabs
    public Material trackMaterial;
    public Junction junctionPrefab;
    

    void Awake()
    {
        renderer = GetComponent<TrackRenderer>();

        ClearTrackLayout();
    }

    void OnValidate()
    {
        renderer = GetComponent<TrackRenderer>();
        trackCollection = TrackCollection.GetInstance();
    }

    public void ClearTrackLayout()
    {
        var children = new List<GameObject>();
        foreach(Transform child in transform) children.Add(child.gameObject);
        if(Application.isEditor)
        {
            children.ForEach(child => DestroyImmediate(child));
        }
        else
        {
            children.ForEach(child => Destroy(child));
        }
       
        trackCollection.Clear();
        if(trackObjects != null) { trackObjects.Clear(); } else { trackObjects = new List<GameObject>(); }
        if(junctionObjects != null) { junctionObjects.Clear(); } else { junctionObjects = new List<GameObject>(); }
    }

    public void LoadTrackLayout(string assetName)
    {
        ClearTrackLayout();

        TrackLayout trackLayout = (TrackLayout)Resources.Load(assetName, typeof(TrackLayout));
        trackCollection = TrackCollection.GetInstance();
        TrackLayout.PopulateTrackCollection(trackLayout, trackCollection);

        foreach(TrackSection section in trackCollection.sections)
        {
            if(section != null && section.index != 0)
                trackObjects.Add(CreateTrackObject(section));
        }
        foreach(TrackJunction section in trackCollection.junctions)
        {
            if(section != null && section.index != 0)
                junctionObjects.Add(CreateJunctionObject(section));
        }
    }

    /*List<TrackSection> FindConnectedTrackSections(TrackSection start)
    {
        List<TrackSection> list = new List<TrackSection>();
        list.Add(start);
        TrackSection current = start;
        while(current.Next != null && !list.Contains(current.Next))
        {
            current = current.Next;
            list.Add(current);
        }
        return list;
    }*/

    /*public void OnDrawGizmos()
    {
        foreach(TrackSection section in trackSections)
        {
            Vector3 start = section.position.Vector3();
            Vector3 end = section.GetPositionOnTrack(section.length).Vector3();
            Gizmos.DrawLine(start, end);
            Gizmos.DrawCube(start, Vector3.one);
            if(section.Next == null)
            {
                Gizmos.DrawCube(end, Vector3.one);
            }
        }
    }*/

    /// <summary>
    /// TODO: Create wrapper for junctions
    /// </summary>
    /// <param name="junction"></param>
    /// <returns></returns>
    GameObject CreateJunctionObject(TrackJunction junction)
    {
        Junction go = GameObject.Instantiate(junctionPrefab);
        go.Init(junction);
        go.transform.SetParent(transform);

        return go.gameObject;
    }

    GameObject CreateTrackObject(TrackSection section)
    {
        GameObject go = new GameObject("Track Section");
        MeshRenderer mr = go.AddComponent<MeshRenderer>();
        mr.material = trackMaterial;
        MeshFilter filter = go.AddComponent<MeshFilter>();
        filter.mesh = renderer.CreateSectionMesh(section);
        
        go.transform.position = section.position.Vector3();
        go.transform.parent = transform;

        TrackObjectMeta meta = go.AddComponent<TrackObjectMeta>();
        meta.Init(section);

        return go;
    }
}
