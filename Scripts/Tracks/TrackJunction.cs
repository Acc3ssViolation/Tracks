using System;
using System.Collections.Generic;

public class TrackJunction : TrackSection
{
    List<int> sections;

    public List<int> Sections
    {
        get
        {
            return sections;
        }
    }
    int activeSectionIndex = 0;
    public override float length
    {
        get
        {
            return TrackCollection.instance.Get(sections[activeSectionIndex]).length;
        }
        set
        {
            _length = value;
        }
    }
    public override bool curved
    {
        get
        {
            return TrackCollection.instance.Get(sections[activeSectionIndex]).curved;
        }
        set
        {
            _curved = value;
        }
    }
    public override float angle
    {
        get
        {
            return TrackCollection.instance.Get(sections[activeSectionIndex]).angle;
        }
        set
        {
            _angle = value;
        }
    }
    public override int NextSectionIndex
    {
        get
        {
            return TrackCollection.instance.Get(sections[activeSectionIndex]).NextSectionIndex;
        }

        set
        {
            base.NextSectionIndex = value;
        }
    }
    public override int PreviousSectionIndex
    {
        get
        {
            return TrackCollection.instance.Get(sections[activeSectionIndex]).PreviousSectionIndex;
        }

        set
        {
            base.PreviousSectionIndex = value;
        }
    }

    /*
     * Constructors
     */

    public TrackJunction()
    {
        position = new WorldPosition();
        rotation = new WorldRotation();
        sections = new List<int>();
    }

    public TrackJunction(WorldPosition pos, WorldRotation rot)
    {
        position = pos;
        rotation = rot;
        sections = new List<int>();
    }

    public TrackJunction(WorldPosition pos, WorldRotation rot, int[] sections)
    {
        position = pos;
        rotation = rot;
        this.sections = new List<int>();
        this.sections.AddRange(sections);
    }

    //Switch the active section to the given target
    public void Switch(int target)
    {
        if(target >= 0 && target < sections.Count)
        {
            activeSectionIndex = target;
        }
    }

    //Switch to the next one
    public void SwitchNext()
    {
        activeSectionIndex = (activeSectionIndex < sections.Count - 1) ? activeSectionIndex + 1 : 0;
    }

    //Checks if subject is our previous section. May seem useless, but is needed for switches
    public override bool CheckPreviousSection(TrackSection subject)
    {
        //Loop trough all our sections and check them
        foreach(int index in sections)
        {
            TrackSection ts = TrackCollection.instance.Get(index);
            if(ts.PreviousSectionIndex == subject.index)
                return true;
        }
        return false;
    }
    //Checks if subject is our next section. May seem useless, but is needed for switches
    public override bool CheckNextSection(TrackSection subject)
    {
        //Loop trough all our sections and check them
        foreach(int index in sections)
        {
            TrackSection ts = TrackCollection.instance.Get(index);
            if(ts.NextSectionIndex == subject.index)
                return true;
        }
        return false;
    }

    public override WorldPosition GetPositionOnTrack(float distance)
    {
        return TrackCollection.instance.Get(sections[activeSectionIndex]).GetPositionOnTrack(distance);
    }

    public override WorldRotation GetRotationOnTrack(float distance)
    {
        return TrackCollection.instance.Get(sections[activeSectionIndex]).GetRotationOnTrack(distance);
    }

    /// <summary>
    /// Creates a new section for this junction. Section is added to TrackCollection automatically.
    /// </summary>
    /// <param name="length"></param>
    /// <param name="curved"></param>
    /// <param name="angle"></param>
    /// <returns>TrackSection that was created</returns>
    public TrackSection CreateSection(float length, bool curved, float angle)
    {
        TrackSection newSection = new TrackSection(position, rotation, length, curved, angle);
        newSection.PreviousSectionIndex = m_prevSectionIndex;
        sections.Add(TrackCollection.instance.Add(newSection));
        return newSection;
    }
}
