using UnityEngine;
using System.Collections;

/**
 * Simple raycast-based bullet firing weapon
 */
public class BulletWeapon : Weapon {

	// --- Configurables ---
	public float Range = 50f;
	public int BulletsPerShot = 5;
	public int DamagePerBullet = 10;
	public float SprayAngle = 5f;
	public float ImpactForce = 10f;
	public LineRenderer BulletTracerPrefab;
	public Transform Muzzle;
	public Vector3 AttachmentOffset;
	public Light MuzzleFlashLight;
	public ParticleSystem BloodImpactParticles;

	// --- Private Fields ---
	private int MAX_BULLETS = 20;
	private LineRenderer[] bulletTracerPool;
	private SimpleTimer muzzleFlashTimer;
	
	void Start()
	{
		muzzleFlashTimer = new SimpleTimer(
			() => MuzzleFlashLight.enabled = true,
			() => MuzzleFlashLight.enabled = false,
			0.1f);
		bulletTracerPool = new LineRenderer[MAX_BULLETS];
		for(int i = 0; i < MAX_BULLETS; i++)
		{
			bulletTracerPool[i] = Instantiate(BulletTracerPrefab);
			bulletTracerPool[i].gameObject.SetActive(false);
		}
	}

	override protected void doUpdate()
	{
		this.muzzleFlashTimer.Update();
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		PlayerWeaponHandler weaponHandler = other.gameObject.GetComponent<PlayerWeaponHandler>();
		if(weaponHandler != null)
		{
			weaponHandler.EquipWeapon(this);
		}
	}

	override protected void fire(Vector3 aimPoint) {
		aimPoint.z = this.Muzzle.position.z;
		Vector2 sourcePosition = new Vector2(this.Muzzle.position.x, this.Muzzle.position.y);
		Vector2 destPosition = new Vector2(aimPoint.x, aimPoint.y);
		Vector2 direction = (destPosition - sourcePosition).normalized;
		
		float firingAngle = Mathf.Atan2 (direction.y,direction.x)*Mathf.Rad2Deg;
		muzzleFlashTimer.Restart();
		
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
				Damageable damageable = hitObj.GetComponent<Damageable>();
				fireEndPosition = new Vector3(hit.point.x, hit.point.y, -10f);
				
				if(damageable != null)
				{
					damageable.Damage(this.DamagePerBullet);
					Instantiate(BloodImpactParticles, fireEndPosition, Quaternion.identity);
					damageable.GetComponent<Rigidbody2D>().AddForce((fireStartPosition - hitObj.transform.position).normalized * -ImpactForce);
				}
			}
			
			// Draw tracers
			LineRenderer tracer = this.bulletTracerPool[i];
			tracer.gameObject.SetActive(true);
			Vector3 shotDelta = fireEndPosition - fireStartPosition;
			fireEndPosition = fireStartPosition + shotDelta.normalized * (shotDelta.magnitude / Random.Range(1f, 2f));
			tracer.SetPosition(0, fireStartPosition);
			tracer.SetPosition(1, fireEndPosition);
		}
	}
}
