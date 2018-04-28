using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A driver controls input for a single or a set of engines (TrainEngineController)
/// </summary>
public class AIDriver : Driver
{
    private bool isBraking = false;

    public void Update()
    {
        base.Update();
        if(Engines.Count > 0)
        {
            SetReverser(Direction.Forward);

            float velocity = Engines[0].Velocity * (float)Engines[0].MoveDirection;
            if(!isBraking)
            {
                if(velocity < 80/3.6)
                {
                    SetThrottle(1.0f);
                }
                else
                {
                    SetThrottle(0.0f);
                    //Engines[0].SetBrake(1.0f);
                    //isBraking = true;
                }
            }
            else
            {
                if(Mathf.Abs(velocity) < 0.1f)
                {
                    isBraking = false;
                    Engines[0].SetBrake(0.0f);
                }
            }
        }
    }

    private void SetReverser(Direction value)
    {
        reverser = (int)value;

        foreach(TrainEngineController engine in Engines)
        {
            engine.SetReverser(reverser);
        }
    }

    private void SetThrottle(float value)
    {
        throttle = value;

        foreach(TrainEngineController engine in Engines)
        {
            engine.SetThrottle(throttle);
        }
    }

    private void SetBrake(float value)
    {
        brake = value;

        foreach(TrainController vehicle in trainVehicles)
        {
            vehicle.SetBrake(brake);
        }
    }
}
