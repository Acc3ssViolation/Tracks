using UnityEngine;
using System.Collections;
using System;

public class TrainController : MonoBehaviour
{
    protected TrackManager trackManager;
    public TrackVehicle trackVehicle;

    [HideInInspector]
    public Driver driver;

    public float length = 7.22f;
    public float wheelBase = 3.20f;
    public float brakeForce = 42000.0f; //0.35

    [Range(1.0f, 900000)]
    public float mass = 21000;
    /// <summary>
    /// Velocity relative to train move direction
    /// Always positive
    /// </summary>
    protected float _velocity = 0.0f;
    /// <summary>
    /// Velocity relative to train face direction
    /// </summary>
    public float Velocity
    {
        get
        {
            return _velocity * (float)trackVehicle.direction;
        }
        set
        {
            if(value >= 0.0f)
            {
                _velocity = value;
                trackVehicle.SetDirection(Direction.Forward);
            }
            else
            {
                _velocity = Mathf.Abs(value);
                trackVehicle.SetDirection(Direction.Reverse);
            }
        }
    }

    public float AbsVelocity
    {
        get
        {
            return _velocity;
        }
    }

    [HideInInspector]
    public Direction MoveDirection
    {
        get
        {
            return trackVehicle.direction;
        }
    }

    //Next and previous TrainControllers in this train
    public TrainController next;
    public TrainController previous;


    private float targetBrakeValue = 0;
    private float currentBrakeValue = 0;

    private bool isDerailed = false;


    public void SetBrake(float value)
    {
        targetBrakeValue = value;
    }


    public void Start()
    {
        trackVehicle.DerailEvent += OnDerail;
    }


    /// <summary>
    /// Returns active brake force
    /// </summary>
    /// <returns></returns>
    public float GetBrakeForce()
    {
        return currentBrakeValue * brakeForce;
    }

    /// <summary>
    /// Returns total vehicle mass in kg
    /// </summary>
    /// <returns></returns>
    public float GetMass()
    {
        return mass;
    }

    /// <summary>
    /// Returns absolute drag for this vehicle
    /// </summary>
    /// <returns></returns>
    public float GetDragForce()
    {
        //1.07 + 0.0011 V + 0.000236 V2
        float weight = GetMass() * 0.00981f; //weight in kN
        float vkmh = (_velocity * 3.6f);
        float dragForce = weight * (1.07f + 0.0011f * vkmh + 0.000236f * vkmh * vkmh);
        return dragForce;
    }


    /// <summary>
    /// Handle input and engine/brake state updates
    /// </summary>
    public void Update()
    {
        if(previous == null)
        {
            OnUpdate();
        }
    }


    public void OnUpdate()
    {
        //TODO: Simulate brakes
        currentBrakeValue = targetBrakeValue;

        if(next != null)
        {
            next.OnUpdate();
        }
    }

    /// <summary>
    /// Handle train physics update
    /// 
    /// </summary>
    public void FixedUpdate()
    {
        //Call if we are the first in the train
        if(previous == null)
        {
            if(isDerailed)
            {
                var r = GetComponent<Rigidbody>();
                if(r != null)
                {
                    _velocity = r.velocity.magnitude;
                }

                return;
            }

            float trainBrakeForce = 0;
            float trainMass = 0;
            float trainEngineForce = 0;
            float trainDragForce = 0;

            TrainController trailer = this;
            while(trailer != null)
            {
                trainBrakeForce += trailer.GetBrakeForce();
                trainMass += trailer.GetMass();
                trainDragForce += trailer.GetDragForce();

                TrainEngineController engine = trailer as TrainEngineController;
                if(engine != null)
                {
                    trainEngineForce += engine.GetTractiveForce();
                }

                trailer = trailer.next;
            }

            //Debug.Log("Train Mass " + trainMass + " kg");
            // Add engine effort
            Velocity += (trainEngineForce / trainMass) * Time.fixedDeltaTime;

            // Calculate and subtract drag and braking
            float brakeValue = -(float)trackVehicle.direction * (trainBrakeForce / trainMass) * Time.fixedDeltaTime;

            trainDragForce *= -(float)trackVehicle.direction;

            // Combine drag and brake into single value
            float dragValue = brakeValue + (trainDragForce / trainMass) * Time.fixedDeltaTime;

            // Stop train from reversing due to brake force...
            if(trackVehicle.direction == Direction.Forward && Velocity + dragValue < 0.0f)
            {
                Velocity = 0.0f;
            }
            else
            {
                Velocity += dragValue;
            }

            //Set the velocity on the entire train
            TrainController t = next;
            while(t != null)
            {
                t.Velocity = Velocity;
                t = t.next;
            }

            // Update positions
            OnFixedUpdate();
        }
    }


