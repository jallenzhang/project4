using UnityEngine;
using System.Collections;
using System;

public class SettingManager  {

	private static SettingManager instance;
	public static SettingManager Instance
	{
		get
		{
			if (instance == null)
				instance = new SettingManager();

			return instance;
		}
	}

	private SettingManager()
	{
	}

	private string GOLD = "GOLD";
	private string DIAMOND = "DIAMOND";
	private string TILI = "TILI";
	private string CURRENTLEVEL = "CURRENTLEVEL";
	private string GRAPHICS = "GRAPHICS"; //1:low  2:middle 3:high
	private string MUSIC = "MUSIC"; //1:OPEN  0:CLOSE
	private string SOUND = "SOUND"; //1:OPEN  0:CLOSE
	private string NEXTLEVEL = "NEXTLEVEL";
	private string JIATELIN = "JIATELIN";
	private string JIAXUE = "JIAXUE";
	private string BINDONG = "BINDONG";
	private string AOE = "AOE";
	private string TILIRECOVERTIME = "TILIRECOVERTIME";

	private string DAOJUCOST_SHUAXIN = "DAOJUCOST_SHUAXIN";
	private string FUGOT_SHUAXIN = "FUGOT_SHUAXIN";
	private string KILLBOSS_SHUAXIN = "KILLBOSS_SHUAXIN";
	private string GOLDGOT_SHUAXIN = "GOLDGOT_SHUAXIN";
	private string JIQIANGGOT_SHUAXIN = "JIQIANGGOT_SHUAXIN";
	private string COSTGOLDNUM_SHUAXIN = "COSTGOLDNUM_SHUAXIN";
	private string NORMALITEM1 = "NORMALITEM1";
	private string NORMALITEM2 = "NORMALITEM2";

	private string WEAPONUPGRADE = "WEAPONUPGRADE";
	private string ADVANTANGEMODETIME = "ADVANTANGEMODETIME";
	private string CHALLEGEMODETIME = "CHALLEGEMODETIME";
	private string USEDAOJUTIME = "USEDAOJUTIME";
	private string KILLBOSSTIME = "KILLBOSSTIME";
	private string USETILINUM = "USETILINUM";

	private string ACHIEVEDTASKS = "ACHIEVEDTASKS";

	private string GUNNUM  = "GUNNUM";
	private string MAXWEAPONLEVEL = "MAXWEAPONLEVEL";
	private string MAXAVATARLEVEL = "MAXAVATARLEVEL";
	private string PETNUM = "PETNUM";
	private string MAXPETLEVEL = "MAXPETLEVEL";
	private string TOTALGOLDGOT = "TOTALGOLDGOT";
	private string TOTALBOSSKILL = "TOTALBOSSKILL";
	private string TASKINDEX13 = "INDEX13";
	private string TASKINDEX14 = "INDEX14";
	private string TASKINDEX15 = "INDEX15";
	private string TASKINDEX16 = "INDEX16";
	private string TASKINDEX17 = "INDEX17";
	private string TASKINDEX18 = "INDEX18";
	private string TASKINDEX19 = "INDEX19";
	private string TASKINDEX20 = "INDEX20";

	private string LASTDAYPLAY = "LASTDAYPLAY";
	private string PETLOCKED = "PETLOCKED";
	private string SCENELOCKED = "SCENELOCKED";
	private string SCENESELECTION = "SCENESELECTION";

	private string GOLDADDTIONAL = "GOLDADDTIONAL";
	private string WEAPONADDTIONAL = "WEAPONADDTIONAL";

	private string DIAMONDJIACHENG = "DIAMONDJIACHENG";

	private string LASTESTDAYOFWEEK = "LASTESTDAYOFWEEK";
	private string DAYSSIGNINWEEK = "DAYSSIGNINWEEK";

	private string HIGESTSCORE = "HIGESTSCORE";

	private string CURRENTAVATARID = "CURRENTAVATARID"; //1: nan01; 2: nv01

