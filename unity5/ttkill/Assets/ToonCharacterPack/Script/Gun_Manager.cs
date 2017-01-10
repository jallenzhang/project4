using UnityEngine;
using System.Collections;

public class Gun_Manager : MonoBehaviour {

	public GameObject Gunner;
	public string[] aniname;
	public GameObject[] GunnerChar;
	public GameObject[] GunnerGun;
	public GameObject[] GunnerGun2;
	public GameObject[] Gun;
	public GameObject[] GunnerSnowboard;
	public GameObject[] Snowboard;
	public GameObject[] GunAni;
	public Mesh[] mGun;
	public Mesh[] mGun2;
	public Material[] maTGun;

	public int iGunner;
	public int iGun;
	public int iSnowboard;
	public int iani;
	
	void OnGUI() {
		//char
		GUI.Label(new Rect(10, 30, 100, 20),"Character ");
		if (GUI.Button(new Rect(80, 30, 25, 25), "<"))
		{
			prevChar();
		}
		GUI.Label(new Rect(110, 30, 100, 20),GunnerChar[iGunner].name);
		if (GUI.Button(new Rect(200, 30, 25, 25), ">"))
		{
			nextChar();
		}
		// Ani
		GUI.Label(new Rect(10, 60, 100, 20),"Animation");
		if (GUI.Button(new Rect(80, 60, 25, 25), "<"))
		{
			prevAni();
		}
		GUI.Label(new Rect(110, 60, 100, 20),aniname[iani]);
		if (GUI.Button(new Rect(200, 60, 25, 25), ">"))
		{
			nextAni();
		}
		//Gun
		GUI.Label(new Rect(10, 90, 100, 20),"Gun");
		if (GUI.Button(new Rect(80, 90, 25, 25), "<"))
		{
			prevGun();
		}
		GUI.Label(new Rect(110, 90, 100, 20),Gun[iGun].name);
		if (GUI.Button(new Rect(200, 90, 25, 25), ">"))
		{
			nextGun();
		}
		
		
		//Snowboard
		GUI.Label(new Rect(10, 120, 100, 20),"Snowboard");
		if (GUI.Button(new Rect(80, 120, 25, 25), "<"))
		{
			prevSnowboard();
		}
		GUI.Label(new Rect(110, 120, 100, 20),Snowboard[iSnowboard].name);
		if (GUI.Button(new Rect(200, 120, 25, 25), ">"))
		{
			nextSnowboard();
		}
	}
	//char
	private void prevChar()
	{
		iGunner--;
		if(iGunner < 0) iGunner = GunnerChar.Length - 1;
		for(int i = 0; i < GunnerChar.Length; i++)
		{
			if(iGunner != i)
			{
				GunnerChar[i].SetActive(false);
			}
		}
		GunnerChar [iGunner].SetActive (true);
		Gunner = GunnerChar [iGunner];
		Gunner.GetComponent<Animation>().CrossFade(aniname[iani]);
	}
	private void nextChar()
	{
		iGunner++;
		if(iGunner >= GunnerChar.Length) iGunner = 0;
		for(int i = 0; i < GunnerChar.Length; i++)
		{
			if(iGunner != i)
			{
				GunnerChar[i].SetActive(false);
			}
		}
		GunnerChar [iGunner].SetActive (true);
		Gunner = GunnerChar [iGunner];
		Gunner.GetComponent<Animation>().CrossFade(aniname[iani]);
	}
	//ani
	private void prevAni()
	{
		iani--;
		if(iani < 0) iani = aniname.Length - 1;
		
		if(iani < 1)
		{
			for(int i = 0; i < GunnerGun.Length; i++)
			{
				GunnerGun[i].SetActive(false);
				GunnerGun2[i].SetActive(false);
			}
			for(int i = 0; i < GunnerSnowboard.Length; i++)
			{
				GunnerSnowboard[i].SetActive(false);
			}
		}
		else if(iani > 0)
		{
			for(int i = 0; i < GunnerGun.Length; i++)
			{
				GunnerGun[i].SetActive(true);
				GunnerGun[i].GetComponent<SkinnedMeshRenderer>().material = maTGun[iGun];
				GunnerGun[i].GetComponent<SkinnedMeshRenderer>().sharedMesh = mGun[iGun];
				GunnerGun2[i].SetActive(true);
				GunnerGun2[i].GetComponent<SkinnedMeshRenderer>().material = maTGun[iGun];
				GunnerGun2[i].GetComponent<SkinnedMeshRenderer>().sharedMesh = mGun2[iGun];
			}
			for(int i = 0; i < GunnerSnowboard.Length; i++)
			{
				if(iSnowboard != i)
				{
					GunnerSnowboard[i].SetActive(false);
				}
			}
			for(int i = 0; i< GunnerChar.Length; i++)
			{
				if(iGunner != i)
				{
					GunnerChar[i].SetActive(false);
				}
			}
			GunnerSnowboard[iSnowboard].SetActive(true);
		}
		GunnerChar[iGunner].SetActive(true);
		Gunner.GetComponent<Animation>().CrossFade(aniname[iani]);
	}
	private void nextAni()
	{
		iani++;
		if(iani >= aniname.Length) iani = 0;
		
		if(iani < 1)
		{
			for(int i = 0; i < GunnerGun.Length; i++)
			{
				GunnerGun[i].SetActive(false);
			}
			for(int i = 0; i < GunnerSnowboard.Length; i++)
			{
				GunnerSnowboard[i].SetActive(false);
			}
		}
		else if(iani > 0)
		{
			for(int i = 0; i < GunnerGun.Length; i++)
			{
				GunnerGun[i].SetActive(true);
				GunnerGun[i].GetComponent<SkinnedMeshRenderer>().material = maTGun[iGun];
				GunnerGun[i].GetComponent<SkinnedMeshRenderer>().sharedMesh = mGun[iGun];
				GunnerGun2[i].SetActive(true);
				GunnerGun2[i].GetComponent<SkinnedMeshRenderer>().material = maTGun[iGun];
				GunnerGun2[i].GetComponent<SkinnedMeshRenderer>().sharedMesh = mGun2[iGun];
			}
			for(int i = 0; i < GunnerSnowboard.Length; i++)
			{
				if(iSnowboard != i)
				{
					GunnerSnowboard[i].SetActive(false);
				}
			}
			for(int i = 0; i< GunnerChar.Length; i++)
			{
				if(iGunner != i)
				{
					GunnerChar[i].SetActive(false);
				}
			}
			GunnerSnowboard[iSnowboard].SetActive(true);
		}
		GunnerChar[iGunner].SetActive(true);
		Gunner.GetComponent<Animation>().CrossFade(aniname[iani]);
	}
	//Gun
	private void prevGun()
	{
		iGun--;
		if(iGun < 0) iGun = Gun.Length - 1;
		for(int i = 0; i < Gun.Length; i++)
		{
			Gun[i].SetActive(false);
		}
		if(iani > 0)
		{
			for(int i = 0; i < GunnerGun.Length; i++)
			{
				GunnerGun[i].SetActive(true);
				GunnerGun[i].GetComponent<SkinnedMeshRenderer>().material = maTGun[iGun];
				GunnerGun[i].GetComponent<SkinnedMeshRenderer>().sharedMesh = mGun[iGun];
				GunnerGun2[i].SetActive(true);
				GunnerGun2[i].GetComponent<SkinnedMeshRenderer>().material = maTGun[iGun];
				GunnerGun2[i].GetComponent<SkinnedMeshRenderer>().sharedMesh = mGun2[iGun];
			}
			for(int i = 0; i < GunnerSnowboard.Length; i++)
			{
				if(iSnowboard != i)
				{
					GunnerSnowboard[i].SetActive(false);
				}
			}
			for(int i = 0; i< GunnerChar.Length; i++)
			{
				if(iGunner != i)
				{
					GunnerChar[i].SetActive(false);
				}
			}
			GunnerSnowboard[iSnowboard].SetActive(true);
		}
		Gun [iGun].SetActive (true);
		Gunner.GetComponent<Animation>().CrossFade(aniname[iani]);
	}
	private void nextGun()
	{
		iGun ++;
		if(iGun >= Gun.Length) iGun = 0;
		for(int i = 0; i < Gun.Length; i++)
		{
			Gun[i].SetActive(false);
		}
		if(iani > 0)
		{
			for(int i = 0; i < GunnerGun.Length; i++)
			{
				GunnerGun[i].SetActive(true);
				GunnerGun[i].GetComponent<SkinnedMeshRenderer>().material = maTGun[iGun];
				GunnerGun[i].GetComponent<SkinnedMeshRenderer>().sharedMesh = mGun[iGun];
				GunnerGun2[i].SetActive(true);
				GunnerGun2[i].GetComponent<SkinnedMeshRenderer>().material = maTGun[iGun];
				GunnerGun2[i].GetComponent<SkinnedMeshRenderer>().sharedMesh = mGun2[iGun];
			}
			for(int i = 0; i < GunnerSnowboard.Length; i++)
			{
				if(iSnowboard != i)
				{
					GunnerSnowboard[i].SetActive(false);
				}
			}
			for(int i = 0; i< GunnerChar.Length; i++)
			{
				if(iGunner != i)
				{
					GunnerChar[i].SetActive(false);
				}
			}
			GunnerSnowboard[iSnowboard].SetActive(true);
		}
		Gun [iGun].SetActive (true);
		Gunner.GetComponent<Animation>().CrossFade(aniname[iani]);
	}
	
