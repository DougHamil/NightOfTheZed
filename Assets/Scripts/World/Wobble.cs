using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Wobble : MonoBehaviour {

	public float Duration = 1.0f;
	public float Magnitude = 1.0f;

	void Start () {
		this.transform.eulerAngles = new Vector3(0f, Magnitude, 0f);
		pickNewRotation();
	}
	
	void Update () {
			//pickNewRotation();
	}

	private void pickNewRotation()
	{
		Vector3 targetAngle = Vector3.zero;
		if(this.transform.eulerAngles.y < 180f)
			targetAngle = new Vector3(0f, -Magnitude, 0f);
		else
			targetAngle = new Vector3(0f, Magnitude, 0f);
		Tween rotTween = this.transform.DORotate(targetAngle, Duration, RotateMode.Fast);
		rotTween.SetEase(Ease.InOutSine);
		rotTween.OnComplete(() => pickNewRotation());
	}
}
