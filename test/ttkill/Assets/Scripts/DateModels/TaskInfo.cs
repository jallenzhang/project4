
using UnityEngine;

public class TaskInfo
{
	public int id { get; set; }

	public int type { get; set; }

	public string desc { get; set; }

	public string value { get; set; }

	public string reward { get; set; }

	public int reward_type { get; set; }

	public string Description
	{
		get
		{
			if (type == 1 || type == 2)
			{
				return desc;
			}
			else if (type == 3)
			{
				string[] values = value.Split(';');
				
				int index = 0;
				switch(id)
				{
				case 13:
					index = SettingManager.Instance.TaskIndex13;
					break;
				case 14:
					index = SettingManager.Instance.TaskIndex14;
					break;
				case 15:
					index = SettingManager.Instance.TaskIndex15;
					break;
				case 16:
					index = SettingManager.Instance.TaskIndex16;
					break;
				case 17:
					index = SettingManager.Instance.TaskIndex17;
					break;
				case 18:
					index = SettingManager.Instance.TaskIndex18;
					break;
				case 19:
					index = SettingManager.Instance.TaskIndex19;
					break;
				case 20:
					index = SettingManager.Instance.TaskIndex20;
					break;
				}
//				string v = value.Substring(0, value.IndexOf(';'));
				index = Mathf.Min(index, values.Length - 1);
				string v = values[index];
				return desc.Replace("%d", v);
			}
			return string.Empty;
		}
	}

}