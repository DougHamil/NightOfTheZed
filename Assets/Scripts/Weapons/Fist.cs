using UnityEngine;
using System.Collections;

public class Fist : MonoBehaviour {

	public float Force = 10f;
	
	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.attachedRigidbody != null)
		{
			Vector3 dir = ( other.transform.position - this.transform.position).normalized;
			Vector2 dir2D = new Vector2(dir.x, dir.y);
			other.attachedRigidbody.AddForce(dir2D * Force);
		}
	}
}