	//tutorial area
	private string TUTORIALSEQ = "TUTORIALSEQ";
	private string FIRSTIN = "FIRSTIN"; // 1: is the first in;  0: is not
	private string TUTORIALADDBLOOD = "TUTORIALADDBLOOD"; //1: means can trigger, other means has been triggered
	private string TUTORIALRETRY = "TUTORIALRETRY";  //1: means can trigger, other means has been triggered
	private string TUTORIALHOME = "TUTORIALHOME";

	public int TutorialRetry
	{
		get
		{
			return PlayerPrefs.HasKey(TUTORIALRETRY)?PlayerPrefs.GetInt(TUTORIALRETRY):1;
		}
		set
		{
			PlayerPrefs.SetInt(TUTORIALRETRY, value);
			PlayerPrefs.Save();
		}
	}

	public int TutorialGoHome
	{
		get
		{
			return PlayerPrefs.HasKey(TUTORIALHOME)?PlayerPrefs.GetInt(TUTORIALHOME):1;
		}
		set
		{
			PlayerPrefs.SetInt(TUTORIALHOME, value);
			PlayerPrefs.Save();
		}
	}

	public int TutorialAddBlood
	{
		get
		{
			return PlayerPrefs.HasKey(TUTORIALADDBLOOD)?PlayerPrefs.GetInt(TUTORIALADDBLOOD):1;
		}
		set
		{
			PlayerPrefs.SetInt(TUTORIALADDBLOOD, value);
			PlayerPrefs.Save();
		}
	}

	public int FirstIn
	{
		get
		{
			return PlayerPrefs.HasKey(FIRSTIN)?PlayerPrefs.GetInt(FIRSTIN):1;
		}
		set
		{
			PlayerPrefs.SetInt(FIRSTIN, value);
			PlayerPrefs.Save();
		}
	}

	public int TutorialSeq
	{
		get
		{
			return PlayerPrefs.HasKey(TUTORIALSEQ)?PlayerPrefs.GetInt(TUTORIALSEQ):1;
		}
		set
		{
			PlayerPrefs.SetInt(TUTORIALSEQ, value);
			PlayerPrefs.Save();
		}
	}

	public int CurrentAvatarId
	{
		get
		{
			return PlayerPrefs.HasKey(CURRENTAVATARID)?PlayerPrefs.GetInt(CURRENTAVATARID):1;
		}
		set
		{
			PlayerPrefs.SetInt(CURRENTAVATARID, value);
			PlayerPrefs.Save();
		}
	}

	public int HighestScore
	{
		get
		{
			return PlayerPrefs.HasKey(HIGESTSCORE)?PlayerPrefs.GetInt(HIGESTSCORE):0;
		}
		set
		{
			PlayerPrefs.SetInt(HIGESTSCORE, value);
			PlayerPrefs.Save();
		}
	}

	/// <summary>
	/// use enum DAYOFSIGN
	/// </summary>
	/// <value>The days sign in week.</value>
	public int DaysSignInWeek
	{
		get
		{
			return PlayerPrefs.HasKey(DAYSSIGNINWEEK)?PlayerPrefs.GetInt(DAYSSIGNINWEEK):0;
		}
		set
		{
			PlayerPrefs.SetInt(DAYSSIGNINWEEK, value);
			PlayerPrefs.Save();
		}
	}

	/// <summary>
	/// 0:sundy 1:monday 2:tuesday .....
	/// </summary>
	/// <value>The lastest day of week.</value>
	public int LastestDayOfWeek
	{
		get
		{
			return PlayerPrefs.HasKey(LASTESTDAYOFWEEK)?PlayerPrefs.GetInt(LASTESTDAYOFWEEK):-1;
		}
		set
		{
			PlayerPrefs.SetInt(LASTESTDAYOFWEEK, value);
			PlayerPrefs.Save();
		}
	}

	/// <summary>
	/// 0:之前没买过
	/// 1:买过一次
	/// >=2:买过一次用过了
	/// </summary>
	/// <value>The diamond jiacheng.</value>
	public int DiamondJiacheng
	{
		get
		{
			return PlayerPrefs.HasKey(DIAMONDJIACHENG)?PlayerPrefs.GetInt(DIAMONDJIACHENG):0;
		}
		set
		{
			PlayerPrefs.SetInt(DIAMONDJIACHENG, value);
			PlayerPrefs.Save();
		}
	}

