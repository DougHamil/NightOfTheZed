using UnityEngine;
using System.Collections;

public class World : MonoBehaviour {

	public Vector2 Gravity;

	void Start () {
		Physics2D.gravity = Gravity;
	}
	
}
