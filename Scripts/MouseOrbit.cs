using UnityEngine;
using System.Collections;

public class MouseOrbit : MonoBehaviour 
{
	public Transform target;
	public float distance = 10.0f;
	
	public float xSpeed = 250.0f;
	public float ySpeed = 120.0f;
	
	public float yMinLimit = -20f;
	public float yMaxLimit = 80f;
	public float zoomSpeed = 2.0f;
	
	private float x = 0.0f;
	private float y = 0.0f;
	
	void Start () 
	{
		Vector3 angles = transform.eulerAngles;
		x = angles.y;
		y = angles.x;
		
		// Make the rigid body not change rotation
		if (GetComponent<Rigidbody>())
			GetComponent<Rigidbody>().freezeRotation = true;
	}
	
	void LateUpdate () {
		if(target)
		{
			if (Input.GetMouseButton(1)) 
			{
				x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
				y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
				
				y = ClampAngle(y, yMinLimit, yMaxLimit);
			}
			distance -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;

			Quaternion rotation = Quaternion.Euler(y, x + target.eulerAngles.y, 0);
			Vector3 position = rotation * new Vector3(0.0f, 0.0f, -distance) + target.position + Vector3.up;
			
			transform.rotation = rotation;
			transform.position = position;
            //TRAINS
            //Code to move along the train
            TrainController train = target.GetComponent<TrainController>();
            if(train != null)
            {
                if(Input.GetKeyDown(KeyCode.LeftArrow) && train.next != null)
                {
                    target = train.next.transform;
                }
                else if(Input.GetKeyDown(KeyCode.RightArrow) && train.previous != null)
                {
                    target = train.previous.transform;
                }
            }
		}
        
	}
	
	static float ClampAngle (float angle, float min, float max) 
	{
		if (angle < -360)
			angle += 360;
		if (angle > 360)
			angle -= 360;
		return Mathf.Clamp (angle, min, max);
	}
}
