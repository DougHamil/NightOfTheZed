using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour {

	public GameObject target;
	public float speed = 20;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		var direction = (target.transform.position - this.transform.position).normalized;
		GetComponent<Rigidbody2D> ().velocity = new Vector2 (direction.x, direction.y) * speed;
		this.transform.rotation = Quaternion.LookRotation (direction, Vector3.forward);
	}
}
