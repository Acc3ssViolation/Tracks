using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A driver controls input for a single or a set of engines (TrainEngineController)
/// </summary>
public class Driver : MonoBehaviour
{
    public List<TrainEngineController> Engines;

    public List<TrainController> trainVehicles;
    protected int reverser;
    protected float brake;
    protected float throttle;

    protected UnityEngine.UI.Text speedText;

    public void Awake()
    {
        Engines = new List<TrainEngineController>();
        trainVehicles = new List<TrainController>();
        reverser = 0;
        brake = 0;
        throttle = 0;
    }

    public virtual void Start()
    {
        GameObject.Find("Throttle Slider").GetComponent<UnityEngine.UI.Slider>().onValueChanged.AddListener(delegate { SetThrottle(GameObject.Find("Throttle Slider").GetComponent<UnityEngine.UI.Slider>()); });
        GameObject.Find("Reverser Slider").GetComponent<UnityEngine.UI.Slider>().onValueChanged.AddListener(delegate { SetReverser(GameObject.Find("Reverser Slider").GetComponent<UnityEngine.UI.Slider>()); });
        GameObject.Find("Brake Slider").GetComponent<UnityEngine.UI.Slider>().onValueChanged.AddListener(delegate { SetBrake(GameObject.Find("Brake Slider").GetComponent<UnityEngine.UI.Slider>()); });
        speedText = GameObject.Find("Speed Display").GetComponent<UnityEngine.UI.Text>();
    }

    public virtual void Update()
    {
        if(Engines.Count > 0)
        {
            speedText.text = (Engines[0].Velocity * 3.6f).ToString("#.##") + "km/h";
        }
    }

    /// <summary>
    /// Adds an engine that we can control
    /// </summary>
    /// <param name="engine"></param>
    public void AddEngine(TrainEngineController engine)
    {
        Engines.Add(engine);
        RecalculateTrain();
    }

    /// <summary>
    /// Removes an engine we are controlling
    /// </summary>
    /// <param name="engine"></param>
    public void RemoveEngine(TrainEngineController engine)
    {
        Engines.Remove(engine);
        RecalculateTrain();
    }

    /// <summary>
    /// Updates internal list of train vehicles.
    /// </summary>
    public void RecalculateTrain()
    {
        HashSet<TrainController> uniqueVehicles = new HashSet<TrainController>();
        if(Engines.Count > 0)
        {
            TrainController vehicle = Engines[0];
            uniqueVehicles.Add(vehicle);
            while(vehicle.next != null)
            {
                vehicle = vehicle.next;
                uniqueVehicles.Add(vehicle);
            }
            vehicle = Engines[0];
            while(vehicle.previous != null)
            {
                vehicle = vehicle.previous;
                uniqueVehicles.Add(vehicle);
            }
        }
        foreach(TrainController vehicle in trainVehicles)
        {
            vehicle.driver = null;
        }

        trainVehicles.Clear();
        trainVehicles.AddRange(uniqueVehicles);

        foreach(TrainController vehicle in uniqueVehicles)
        {
            vehicle.driver = this;
        }
    }

    // UI Things

    public void SetReverser(UnityEngine.UI.Slider value)
    {
        reverser = (int)value.value;

        foreach(TrainEngineController engine in Engines)
        {
            engine.SetReverser(reverser);
        }
    }

    public void SetThrottle(UnityEngine.UI.Slider value)
    {
        throttle = value.value;

        foreach(TrainEngineController engine in Engines)
        {
            engine.SetThrottle(throttle);
        }
    }

    public void SetBrake(UnityEngine.UI.Slider value)
    {
        brake = value.value;

        foreach(TrainController vehicle in trainVehicles)
        {
            vehicle.SetBrake(brake);
        }
    }
}
