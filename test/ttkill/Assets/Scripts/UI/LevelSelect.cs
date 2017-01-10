using UnityEngine;
using System.Collections;

public class LevelSelect : MonoBehaviour {
	public int level = 1;

	public GameObject NextLevelEffect;
	public MeshRenderer poly;
	public MeshRenderer baseRender;
	public Material enablePolyMat;
	public Material disablePolyMat;
	public Material enableBaseMat;
	public Material disableBaseMat;

	private float speed = 0.05f;
	private float lastX = 0;
	private Transform targetTrans;
	// Use this for initialization
	void Start () {
		targetTrans = GameObject.Find("mapout").transform;
		if (level == SettingManager.Instance.NextLevel)
			NextLevelEffect.SetActive(true);
		else
			NextLevelEffect.SetActive(false);

		Material[] enabalePolyMats = new Material[1];
		enabalePolyMats[0] = enablePolyMat;
		Material[] enabaleBaseMats = new Material[1];
		enabaleBaseMats[0] = enableBaseMat;

		Material[] disablePolyMats = new Material[1];
		disablePolyMats[0] = disablePolyMat;
		Material[] disableBaseMats = new Material[1];
		disableBaseMats[0] = disableBaseMat;

		if (level <= SettingManager.Instance.NextLevel)
		{
			Debug.Log("this dian level is: " + level);
//			poly.materials[0] = enablePolyMat;
//			baseRender.materials[0] = enableBaseMat;
			collider.enabled = true;
			poly.materials = enabalePolyMats;
			baseRender.materials = enabaleBaseMats;
		}
		else
		{
			poly.materials = disablePolyMats;
			baseRender.materials = disableBaseMats;
			collider.enabled = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnBecameInvisible()
	{
		Debug.Log("current level " + level);
		if (transform.parent.position.x < 0f)
		{
			ChuangGuanLogic.Instance.SetLevelInactive(level - 3);
		}
		else
		{
			ChuangGuanLogic.Instance.SetLevelInactive(level + 3);
		}
	}

	void OnBecameVisible()
	{
		ChuangGuanLogic.Instance.SetLevelActive(level - 3);
		ChuangGuanLogic.Instance.SetLevelActive(level + 3);
	}

	void OnMouseUpAsButton()
	{
		GameData.Instance.GotoLevel(level);
		WeaponSelectionDialog.Popup("changjing");

	}

	void OnMouseDrag()
	{
		float currentX = Input.mousePosition.x;
		float deltaX = currentX - lastX;
		lastX = currentX;
		targetTrans.rotation = targetTrans.localRotation * Quaternion.Euler(new Vector3(0, 0, deltaX * speed));
	}
	
	void OnMouseDown()
	{
		lastX = Input.mousePosition.x;
	}
}
