using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
	public Transform TorsoTransform;
	public Transform LegsTransform;
	public Transform TorsoSkeletonRoot;
	public float MoveSpeed = 10.0f;
	public bool rightStickAim = false;

	private Animator torsoAnimator;
	private Animator legsAnimator;
	private Rigidbody2D rigidBody;
	private Shotgun activeWeapon;

	private delegate Quaternion getRotationDelegate();
	private getRotationDelegate getRotation;
	
	void Start () {
		this.rigidBody = this.gameObject.GetComponent<Rigidbody2D>();
		this.torsoAnimator = TorsoTransform.gameObject.GetComponent<Animator>();
		this.legsAnimator = LegsTransform.gameObject.GetComponent<Animator>();
		if (rightStickAim)
			getRotation = getRightStickRotation;
		else
			getRotation = getMouseRotation;
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
			activeWeapon.SetAimPoint(this.transform.forward * 500f);

		// Update Animations
		legsAnimator.SetBool("isWalking", isMoving);
		torsoAnimator.SetBool("isWalking", isMoving);
		torsoAnimator.SetBool("isFiring", Input.GetButton("Fire1"));
		
		if(Input.GetButtonDown("Fire1") && activeWeapon != null)
			activeWeapon.BeginFire(this.transform.forward * 500f);
		if(Input.GetButtonUp("Fire1") && activeWeapon != null)
			activeWeapon.EndFire();
	}

	public void PickUpWeapon(Shotgun weapon)
	{
		this.activeWeapon = weapon;
		this.torsoAnimator.SetBool("isHoldingWeapon", true);
		if(weapon.AttachmentPoint != null)
		{
			Vector3 offset = weapon.transform.position -weapon.AttachmentTransform.position;
			Vector3 scale = weapon.transform.localScale;
			Transform attachParent = this.TorsoSkeletonRoot.FindChild(weapon.AttachmentPoint);

			weapon.transform.parent = attachParent;
			weapon.transform.localPosition = offset;
			weapon.transform.position = new Vector3(weapon.transform.position.x, weapon.transform.position.y, attachParent.position.z - 1.5f);
			weapon.transform.rotation = attachParent.rotation;
			weapon.transform.Rotate(new Vector3(0, 0, -90f));
			weapon.transform.localScale = scale;
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
