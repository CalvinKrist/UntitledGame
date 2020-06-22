using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace Untitled
{
	namespace UI
	{
		[RequireComponent(typeof(Button))]
		public class ToggleButton : MonoBehaviour
		{
			private Button button;
			public float outlineWidth;
			public float scaleFactor;
			public Color outlineColor;
			private Outline outline;
			
			// Callbacks
			public EventTrigger.TriggerEvent toggleOnCallbacks;
			public EventTrigger.TriggerEvent toggleOffCallbacks;
			
			private bool toggleState;
			
			void Awake()
			{
				button = this.gameObject.GetComponent<Button>();
				button.onClick.AddListener(OnButtonClick);
				toggleState = false;
				
				if(this.gameObject.GetComponent<Outline>() == null)
					outline = this.gameObject.AddComponent<Outline>();
				else
					outline = this.gameObject.GetComponent<Outline>();
				
				outline.effectDistance = new Vector2(outlineWidth, outlineWidth);
				outline.effectColor    = outlineColor;
				outline.enabled        = false;
			}

			private void OnButtonClick()
			{
				toggleState = !toggleState;
				
				if(toggleState)
				{
					outline.enabled = true;
					this.transform.localScale *= scaleFactor;
					toggleOnCallbacks.Invoke(new BaseEventData(EventSystem.current));
				}
				else 
				{
					outline.enabled = false;
					this.transform.localScale /= scaleFactor;
					toggleOffCallbacks.Invoke(new BaseEventData(EventSystem.current));
				}
			}
		}
	}
}
