using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SM;

public class PlayerAnimator : MonoBehaviour {

	// --- Configurables ---
	public SkeletonAnimation TorsoAnimator;
	public Transform TorsoSkeletonRoot;
	public Animator LegsAnimator;

	// --- Private Fields ---
	private PlayerWeaponHandler weaponComponent;
	private string currentTorsoAnimation;
	private PlayerAnimationStateMachine animStateMachine;
	private Dictionary<string, float> torsoDefaultAnimationDurations;

	void Start() {
		weaponComponent = gameObject.GetComponent<PlayerWeaponHandler>();
		torsoDefaultAnimationDurations = new Dictionary<string, float>();
		animStateMachine = new PlayerAnimationStateMachine();
		animStateMachine.AddCallback("Idle", () => setTorsoAnimation("idle"));
		animStateMachine.AddCallback("Walk", () => setTorsoAnimation("walk"));
		animStateMachine.AddCallback("Shoot", () => setTorsoAnimation(weaponComponent.EquippedWeapon.FireAnimation, true, weaponComponent.EquippedWeapon.FireAnimationDuration));
		animStateMachine.SetParameter("isWalking", false);
		animStateMachine.SetParameter("isShooting", false);

		TorsoAnimator.state.Event += OnSpineEvent;
	}

	void OnSpineEvent(Spine.AnimationState state, int trackIndex, Spine.Event e)
	{
		if(e.Data.name == "fire")
		{
			weaponComponent.OnFireEvent();
		}
	}

	void Update() {
		animStateMachine.SetParameter("isShooting", weaponComponent.IsFiring);
		animStateMachine.Update();
	}

	void OnDamaged(){
		this.setTorsoAnimation("hit_01");
	}

	public Transform GetTorsoBone(string path) {
		return this.TorsoSkeletonRoot.FindChild(path);
	}

	public void SetMoveDirection(Vector2 moveDirection) {
		bool isMoving = moveDirection.sqrMagnitude > 0;
		if(isMoving)
			LegsAnimator.transform.eulerAngles = new Vector3(0f, 0, -Mathf.Rad2Deg * Mathf.Atan2 (moveDirection.x, moveDirection.y));
		LegsAnimator.SetBool("isWalking", isMoving);
		animStateMachine.SetParameter("isWalking", isMoving);
	}

	public void SetRotation(Quaternion rotation) {
		TorsoAnimator.transform.rotation = rotation;
		TorsoAnimator.transform.eulerAngles = new Vector3(0, 0, TorsoAnimator.transform.eulerAngles.z);
	}

	private void setTorsoAnimation(string animationName, bool overrideDuration, float duration) {
		if(this.currentTorsoAnimation == animationName)
			return;
		if(overrideDuration)
			TorsoAnimator.skeleton.data.FindAnimation(animationName).Duration = duration;
		else
			TorsoAnimator.skeleton.data.FindAnimation(animationName).Duration = getTorsoDefaultAnimationDuration(animationName);
		TorsoAnimator.state.ClearTrack(0);
		TorsoAnimator.state.SetAnimation(0, animationName, true);
		this.currentTorsoAnimation = animationName;
	}

	private void setTorsoAnimation(string animationName) {
		setTorsoAnimation(animationName, false, 1.0f);
	}

	private float getTorsoDefaultAnimationDuration(string name) {
		if(!this.torsoDefaultAnimationDurations.ContainsKey(name))
			this.torsoDefaultAnimationDurations[name] = TorsoAnimator.skeleton.data.FindAnimation(name).duration;
		return this.torsoDefaultAnimationDurations[name];
	}
}
