using UnityEngine;
using System.Collections;

public class Witch_Manager : MonoBehaviour {

	public GameObject Witch;
	public string[] aniname;
	public GameObject[] WitchChar;
	public GameObject[] WitchWand;
	public GameObject[] Wand;
	public GameObject[] WitchBroom;
	public GameObject[] Broom;
	public Mesh[] mWand;
	public Material[] maTWand;
	public Mesh[] mBroom;
	public Material[] maTBroom;
	
	public int iWitch;
	public int iWand;
	public int iBroom;
	public int iani;
	
	void OnGUI() {
		//char
		GUI.Label(new Rect(10, 30, 100, 20),"Character ");
		if (GUI.Button(new Rect(80, 30, 25, 25), "<"))
		{
			prevChar();
		}
		GUI.Label(new Rect(110, 30, 100, 20),WitchChar[iWitch].name);
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
		//Wand
		GUI.Label(new Rect(10, 90, 100, 20),"Wand");
		if (GUI.Button(new Rect(80, 90, 25, 25), "<"))
		{
			prevWand();
		}
		GUI.Label(new Rect(110, 90, 100, 20),Wand[iWand].name);
		if (GUI.Button(new Rect(200, 90, 25, 25), ">"))
		{
			nextWand();
		}
		
		
		//Broom
		GUI.Label(new Rect(10, 120, 100, 20),"Broom");
		if (GUI.Button(new Rect(80, 120, 25, 25), "<"))
		{
			prevBroom();
		}
		GUI.Label(new Rect(110, 120, 100, 20),Broom[iBroom].name);
		if (GUI.Button(new Rect(200, 120, 25, 25), ">"))
		{
			nextBroom();
		}
	}
	//char
	private void prevChar()
	{
		iWitch--;
		if(iWitch < 0) iWitch = WitchChar.Length - 1;
		for(int i = 0; i < WitchChar.Length; i++)
		{
			if(iWitch != i)
			{
				WitchChar[i].SetActive(false);
			}
		}
		WitchChar [iWitch].SetActive (true);
		Witch = WitchChar [iWitch];
		Witch.GetComponent<Animation>().CrossFade(aniname[iani]);
	}
	private void nextChar()
	{
		iWitch++;
		if(iWitch >= WitchChar.Length) iWitch = 0;
		for(int i = 0; i < WitchChar.Length; i++)
		{
			if(iWitch != i)
			{
				WitchChar[i].SetActive(false);
			}
		}
		WitchChar [iWitch].SetActive (true);
		Witch = WitchChar [iWitch];
		Witch.GetComponent<Animation>().CrossFade(aniname[iani]);
	}
	//ani
	private void prevAni()
	{
		iani--;
		if(iani < 0) iani = aniname.Length - 1;
		
		if(iani < 3)
		{
			for(int i = 0; i < WitchWand.Length; i++)
			{
				WitchWand[i].SetActive(false);
			}
			for(int i = 0; i < WitchBroom.Length; i++)
			{
				WitchBroom[i].SetActive(false);
			}
		}
		else if(iani > 2)
		{
			for(int i = 0; i < WitchWand.Length; i++)
			{
				WitchWand[i].SetActive(true);
				WitchWand[i].GetComponent<SkinnedMeshRenderer>().material = maTWand[iWand];
				WitchWand[i].GetComponent<SkinnedMeshRenderer>().sharedMesh = mWand[iWand];
			}
			for(int i = 0; i < WitchBroom.Length; i++)
			{
				WitchBroom[i].SetActive(true);
				WitchBroom[i].GetComponent<SkinnedMeshRenderer>().material = maTBroom[iBroom];
				WitchBroom[i].GetComponent<SkinnedMeshRenderer>().sharedMesh = mBroom[iBroom];
			}
			for(int i = 0; i< WitchChar.Length; i++)
			{
				if(iWitch != i)
				{
					WitchChar[i].SetActive(false);
				}
			}
		}
		WitchChar[iWitch].SetActive(true);
		Witch.GetComponent<Animation>().CrossFade(aniname[iani]);
	}
	private void nextAni()
	{
		iani++;
		if(iani >= aniname.Length) iani = 0;
		
		if(iani < 3)
		{
			for(int i = 0; i < WitchWand.Length; i++)
			{
				WitchWand[i].SetActive(false);
			}
			for(int i = 0; i < WitchBroom.Length; i++)
			{
				WitchBroom[i].SetActive(false);
			}
		}
		else if(iani > 2)
		{
			for(int i = 0; i < WitchWand.Length; i++)
			{
				WitchWand[i].SetActive(true);
				WitchWand[i].GetComponent<SkinnedMeshRenderer>().material = maTWand[iWand];
				WitchWand[i].GetComponent<SkinnedMeshRenderer>().sharedMesh = mWand[iWand];
			}
			for(int i = 0; i < WitchBroom.Length; i++)
			{
				WitchBroom[i].SetActive(true);
				WitchBroom[i].GetComponent<SkinnedMeshRenderer>().material = maTBroom[iBroom];
				WitchBroom[i].GetComponent<SkinnedMeshRenderer>().sharedMesh = mBroom[iBroom];
			}
			for(int i = 0; i< WitchChar.Length; i++)
			{
				if(iWitch != i)
				{
					WitchChar[i].SetActive(false);
				}
			}
		}
		WitchChar[iWitch].SetActive(true);
		Witch.GetComponent<Animation>().CrossFade(aniname[iani]);
	}
	//Wand
	private void prevWand()
	{
		iWand--;
		if(iWand < 0) iWand = Wand.Length - 1;
		for(int i = 0; i < Wand.Length; i++)
		{
			Wand[i].SetActive(false);
		}
		if(iani > 2)
		{
			for(int i = 0; i < WitchWand.Length; i++)
			{
				WitchWand[i].SetActive(true);
				WitchWand[i].GetComponent<SkinnedMeshRenderer>().material = maTWand[iWand];
				WitchWand[i].GetComponent<SkinnedMeshRenderer>().sharedMesh = mWand[iWand];
			}
			for(int i = 0; i < WitchBroom.Length; i++)
			{
				WitchBroom[i].SetActive(true);
				WitchBroom[i].GetComponent<SkinnedMeshRenderer>().material = maTBroom[iBroom];
				WitchBroom[i].GetComponent<SkinnedMeshRenderer>().sharedMesh = mBroom[iBroom];
			}
			for(int i = 0; i< WitchChar.Length; i++)
			{
				if(iWitch != i)
				{
					WitchChar[i].SetActive(false);
				}
			}
		}
		Wand [iWand].SetActive (true);
		Witch.GetComponent<Animation>().CrossFade(aniname[iani]);
	}
	private void nextWand()
	{
		iWand ++;
		if(iWand >= Wand.Length) iWand = 0;
		for(int i = 0; i < Wand.Length; i++)
		{
			Wand[i].SetActive(false);
		}
		if(iani > 2)
		{
			for(int i = 0; i < WitchWand.Length; i++)
			{
				WitchWand[i].SetActive(true);
				WitchWand[i].GetComponent<SkinnedMeshRenderer>().material = maTWand[iWand];
				WitchWand[i].GetComponent<SkinnedMeshRenderer>().sharedMesh = mWand[iWand];
			}
			for(int i = 0; i < WitchBroom.Length; i++)
			{
				WitchBroom[i].SetActive(true);
				WitchBroom[i].GetComponent<SkinnedMeshRenderer>().material = maTBroom[iBroom];
				WitchBroom[i].GetComponent<SkinnedMeshRenderer>().sharedMesh = mBroom[iBroom];
			}
			for(int i = 0; i< WitchChar.Length; i++)
			{
				if(iWitch != i)
				{
					WitchChar[i].SetActive(false);
				}
			}
		}
		Wand [iWand].SetActive (true);
		Witch.GetComponent<Animation>().CrossFade(aniname[iani]);
	}
	
