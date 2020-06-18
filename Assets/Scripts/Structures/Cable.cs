using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Untitled.Utils;

[RequireComponent(typeof(SpriteRenderer))]
public class Cable : Placeable
{
	
	// Constants to help with the sprite updating
	private const int UP = 1;
	private const int RIGHT = 2;
	private const int DOWN = 4;
	private const int LEFT = 8;
	
	private SpriteRenderer renderer;
	private Sprite[] sprites;
	private const int PIPE_MAJOR = 0;
	private const int PIPE_MINOR = 1;
	private const int PIPE_TRIP_UR = 2;
	private const int PIPE_TRIP_UL = 3;
	private const int PIPE_TRIP_LL = 4;
	private const int PIPE_TRIP_LR = 5;
	private const int PIPE_CROSS = 6;
	private const int PIPE_L_LEFT = 7;
	private const int PIPE_L_DOWN = 8;
	private const int PIPE_L_RIGHT = 9;
	private const int PIPE_L_UP = 10;
	
	private void Start()
	{
		renderer = GetComponent<SpriteRenderer>();
		sprites = Resources.LoadAll<Sprite>("Sprites/pipe_tiles");
		
		base.Start();
		Placeable.OnPlaceableCreateEvent += OnPlaceablePlaced;
		
		UpdateSprite();
	}
	
	// Called when the sprite needs updating,
	// normally when an adjacent cable was placed
	private void UpdateSprite()
	{		
		int directions = 0;
		
		if(GridUtils.GetPlaceableAt(coords + Vector2Int.right) != null)
			directions |= RIGHT;
		if(GridUtils.GetPlaceableAt(coords + Vector2Int.up) != null)
			directions |= UP;
		if(GridUtils.GetPlaceableAt(coords + Vector2Int.left) != null)
			directions |= LEFT;
		if(GridUtils.GetPlaceableAt(coords + Vector2Int.down) != null)
			directions |= DOWN;
		
		switch(directions) {
			case 1: //up
				renderer.sprite = sprites[PIPE_MAJOR];
				break;
			case 2: // right
				renderer.sprite = sprites[PIPE_MINOR];
				break;
			case 3: // up + right
				renderer.sprite = sprites[PIPE_L_UP];
				break;
			case 4: // down
				renderer.sprite = sprites[PIPE_MAJOR];
				break;
			case 5: // up + down
				renderer.sprite = sprites[PIPE_MAJOR];
				break;
			case 6: // right + down
				renderer.sprite = sprites[PIPE_L_RIGHT];
				break;
			case 7: // right + down + up
				renderer.sprite = sprites[PIPE_TRIP_UR];
				break;
			case 8: // left
				renderer.sprite = sprites[PIPE_MINOR];
				break;
			case 9: // up + left
				renderer.sprite = sprites[PIPE_L_LEFT];
				break;
			case 10: // right + left
				renderer.sprite = sprites[PIPE_MINOR];
				break;
			case 11: // up + right + left
				renderer.sprite = sprites[PIPE_TRIP_UL];
				break;
			case 12: // down + left
				renderer.sprite = sprites[PIPE_L_DOWN];
				break;
			case 13: // up + down + left
				renderer.sprite = sprites[PIPE_TRIP_LL];
				break;
			case 14: // right + down + left
				renderer.sprite = sprites[PIPE_TRIP_LR];
				break;
			case 15: // right + down + left
				renderer.sprite = sprites[PIPE_CROSS];
				break;
			default:
				break;
		};
		
	}
	
	private void OnPlaceablePlaced(Placeable other)
	{
		if(this.IsNextTo(other))
			UpdateSprite();
	}
}
