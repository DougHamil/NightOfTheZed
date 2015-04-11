using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour {

	public float attackRange = 1f;
	public GameObject target;
	public float speed = 20;
	public Animator TorsoAnimator;

	private Rigidbody2D rigidBody;
	private SimpleTimer reelFromHitTimer;

	void Start () {
		this.rigidBody = this.GetComponent<Rigidbody2D>();
		this.reelFromHitTimer = new SimpleTimer(
			() => TorsoAnimator.SetBool("isHit", true),
			() => TorsoAnimator.SetBool("isHit", false),
			0.5f);
	}

	void OnDamaged(int damage){
		this.reelFromHitTimer.Restart();
	}

	void Update () {
		reelFromHitTimer.Update();
		// Update Movement
		var targetPosition = new Vector3(target.transform.position.x, target.transform.position.y, this.transform.position.z);
		var direction = (targetPosition - this.transform.position);
		float distance = direction.magnitude;

		bool isReeling = reelFromHitTimer.IsActive;
		if(isReeling)
		{
			//rigidBody.velocity = Vector2.zero;
		}
		else
		{
			bool isMoving = distance > this.attackRange;
			bool isAttacking = !isMoving;

			if(isMoving)
			{
				direction.Normalize();
				this.rigidBody.velocity = new Vector2 (direction.x, direction.y) * speed;
			}

			// Update Rotation
			this.transform.rotation = Quaternion.AngleAxis(Mathf.Atan2 (direction.y,direction.x)*Mathf.Rad2Deg - 90f, Vector3.forward);
			this.transform.eulerAngles = new Vector3(0f, 0f, this.transform.eulerAngles.z);
			this.rigidBody.rotation = this.transform.eulerAngles.z;

			// Update animation
			this.TorsoAnimator.SetBool("isWalking", isMoving);
			this.TorsoAnimator.SetBool("isAttacking", isAttacking);
		}
	}
}
