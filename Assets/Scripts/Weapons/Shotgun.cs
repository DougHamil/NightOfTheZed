﻿using UnityEngine;
using System.Collections;

public class Shotgun : MonoBehaviour {

	public LineRenderer BulletTracerPrefab;
	public string AttachmentPoint = "hand_r";
	public int BulletsPerShot = 12;
	public float Range = 50f;
	public float SprayAngle = 5f;
	public float FiringDelay = 1f;
	public Transform Muzzle;
	public Transform AttachmentTransform;

	private LineRenderer[] bulletTracerPool;
	private bool isFiring = false;
	private float shotTimer = 0f;
	private Vector3 aimPoint = Vector3.zero;

	void Start()
	{
		bulletTracerPool = new LineRenderer[this.BulletsPerShot];
		for(int i = 0; i < this.BulletsPerShot; i++)
		{
			bulletTracerPool[i] = Instantiate(BulletTracerPrefab);
			bulletTracerPool[i].gameObject.SetActive(false);
		}
	}

	public void OnTriggerEnter2D(Collider2D other)
	{
		Player player = other.gameObject.GetComponent<Player>();

		if(player != null)
		{
			player.PickUpWeapon(this);
			this.gameObject.GetComponent<Collider2D>().enabled = false;
		}
	}

	public void BeginFire(Vector3 aimPoint){
		isFiring = true;
		this.aimPoint = aimPoint;
		this.shotTimer = 0f;
	}

	public void EndFire(){
		isFiring = false;
	}

	public void SetAimPoint(Vector3 aimPoint) {
		this.aimPoint = aimPoint;
	}

	public void fire() {
		this.aimPoint.z = this.Muzzle.position.z;
		Vector2 sourcePosition = new Vector2(this.Muzzle.position.x, this.Muzzle.position.y);
		Vector2 destPosition = new Vector2(this.aimPoint.x, this.aimPoint.y);
		Vector2 direction = (destPosition - sourcePosition).normalized;

		float firingAngle = Mathf.Atan2 (direction.y,direction.x)*Mathf.Rad2Deg;

		for(int i = 0; i < BulletsPerShot; i++)
		{
			float angle = (firingAngle + Random.Range(-SprayAngle, SprayAngle)) * Mathf.Deg2Rad;
			direction = new Vector2(Mathf.Cos (angle), Mathf.Sin (angle));
			RaycastHit2D hit = Physics2D.Raycast(sourcePosition, direction, Range);
			Vector3 fireStartPosition = this.Muzzle.position;
			Vector3 fireEndPosition = fireStartPosition + new Vector3(direction.x * Range, direction.y * Range, fireStartPosition.z);

			if(hit.collider != null)
			{
				GameObject hitObj = hit.collider.gameObject;
				if(hitObj.tag == "zombie")
				{
					Destroy (hitObj);
				}
			}

			// Draw tracers
			LineRenderer tracer = this.bulletTracerPool[i];
			tracer.gameObject.SetActive(true);
			tracer.SetPosition(0, fireStartPosition);
			tracer.SetPosition(1, fireEndPosition);
		}
	}

	void Update () {
		if(shotTimer > 0f)
			shotTimer -= Time.deltaTime;
		if(isFiring && shotTimer <= 0f)
		{
			shotTimer = FiringDelay;
			//fire();
		}
	}
}
