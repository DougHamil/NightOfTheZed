using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
	public Transform TorsoTransform;
	public Transform LegsTransform;
	public float MoveSpeed = 10.0f;
	
	private Rigidbody2D rigidBody;
	
	void Start () {
		this.rigidBody = this.gameObject.GetComponent<Rigidbody2D>();
	}
	
	void Update () {
		// Update Movement
		Vector2 moveDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;
		Vector2 moveVector = moveDirection * MoveSpeed;
		// Use rigidbody movement to allow for forces to affect character
		this.rigidBody.velocity = moveVector;

		// Point legs in direction of movement, if moving
		if(moveDirection.sqrMagnitude > 0f)
			LegsTransform.eulerAngles = new Vector3(0f, 0, -Mathf.Rad2Deg * Mathf.Atan2 (moveDirection.x, moveDirection.y));
		
		// Update Aim
		Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Quaternion rot = Quaternion.LookRotation(TorsoTransform.position - mousePosition, Vector3.forward);
		TorsoTransform.rotation = rot;
		TorsoTransform.eulerAngles = new Vector3(0, 0, TorsoTransform.eulerAngles.z);
	}
}
