using UnityEngine;
using System.Collections;
using System;

public class Consist : ScriptableObject
{
    [Serializable]
    public class ConsistVehicle
    {
        public Direction faceDirection = Direction.Forward;
        public TrainController vehicleController;

        public ConsistVehicle()
        {
            faceDirection = Direction.Forward;
        }
    }


    public Driver driver;
    public ConsistVehicle[] vehicles;

    /// <summary>
    /// Spawns a consist on a track section
    /// </summary>
    /// <param name="startSection"></param>
    /// <returns>The lead car of this consist</returns>
    public TrainController Spawn(TrackSection startSection)
    {
        Driver driverInstance = null;
        if(driver != null)
        {
            driverInstance = GameObject.Instantiate(driver.gameObject).GetComponent<Driver>();
        }

        TrainController controller;
        TrainController previous = null;
        TrainController lead = null;

        TrackSection currentSection;
        currentSection = startSection;
        // Spawn from the end of the track
        float spawnPosition = currentSection.length - 10.0f;

        // Spawn vehicles front to back
        for(int i = 0; i < vehicles.Length; i++)
        {
            controller = GameObject.Instantiate(vehicles[i].vehicleController.gameObject).GetComponent<TrainController>();
            //TODO: TrackVehicle facing directions works but gets ignored by driving logic, e.g. it's only visual.
            controller.trackVehicle = new TrackVehicle(currentSection, spawnPosition, Direction.Forward, vehicles[i].faceDirection, controller.length, controller.wheelBase);

            if(i == 0)
            {
                lead = controller;
            }

            // Add engines to driver if we have one
            TrainEngineController engine = controller as TrainEngineController;
            if(engine != null && driverInstance != null)
            {
                driverInstance.AddEngine(engine);
            }

            if(previous != null)
            {
                previous.next = controller;
                controller.previous = previous;

                //Move ourselfs back on the track
                controller.trackVehicle.SetDirection(Direction.Reverse);
                controller.trackVehicle.Move(controller.length * 0.5f + previous.length * 0.5f);
                controller.trackVehicle.SetDirection(Direction.Forward);
                //Change spawn position to our new location
                spawnPosition -= controller.length * 0.5f + previous.length * 0.5f;
            }
            previous = controller;
        }

        if(driverInstance != null)
        {
            driverInstance.RecalculateTrain();
        }

        return lead;
    }
}
