using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Untitled.UI;
using Untitled.Configs;

namespace Untitled
{
	namespace UI
	{
		
		[RequireComponent(typeof(PolygonCollider2D))]
		[RequireComponent(typeof(SpriteRenderer))]
		public class ClickableSprite : MonoBehaviour
		{

			[Header("Shader Settings")]
			public bool enableDialogue;
			[Range(0.0f, 2.0f)]
			public float outlineWidth;
			public Shader shader;
			
			private Material mat;

			protected void Start()
			{
				if(enableDialogue) 
				{
					mat = GetComponent<Renderer>().material;
					mat.shader = shader;
					
					mat.SetFloat("_XWidth", 0f);
					mat.SetFloat("_YWidth", 0f);
				}				
			}
			
			void OnMouseEnter()
			{
				if(enableDialogue) 
				{
					mat.SetFloat("_XWidth", outlineWidth);
					mat.SetFloat("_YWidth", outlineWidth);
				}
			}

			void OnMouseExit()
			{
				if(enableDialogue) 
				{
					mat.SetFloat("_XWidth", 0f);
					mat.SetFloat("_YWidth", 0f);
				}
			}
			
			void OnMouseDown()
			{
				
			}
			
			void OnMouseUp()
			{
				
				OnMouseClick();
			}
			
			void OnMouseClick()
			{
				UI_Manager.OnSpriteClick(this);
			}
		}
		
	}
}
