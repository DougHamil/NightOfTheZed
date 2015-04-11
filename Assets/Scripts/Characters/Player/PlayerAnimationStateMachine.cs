using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SM;

/**
 * Encodes the state machine for player animations
 */
public class PlayerAnimationStateMachine : StateMachine {

	public PlayerAnimationStateMachine() : base("Idle") {
		TransitionDelegate idleToWalk = b => ((bool)b["isWalking"]) ? "Walk" : null;
		TransitionDelegate idleToShoot = b => ((bool)b["isShooting"]) ? "Shoot" : null;
		this.AddTransition("Idle", idleToWalk);
		this.AddTransition("Idle", idleToShoot);
		
		TransitionDelegate walkToIdle = b => ((bool)b["isWalking"]) ? null : "Idle";
		TransitionDelegate walkToShoot = b => ((bool)b["isShooting"]) ? "Shoot" : null;
		this.AddTransition("Walk", walkToIdle);
		this.AddTransition("Walk", walkToShoot);

		TransitionDelegate shootToIdle = b => !((bool)b["isShooting"]) && !((bool)b["isWalking"]) ? "Idle" : null;
		TransitionDelegate shootToWalk = b => !((bool)b["isShooting"]) && ((bool)b["isWalking"]) ? "Walk" : null;
		this.AddTransition("Shoot", shootToIdle);
		this.AddTransition("Shoot", shootToWalk);
	}
}
