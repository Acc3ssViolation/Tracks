using System;

/// <summary>
/// Singleton collection of track sections
/// </summary>
public class TrackCollection
{
    public static TrackCollection instance;

    /// <summary>
    /// Contains ALL sections including junctions
    /// </summary>
    public TrackSection[] sections;

    /// <summary>
    /// Contains ONLY junctions
    /// </summary>
    public TrackJunction[] junctions;

    //Shitty array backend
    private int currentIndex = 1;

    private TrackCollection()
    {
        sections = new TrackSection[10004];
        junctions = new TrackJunction[10004];
    }

    public static TrackCollection GetInstance()
    {
        if(instance == null)
        {
            instance = new TrackCollection();
        }
        return instance;
    }

    public static TrackCollection GetInstance(TrackCollection c)
    {
        instance = c;
        return instance;
    }

    /// <summary>
    /// Add a section to the collection. Junctions get detected automatically.
    /// </summary>
    /// <param name="section"></param>
    /// <returns>Index of section</returns>
    public int Add(TrackSection section)
    {
        sections[currentIndex] = section;
        TrackJunction j = section as TrackJunction;
        if(j != null)
        {
            junctions[currentIndex] = j;
        }
        section.index = currentIndex;
        return currentIndex++;
    }

    /// <summary>
    /// Add a section to the collection. Junctions get detected automatically.
    /// </summary>
    /// <param name="section"></param>
    /// <returns>Index of section</returns>
    public bool Add(TrackSection section, int index)
    {
        if(Get(index) == null)
        {
            sections[index] = section;
            TrackJunction j = section as TrackJunction;
            if(j != null)
            {
                junctions[index] = j;
            }

            if(currentIndex <= index)
            {
                currentIndex = index + 1;
            }
            return true;
        }
        return false;
    }

    /// <summary>
    /// Returns a section based on index
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public TrackSection Get(int index)
    {
        return sections[index];
    }

    /// <summary>
    /// Clears all sections, courtesy of the GC
    /// </summary>
    public void Clear()
    {
        currentIndex = 1;
        sections = new TrackSection[10004];
        junctions = new TrackJunction[10004];
    }
}

