using UnityEngine;
using System.Collections;

public class XiaomonvSM : HeroController
{
	private float animChangeDuration = 0.05f;
	void Start ()
	{
//		IOHelper.GetAvaterUpgradeInfos();
		int level = AvatarDB.Instance.GetAvatarLvById(1);
		level = Mathf.Max(1, level);
		AvatarUpgradeInfo info = IOHelper.GetAvaterUpgradeInfoByIdAndLevel(1, level);
		hp = maxHp = info.hp * (1 + hpAddtional);
		Probability = info.Probability * 100f;
		crit = info.crit;
		hitDistance = info.distance;
		Init ();
	}

	void Init ()
	{
		Initialize<AnimState> ();
		ChangeState (AnimState.idle);
	}

	protected void idle_Enter ()
	{
		//jinzhan
		if (followSmoke != null)
			followSmoke.SetActive(false);
		if (currentWeaponId < WeaponType.gun_area)
		{
			if (currentWeaponId == WeaponType.dianju)
			{
				animation.CrossFade ("gunidle");
			}
			else
			{
				animation.CrossFade("meleeidle");
			}
		}
		//持枪
		else if (currentWeaponId < WeaponType.other_area)
		{
			if (currentWeaponId < WeaponType.gun_single)
				animation.CrossFade ("gunidle");
			else if (currentWeaponId < WeaponType.gun_double)
				animation.CrossFade("doublegunidle");
		}
//		else if (currentWeaponId == WeaponType.dianju)
//		{
//			animation.CrossFade ("gunidle");
//		}
	}
	
	protected void idle_Exit ()
	{
	}

	private Vector3 oldDir = Vector3.zero;
	IEnumerator skill_Enter()
	{
		if (followSmoke != null)
			followSmoke.SetActive(false);
		speed = 0;
		oldDir = dir;
		dir = Vector3.zero;
		rigidbody.velocity = Vector3.zero;
		rigidbody.constraints |= RigidbodyConstraints.FreezeRotationY;

//		if (currentWeaponId == WeaponType.chuizi)
//		{
//			animation.CrossFade("hammerattack02");
//			yield return new WaitForSeconds(1.7f);
//			ChangeState(AnimState.attack);
//		}
//		else
		{
			animation.CrossFade("meleeattack02");
			yield return new WaitForSeconds(1.36f);
			ChangeState(AnimState.attack);
		}

	}

	protected void skill_Exit()
	{
		speed = 8;
		rigidbody.constraints ^= RigidbodyConstraints.FreezeRotationY;
		dir = oldDir;
	}
	
	protected void walk_Enter ()
	{
		if (followSmoke != null)
			followSmoke.SetActive(true);
		if (!isFired)
		{
			if (currentWeaponId < WeaponType.gun_area)
			{
				if (currentWeaponId == WeaponType.dianju)
				{
					animation.CrossFade ("gunwalk");
				}
				else
				{
					animation.CrossFade ("meleewalk");
				}
			}
			else if (currentWeaponId < WeaponType.other_area)
			{
				if (currentWeaponId < WeaponType.gun_single)
					animation.CrossFade ("gunwalk");
				else if (currentWeaponId < WeaponType.gun_double)
					animation.CrossFade("doublegunidlewalk");

			}
//			else if (currentWeaponId == WeaponType.dianju)
//			{
//				animation.CrossFade ("gunwalk");
//			}
		}
		else
		{
			ChangeState(AnimState.attack);
		}

	}

