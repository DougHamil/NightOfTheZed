using UnityEngine;
using System.Collections;

public class MeleeWeapon : MonoBehaviour {

	public int Damage = 10;

	private Collider2D meleeCollider;

	void Start () {
		this.meleeCollider = this.GetComponent<Collider2D>();
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		Damageable damageable = other.GetComponent<Damageable>();
		if(damageable != null && damageable.gameObject.tag == "Player")
		{
			damageable.Damage(this.Damage);
		}
	}
}
