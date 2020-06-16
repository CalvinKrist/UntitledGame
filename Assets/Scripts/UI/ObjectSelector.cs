using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Untitled;

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
	private ObjectPlacer placer;
	
	private Button button;
	private Outline outline;
	
	void Awake() 
	{
		button = this.gameObject.GetComponent<Button>();
		button.onClick.AddListener(OnClick);
		
		player = Player.Instance;
		placer = player.GetComponent<ObjectPlacer>();
		
		if(this.gameObject.GetComponent<Outline>() == null)
		{
			outline = this.gameObject.AddComponent<Outline>();
		}
		outline.effectDistance = new Vector2(2, 2);
		outline.effectColor  = new Color(0, 0, 0);
		outline.enabled = false;
	}
	
    public void OnClick()
	{
		player.OnStateChange(PlayerState.Placing);
		placer.SetObject(placeableObject);
		Select();
	}
	
	/*************************
	****  Event Handlers  ****
	**************************/
	public void Select()
	{
		if(!outline.enabled)
			outline.enabled = true;
	}
	public void Deselect()
	{
		if(outline.enabled)
			outline.enabled = false;
	}
}