	protected IEnumerator staticAttack_Enter()
	{
//		Debug.Log("############  staticAttack_Enter()");
//		animation["attack"].weight = 1;
//		animation["attack"].enabled = true;
//		yield return null;
		if (followSmoke != null)
			followSmoke.SetActive(false);
		if (currentWeaponId < WeaponType.gun_area) {
//			if (currentWeaponId == WeaponType.chuizi)
//			{
//				animation.CrossFade("hammerattack01");
//			}
//			else 
			if (currentWeaponId == WeaponType.dianju) {
				animation.CrossFade("gunattack", animChangeDuration, PlayMode.StopAll);
			}
			else
			{
				animation.CrossFade("meleeattack01", animChangeDuration, PlayMode.StopAll);
			}
		} else if (currentWeaponId < WeaponType.other_area) {
			if (currentWeaponId < WeaponType.gun_single)
			{
				if (currentWeaponId == WeaponType.gun_shouqiang)
					animation.CrossFade("gunattack", animChangeDuration, PlayMode.StopAll);
				else if (currentWeaponId == WeaponType.gun_m4)
					animation.CrossFade("gunattack_m4", animChangeDuration, PlayMode.StopAll);
				else if (currentWeaponId == WeaponType.gun_sandan)
					animation.CrossFade("gunattack_sandan", animChangeDuration, PlayMode.StopAll);
				else if (currentWeaponId == WeaponType.gun_fire)
					animation.CrossFade("gunattack_fire", animChangeDuration, PlayMode.StopAll);
				else if (currentWeaponId == WeaponType.gun_jiatelin)
					animation.CrossFade("gunattack_jiatelin", animChangeDuration, PlayMode.StopAll);
				else
					animation.CrossFade("gunattack_liudan", animChangeDuration, PlayMode.StopAll);
			}
			else
			{
				if (currentWeaponId == WeaponType.gundouble_shouqiang)
					animation.CrossFade("doublegunattack", animChangeDuration, PlayMode.StopAll);
				else if (currentWeaponId == WeaponType.gundouble_m4)
					animation.CrossFade("doublegunattack_m4", animChangeDuration, PlayMode.StopAll);
				else if (currentWeaponId == WeaponType.gundouble_sandan)
					animation.CrossFade("doublegunattack_sandan", animChangeDuration, PlayMode.StopAll);
				else if (currentWeaponId == WeaponType.gundouble_fire)
					animation.CrossFade("doublegunattack_fire", animChangeDuration, PlayMode.StopAll);
				else
					animation.CrossFade("doublegunattack_liudan", animChangeDuration, PlayMode.StopAll);
			}

		} 
		yield return null;
	}

	protected void staticAttack_Exit ()
	{
//		animation["attack"].enabled = false;
//		animation["attackgun"].enabled = false;
	}

	IEnumerator daojuAttack_Enter()
	{
		rigidbody.velocity = Vector3.zero;
		rigidbody.angularVelocity = Vector3.zero;
		collider.enabled = false;
		rigidbody.useGravity = false;
		animation.CrossFade("skill", animChangeDuration, PlayMode.StopAll);
		yield return new WaitForSeconds(2f);
		collider.enabled = true;
		rigidbody.useGravity = true;
		ChangeState(AnimState.idle);
	}

	void daojuAttack_Exit()
	{
		collider.enabled = true;
		rigidbody.useGravity = true;
	}
	