    /// <summary>
    /// Detects collisions between trains
    /// </summary>
    /// <param name="collision"></param>
    void OnTriggerStay(Collider other)
    {
        TrainController tc = other.gameObject.GetComponent<TrainController>();
        if(tc != null)
        {
            OnCollisionDetected(tc);
        }
    }

    public void OnCollisionDetected(TrainController tc)
    {
        // TODO: Disable collisions between next and previous completely via physics system.
        if(tc == next || tc == previous || isDerailed)
        {
            return;
        }

        if(tc == this)
        {
            Debug.LogWarning("OnCollisionDetected with self! " + name + " iid: " + this.GetInstanceID());
        }

        //if(tc._velocity + this._velocity > 2f)
        if(Vector3.Distance(tc.CalculateVelocityVector(), CalculateVelocityVector()) > 2f)
        {
            Derail();
        }
    }

    /// <summary>
    /// Updates position. Cascades down the train.
    /// </summary>
    public void OnFixedUpdate()
    {
        if(trackVehicle != null)
        {
            trackVehicle.Move(_velocity * Time.fixedDeltaTime);
            Vector3 front = trackVehicle.axleFront.GetWorldPosition().Vector3();
            Vector3 rear = trackVehicle.axleRear.GetWorldPosition().Vector3();
            transform.position = Vector3.Lerp(front, rear, 0.5f);
            transform.rotation = Quaternion.LookRotation(front - rear);
            //Update the next car in our train
            if(next != null)
            {
                next.OnFixedUpdate();
            }
        }
    }


    /// <summary>
    /// Forces a derail
    /// </summary>
    public void Derail()
    {
        OnDerail(null, null);
    }

    public void OnDerail(object o, EventArgs e)
    {
        if(isDerailed) { return; }

        if(previous != null)
        {
            if(previous.previous == this)
                previous.previous = null;
            else if(previous.next == this)
                previous.next = null;
            previous = null;
        }
        if(next != null)
        {
            if(next.previous == this)
                next.previous = null;
            else if(next.next == this)
                next.next = null;
            next = null;
        }

        BoxCollider b = GetComponentInChildren<BoxCollider>();
        if(b != null)
        {
            b.isTrigger = false;
        }

        Rigidbody r = GetComponentInChildren<Rigidbody>();
        if(r != null)
        {
            r.isKinematic = false;
            r.useGravity = true;
            r.mass = mass;
            r.velocity = CalculateVelocityVector();
        }

        Debug.Log("Train Derailed!");
        driver = null;

        isDerailed = true;
    }

    
    public Vector3 CalculateVelocityVector()
    {
        Vector3 front = trackVehicle.axleFront.GetWorldPosition().Vector3();
        Vector3 rear = trackVehicle.axleRear.GetWorldPosition().Vector3();

        Vector3 direction = rear - front;
        direction.Normalize();
        direction *= -(float)trackVehicle.direction;

        return direction * _velocity;
    }

    /// <summary>
    /// Draw track vehicle gizmos
    /// </summary>
    public void OnDrawGizmos()
    {
        if(trackVehicle != null)
       {
           Vector3 front = trackVehicle.axleFront.GetWorldPosition().Vector3() + Vector3.up;
           Vector3 rear = trackVehicle.axleRear.GetWorldPosition().Vector3() + Vector3.up;
           Gizmos.DrawCube(front, Vector3.one);
           Gizmos.DrawCube(rear, Vector3.one);
           Gizmos.DrawLine(front, rear);
       }
    }
}
