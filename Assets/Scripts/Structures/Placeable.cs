using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Untitled.Tiles;
using System;
using Untitled.Utils;

public class Placeable : MonoBehaviour
{
    public int cost;
	public List<TileType> placeableTiles;
	public Vector2Int size;
	public Coords coords;
	
	protected void Start()
	{
		coords = new Coords(this.gameObject.transform.position);
		OnPlaceableCreateEvent?.Invoke(this);
	}
	
	protected void OnDestroy() 
	{
		OnPlaceableDestroyEvent?.Invoke(this);
	}
	
	public bool IsBuilding()
	{
		return this.gameObject.GetComponent<Building>() != null;
	}
	public bool IsCable()
	{
		return this.gameObject.GetComponent<Cable>() != null;
	}
	
	/***************
	***  Events  ***
	****************/
	public static event Action<Placeable> OnPlaceableCreateEvent;
	public static event Action<Placeable> OnPlaceableDestroyEvent;
}
