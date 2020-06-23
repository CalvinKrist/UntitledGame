using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using Untitled.Controller;

/*
* A class attached to a button. When the button is clicked,
* it will change the object placer to select the new
* object type.
*/
[RequireComponent(typeof(Button))]
public class ObjectSelector : MonoBehaviour
{
	
	public Placeable placeableObject;
	
	private Player player;
	
	private Button button;
	private Outline outline;
	
	void Awake() 
	{
		button = this.gameObject.GetComponent<Button>();
		button.onClick.AddListener(OnClick);
		
		if(this.gameObject.GetComponent<Outline>() == null)
		{
			outline = this.gameObject.AddComponent<Outline>();
		}
		outline.effectDistance = new Vector2(2, 2);
		outline.effectColor  = new Color(0, 0, 0);
		outline.enabled = false;
		
		OnPlaceableButtonSelectedEvent += (Placeable obj) => {
			if(obj != placeableObject)
				Deselect();
		};
	}
	
    public void OnClick()
	{
		Select();
	}
	
	/*************************
	****  Event Handlers  ****
	**************************/
	public void Select()
	{
		OnPlaceableButtonSelectedEvent?.Invoke(placeableObject);
		
		if(!outline.enabled)
			outline.enabled = true;
	}
	public void Deselect()
	{
		if(outline.enabled)
			outline.enabled = false;
	}
	
	/***************
	***  Events  ***
	****************/
	public static event Action<Placeable> OnPlaceableButtonSelectedEvent;
}