	public int GoldAddtional
	{
		get
		{
			return PlayerPrefs.HasKey(GOLDADDTIONAL)?PlayerPrefs.GetInt(GOLDADDTIONAL):0;
		}
		set
		{
			PlayerPrefs.SetInt(GOLDADDTIONAL, value);
			PlayerPrefs.Save();
		}
	}

	public int WeaponAddtional
	{
		get
		{
			return PlayerPrefs.HasKey(WEAPONADDTIONAL)?PlayerPrefs.GetInt(WEAPONADDTIONAL):0;
		}
		set
		{
			PlayerPrefs.SetInt(WEAPONADDTIONAL, value);
			PlayerPrefs.Save();
		}
	}

	public int SceneSelection
	{
		get
		{
			return PlayerPrefs.HasKey(SCENESELECTION)?PlayerPrefs.GetInt(SCENESELECTION):1;
		}
		set
		{
			PlayerPrefs.SetInt(SCENESELECTION, value);
			PlayerPrefs.Save();
		}
	}

	public int SceneLocked
	{
		get
		{
			return PlayerPrefs.HasKey(SCENELOCKED)?PlayerPrefs.GetInt(SCENELOCKED):1;
		}
		set
		{
			PlayerPrefs.SetInt(SCENELOCKED, value);
			PlayerPrefs.Save();
		}
	}

	public int PetLocked
	{
		get
		{
			return PlayerPrefs.HasKey(PETLOCKED)?PlayerPrefs.GetInt(PETLOCKED):1;
		}
		set
		{
			PlayerPrefs.SetInt(PETLOCKED, value);
			PlayerPrefs.Save();
		}
	}

	public int LastDayPlay
	{
		get
		{
			return PlayerPrefs.HasKey(LASTDAYPLAY)?PlayerPrefs.GetInt(LASTDAYPLAY):System.DateTime.Now.Day;
		}
		set
		{
			PlayerPrefs.SetInt(LASTDAYPLAY, value);
			PlayerPrefs.Save();
		}
	}

	public int TaskIndex13
	{
		get
		{
			return PlayerPrefs.HasKey(TASKINDEX13)?PlayerPrefs.GetInt(TASKINDEX13):0;
		}
		set
		{
			PlayerPrefs.SetInt(TASKINDEX13, value);
			PlayerPrefs.Save();
		}
	}

	public int TaskIndex14
	{
		get
		{
			return PlayerPrefs.HasKey(TASKINDEX14)?PlayerPrefs.GetInt(TASKINDEX14):0;
		}
		set
		{
			PlayerPrefs.SetInt(TASKINDEX14, value);
			PlayerPrefs.Save();
		}
	}

	public int TaskIndex15
	{
		get
		{
			return PlayerPrefs.HasKey(TASKINDEX15)?PlayerPrefs.GetInt(TASKINDEX15):0;
		}
		set
		{
			PlayerPrefs.SetInt(TASKINDEX15, value);
			PlayerPrefs.Save();
		}
	}

	public int TaskIndex16
	{
		get
		{
			return PlayerPrefs.HasKey(TASKINDEX16)?PlayerPrefs.GetInt(TASKINDEX16):0;
		}
		set
		{
			PlayerPrefs.SetInt(TASKINDEX16, value);
			PlayerPrefs.Save();
		}
	}

	public int TaskIndex17
	{
		get
		{
			return PlayerPrefs.HasKey(TASKINDEX17)?PlayerPrefs.GetInt(TASKINDEX17):0;
		}
		set
		{
			PlayerPrefs.SetInt(TASKINDEX17, value);
			PlayerPrefs.Save();
		}
	}

	public int TaskIndex18
	{
		get
		{
			return PlayerPrefs.HasKey(TASKINDEX18)?PlayerPrefs.GetInt(TASKINDEX18):0;
		}
		set
		{
			PlayerPrefs.SetInt(TASKINDEX18, value);
			PlayerPrefs.Save();
		}
	}

