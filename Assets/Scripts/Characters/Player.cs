using UnityEngine;
using System.Collections;
using Spine;

public class Player : MonoBehaviour
{
	public Transform TorsoTransform;
	public Transform LegsTransform;
	public Transform TorsoSkeletonRoot;
	public float MoveSpeed = 10.0f;
	public bool rightStickAim = false;

	private SkeletonAnimation torsoAnimator;
	private Animator legsAnimator;
	private Rigidbody2D rigidBody;
	private Shotgun activeWeapon;

	private delegate Quaternion getRotationDelegate();
	private getRotationDelegate getRotation;
	
	void Start () {
		this.rigidBody = this.gameObject.GetComponent<Rigidbody2D>();
		this.torsoAnimator = TorsoTransform.gameObject.GetComponent<SkeletonAnimation>();
		this.legsAnimator = LegsTransform.gameObject.GetComponent<Animator>();
		if (rightStickAim)
			getRotation = getRightStickRotation;
		else
			getRotation = getMouseRotation;

		this.torsoAnimator.skeleton.data.FindEvent("fire");
		this.torsoAnimator.state.Event += OnSpineEvent;
	}

	void OnSpineEvent (Spine.AnimationState state, int trackIndex, Spine.Event e)
	{
		if(e.Data.name == "fire")
		{
			if(this.activeWeapon)
				this.activeWeapon.fire ();//this.activeWeapon.BeginFire(this.TorsoTransform.up * 500f);
		}
	}
	
	void Update () {
		// Update Movement
		Vector2 moveDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;
		Vector2 moveVector = moveDirection * MoveSpeed;
		// Use rigidbody movement to allow for forces to affect character
		this.rigidBody.velocity = moveVector;

		// Point legs in direction of movement, if moving
		bool isMoving = moveDirection.sqrMagnitude > 0f;
		if(isMoving)
			LegsTransform.eulerAngles = new Vector3(0f, 0, -Mathf.Rad2Deg * Mathf.Atan2 (moveDirection.x, moveDirection.y));

		// Update Aim
		TorsoTransform.rotation = getRotation();
		TorsoTransform.eulerAngles = new Vector3(0, 0, TorsoTransform.eulerAngles.z);
		if(activeWeapon != null)
			activeWeapon.SetAimPoint(this.TorsoTransform.up * 500f);

		// Update Animations
		legsAnimator.SetBool("isWalking", isMoving);
		if(Input.GetButton("Fire1"))
			torsoAnimator.AnimationName = "fire_weapon_2hand";
		else if(isMoving)
			torsoAnimator.AnimationName = "walk";
		else
			torsoAnimator.AnimationName = "idle";
		
		if(Input.GetButtonUp("Fire1") && activeWeapon != null)
			activeWeapon.EndFire();
	}

	public void PickUpWeapon(Shotgun weapon)
	{
		this.activeWeapon = weapon;
		if(weapon.AttachmentPoint != null)
		{
			Vector3 offset = weapon.transform.position - weapon.AttachmentTransform.position;
			Vector3 scale = weapon.transform.localScale;
			Transform attachParent = this.TorsoSkeletonRoot.FindChild(weapon.AttachmentPoint);
			weapon.transform.parent = attachParent;
			weapon.transform.position = new Vector3(weapon.transform.position.x, weapon.transform.position.y, attachParent.position.z - 1.5f);
			weapon.transform.rotation = attachParent.rotation;
			weapon.transform.Rotate(new Vector3(0, 0, -90f));
			weapon.transform.localScale = scale;
			offset.z = -0.1f;
			weapon.transform.localPosition = offset;
			
		}
	}

	private Quaternion getMouseRotation() {
		Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		return Quaternion.LookRotation(TorsoTransform.position - mousePosition, Vector3.forward);
	}

	private Quaternion lastRotation = Quaternion.identity;
	private Quaternion getRightStickRotation() {
		Vector3 rightStickAim = new Vector3 (Input.GetAxis ("RightHorizontal"), Input.GetAxis ("RightVertical"));
		if(rightStickAim.sqrMagnitude != 0)
			lastRotation = Quaternion.AngleAxis(Mathf.Atan2 (rightStickAim.y,rightStickAim.x)*Mathf.Rad2Deg, Vector3.forward);
		return lastRotation;
	}
}
