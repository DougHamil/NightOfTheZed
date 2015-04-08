using UnityEngine;
using System.Collections;

public class DestroyOnParticleEnd : MonoBehaviour {

	private ParticleSystem ps;
	void Start () {
		this.ps = this.GetComponent<ParticleSystem>();
	}
	
	void Update () {
		if(ps.isStopped)
			Destroy (this.gameObject);
	}
}
