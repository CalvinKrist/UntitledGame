using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Untitled.Resource;
using Untitled.UI;
using Untitled.Controller;
using UnityEditor;

namespace Untitled 
{
	namespace UI
	{
		[RequireComponent(typeof(Canvas))]
		public class BuildingInspectorMenu : Dialogue
		{			
			
			private Building building;
			
			public float UIRefreshRate = 0.1f;
			
			// UI elements
			private Text title;
			private Text moneyLabel;
			private Text powerLabel;
			private Text populationLabel;
			private Text generatedResourceLabel;
			private Image generatedResourceImage;
			private GameObject buildingInfoPanel;
			private GameObject customBuildingInspector;
		
			// Start is called before the first frame update
			void Awake()
			{
				base.Awake();
				
				// Find UI elements
				title = GameObject.Find("BuildingInspectorTitle").GetComponent<Text>();
				moneyLabel = GameObject.Find("MoneyIncomeLabel").GetComponent<Text>();
				powerLabel = GameObject.Find("PowerDrainLabel").GetComponent<Text>();
				populationLabel = GameObject.Find("PopManagementLabel").GetComponent<Text>();
				generatedResourceLabel = GameObject.Find("GeneratedResourceLabel").GetComponent<Text>();
				generatedResourceImage = GameObject.Find("GeneratedResourceIcon").GetComponent<Image>();
				buildingInfoPanel = GameObject.Find("BuildingInfoPanel");
				
				// Subscribe to events
				ClickableSprite.OnSpriteClickEvent += OnSpriteClick;
			}
			
			/*
			* Coroutine that periodically checks to see if resource values have updated.
			* If so, it updates the labels in the resource bar.
			*/
			private IEnumerator UpdateUI()
			{
				for(;;) 
				{
					if(!canvas.enabled)
						break;
					
					title.text = building.name;
					powerLabel.text = (building.GetPowerDrain()).ToString();
					moneyLabel.text = (building.GetMoneyIncome()).ToString();
					ResourceStorage storage = building.GetComponent<ResourceStorage>();
					populationLabel.text = ((int)storage.GetResourceCount(ResourceType.Population)).ToString() + " / " + building.maxPops.ToString();
					
					if(building.generatedResourceType != ResourceType.None)
						generatedResourceLabel.text = building.GetResourceIncome().ToString();
										
					yield return new WaitForSeconds(UIRefreshRate);
				}
			}	
			
			private void Enable(Building building)
			{
				if(building != this.building) {
					this.building = building;
					Destroy(customBuildingInspector);
					
					if (building.name == "Power Plant") {
						customBuildingInspector = Instantiate(buildingInfoPanel);
						customBuildingInspector.transform.SetParent(buildingInfoPanel.transform);
						customBuildingInspector.AddComponent<PowerPlantInspector>();
						customBuildingInspector.GetComponent<PowerPlantInspector>().SetBuilding(building);
					}
				}
				
				switch(building.generatedResourceType) {
					case ResourceType.Money:
						generatedResourceImage.enabled = true;
						generatedResourceLabel.enabled = true;
						generatedResourceImage.overrideSprite = Resources.Load<Sprite>("UI_Sprites/coinPile");
						break;
					case ResourceType.Power:
						generatedResourceImage.enabled = true;
						generatedResourceLabel.enabled = true;
						generatedResourceImage.overrideSprite = Resources.Load<Sprite>("UI_Sprites/energy");
						break;
					case ResourceType.Population:
						generatedResourceImage.enabled = true;
						generatedResourceLabel.enabled = true;
						generatedResourceImage.overrideSprite = Resources.Load<Sprite>("UI_Sprites/Person");
						break;
					default:
						generatedResourceImage.enabled = false;
						generatedResourceLabel.enabled = false;
						break;
				}
				
				base.Enable();
				StartCoroutine("UpdateUI");
			}
			
			public override void Disable()
			{
				base.Disable();
			}

			private void OnSpriteClick(ClickableSprite sprite) 
			{
				if(Player.Instance.GetCurrentState() is PlayerSelectingState && 
					!canvas.enabled && 
					sprite.gameObject.GetComponent<Building>() != null)
					Enable(sprite.gameObject.GetComponent<Building>());
			}
			
			/**************************
			***   Button Bindings   ***
			***************************/
			
			public void PopulationIncreased()
			{
				PopulationManager.AssignWorker(building);
			}
			
			public void PopulationDecreased()
			{
				PopulationManager.UnassignWorker(building);
			}
			
			public void DeleteBuilding()
			{
				Destroy(building.gameObject);
				Disable();
			}
		
		}
	}
}