using UnityEngine;
using System.Collections;

public class DestroyOnDeath : MonoBehaviour {


	void OnKilled()
	{
		Destroy (this.gameObject);
	}
}
