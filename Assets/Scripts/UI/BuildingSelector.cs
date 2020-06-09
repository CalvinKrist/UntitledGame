using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Untitled;

public class BuildingSelector : MonoBehaviour
{
	
	public Building building;
	
	private Player player;
	private BuildingBuyer buyer;
	
	private Button button;
	private Outline outline;
	
	void Awake() 
	{
		button = this.gameObject.GetComponent<Button>();
		button.onClick.AddListener(OnClick);
		
		player = Player.Instance;
		buyer = player.GetComponent<BuildingBuyer>();
		
		if(this.gameObject.GetComponent<Outline>() == null)
		{
			outline = this.gameObject.AddComponent<Outline>();
		}
		outline.effectDistance = new Vector2(2, 2);
		outline.enabled = false;
	}
	
    public void OnClick()
	{
		player.OnStateChange(PlayerState.Placing);
		buyer.SetBuilding(building);
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
