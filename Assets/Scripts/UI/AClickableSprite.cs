using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Untitled.UI;
using Untitled.Configs;

namespace Untitled
{
	namespace UI
	{
		
		public enum SpriteType
		{
			Building,
			None
		}
		
		[RequireComponent(typeof(PolygonCollider2D))]
		[RequireComponent(typeof(SpriteRenderer))]
		public class AClickableSprite : Placeable
		{

			[Header("Shader Settings")]
			public bool outlineOnHover;
			[Range(0.0f, 2.0f)]
			public float outlineWidth;
			public Shader shader;
			
			private Material mat;
			
			protected SpriteType spriteType = SpriteType.None;

			protected void Start()
			{
				if(outlineOnHover) 
				{
					mat = GetComponent<Renderer>().material;
					mat.shader = shader;
					
					mat.SetFloat("_XWidth", 0f);
					mat.SetFloat("_YWidth", 0f);
				}				
			}
			
			public SpriteType GetType()
			{
				return this.spriteType;
			}
			
			void OnMouseEnter()
			{
				if(outlineOnHover) 
				{
					mat.SetFloat("_XWidth", outlineWidth);
					mat.SetFloat("_YWidth", outlineWidth);
				}
			}

			void OnMouseExit()
			{
				if(outlineOnHover) 
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
