using UnityEngine;
using System.Collections;

public class ExperienceOnDeath : MonoBehaviour {
	public int ExperienceGranted = 10;

	void OnKilled() {

		GameObject.FindGameObjectWithTag("Player").GetComponent<LevelHandler>().AddXp(this.ExperienceGranted);
		                                                                           
	}
}
