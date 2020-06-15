using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Untitled.Resource;

namespace Untitled 
{
	namespace UI
	{
		[RequireComponent(typeof(Canvas))]
		public class PowerPlantInspector : MonoBehaviour
		{
			
			private Building powerPlant;
			
			private Text infoText;
			private GameObject infoTextGO;
			
			public float UIRefreshRate = 0.1f;
			
			// Start is called before the first frame update
			void Start()
			{
				// Center the custom display
				RectTransform transform = GetComponent<RectTransform>();
				transform.offsetMin = new Vector2(0, 0);
				transform.offsetMax = new Vector2(0, 0);
				
				// Create text game object
				infoTextGO = new GameObject();
				infoTextGO.transform.SetParent(this.gameObject.transform);
				
				// Create Text component
				infoText = infoTextGO.AddComponent<Text>();
				infoText.text = "";
				infoText.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
				infoText.color = Color.black;
				
				// Position text game object
				// Note: can't be done just after creating the GO
				// or the RectTransform doesn't exist.
				transform = infoTextGO.GetComponent<RectTransform>();
				transform.anchoredPosition = new Vector2(0, 0);
				
				StartCoroutine("UpdateUI");
			}

			// Update is called once per frame
			private IEnumerator UpdateUI()
			{
				for(;;) 
				{	
					infoText.text = "Remaining coal: " + powerPlant.inputStorage.GetResourceCount(ResourceType.Coal).ToString();
					
					yield return new WaitForSeconds(UIRefreshRate);
				}
			}
			
			public void SetBuilding(Building plant)
			{
				powerPlant = plant;
			}
		}
	}
}