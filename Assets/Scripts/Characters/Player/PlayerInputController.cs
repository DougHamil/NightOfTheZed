using UnityEngine;
using System.Collections;

/**
 * Translates raw input to game-specific input
 */
public class PlayerInputController : MonoBehaviour {

	// ---- Configurables ----
	public bool rightStickAim = false;	// Use the gamepad stick for aiming?

	// ---- Delegates ----
	private delegate Quaternion getRotationDelegate();
	private delegate Vector3 getAimPointDelegate();

	// ---- Private Fields ----
	private PlayerMovement movementComponent;
	private PlayerWeaponHandler weaponComponent;
	private PlayerAnimator animator;
	private getRotationDelegate getRotation;
	private getAimPointDelegate getAimPoint;
	private Quaternion lastRotation = Quaternion.identity;
	
	void Start () {

		this.movementComponent = this.gameObject.GetComponent<PlayerMovement>();
		this.weaponComponent = this.gameObject.GetComponent<PlayerWeaponHandler>();
		this.animator = this.gameObject.GetComponent<PlayerAnimator>();

		if (rightStickAim)
		{
			getRotation = getRightStickRotation;
			getAimPoint = getRightStickAimPosition;
		}
		else
		{
			getRotation = getMouseRotation;
			getAimPoint = getMouseAimPosition;
		}
	}
	
	void Update () {

		// Movement
		Vector2 moveDirection = new Vector2(Input.GetAxis("Horizontal"),Input.GetAxis ("Vertical")).normalized;
		movementComponent.SetMoveDirection(moveDirection);
		animator.SetMoveDirection(moveDirection);
		animator.SetRotation(getRotation());

		// Aim
		weaponComponent.SetAimPoint(getAimPoint());

		// Interaction TODO

		// Firing
		if(Input.GetButtonDown("Fire1"))
			weaponComponent.BeginFire();
		else if(Input.GetButtonUp("Fire1"))
			weaponComponent.EndFire();
	}

	private Quaternion getMouseRotation() {
		Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector3 delta = (transform.position - mousePosition).normalized;
		float zAngle = Mathf.Atan2 (delta.y, delta.x) * Mathf.Rad2Deg + 90f;
		return Quaternion.AngleAxis(zAngle, new Vector3(0, 0, 1f));
	}
	
	private Quaternion getRightStickRotation() {
		Vector3 rightStickAim = new Vector3 (Input.GetAxis ("RightHorizontal"), Input.GetAxis ("RightVertical"));
		if(rightStickAim.sqrMagnitude != 0)
			lastRotation = Quaternion.AngleAxis(Mathf.Atan2 (rightStickAim.y,rightStickAim.x)*Mathf.Rad2Deg, Vector3.forward);
		return lastRotation;
	}
	
	private Vector3 getMouseAimPosition(){
		Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mousePosition.z = transform.position.z;
		Vector3 delta = (mousePosition - transform.position ).normalized;
		return delta * 1000f;
	}
	
	private Vector3 getRightStickAimPosition() {
		Vector3 rightStickAim = new Vector3 (Input.GetAxis ("RightHorizontal"), Input.GetAxis ("RightVertical"));
		return rightStickAim * 1000f;
	}
}
