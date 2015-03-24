using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	public Transform Target;

	private float height; 
	void Start () {
		height = this.transform.position.z;
	}
	
	void Update () {
		this.transform.position = new Vector3(Target.position.x, Target.position.y, height);
	}
}
