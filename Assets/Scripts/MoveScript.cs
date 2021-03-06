﻿using UnityEngine;
using System.Collections;

public class MoveScript : MonoBehaviour 
{
	public Vector2 speed = new Vector2(0f, 0f);
	public Vector2 Direction = new Vector2(0f, 0f);
	public float rotation;
	public int size;
	private Vector2 movement;

	void Start () 
	{
		float bulletSpeed = getCalculatedBulletSpeed();
		speed = new Vector2 (bulletSpeed, bulletSpeed);
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

			transform.Rotate(transform.rotation.x, transform.rotation.y, rotation);
			
			Direction.x += (x * domainX);
			Direction.y += (y * domainY);
		}
		else
			transform.Rotate(transform.rotation.x, transform.rotation.y, tempRot);
	}

	float getCalculatedBulletSpeed()
	{
		float calculatedSpeed = 40f;
		if (size >= 250)
			calculatedSpeed = 40f - 5f;
		else if(size >= 500)
			calculatedSpeed = 40f - 10f;
		else if(size >= 1500)
			calculatedSpeed = 40f - 15f;
		else if(size >= 3500)
			calculatedSpeed = 40f - 20f;
		return calculatedSpeed;
	}

	void Update()
	{
		movement = new Vector2(
			speed.x * Direction.x,
			speed.y * Direction.y);

		if (speed.x < 2f || speed.y < 2f)
			Destroy (gameObject);
	}

	void FixedUpdate()
	{
		GetComponent<Rigidbody2D>().velocity = movement * 3f;
	}

	void OnTriggerEnter2D (Collider2D other) 
	{
		if (other.name.Contains ("vertical") || other.name.Contains ("horizontal")) 
		{
			Destroy (gameObject );
		}
	}
}