	//broom
	private void prevBroom()
	{
		iBroom--;
		if(iBroom < 0) iBroom = Broom.Length - 1;
		for(int i = 0; i < Broom.Length; i++)
		{
			if(iBroom != i)
			{
				Broom[i].SetActive(false);
			}
		}
		if(iani > 2)
		{
			for(int i = 0; i < WitchWand.Length; i++)
			{
				WitchWand[i].SetActive(true);
				WitchWand[i].GetComponent<SkinnedMeshRenderer>().material = maTWand[iWand];
				WitchWand[i].GetComponent<SkinnedMeshRenderer>().sharedMesh = mWand[iWand];
			}
			for(int i = 0; i < WitchBroom.Length; i++)
			{
				WitchBroom[i].SetActive(true);
				WitchBroom[i].GetComponent<SkinnedMeshRenderer>().material = maTBroom[iBroom];
				WitchBroom[i].GetComponent<SkinnedMeshRenderer>().sharedMesh = mBroom[iBroom];
			}
			for(int i = 0; i< WitchChar.Length; i++)
			{
				if(iWitch != i)
				{
					WitchChar[i].SetActive(false);
				}
			}
		}
		Broom [iBroom].SetActive (true);
	}
	private void nextBroom()
	{
		iBroom++;
		if(iBroom >= Broom.Length) iBroom = 0;
		for(int i = 0; i < Broom.Length; i++)
		{
			if(iBroom != i)
			{
				Broom[i].SetActive(false);
			}
		}
		if(iani > 2)
		{
			for(int i = 0; i < WitchWand.Length; i++)
			{
				WitchWand[i].SetActive(true);
				WitchWand[i].GetComponent<SkinnedMeshRenderer>().material = maTWand[iWand];
				WitchWand[i].GetComponent<SkinnedMeshRenderer>().sharedMesh = mWand[iWand];
			}
			for(int i = 0; i < WitchBroom.Length; i++)
			{
				WitchBroom[i].SetActive(true);
				WitchBroom[i].GetComponent<SkinnedMeshRenderer>().material = maTBroom[iBroom];
				WitchBroom[i].GetComponent<SkinnedMeshRenderer>().sharedMesh = mBroom[iBroom];
			}
			for(int i = 0; i< WitchChar.Length; i++)
			{
				if(iWitch != i)
				{
					WitchChar[i].SetActive(false);
				}
			}
		}
		Broom [iBroom].SetActive (true);
	}
}