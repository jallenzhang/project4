using UnityEngine;
using System.Collections;

public class ObjectItem : MonoBehaviour {
	private GuideCamera mini;
	private Vector3 heroScreenPos;
	private Vector3 selfScreenPos;

	public GameObject flag;
	// Use this for initialization
	void Start () {
		mini =  Camera.main.GetComponent<GuideCamera>();
		heroScreenPos = Camera.main.WorldToScreenPoint(GameObject.FindGameObjectWithTag("Player").transform.position);
	}
	
	// Update is called once per frame
	void Update () {
		float x, y = 0;
		if (Helper.isINRect(transform.position, mini.left_bottom, mini.left_top, mini.right_top, mini.right_bottom))
		{
			flag.SetActive(false);
		}
		else
		{
			selfScreenPos = Camera.main.WorldToScreenPoint(transform.position);

			if (selfScreenPos.x <= heroScreenPos.x)
			{
				//when x = 0, find y
				float tmp_y = selfScreenPos.y - ((selfScreenPos.y - heroScreenPos.y) * selfScreenPos.x / (selfScreenPos.x - heroScreenPos.x));

				if (tmp_y < 0)
				{
					// make y == 0
					x = selfScreenPos.x - ((heroScreenPos.x - selfScreenPos.x) *selfScreenPos.y /(heroScreenPos.y - selfScreenPos.y));
					y = 0;
				}
				else
				{
					if (tmp_y <= Screen.height)
					{
						x = 0;
						y = tmp_y;
					}
					else
					{
						// make y == screen.height
						x = selfScreenPos.x - ((selfScreenPos.x - heroScreenPos.x) *(selfScreenPos.y - Screen.height) /(selfScreenPos.y - heroScreenPos.y));
						y = Screen.height;
					}
				}
			}
			else
			{
				//when x = screenwidth, find y
				float tmp_y = selfScreenPos.y - ((selfScreenPos.y - heroScreenPos.y) * (selfScreenPos.x - Screen.width) / (selfScreenPos.x - heroScreenPos.x));

				if (tmp_y < 0)
				{
					// make y == 0
					x = selfScreenPos.x - ((selfScreenPos.x - heroScreenPos.x) *selfScreenPos.y /(selfScreenPos.y - heroScreenPos.y));
					y = 0;
				}
				else
				{
					if (tmp_y <= Screen.height)
					{
						x = Screen.width;
						y = tmp_y;
					}
					else
					{
						// make y == screen.height
						x = selfScreenPos.x - ((selfScreenPos.x - heroScreenPos.x) *(selfScreenPos.y - Screen.height) / (selfScreenPos.y - heroScreenPos.y));
						y = Screen.height;
					}
				}
			}

			UpdateFlag(x / (float)Screen.width, y / (float)Screen.height);
		}
	}

	void UpdateFlag(float x, float y)
	{
		flag.transform.position = new Vector3(x, y, flag.transform.position.z);
		flag.SetActive(true);
	}
}
