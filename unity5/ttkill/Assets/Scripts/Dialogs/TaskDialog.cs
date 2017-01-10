//#define US_VERSION
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 *任务的type是这个意思：
 *1，赏金任务，在任务面板上长期存在有两个赏金任务，
 *没做完一个都会从人物库里面再随机出来一个（就是type为1的那8个任务），随机的
 *时候只要保证，存在面板上的两个任务不一样，且刷新之后跟原来的老任务不一样即可；
 *2，日常任务。 每天刷新一次，当天做完就没了，第二天再刷新。 日常任务每天刷新7个（即表格上的所有任务）
 *3，成长任务，这些所有的成长任务都显示在面板上，每个成长任务都有多个小任务，成迭代状，
 *即完成一个出现下一个。如果该任务所有的小任务都完成，则这个任务在面板里消失
 */
public class TaskDialog : DialogBase
{
	public static void Popup()
	{
#if US_VERSION
		DialogManager.Instance.PopupDialog("prefabs/Dialogs/renwu_us");
#else
		DialogManager.Instance.PopupDialog("prefabs/Dialogs/renwu");
#endif
	}

	public GameObject dailyItemPrefab, normalItemPrefab, growItemPrefab;

	public UIGrid grid;

	TaskNormalItem normalTask1, normalTask2;
	int normalTaskInfo1, normalTaskInfo2;

	List<TaskInfo> goldTask, dailyTask, growTask;

	int goldTaskIndex;

	// Use this for initialization
	void Start ()
	{
		var data = IOHelper.GetTaskInfos();
		goldTask = new List<TaskInfo>();
		dailyTask = new List<TaskInfo>();
		growTask = new List<TaskInfo>();
		foreach (var e in data)
		{
			if (e.type == 1) goldTask.Add(e);
			else if (e.type == 2) dailyTask.Add(e);
			else if (e.type == 3) growTask.Add(e);
		}

		foreach (var e in dailyTask)
		{
			var item = NGUITools.AddChild(grid.gameObject, dailyItemPrefab).GetComponent<TaskDailyItem>();
			item.description.text = e.Description;
			item.Init(e);
		}

		normalTask1 = NGUITools.AddChild(grid.gameObject, normalItemPrefab).GetComponent<TaskNormalItem>();
		normalTask2 = NGUITools.AddChild(grid.gameObject, normalItemPrefab).GetComponent<TaskNormalItem>();
		normalTaskInfo1 = SettingManager.Instance.NormalItem1;
		normalTaskInfo2 = SettingManager.Instance.NormalItem2;
		normalTask1.description.text = goldTask[normalTaskInfo1].Description;
		normalTask1.Init(goldTask[normalTaskInfo1]);
		normalTask2.description.text = goldTask[normalTaskInfo2].Description;
		normalTask2.Init(goldTask[normalTaskInfo2]);

		foreach (var e in growTask)
		{
			var item = NGUITools.AddChild(grid.gameObject, growItemPrefab).GetComponent<TaskGrowItem>();
			item.Init(e);
			item.description.text = e.Description;
		}

		grid.Reposition();


	}

	// Update is called once per frame
	void Update () {
	
	}

	public void OnNormalRefreshClick(TaskNormalItem item)
	{
		if (item == normalTask1)
		{
			do
			{
				normalTaskInfo1 = (normalTaskInfo1 + 1) % goldTask.Count;
			} while (normalTaskInfo1 == normalTaskInfo2);
			SettingManager.Instance.NormalItem1 = normalTaskInfo1;
			normalTask1.Reset(goldTask[normalTaskInfo1]);
			normalTask1.Init(goldTask[normalTaskInfo1]);
			normalTask1.description.text = goldTask[normalTaskInfo1].Description;
		}
		else if (item == normalTask2)
		{
			do
			{
				normalTaskInfo2 = (normalTaskInfo2 + 1) % goldTask.Count;
			} while (normalTaskInfo2 == normalTaskInfo1);
			SettingManager.Instance.NormalItem2 = normalTaskInfo2;
			normalTask2.Reset(goldTask[normalTaskInfo2]);
			normalTask2.Init(goldTask[normalTaskInfo2]);
			normalTask2.description.text = goldTask[normalTaskInfo2].Description;
		}
	}

	public void CloseDialog()
	{
		UILayout.Instance.BottomIn();
		DialogManager.Instance.CloseDialog();
		MainArea.Instance.Show();
	}
	
	IEnumerator ShowMain()
	{
		yield return new WaitForSeconds(0.3f);
		MainArea.Instance.Show();
	}

}
