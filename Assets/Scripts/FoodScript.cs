﻿using UnityEngine;
using System.Collections;

public class FoodScript : MonoBehaviour 
{
	public Vector2 speed = new Vector2(10f, 10f);
	public Vector2 Direction = new Vector2(0f, 0f);
	public float rotation;
	public Transform food;
	public int mass;
	private Vector2 movement;
	

	void Start () 
	{
		var tempRot = rotation;
		var multiplier = 1;
		
		if (tempRot == 90f)
			Direction.y += (multiplier);
		if (tempRot == 270f)
			Direction.y -= (multiplier);
		else if(tempRot == 180f)
			Direction.x -= (multiplier);
		else if(tempRot == 0f || tempRot == 360f)
			Direction.x += (multiplier);
		
		if (tempRot != 90f && tempRot != 180f && tempRot != 0f && tempRot != 360f && tempRot != 270f) 
		{
			//Get ratio from angle after converting radians to degrees, absolute value to prevent negatives
			var ratio = Mathf.Abs(Mathf.Tan(tempRot * Mathf.PI / 180f));
			var y = 0f;
			var x = 0f;
			var domainX = 1;
			var domainY = 1;
			
			if (tempRot > 90f && tempRot < 180f) 
			{
				domainX = -1;
			} else if (tempRot > 180f && tempRot < 270f) 
			{
				domainX = -1;
				domainY = -1;
			} else if (tempRot > 270f && tempRot < 360f) 
			{
				domainY = -1;
			}
			//If ratio of y/x is greater than 1, we have to use percentages to determine how we increment x and y
			if (ratio > 1f) 
			{
				y = ratio / (ratio + 1f);
				x = 1f / (ratio + 1f);
			}
			else 
			{
				x = (1f / (1f + ratio));
				y = (ratio / (1f + ratio));
			}
			
			Direction.x += (x * domainX);
			Direction.y += (y * domainY);
		}
		movement = new Vector2(
			speed.x * Direction.x,
			speed.y * Direction.y);
	}
	
	void FixedUpdate()
	{
		movement = new Vector2(
			speed.x * Direction.x,
			speed.y * Direction.y);

		GetComponent<Rigidbody2D>().velocity = movement;
	}
	
	void OnTriggerEnter2D (Collider2D other) 
	{
		if (other.name.Contains ("vertical")) 
		{
			Direction.x = Direction.x * -1;
		}
		if (other.name.Contains ("horizontal")) 
		{
			Direction.y = Direction.y * -1;
		}

		if (other.name.Contains ("Food")) 
		{
			if (GameObject.FindGameObjectsWithTag ("Background").Length > 0) 
			{
				GameObject Background = GameObject.FindGameObjectsWithTag ("Background") [0];

				if (Background != null)
				{

					Space s = Background.transform.GetComponent<Space>();

					if(!s.collison)
					{

						var foodTransform = Instantiate(food) as Transform;
						foodTransform.position = transform.position;
						FoodScript f = foodTransform.GetComponent<FoodScript>();
						f.Direction = Direction;
						f.mass = mass + (int)(other.gameObject.transform.localScale.x / 0.02f);
						f.speed = speed;
						foodTransform.localScale = new Vector3 (f.mass * 0.02f, f.mass * 0.02f, 1f);

						s.collison = true;

						Destroy(gameObject);

					}
					else
					{
						Destroy(gameObject);

						s.collison = false;
					}
				}
			}
		}

	}
}