	//broom
	private void prevSnowboard()
	{
		iSnowboard--;
		if(iSnowboard < 0) iSnowboard = Snowboard.Length - 1;
		for(int i = 0; i < Snowboard.Length; i++)
		{
			if(iSnowboard != i)
			{
				Snowboard[i].SetActive(false);
			}
		}
		if(iani > 0)
		{
			for(int i = 0; i < GunnerGun.Length; i++)
			{
				GunnerGun[i].SetActive(true);
				GunnerGun[i].GetComponent<SkinnedMeshRenderer>().material = maTGun[iGun];
				GunnerGun[i].GetComponent<SkinnedMeshRenderer>().sharedMesh = mGun[iGun];
				GunnerGun2[i].SetActive(true);
				GunnerGun2[i].GetComponent<SkinnedMeshRenderer>().material = maTGun[iGun];
				GunnerGun2[i].GetComponent<SkinnedMeshRenderer>().sharedMesh = mGun2[iGun];
			}
			for(int i = 0; i < GunnerSnowboard.Length; i++)
			{
				if(iSnowboard != i)
				{
					GunnerSnowboard[i].SetActive(false);
				}
			}
			for(int i = 0; i< GunnerChar.Length; i++)
			{
				if(iGunner != i)
				{
					GunnerChar[i].SetActive(false);
				}
			}
			GunnerSnowboard[iSnowboard].SetActive(true);
		}
		Snowboard [iSnowboard].SetActive (true);
	}
	private void nextSnowboard()
	{
		iSnowboard++;
		if(iSnowboard >= Snowboard.Length) iSnowboard = 0;
		for(int i = 0; i < Snowboard.Length; i++)
		{
			if(iSnowboard != i)
			{
				Snowboard[i].SetActive(false);
			}
		}
		if(iani > 0)
		{
			for(int i = 0; i < GunnerGun.Length; i++)
			{
				GunnerGun[i].SetActive(true);
				GunnerGun[i].GetComponent<SkinnedMeshRenderer>().material = maTGun[iGun];
				GunnerGun[i].GetComponent<SkinnedMeshRenderer>().sharedMesh = mGun[iGun];
				GunnerGun2[i].SetActive(true);
				GunnerGun2[i].GetComponent<SkinnedMeshRenderer>().material = maTGun[iGun];
				GunnerGun2[i].GetComponent<SkinnedMeshRenderer>().sharedMesh = mGun2[iGun];
			}
			for(int i = 0; i < GunnerSnowboard.Length; i++)
			{
				if(iSnowboard != i)
				{
					GunnerSnowboard[i].SetActive(false);
				}
			}
			for(int i = 0; i< GunnerChar.Length; i++)
			{
				if(iGunner != i)
				{
					GunnerChar[i].SetActive(false);
				}
			}
			GunnerSnowboard[iSnowboard].SetActive(true);
		}
		Snowboard [iSnowboard].SetActive (true);
	}
}