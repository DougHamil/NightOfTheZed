using UnityEngine;
using System.Collections;

public class PlayerWeaponHandler : MonoBehaviour {

	// --- Configurables ---

	// --- Public Fields ---
	public Weapon EquippedWeapon { get; private set;}
	public bool IsFiring {
		get {
			return EquippedWeapon != null && EquippedWeapon.IsFiring;
		}
	}

	// --- Private Fields ---
	private PlayerAnimator animator;

	void Start() {
		animator = gameObject.GetComponent<PlayerAnimator>();

	}

	public void EquipWeapon(Weapon weapon)
	{
		this.EquippedWeapon = weapon;
		Vector3 weaponScale = weapon.transform.localScale;
		Transform attachPoint = animator.GetTorsoBone(weapon.AttachmentBone);
		weapon.transform.parent = attachPoint;
		weapon.transform.localPosition = weapon.AttachmentPositionOffset;
		weapon.transform.rotation = attachPoint.rotation;
		weapon.transform.localRotation = Quaternion.Euler(weapon.AttachmentRotationOffset);
		weapon.transform.localScale = weaponScale;
	}

	public void SetAimPoint(Vector3 point) {
		if(EquippedWeapon != null)
			EquippedWeapon.SetAimPoint(point);
	}

	public void OnFireEvent() {
		if(EquippedWeapon != null)
			EquippedWeapon.OnFireEvent();
	}

	public void BeginFire() {
		if(EquippedWeapon != null)
			EquippedWeapon.BeginFire();
	}

	public void EndFire() {
		if(EquippedWeapon != null)
			EquippedWeapon.EndFire();
	}
}
