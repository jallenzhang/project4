using UnityEngine;
using UnityEditor;
using System.Collections;


public class AnimationEventBind : ScriptableWizard {
	
	public AnimationClip clip;
	public string[] functionname;
	public float[] time;
	private float length;
	
	
	[MenuItem("Tools/AnimationEventsBind")]
	static void CreateWindow()
	{
		ScriptableWizard.DisplayWizard<AnimationEventBind>("AnimationEventBindWizard","Bind","Cancel");
	}
	
	
	void OnWizardUpdate()
	{
		helpString = "请拖入动画剪辑并写好动画绑定事件的函数名";
		if(functionname == null || time == null || clip == null)
		{
			errorString = "请拖入剪辑写好绑定函数名";
			isValid = false;
		}
		else
		{
			if(functionname.Length == 0 || time.Length == 0 || functionname.Length != time.Length)
			{
				return;
			}
			length = clip.length;
			errorString = "可以执行绑定,该动画剪辑的长度为：" + length;
			isValid = true;
		}
	}
	
	
	void OnWizardCreate()
	{
		AnimationEvent[] animevent = new AnimationEvent[functionname.Length];
		for(int i = 0;i<functionname.Length;i++)
		{
			animevent[i] = new AnimationEvent();
			animevent[i].functionName = functionname[i];
			animevent[i].time = time[i];
			animevent[i].messageOptions = SendMessageOptions.DontRequireReceiver;
		}
		AnimationUtility.SetAnimationEvents(clip,animevent);
	}
	
	
	void OnWizardOtherButton()
	{
		Close();
	}
}