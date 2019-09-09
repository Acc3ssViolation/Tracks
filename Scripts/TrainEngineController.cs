using UnityEngine;
using System.Collections;

public class TrainEngineController : TrainController
{
    protected float throttle = 0.0f;
    protected int reverser = 0;
    protected float brake = 0.0f;

    public float Throttle { get { return throttle; } }
    public int   Reverser { get { return reverser; } }
    public float Brake { get { return brake; } }


    // Engine parameters

    [Tooltip("Maximum tractive effort in kN")]
    public float peakTractiveEffort = 400.0f;
    [Tooltip("Maximum speed in km/h")]
    public float maxSpeed = 120f;
    [Tooltip("Curve of tractive effort, normalized")]
    public AnimationCurve tractiveEffortCurve = AnimationCurve.EaseInOut(0.0f, 1.0f, 1.0f, 0.2f);
    [Tooltip("Drive wheel diameter in meters")]
    public float wheelDiameter = 1.0f;
    

    /// <summary>
    /// Returns tractive effort in N
    /// </summary>
    /// <returns></returns>
    public float GetTractiveForce()
    {
        if(_velocity * 3.6f > maxSpeed)
            return 0;

        //RPM formula: v = r × RPM × 0.10472, v in m/s, r in m
        float curveValue = Mathf.Clamp01((_velocity * 3.6f) / maxSpeed);
        return peakTractiveEffort * throttle * reverser * tractiveEffortCurve.Evaluate(curveValue);
    }


    public void SetReverser(int value)
    {
        reverser = (int)value;
    }

    public void SetThrottle(float value)
    {
        throttle = value;
    }
}
