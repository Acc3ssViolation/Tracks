
using System;

[Serializable]
public class WorldPosition
{
	public static int tileSize = 20;
	public int tileX;
	public int tileY;
	public float x;
	public float y;
	
	public WorldPosition(int tileX, int tileY, float x, float y)
	{
		this.tileX = tileX;
		this.tileY = tileY;
		this.x = x;
		this.y = y;
	}
	
	public WorldPosition() : this(0, 0, 0, 0)
	{
	}

    public UnityEngine.Vector3 Vector3()
    {
        float vx = tileX * WorldPosition.tileSize + x;
        float vy = tileY * WorldPosition.tileSize + y;
        return new UnityEngine.Vector3(vx, 0, vy);
    }
	
	/*
		Operators
		TODO: Implement this properly
	*/
	public static WorldPosition operator +(WorldPosition a, WorldPosition b)
	{
		int ctx = a.tileX + b.tileX;
		int cty = a.tileY + b.tileY;
		float cx = a.x + b.x;
		float cy = a.y + b.y;
        while(cx > (float)WorldPosition.tileSize)
        {
			cx -= (float)WorldPosition.tileSize;
			ctx++;
		}
        while(cy > (float)WorldPosition.tileSize)
        {
			cy -= (float)WorldPosition.tileSize;
			cty++;
		}
        while(cx < 0.0f)
        {
            cx += (float)WorldPosition.tileSize;
            ctx--;
        }
        while(cy < 0.0f)
        {
            cy += (float)WorldPosition.tileSize;
            cty--;
        }
 		return new WorldPosition(ctx, cty, cx, cy);
	}
	
	public static WorldPosition operator -(WorldPosition a, WorldPosition b)
	{
		int ctx = a.tileX - b.tileX;
		int cty = a.tileY - b.tileY;
		float cx = a.x - b.x;
		float cy = a.y - b.y;
        while(cx > (float)WorldPosition.tileSize)
        {
            cx -= (float)WorldPosition.tileSize;
            ctx++;
        }
        while(cy > (float)WorldPosition.tileSize)
        {
            cy -= (float)WorldPosition.tileSize;
            cty++;
        }
        while(cx < 0.0f)
        {
            cx += (float)WorldPosition.tileSize;
            ctx--;
        }
        while(cy < 0.0f)
        {
            cy += (float)WorldPosition.tileSize;
            cty--;
        }
 		return new WorldPosition(ctx, cty, cx, cy);
	}
	
	public static WorldPosition FromRaw(float x, float y)
	{
		int ctx = (int)x / WorldPosition.tileSize;
		int cty = (int)y / WorldPosition.tileSize;
		float cx = x % (float)WorldPosition.tileSize;
		float cy = y % (float)WorldPosition.tileSize;
		return new WorldPosition(ctx, cty, cx, cy);
	}
}