/*
	author:			Wouter Brookhuis
	discription:	Contains data about a section of track
	version:		0.1
*/

using System;

public class TrackSection
{
    /*
		Fields
	*/
    public int index;               //index inside TrackCollection
	public WorldPosition position;	//position of this section's origin in the world
	public WorldRotation rotation;	//rotation of this section in the world

    protected float _length;                  //length in meters
	public virtual float length
    {
        get
        {
            return _length;
        }
        set
        {
            _length = value;
        }
    }
    protected bool _curved;                   //Curved indicator
    public virtual bool curved
    {
        get
        {
            return _curved;
        }
        set
        {
            _curved = value;
        }
    }
    protected float _angle;                   //Change in angle if this is a curved track
    public virtual float angle
    {
        get
        {
            return _angle;
        }
        set
        {
            _angle = value;
        }
    }

    protected int m_nextSectionIndex;
    protected int m_prevSectionIndex;

    /// <summary>
    /// Index of next track section
    /// </summary>
    public virtual int NextSectionIndex
    {
        get
        {
            return m_nextSectionIndex;
        }
        set
        {
            m_nextSectionIndex = value;
        }
    }

    /// <summary>
    /// Index of previous track section
    /// </summary>
    public virtual int PreviousSectionIndex
    {
        get
        {
            return m_prevSectionIndex;
        }
        set
        {
            m_prevSectionIndex = value;
        }
    }

    /*
		Constructors
	*/
    public TrackSection(WorldPosition position, WorldRotation rotation, float length, bool curved, float angle)
	{
		this.position = position;
		this.rotation = rotation;
		this.length = length;
		this.curved = curved;
		this.angle = angle;
	}

    public TrackSection(WorldPosition position, WorldRotation rotation, float length)
        : this(position, rotation, length, false, 0)
	{
	}
	
	public TrackSection(WorldPosition position, WorldRotation rotation) : this(position, rotation, 0, false, 0)
	{
	}

    public TrackSection() : this(new WorldPosition(), new WorldRotation())
    {
    }
	
	/*
		Public methods
	*/
    //Checks if subject is our previous section. May seem useless, but is needed for switches
    public virtual bool CheckPreviousSection(TrackSection subject)
    {
        //If a junction is passed as the subject, check all it's sections to see if those match
        TrackJunction junction = subject as TrackJunction;
        if(junction != null)
        {
            foreach(int s in junction.Sections)
            {
                if(CheckPreviousSection(TrackCollection.instance.Get(s))){
                    return true;
                }
            }
            return false;
        }
        return subject.index == m_prevSectionIndex;
    }
    //Checks if subject is our next section. May seem useless, but is needed for switches
    public virtual bool CheckNextSection(TrackSection subject)
    {
        //If a junction is passed as the subject, check all it's sections to see if those match
        TrackJunction junction = subject as TrackJunction;
        if(junction != null)
        {
            foreach(int s in junction.Sections)
            {
                if(CheckNextSection(TrackCollection.instance.Get(s)))
                {
                    return true;
                }
            }
            return false;
        }
        return subject.index == m_nextSectionIndex;
    }


    public virtual WorldPosition GetPositionOnTrack(float distance)
	{
		if(distance > length || distance < 0.0f){
			return null;
		}
		
		WorldPosition result = new WorldPosition();
		
		if(!curved){
			//Calculate for straight section
			//Right handed - Y up
			float x = (float)Math.Cos(rotation.Radians) * distance;
			float y = (float)Math.Sin(rotation.Radians) * distance;
            //UnityEngine.Debug.Log("x: " + x + " y: " + y);
			result = position + WorldPosition.FromRaw(x, y);
		}else{
            //Calculate for curved section
            double angleRadians = (Math.PI * angle / 180.0);
            //Curve radius in meters
            double r = length / angleRadians;
            //Angle to go along the circle in radians
            double a = angleRadians * (distance / length);
            double x = Math.Sin(a) * r;
            double y = r - (Math.Cos(a) * r);

            //Correct rotation
            double wx = Math.Cos(-rotation.Radians) * x + Math.Sin(-rotation.Radians) * y;
            double wy = -Math.Sin(-rotation.Radians) * x + Math.Cos(-rotation.Radians) * y;

            //UnityEngine.Debug.Log("wx: " + wx + " wy: " + wy);
            result = position + WorldPosition.FromRaw((float)wx, (float)wy);
		}
		
		return result;
	}
	
	public virtual WorldRotation GetRotationOnTrack(float distance)
	{
		if(distance > length || distance < 0.0f){
			return null;
		}
		
		if(!curved){
			//It's straight...
			return rotation;
		}else{
			//Calculate for curved section
            float a = angle * (distance / length);
			return new WorldRotation(rotation.Degrees + a, false);
		}
	}
	
	/// <summary>
    /// Creates another track section after this one (as 'next') and adds it to the TrackCollection
    /// </summary>
    /// <param name="length"></param>
    /// <param name="curved"></param>
    /// <param name="angle"></param>
    /// <returns></returns>
	public TrackSection CreateNext(float length, bool curved, float angle)
	{
		if(NextSectionIndex == 0){
			WorldPosition nextPos = GetPositionOnTrack(this.length);
			WorldRotation nextRot = GetRotationOnTrack(this.length);
			TrackSection nextTrackSection = new TrackSection(nextPos, nextRot, length, curved, angle);
			this.NextSectionIndex = TrackCollection.instance.Add(nextTrackSection);
            nextTrackSection.PreviousSectionIndex = this.index;
			return nextTrackSection;
		}
		return null;
	}

    public override string ToString()
    {
        return "Length: " + _length.ToString() + ", Curved: " + _curved.ToString() + ", Angle: " + _angle.ToString();
    }
}