using UnityEngine;
using System.Collections;

public class UIRootImprovement : MonoBehaviour
{
	public int designWidth = 1536;
	public int designHeight = 2048;

	private float designRatio;

	private UIRoot uiroot;

	// Use this for initialization
	void Start()
	{
		uiroot = gameObject.GetComponent<UIRoot>();
		uiroot.scalingStyle = UIRoot.Scaling.Flexible;
		designRatio = (float)designHeight / designWidth;
		Update();
	}

	// Update is called once per frame
	void Update()
	{
		float screenRatio = (float)ScreenHelper.Instance.Height / ScreenHelper.Instance.Width;
		if (screenRatio > designRatio) // fill width
		{
			uiroot.manualHeight = (int)(designWidth * screenRatio);
		}
		else // fill height
		{
			uiroot.manualHeight = designHeight;
		}
	}
}

