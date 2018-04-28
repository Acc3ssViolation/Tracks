using System;

[Serializable]
public class WorldRotation
{
	double radians;
	public double Radians
	{
		get{
			return radians;
		}
		set{
			radians = value % (Math.PI * 2);
			degrees = radians * 180.0 / Math.PI;
		}
	}
	double degrees;
	public double Degrees
	{
		get{
			return degrees;
		}
		set{
			degrees = value % 360.0;
			radians = Math.PI * degrees / 180.0;
		}
	}
    public WorldRotation Oppisite
    {
        get
        {
            return new WorldRotation(degrees - 180.0f, false);
        }
    }

	public WorldRotation()
	{
		Radians = 0.0f;
	}
	
	public WorldRotation(double value, bool inRadians = true)
	{
		if(!inRadians){
			Degrees = value;
		}else{
			Radians = value;
		}
	}

}