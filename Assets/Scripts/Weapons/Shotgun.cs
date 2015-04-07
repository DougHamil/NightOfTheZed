using UnityEngine;
using System.Collections;

public class Shotgun : MonoBehaviour {

	public string AttachmentPoint = "hand_r";
	public int BulletsPerShot = 12;
	public float FiringDelay = 1f;
	public Transform Muzzle;
	public Transform AttachmentTransform;

	private bool isFiring = false;
	private float shotTimer = 0f;
	private Vector3 aimPoint = Vector3.zero;

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
	}

	public void EndFire(){
		isFiring = false;
	}

	public void SetAimPoint(Vector3 aimPoint) {
		this.aimPoint = aimPoint;
	}

	private void fire() {
		Vector2 sourcePosition = new Vector2(this.Muzzle.position.x, this.Muzzle.position.y);
		Vector2 destPosition = new Vector2(this.aimPoint.x, this.aimPoint.y);
		RaycastHit2D hit = Physics2D.Raycast(sourcePosition, (destPosition - sourcePosition).normalized);
		if(hit.collider != null)
		{
			Debug.Log (hit.collider.gameObject);
		}
	}

	void Update () {
		if(shotTimer > 0f)
			shotTimer -= Time.deltaTime;
		if(isFiring && shotTimer <= 0f)
		{
			shotTimer = FiringDelay;
			fire();
		}
	}
}
