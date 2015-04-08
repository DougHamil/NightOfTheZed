using UnityEngine;
using System.Collections;

public class Damageable : MonoBehaviour {

	public int Health = 100;

	private int currentHealth;
	private bool isDead = false;

	void Start()
	{
		this.currentHealth = Health;
		this.isDead = false;
	}

	public int GetCurrentHealth()
	{
		return this.currentHealth;
	}

	public void Damage(int damage)
	{
		this.currentHealth -= damage;
		if(this.currentHealth < 0)
			this.currentHealth = 0;

		if(this.currentHealth == 0 && !isDead)
		{
			isDead = true;
			this.gameObject.SendMessage("OnKilled", SendMessageOptions.DontRequireReceiver);
		}
		else
		{
			this.gameObject.SendMessage("OnDamaged", damage, SendMessageOptions.DontRequireReceiver);
		}
	}



}
