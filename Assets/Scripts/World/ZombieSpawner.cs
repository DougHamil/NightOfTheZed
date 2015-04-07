using UnityEngine;
using System.Collections;

public class ZombieSpawner : MonoBehaviour {

	public GameObject ZombiePrefab;
	public float Cooldown;
	public int NumberToSpawn;

	private float cooldownTimer = 0f;

	private void spawn()
	{
		for(int i =0; i < NumberToSpawn; i++)
		{
			Vector3 pos = this.transform.position;
			pos.x += Random.Range(0f, 15f);
			pos.y += Random.Range (0f, 15f);
			GameObject zombo = (GameObject)Instantiate(ZombiePrefab, pos, this.transform.rotation);
			zombo.GetComponent<EnemyMovement>().target = GameObject.FindGameObjectWithTag("Player");
		}
	}
	
	void Update () {
		if(cooldownTimer <= 0f)
		{
			cooldownTimer = Cooldown;
			spawn();
		}

		cooldownTimer -= Time.deltaTime;
	}
}