	public int TaskIndex19
	{
		get
		{
			return PlayerPrefs.HasKey(TASKINDEX19)?PlayerPrefs.GetInt(TASKINDEX19):0;
		}
		set
		{
			PlayerPrefs.SetInt(TASKINDEX19, value);
			PlayerPrefs.Save();
		}
	}

	public int TaskIndex20
	{
		get
		{
			return PlayerPrefs.HasKey(TASKINDEX20)?PlayerPrefs.GetInt(TASKINDEX20):0;
		}
		set
		{
			PlayerPrefs.SetInt(TASKINDEX20, value);
			PlayerPrefs.Save();
		}
	}

	public int NormalItem1
	{
		get
		{
			return PlayerPrefs.HasKey(NORMALITEM1)?PlayerPrefs.GetInt(NORMALITEM1):0;
		}
		set
		{
			PlayerPrefs.SetInt(NORMALITEM1, value);
			PlayerPrefs.Save();
		}
	}

	public int NormalItem2
	{
		get
		{
			return PlayerPrefs.HasKey(NORMALITEM2)?PlayerPrefs.GetInt(NORMALITEM2):1;
		}
		set
		{
			PlayerPrefs.SetInt(NORMALITEM2, value);
			PlayerPrefs.Save();
		}
	}

	public int DaojuCost_Shuaxin
	{
		get
		{
			return PlayerPrefs.HasKey(DAOJUCOST_SHUAXIN)?PlayerPrefs.GetInt(DAOJUCOST_SHUAXIN):0;
		}
		set
		{
			PlayerPrefs.SetInt(DAOJUCOST_SHUAXIN, value);
			PlayerPrefs.Save();
		}
	}

	public int FuGot_Shuaxin
	{
		get
		{
			return PlayerPrefs.HasKey(FUGOT_SHUAXIN)?PlayerPrefs.GetInt(FUGOT_SHUAXIN):0;
		}
		set
		{
			PlayerPrefs.SetInt(FUGOT_SHUAXIN, value);
			PlayerPrefs.Save();
		}
	}

	public int KillBoss_Shuaxin
	{
		get
		{
			return PlayerPrefs.HasKey(KILLBOSS_SHUAXIN)?PlayerPrefs.GetInt(KILLBOSS_SHUAXIN):0;
		}
		set
		{
			PlayerPrefs.SetInt(KILLBOSS_SHUAXIN, value);
			PlayerPrefs.Save();
		}
	}

	public int GoldGot_Shuaxin
	{
		get
		{
			return PlayerPrefs.HasKey(GOLDGOT_SHUAXIN)?PlayerPrefs.GetInt(GOLDGOT_SHUAXIN):0;
		}
		set
		{
			PlayerPrefs.SetInt(GOLDGOT_SHUAXIN, value);
			PlayerPrefs.Save();
		}
	}

	public int JiqiangGot_Shuaxin
	{
		get
		{
			return PlayerPrefs.HasKey(JIQIANGGOT_SHUAXIN)?PlayerPrefs.GetInt(JIQIANGGOT_SHUAXIN):0;
		}
		set
		{
			PlayerPrefs.SetInt(JIQIANGGOT_SHUAXIN, value);
			PlayerPrefs.Save();
		}
	}

	public int TotalGoldGot
	{
		get
		{
			return PlayerPrefs.HasKey(TOTALGOLDGOT)?PlayerPrefs.GetInt(TOTALGOLDGOT):0;
		}
		set
		{
			PlayerPrefs.SetInt(TOTALGOLDGOT, value);
			PlayerPrefs.Save();
		}
	}

	public int TotalBossKill
	{
		get
		{
			return PlayerPrefs.HasKey(TOTALBOSSKILL)?PlayerPrefs.GetInt(TOTALBOSSKILL):0;
		}
		set
		{
			PlayerPrefs.SetInt(TOTALBOSSKILL, value);
			PlayerPrefs.Save();
		}
	}

	public int MaxPetLevel
	{
		get
		{
			return PlayerPrefs.HasKey(MAXPETLEVEL)?PlayerPrefs.GetInt(MAXPETLEVEL):1;
		}
		set
		{
			PlayerPrefs.SetInt(MAXPETLEVEL, value);
			PlayerPrefs.Save();
		}
	}