	protected IEnumerator attack_Enter ()
	{
//		Debug.Log("############  attack_Enter()");
//		animation["attack"].weight = 1;
//		animation["attack"].enabled = true;
//		yield return null;
		if (followSmoke != null)
			followSmoke.SetActive(true);
		if (currentWeaponId < WeaponType.gun_area) {
//			if (currentWeaponId == WeaponType.chuizi)
//				animation.CrossFade("hammerattackwalk");
//			else 
			if (currentWeaponId == WeaponType.dianju) {
				
				animation.CrossFade("gunattackwalk", animChangeDuration, PlayMode.StopAll);
			}
			else
				animation.CrossFade("meleeattackwalk", animChangeDuration, PlayMode.StopAll);
		} else if (currentWeaponId < WeaponType.other_area) {
			if (currentWeaponId < WeaponType.gun_single)
			{
				if (currentWeaponId == WeaponType.gun_shouqiang)
					animation.CrossFade("gunattackwalk", animChangeDuration, PlayMode.StopAll);
				else if (currentWeaponId == WeaponType.gun_m4)
					animation.CrossFade("gunattackwalk_m4", animChangeDuration, PlayMode.StopAll);
				else if (currentWeaponId == WeaponType.gun_sandan)
					animation.CrossFade("gunattackwalk_sandan", animChangeDuration, PlayMode.StopAll);
				else if (currentWeaponId == WeaponType.gun_fire)
					animation.CrossFade("gunattackwalk_fire", animChangeDuration, PlayMode.StopAll);
				else if (currentWeaponId == WeaponType.gun_jiatelin)
					animation.CrossFade("gunattackwalk_jiatelin", animChangeDuration, PlayMode.StopAll);
				else
					animation.CrossFade("gunattackwalk_liudan", animChangeDuration, PlayMode.StopAll);
			}
			else if (currentWeaponId < WeaponType.gun_double)
			{
				if (currentWeaponId == WeaponType.gundouble_m4)
					animation.CrossFade("doublegunattackwalk_m4", animChangeDuration, PlayMode.StopAll);	
				else if (currentWeaponId == WeaponType.gundouble_liudan)
					animation.CrossFade("doublegunattackwalk_liudan", animChangeDuration, PlayMode.StopAll);
				else if (currentWeaponId == WeaponType.gundouble_fire)
					animation.CrossFade("doublegunattackwalk_fire", animChangeDuration, PlayMode.StopAll);
				else if (currentWeaponId == WeaponType.gundouble_sandan)
					animation.CrossFade("doublegunattackwalk_sandan", animChangeDuration, PlayMode.StopAll);
				else
					animation.CrossFade("doublegunattackwalk", animChangeDuration, PlayMode.StopAll);
			}
		} 
//		else if (currentWeaponId == WeaponType.dianju) {
//
//			animation.CrossFade("gunattackwalk");
//		}
		yield return null;
	}
	
	protected void attack_Exit ()
	{
//		animation["attack"].enabled = false;
//		animation["attackgun"].enabled = false;
	}
	
	protected IEnumerator take_Enter ()
	{
		if (followSmoke != null)
			followSmoke.SetActive(false);
		if (currentWeaponId < WeaponType.gun_area) {
			if (currentWeaponId == WeaponType.dianju) {
				animation.CrossFade("guntake", animChangeDuration, PlayMode.StopAll);
			}
			else
				animation.CrossFade("meleetake", animChangeDuration, PlayMode.StopAll);
		} else if (currentWeaponId < WeaponType.other_area) {
			if (currentWeaponId < WeaponType.gun_single)
				animation.CrossFade("guntake", animChangeDuration, PlayMode.StopAll);
			else
				animation.CrossFade("doubleguntake", animChangeDuration, PlayMode.StopAll);
		} 
		else if (currentWeaponId == WeaponType.dianju) {
			animation.CrossFade("guntake", animChangeDuration, PlayMode.StopAll);
		}
		yield return null;
	}
	
	protected void take_Exit ()
	{
	}
	
	protected IEnumerator dead_Enter ()
	{
		animation.CrossFade ("dead", animChangeDuration, PlayMode.StopAll);
		rigidbody.velocity = Vector3.zero;
		rigidbody.angularVelocity = Vector3.zero;
		collider.enabled = false;
		rigidbody.useGravity = false;
		yield return new WaitForSeconds (2f);
//		Destroy(gameObject);
		if (!automatic)
		{

			if (GameData.Instance.DeadTime < 3)
			{
				GameData.Instance.DeadTime += 1;
				FuhuoDialog.PopUp(GameData.Instance.DeadTime);
			}
			else
			{
				Camera.main.GetComponent<GrayscaleEffect>().enabled = true;
				ResultDialog.Popup(false);
			}
		}

//		Application.LoadLevel("ui");
	}
	
	protected void dead_Exit ()
	{
		
	}
}
