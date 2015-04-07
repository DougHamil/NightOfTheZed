using UnityEngine;
using System.Collections;

public class Inactivate : MonoBehaviour {

	public float Duration = 0.5f;
	private float timer = 0f;
	
	// Update is called once per frame
	void Update () {
		if(timer <= 0f)
		{
			timer = Duration;
		}
		timer -= Time.deltaTime;
		if(timer <= 0f)
		{
			this.gameObject.SetActive(false);
		}
	}
}
