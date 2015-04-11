using UnityEngine;
using System.Collections;

public class SimpleTimer {

	public delegate void StartTimerDelegate();
	public delegate void EndTimerDelegate();

	public bool IsActive {
		get { return this.timer > 0f; }
	}

	private StartTimerDelegate startDele;
	private EndTimerDelegate endDele;
	private float time;
	private float timer;

	public SimpleTimer(StartTimerDelegate startDele, EndTimerDelegate endDele, float time)
	{
		this.startDele = startDele;
		this.endDele = endDele;
		this.time = time;
	}

	public void Restart()
	{
		this.timer = this.time;
		startDele();
	}

	public void Update () {
		if(this.timer > 0f)
		{
			this.timer -= Time.deltaTime;
			if(this.timer <= 0f)
			{
				this.endDele();
			}
		}
	}
}
