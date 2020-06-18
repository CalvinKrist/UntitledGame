using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Untitled.Tiles;
using System;

public class Placeable : MonoBehaviour
{
    public int cost;
	public List<TileType> placeableTiles;
	public Vector2 size;
	
	protected void Start()
	{
		OnPlaceableCreateEvent?.Invoke(this);
	}
	
	protected void OnDestroy() 
	{
		OnPlaceableDestroyEvent?.Invoke(this);
	}
	
	/***************
	***  Events  ***
	****************/
	public static event Action<Placeable> OnPlaceableCreateEvent;
	public static event Action<Placeable> OnPlaceableDestroyEvent;
}
