using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrainManager : MonoBehaviour
{
    public Consist consist;
    public Consist consist2;

    /// <summary>
    /// List of all lead vehicles of all trains.
    /// </summary>
    public List<TrainController> trains { get; private set; }

    public void Awake()
    {
        trains = new List<TrainController>();
    }

    public void SpawnDemoConsists()
    {
        TrainController leadCar = consist.Spawn(TrackCollection.instance.Get(1));
        trains.Add(leadCar);

        leadCar = consist2.Spawn(TrackCollection.instance.Get(15));
        trains.Add(leadCar);
    }
}
