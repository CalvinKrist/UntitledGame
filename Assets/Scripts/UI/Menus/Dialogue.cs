using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Untitled.UI;
using UnityEditor;
using UnityEngine.EventSystems;

namespace Untitled 
{
	namespace UI
	{
		/**
		* A UI element that is an entire menu and submenus will
		* mostly be disabled, and is closed by clicking off it or
		* clicking escape. It is intended to be the sole focus of
		* player while open. Abstracts out the logic of hiding / revealing.
		*
		* Example: The BuildingInspector
		*/
		[RequireComponent(typeof(Canvas))]
		public class Dialogue : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
		{
			protected Canvas canvas;
			protected bool hasMouseFocus; // true if mouse is over object
			
			protected void Awake()
			{
				canvas = GetComponent<Canvas>();
				hasMouseFocus = false;
				Disable();
			}
			
			protected void Update()
			{
				// Close if escape is pressed
				if(Input.GetKeyDown("escape") && canvas.enabled)
					Disable();
				// Close if click outside menu
				if(Input.GetMouseButtonDown(0) && !hasMouseFocus)
					Disable();
			}
			
			public virtual void Disable()
			{
				canvas.enabled = false;
				hasMouseFocus = false;
			}
			
			public virtual void Enable()
			{
				canvas.enabled = true;
			}
			
			/*************************
			****  Event Handlers  ****
			**************************/
			public void OnPointerEnter(PointerEventData data) 
			{
				hasMouseFocus = true;
			}
			public void OnPointerExit(PointerEventData data) 
			{
				hasMouseFocus = false;
			}
		}
	}
}