using System;
using UnityEngine;

public abstract class AbstractTileManager
{
	public static AbstractTileManager instance { get; set;}
	public abstract Vector2 size { get; set; }
	public abstract bool TileIsPassable (int x, int y);
}
