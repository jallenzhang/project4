//#define US_VERSION
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChuangGuanLogic : MonoBehaviour {
	public Camera cameraUI, camera3D;
	public List<LevelSelect> levels = new List<LevelSelect>();
	public List<UILabel> levelLabels = new List<UILabel>();
	public Transform targetTrans;
	public GameObject dian;
	public GameObject labelLevel;
	public float speed = 1f;
	private float lastX = 0;
	public int maxLevels = 200;
	public Transform labelRootPos;
	public GameObject desertLockObj;
	public GameObject map01;
	public GameObject map02;
	public GameObject background01;
	public GameObject background02;
	public UISprite btnNormalScene;
	public UISprite btnDesertScene;

	private string normal_nomal = "map01_b";
	private string normal_select = "map01_a";
	private string desert_normal = "map02_b";
	private string desert_select = "map02_a";

	private float distance = 47.3879f;

	private float offsetRot = 360f / 50f;

	private static ChuangGuanLogic instance = null;
	public static ChuangGuanLogic Instance
	{
		get
		{
			return instance;
		}
	}

	void Start ()
	{
		instance = this;

		for(int i = 0; i < maxLevels; i++)
		{
			GameObject obj = (GameObject)Instantiate(dian);
			obj.transform.parent = targetTrans;
			obj.transform.localScale = Vector3.one * 5f;
			obj.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 90f + 360f/(50) * i));
			obj.transform.position = targetTrans.position + targetTrans.rotation * Quaternion.Euler(0, 0, 180f + 360f/(50) * i) * new Vector3(distance, 0, 0) + new Vector3(0, 0, -4);
			levels.Add(obj.GetComponentInChildren<LevelSelect>());
			if (i >= SettingManager.Instance.CurrentLevel + 4 || i <= SettingManager.Instance.CurrentLevel - 4)
				obj.SetActive(false);
		}

		for (int i = 0;  i < maxLevels; i++)
		{
			GameObject obj = (GameObject)Instantiate(labelLevel);
			obj.transform.parent = labelRootPos;
			obj.transform.localScale = Vector3.one;
			levelLabels.Add(obj.GetComponent<UILabel>());
			if (i >= SettingManager.Instance.CurrentLevel + 4 || i <= SettingManager.Instance.CurrentLevel - 4)
				obj.SetActive(false);
		}

		for (int i = 0; i < maxLevels; i++)
		{
			levels[i].level = i + 1;
			levelLabels[i].text = (i + 1).ToString();
		}

		targetTrans.localRotation = targetTrans.localRotation * Quaternion.Euler(new Vector3(0, 0, -offsetRot * SettingManager.Instance.CurrentLevel));

		UpdateLeftArea();

	}

	private void UpdateLeftArea()
	{
		if (SettingManager.Instance.SceneLocked == 1)
		{
			desertLockObj.SetActive(true);
		}
		else
		{
			desertLockObj.SetActive(false);
		}

		if (SettingManager.Instance.SceneSelection == 1)
		{
			map01.SetActive(true);
			background01.SetActive(true);
			map02.SetActive(false);
			background02.SetActive(false);
			btnNormalScene.spriteName = normal_select;//btnNormalScene.GetComponent<UIButton>().pressedSprite;
			btnNormalScene.GetComponent<UIButton>().pressedSprite = normal_select;
			btnNormalScene.GetComponent<UIButton>().normalSprite = normal_select;
			btnDesertScene.spriteName = desert_normal;
			btnDesertScene.GetComponent<UIButton>().pressedSprite = desert_normal;
			btnDesertScene.GetComponent<UIButton>().normalSprite = desert_normal;
		}
		else if (SettingManager.Instance.SceneSelection == 2)
		{
			map01.SetActive(false);
			background01.SetActive(false);
			map02.SetActive(true);
			background02.SetActive(true);
			btnDesertScene.spriteName = desert_select;//btnDesertScene.GetComponent<UIButton>().pressedSprite;
			btnDesertScene.GetComponent<UIButton>().pressedSprite = desert_select;
			btnDesertScene.GetComponent<UIButton>().normalSprite = desert_select;

			btnNormalScene.GetComponent<UIButton>().pressedSprite = normal_nomal;
			btnNormalScene.GetComponent<UIButton>().normalSprite = normal_nomal;
			btnNormalScene.spriteName = normal_nomal;
		}
	}

	public void onSceneChange(GameObject obj)
	{
		if (obj.name == "normal")
		{
			SettingManager.Instance.SceneSelection = 1;
//			map01.SetActive(true);
//			map02.SetActive(false);
			UpdateLeftArea();
		}
		else if (obj.name == "desert")
		{
			SettingManager.Instance.SceneSelection = 2;
//			map01.SetActive(false);
//			map02.SetActive(true);
			UpdateLeftArea();
		}
		obj.GetComponent<UISprite>().spriteName = obj.GetComponent<UIButton>().pressedSprite;
	}

	public void UnlockDesertScene()
	{
		UnlockSceneDialog.Popup(500, UpdateLeftArea);
	}

	public void onMoreClicked()
	{
#if US_VERSION
		DialogManager.Instance.PopupFadeOutMessage("Building, Open soon!");
#else
		DialogManager.Instance.PopupFadeOutMessage("场景建设中，敬请期待");
#endif
	}

	public void SetLevelActive(int level)
	{
		if (level >= 1 && level < 200)
		{
			levels[level -1].transform.parent.gameObject.SetActive(true);
			levelLabels[level -1].gameObject.SetActive(true);
		}
	}

	public void SetLevelInactive(int level)
	{
		if (level >= 1 && level < 200)
		{
			Debug.Log("SetLevelInactive " + level);
			levels[level -1].transform.parent.gameObject.SetActive(false);
			levelLabels[level -1].gameObject.SetActive(false);
		}
	}

	void Update()
	{
		for (int i = 0; i < maxLevels; i++)
		{
			levelLabels[i].transform.position =
				cameraUI.ScreenToWorldPoint(
				camera3D.WorldToScreenPoint(levels[i].transform.position));
		}
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
	
	void OnMouseUp()
	{
		//		lastX = 0;
	}

	public void LoadUIScene()
	{
		Ultilities.CleanMemory();
#if US_VERSION
		Application.LoadLevel("ui_us");
#else
		Application.LoadLevel("ui");
#endif
	}

}
