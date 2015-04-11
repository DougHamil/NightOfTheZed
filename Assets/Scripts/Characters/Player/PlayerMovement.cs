using UnityEngine;
using System.Collections;

/**
 * Handles moving the player through the world.
 */
public class PlayerMovement : MonoBehaviour {

	public float MoveSpeed = 10.0f;					// Units/sec
	public float HitRecoveryTime = 0.5f;			// Seconds

	private Rigidbody2D rigidBody;
	private Vector2 moveDirection;

	void Start () {
		this.rigidBody = this.gameObject.GetComponent<Rigidbody2D>();
	}
	
	void FixedUpdate() {
		Vector2 moveVector = moveDirection * MoveSpeed;
		this.rigidBody.velocity = moveVector;
	}

	public void SetMoveDirection(Vector2 moveDirection){
		this.moveDirection = moveDirection;
	}
}
