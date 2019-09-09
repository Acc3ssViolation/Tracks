using System;

public enum Direction
{
	Forward = 1,
	Reverse = -1
}

public class Traveler
{
	public Direction direction = Direction.Forward;		//
	public TrackSection currentTrackSection;			//
	public float position = 0.0f;						//aww

    public event EventHandler<EventArgs> DerailEvent;
	
	public Traveler()
	{
		currentTrackSection = null;
	}
	
	public Traveler(TrackSection trackSection)
	{
		currentTrackSection = trackSection;
	}
	
	public Traveler(TrackSection trackSection, Direction dir)
	{
		currentTrackSection = trackSection;
		direction = dir;
	}
	
	public Traveler(TrackSection trackSection, Direction dir, float pos)
	{
		currentTrackSection = trackSection;
		direction = dir;
		position = pos;
	}
	
	public Traveler(Traveler template)
	{
		direction = template.direction;
		currentTrackSection = template.currentTrackSection;
		position = template.position;
	}
	
	public void FlipDirection()
	{
		direction = (direction == Direction.Forward) ? Direction.Reverse : Direction.Forward;
	}
	
	//Warning: Recursive
	public bool Move(float distance)
	{
		bool hasMoved = false;
		
		float newPosition = position + distance * (int)direction;
		if(currentTrackSection.GetPositionOnTrack(newPosition) != null){
			//Do the thing
			position = newPosition;
			hasMoved = true;
		}else{
			TrackSection newSection = (direction == Direction.Forward) ? TrackCollection.instance.Get(currentTrackSection.NextSectionIndex) : TrackCollection.instance.Get(currentTrackSection.PreviousSectionIndex);
            if(newSection != null){
				//Check to see if we need to switch direction
				Direction newDirection = direction;
				float distanceOnNewSection = 0.0f;
				float positionOnNewSection = 0.0f;
				
				if(direction == Direction.Forward){
					distanceOnNewSection = newPosition - currentTrackSection.length;
					
					//TODO: Make a function to determine the correct direction in the TrackSection class
					//Might be needed when working with switches, so we can override it in that case
					if(newSection.CheckNextSection(currentTrackSection)){
						newDirection = Direction.Reverse;
						positionOnNewSection = newSection.length;
					}
				}else{
                    distanceOnNewSection = -newPosition;
					positionOnNewSection = newSection.length;
					
					if(newSection.CheckPreviousSection(currentTrackSection)){
						newDirection = Direction.Forward;
						positionOnNewSection = 0.0f;
					}
				}
				//TODO: Try to get a stack overflow by calling move with a really high distance value
				Traveler traveler = new Traveler(newSection, newDirection, positionOnNewSection);
				hasMoved = traveler.Move(distanceOnNewSection);
				
				this.direction = traveler.direction;
				this.currentTrackSection = traveler.currentTrackSection;
				this.position = traveler.position;
			}else{
                //There was no new section!
                //TODO: Derail, move to the end or something else?
                UnityEngine.Debug.Log("Traveler got stuck at track section " + currentTrackSection.index);
                Derail();
			}
		}
		return hasMoved;
	}

    /// <summary>
    /// Sends an event to all derail listeners
    /// </summary>
    public void Derail()
    {
        EventHandler<EventArgs> handler = DerailEvent;
        if(handler != null)
        {
            handler(this, null);
        }
    }
	
	public WorldPosition GetWorldPosition()
	{
		if(currentTrackSection != null){
			return currentTrackSection.GetPositionOnTrack(position);
		}
		return null;
	}
	
	public WorldRotation GetWorldRotation()
	{
		if(currentTrackSection != null){
			return currentTrackSection.GetRotationOnTrack(position);
		}
		return null;
	}
}