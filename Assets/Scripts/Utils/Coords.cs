using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Untitled
{
	namespace Utils
	{
		/*
		* Wrapps Vector objects to make it explicit
		* which coordinate system is being used, and 
		* to simplify conversions between them.
		*/
		public class Coords
		{
			private readonly Vector2Int gridCoords;
			private readonly Vector3 tileCoords;
			
			public Coords(Vector3 tileCoords)
			{
				this.tileCoords = tileCoords;
				
				var dx = GridUtils.Instance.dx;
				var dy = GridUtils.Instance.dy;
				
				float xCount = tileCoords.x / dx.x;
				float yCount = tileCoords.y / dy.y;
								
				bool roundedX = false;
				if(xCount % 2 != 0) {
					roundedX = true;
					xCount = xCount > 0 ? xCount + 1 : xCount - 1;
				}
				bool roundedY = false;
				if(yCount % 2 != 0) {
					roundedY = true;
					yCount = yCount > 0 ? yCount + 1 : yCount - 1;
				}
				
				// Create initial x and y based on rounding
				// by tracing across the major diagonal
				int x = (int)(xCount / 2);
				int y = x;
				if(y <= x)
					y *=  -1;
								
				// Correct by tracing back across the minor 
				// diagonal
				int correction = (int)(yCount / 2);
				x += correction;
				y += correction;
								
				// Account for rounding
				if(correction > 0 && roundedX) {
					if(xCount > 0)
						x -= 1;
					else
						y -= 1;
				}
				else if(correction < 0 && roundedY) {
					if(xCount > 0)
						y += 1;
					else
						x += 1;
				}
								
				gridCoords = new Vector2Int(x, y);
			}
			
			public Vector3 AsTile()
			{
				return this.tileCoords;
			}
			
			public Vector2Int AsGrid()
			{
				return this.gridCoords;
			}
			
			/*********************************
			***  Operator Implementations  ***
			**********************************/
			
			// Arithmatic on tile coords
			public static Coords operator +(Coords coords, Vector3 tileCoords)
			{
				return new Coords(coords.AsTile() + tileCoords);
			}
			public static Coords operator +(Vector3 tileCoords, Coords coords)
			{
				return new Coords(tileCoords + coords.AsTile());
			}
			public static Coords operator -(Coords coords, Vector3 tileCoords)
			{
				return new Coords(coords.AsTile() - tileCoords);
			}
			public static Coords operator -(Vector3 tileCoords, Coords coords)
			{
				return new Coords(tileCoords - coords.AsTile());
			}
			
			// Arithmatic on grid coords
			public static Coords operator +(Coords coords, Vector2Int gridCoords)
			{
				return new Coords(coords.AsTile() + GridOffsetToTileOffset(gridCoords));
			}
			public static Coords operator +(Vector2Int gridCoords, Coords coords)
			{
				return GridOffsetToTileOffset(gridCoords) + new Coords(coords.AsTile());
			}
			public static Coords operator -(Coords coords, Vector2Int gridCoords)
			{
				return new Coords(coords.AsTile() - GridOffsetToTileOffset(gridCoords));
			}
			public static Coords operator -(Vector2Int gridCoords, Coords coords)
			{
				return GridOffsetToTileOffset(gridCoords) - new Coords(coords.AsTile());
			}
			
			// Override for Vector2 cuz it get's autocasted to Vector3 otherwise
			public static Coords operator +(Coords coords, Vector2 gridCoords)
			{
				throw new System.InvalidOperationException("Vector2 not supported: use Vector2Int (gridCoords) or Vector3 (tileCoords)");
			}
			public static Coords operator +(Vector2 gridCoords, Coords coords)
			{
				throw new System.InvalidOperationException("Vector2 not supported: use Vector2Int (gridCoords) or Vector3 (tileCoords)");
			}
			public static Coords operator -(Coords coords, Vector2 gridCoords)
			{
				throw new System.InvalidOperationException("Vector2 not supported: use Vector2Int (gridCoords) or Vector3 (tileCoords)");
			}
			public static Coords operator -(Vector2 gridCoords, Coords coords)
			{
				throw new System.InvalidOperationException("Vector2 not supported: use Vector2Int (gridCoords) or Vector3 (tileCoords)");
			}
			
			private static Vector3 GridOffsetToTileOffset(Vector2Int gridOffset)
			{
				var dx = GridUtils.Instance.dx.x;
				var dy = GridUtils.Instance.dy.y;
				Vector3 offset = new Vector3(0, 0, 0);
				
				// Apply y portion of grid offset
				offset += new Vector3(-dx * gridOffset.y, dy * gridOffset.y, 0);
				
				// Apply x portion of grid offset
				offset += new Vector3(dx * gridOffset.x, dy * gridOffset.x, 0);
								
				return offset;
			}
			
			public override int GetHashCode()
			{
				return this.gridCoords.GetHashCode();
			}
			
			public static bool operator ==(Coords c1, Coords c2)
			{
				return c1.Equals(c2);
			}
			public static bool operator !=(Coords c1, Coords c2)
			{
				return !c1.Equals(c2);
			}
			public override bool Equals(object obj)
			{
				return Equals(obj as Coords);
			}
			public bool Equals(Coords other)
			{
				return this.gridCoords.Equals(other.gridCoords);
			}
		}
	}
}