	public int PetNum
	{
		get
		{
			return PlayerPrefs.HasKey(PETNUM)?PlayerPrefs.GetInt(PETNUM):0;
		}
		set
		{
			PlayerPrefs.SetInt(PETNUM, value);
			PlayerPrefs.Save();
		}
	}

	public int MaxAvatarLevel
	{
		get
		{
			return PlayerPrefs.HasKey(MAXAVATARLEVEL)?PlayerPrefs.GetInt(MAXAVATARLEVEL):1;
		}
		set
		{
			PlayerPrefs.SetInt(MAXAVATARLEVEL, value);
			PlayerPrefs.Save();
		}
	}

	public int MaxWeaponLevel
	{
		get
		{
			return PlayerPrefs.HasKey(MAXWEAPONLEVEL)?PlayerPrefs.GetInt(MAXWEAPONLEVEL):1;
		}
		set
		{
			PlayerPrefs.SetInt(MAXWEAPONLEVEL, value);
			PlayerPrefs.Save();
		}
	}

	public int GunNum
	{
		get
		{
			return PlayerPrefs.HasKey(GUNNUM)?PlayerPrefs.GetInt(GUNNUM):4;
		}
		set
		{
			PlayerPrefs.SetInt(GUNNUM, value);
			PlayerPrefs.Save();
		}
	}

	public string AchievedTasks
	{
		get
		{
			return PlayerPrefs.HasKey(ACHIEVEDTASKS)?PlayerPrefs.GetString(ACHIEVEDTASKS):"0";
		}
		set
		{
			PlayerPrefs.SetString(ACHIEVEDTASKS, value);
			PlayerPrefs.Save();
		}
	}

	public int CostGoldNum_Shuaxin
	{
		get
		{
			return PlayerPrefs.HasKey(COSTGOLDNUM_SHUAXIN)?PlayerPrefs.GetInt(COSTGOLDNUM_SHUAXIN):0;
		}
		set
		{
			PlayerPrefs.SetInt(COSTGOLDNUM_SHUAXIN, value);
			PlayerPrefs.Save();
		}
	}

	public int UseTiliNum
	{
		get
		{
			return PlayerPrefs.HasKey(USETILINUM)?PlayerPrefs.GetInt(USETILINUM):0;
		}
		set
		{
			PlayerPrefs.SetInt(USETILINUM, value);
			PlayerPrefs.Save();
		}
	}

	public int KillBossTime
	{
		get
		{
			return PlayerPrefs.HasKey(KILLBOSSTIME)?PlayerPrefs.GetInt(KILLBOSSTIME):0;
		}
		set
		{
			PlayerPrefs.SetInt(KILLBOSSTIME, value);
			PlayerPrefs.Save();
		}
	}

	public int UseDaojuTime
	{
		get
		{
			return PlayerPrefs.HasKey(USEDAOJUTIME)?PlayerPrefs.GetInt(USEDAOJUTIME):0;
		}
		set
		{
			PlayerPrefs.SetInt(USEDAOJUTIME, value);
			PlayerPrefs.Save();
		}
	}

	public int ChallegeModeTime
	{
		get
		{
			return PlayerPrefs.HasKey(CHALLEGEMODETIME)?PlayerPrefs.GetInt(CHALLEGEMODETIME):0;
		}
		set
		{
			PlayerPrefs.SetInt(CHALLEGEMODETIME, value);
			PlayerPrefs.Save();
		}
	}

	public int AdvantageModeTime
	{
		get
		{
			return PlayerPrefs.HasKey(ADVANTANGEMODETIME)?PlayerPrefs.GetInt(ADVANTANGEMODETIME):0;
		}
		set
		{
			PlayerPrefs.SetInt(ADVANTANGEMODETIME, value);
			PlayerPrefs.Save();
		}
	}

	public int WeaponUpgrade
	{
		get
		{
			return PlayerPrefs.HasKey(WEAPONUPGRADE)?PlayerPrefs.GetInt(WEAPONUPGRADE):0;
		}
		set
		{
			PlayerPrefs.SetInt(WEAPONUPGRADE, value);
			PlayerPrefs.Save();
		}
	}

