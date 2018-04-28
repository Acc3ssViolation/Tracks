using System;

public class TrackVehicle
{
	public Direction direction;
	public Traveler axleFront;
	public Traveler axleRear;
    public float length = 7.22f;    //Sik
	public float wheelBase = 3.20f;	//Sik

    public event EventHandler<EventArgs> DerailEvent;

    public TrackVehicle(TrackSection startingSection, float position, Direction startingDirection, Direction facingDirection, float length, float wheelBase)
	{
        this.length = length;
        this.wheelBase = wheelBase;
        direction = facingDirection;
        axleFront = new Traveler(startingSection, facingDirection, position + wheelBase * 0.5f * (float)facingDirection);
        axleRear = new Traveler(startingSection, facingDirection, position - wheelBase * 0.5f * (float)facingDirection);

        axleFront.DerailEvent += OnDerail;
        axleRear.DerailEvent += OnDerail;

        //Move the forward axle so it's in the correct position
        //axleFront.Move(wheelBase);
        //Set our actual direction?
        SetDirection(startingDirection);
	}
	
	public bool Move(float distance)
	{
		//Move the most forward axle first
		if(direction == Direction.Forward){
			if(axleFront.Move(distance)){
				axleRear.Move(distance);
                return true;
			}
		}else{
			if(axleRear.Move(distance)){
				axleFront.Move(distance);
                return true;
			}
		}
        return false;
	}

    public void OnDerail(object o, EventArgs e)
    {
        EventHandler<EventArgs> handler = DerailEvent;
        if(handler != null)
        {
            handler(this, e);
        }
    }
	
	public void SetDirection(Direction newDirection)
	{
		if(newDirection == direction)
			return;
		
		direction = newDirection;
		axleFront.FlipDirection();
		axleRear.FlipDirection();
	}
}