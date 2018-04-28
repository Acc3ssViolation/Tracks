using UnityEngine;
using System.Collections;

/// <summary>
/// Loading manager that handles world load order and initialization
/// </summary>
public class LoadingManager : MonoBehaviour
{
    public TrackManager trackManager;
    public TrainManager trainManager;

    /// <summary>
    /// Finds all required scripts
    /// </summary>
    public void Awake()
    {
        trackManager = GameObject.Find("Track Manager").GetComponent<TrackManager>();
        trainManager = GameObject.Find("Train Manager").GetComponent<TrainManager>();
    }

    /// <summary>
    /// Starts loading
    /// </summary>
    public void Start()
    {
        //Load();
    }

    /// <summary>
    /// Loads demo
    /// </summary>
    public void LoadDemo()
    {
        trackManager.LoadTrackLayout("DemoTrackLayout");
        trainManager.SpawnDemoConsists();

        //Set camera to player train
        Camera.main.GetComponent<MouseOrbit>().target = trainManager.trains[0].transform;
    }
}
