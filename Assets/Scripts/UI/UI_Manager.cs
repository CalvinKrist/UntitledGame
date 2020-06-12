using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Untitled.Resource;
using Untitled;
using System;

namespace Untitled
{
	namespace UI 
	{
		
		public class UI_Manager : MonoBehaviour
		{
			
			/***************
			***  Events  ***
			****************/
			public event Action<AClickableSprite> OnSpriteClickEvent;
			
			#region SINGLETON PATTERN
			public static UI_Manager _instance;
			public static UI_Manager Instance
			{
			 get {
				 if (_instance == null)
				 {
					 _instance = GameObject.FindObjectOfType<UI_Manager>();
					 
					 if (_instance == null)
					 {
						 GameObject container = new GameObject("UI");
						 _instance = container.AddComponent<UI_Manager>();
					 }
				 }
			 
				 return _instance;
			 }
			}
			#endregion
			
			public float UIRefreshRate = 0.05f;
			
			[Header("Resource Palel")]
			public string powerLabelName = "PowerLabel";
			public string moneyLabelName = "MoneyLabel";
			public string popLabelName = "PopLabel";
			private ResourceStorage playerStorage;
			
			private Text powerLabel;
			private Text moneyLabel;
			private Text popLabel;

			
			// Start is called before the first frame update
			void Start()
			{
				powerLabel = GameObject.Find(powerLabelName).GetComponent<Text>();
				moneyLabel = GameObject.Find(moneyLabelName).GetComponent<Text>();
				popLabel = GameObject.Find(popLabelName).GetComponent<Text>();
				
				playerStorage = Player.Instance.GetStorage();
				
				StartCoroutine("UpdateUI");
			}

			/*
			* Coroutine that periodically checks to see if resource values have updated.
			* If so, it updates the labels in the resource bar.
			*/
			IEnumerator UpdateUI()
			{
				for(;;) 
				{
					powerLabel.text = ((int)playerStorage.GetResourceCount(ResourceType.Power)).ToString();
					moneyLabel.text = ((int)playerStorage.GetResourceCount(ResourceType.Money)).ToString();
					popLabel.text = ((int)playerStorage.GetResourceCount(ResourceType.Population)).ToString();
					
					yield return new WaitForSeconds(UIRefreshRate);
				}
			}
			
			public static void OnSpriteClick(AClickableSprite sprite)
			{
				if(Player.Instance.state == PlayerState.Selecting)
					Instance.OnSpriteClickEvent?.Invoke(sprite);
			}
		}
	}
}
