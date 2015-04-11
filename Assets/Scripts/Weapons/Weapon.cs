using UnityEngine;
using System.Collections;

/**
 * Contains configuration information about a weapono
 */
public abstract class Weapon : MonoBehaviour {

	// --- Configurables ---
	public string Name;
	public string FireAnimation;
	public float FireAnimationDuration = 1.0f;
	public string AttachmentBone;
	public Vector3 AttachmentPositionOffset;
	public Vector3 AttachmentRotationOffset;
	public float FireDelay = 0.5f;			// Seconds between shots

	public bool IsFiring {
		get { return isFiring;}
	}

	// --- Fields ---
	private Vector3 aimPoint;
	private float fireTimer;
	private bool isFiring = false;

	void Update() {
		doUpdate();
		fireTimer -= Time.deltaTime;
		if(fireTimer < 0f)
			fireTimer = 0f;
		if(fireTimer == 0f && isFiring)
		{
			fireTimer = FireDelay;
			//fire(this.aimPoint);
		}
	}

	protected abstract void fire(Vector3 aimPoint);
	protected abstract void doUpdate();

	public void OnFireEvent() { fire (this.aimPoint);}
	public void BeginFire(){ isFiring = true;}
	public void EndFire(){ isFiring = false;}
	public void SetAimPoint(Vector3 aimPoint){ this.aimPoint = aimPoint;}
}