	public string TiliRecoverTime
	{
		get
		{
			return PlayerPrefs.HasKey(TILIRECOVERTIME) ? PlayerPrefs.GetString(TILIRECOVERTIME): System.DateTime.Now.ToString();
		}
		set
		{
			PlayerPrefs.SetString(TILIRECOVERTIME, value);
			PlayerPrefs.Save();
		}
	}

	public int NextLevel
	{
		get
		{
			return PlayerPrefs.HasKey(NEXTLEVEL)?PlayerPrefs.GetInt(NEXTLEVEL):1;
		}
		set
		{
			PlayerPrefs.SetInt(NEXTLEVEL, value);
			PlayerPrefs.Save();
		}
	}

	public int Music
	{
		get
		{
			return PlayerPrefs.HasKey(MUSIC)?PlayerPrefs.GetInt(MUSIC):1;
		}
		set
		{
			PlayerPrefs.SetInt(MUSIC, value);
			PlayerPrefs.Save();
		}
	}

	public int Sound
	{
		get
		{
			return PlayerPrefs.HasKey(SOUND)?PlayerPrefs.GetInt(SOUND):1;
		}
		set
		{
			PlayerPrefs.SetInt(SOUND, value);
			PlayerPrefs.Save();
		}
	}

	public int Graphic
	{
		get
		{
			return PlayerPrefs.HasKey(GRAPHICS)?PlayerPrefs.GetInt(GRAPHICS):2;
		}
		set
		{
			PlayerPrefs.SetInt(GRAPHICS, value);
			PlayerPrefs.Save();
		}
	}

	public int TotalGold 
	{
		get
		{
			return PlayerPrefs.HasKey(GOLD)?PlayerPrefs.GetInt(GOLD):2000;
		}
		set
		{
			PlayerPrefs.SetInt(GOLD, value);
			PlayerPrefs.Save();
		}
	}

	public int TotalDiamond
	{
		get
		{
			return PlayerPrefs.HasKey(DIAMOND)?PlayerPrefs.GetInt(DIAMOND):100;
		}
		set
		{
			PlayerPrefs.SetInt(DIAMOND, value);
			PlayerPrefs.Save();
		}
	}

	public int TotalTili
	{
		get
		{
			return PlayerPrefs.HasKey(TILI)?PlayerPrefs.GetInt(TILI):20;
		}
		set
		{
			PlayerPrefs.SetInt(TILI, value);
			PlayerPrefs.Save();
		}
	}

	public int CurrentLevel
	{
		get
		{
			return PlayerPrefs.HasKey(CURRENTLEVEL)?PlayerPrefs.GetInt(CURRENTLEVEL):1;
		}
		set
		{
			PlayerPrefs.SetInt(CURRENTLEVEL, value);
			PlayerPrefs.Save();
		}
	}

	public int TotalJiatelin
	{
		get
		{
			return PlayerPrefs.HasKey(JIATELIN)?PlayerPrefs.GetInt(JIATELIN):5;
		}
		set
		{
			PlayerPrefs.SetInt(JIATELIN, value);
			PlayerPrefs.Save();
		}
	}

	public int TotalJiaxue
	{
		get
		{
			return PlayerPrefs.HasKey(JIAXUE)?PlayerPrefs.GetInt(JIAXUE):5;
		}
		set
		{
			PlayerPrefs.SetInt(JIAXUE, value);
			PlayerPrefs.Save();
		}
	}

	public int TotalBindong
	{
		get
		{
			return PlayerPrefs.HasKey(BINDONG)?PlayerPrefs.GetInt(BINDONG):5;
		}
		set
		{
			PlayerPrefs.SetInt(BINDONG, value);
			PlayerPrefs.Save();
		}
	}

	public int TotalAoe
	{
		get
		{
			return PlayerPrefs.HasKey(AOE)?PlayerPrefs.GetInt(AOE):5;
		}
		set
		{
			PlayerPrefs.SetInt(AOE, value);
			PlayerPrefs.Save();
		}
	}
}
