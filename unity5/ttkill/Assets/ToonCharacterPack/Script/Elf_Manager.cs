using UnityEngine;
using System.Collections;

public class Elf_Manager : MonoBehaviour {

	public GameObject Elf;
	public string[] aniname;
	public GameObject[] ElfChar;
	public GameObject[] ElfWand;
	public GameObject[] ElfWand1;
	public GameObject[] ElfWand2;
	public GameObject[] Wand;
	public GameObject[] ElfPet;
	public GameObject[] Pet;
	public GameObject[] WandAni;
	public Mesh[] mWand;
	public Material[] maTWand;
	public Vector3[] cWand;
	public Vector3[] eWand;

	public int iElf;
	public int iWand;
	public int iPet;
	public int iani;

	void OnGUI() {
		//char
		GUI.Label(new Rect(10, 30, 100, 20),"Character ");
		if (GUI.Button(new Rect(80, 30, 25, 25), "<"))
		{
			prevChar();
		}
		GUI.Label(new Rect(110, 30, 100, 20),ElfChar[iElf].name);
		if (GUI.Button(new Rect(160, 30, 25, 25), ">"))
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
		if (GUI.Button(new Rect(160, 60, 25, 25), ">"))
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
		if (GUI.Button(new Rect(160, 90, 25, 25), ">"))
		{
			nextWand();
		}

		
		//Pet
		GUI.Label(new Rect(10, 120, 100, 20),"Pet");
		if (GUI.Button(new Rect(80, 120, 25, 25), "<"))
		{
			prevPet();
		}
		GUI.Label(new Rect(110, 120, 100, 20),Pet[iPet].name);
		if (GUI.Button(new Rect(160, 120, 25, 25), ">"))
		{
			nextPet();
		}

	}
	//char
	private void prevChar()
	{
		iElf--;
		if(iElf < 0) iElf = ElfChar.Length - 1;
		for(int i = 0; i < ElfChar.Length; i++)
		{
			if(iElf != i)
			{
				ElfChar[i].SetActive(false);
			}
		}
		ElfChar [iElf].SetActive (true);
		Elf = ElfChar [iElf];
		Elf.GetComponent<Animation>().CrossFade(aniname[iani]);
	}
	private void nextChar()
	{
		iElf++;
		if(iElf >= ElfChar.Length) iElf = 0;
		for(int i = 0; i < ElfChar.Length; i++)
		{
			if(iElf != i)
			{
				ElfChar[i].SetActive(false);
			}
		}
		ElfChar [iElf].SetActive (true);
		Elf = ElfChar [iElf];
		Elf.GetComponent<Animation>().CrossFade(aniname[iani]);
	}
	//ani
	private void prevAni()
	{
		iani--;
		if(iani < 0) iani = aniname.Length - 1;
		
		if(iani < 1)
		{
			for(int i = 0; i < ElfWand.Length; i++)
			{
				ElfWand[i].SetActive(false);
			}
			for(int i = 0; i < ElfPet.Length; i++)
			{
				ElfPet[i].SetActive(false);
			}
		}
		else if(iani > 0)
		{
			for(int i = 0; i < ElfWand.Length; i++)
			{
				ElfWand[i].SetActive(true);
				ElfWand[i].GetComponent<SkinnedMeshRenderer>().material = maTWand[iWand];
				ElfWand[i].GetComponent<SkinnedMeshRenderer>().sharedMesh = mWand[iWand];
			}
			for(int i = 0; i < ElfPet.Length; i++)
			{
				if(iPet != i)
				{
					ElfPet[i].SetActive(false);
				}
			}
			for(int i = 0; i< ElfChar.Length; i++)
			{
				if(iElf != i)
				{
					ElfChar[i].SetActive(false);
				}
			}
			ElfPet[iPet].SetActive(true);
		}
		ElfChar[iElf].SetActive(true);
		Elf.GetComponent<Animation>().CrossFade(aniname[iani]);
	}
	private void nextAni()
	{
		iani++;
		if(iani >= aniname.Length) iani = 0;
		
		if(iani < 1)
		{
			for(int i = 0; i < ElfWand.Length; i++)
			{
				ElfWand[i].SetActive(false);
			}
			for(int i = 0; i < ElfPet.Length; i++)
			{
				ElfPet[i].SetActive(false);
			}
		}
		else if(iani > 0)
		{
			for(int i = 0; i < ElfWand.Length; i++)
			{
				ElfWand[i].SetActive(true);
				ElfWand[i].GetComponent<SkinnedMeshRenderer>().material = maTWand[iWand];
				ElfWand[i].GetComponent<SkinnedMeshRenderer>().sharedMesh = mWand[iWand];
			}
			for(int i = 0; i < ElfPet.Length; i++)
			{
				if(iPet != i)
				{
					ElfPet[i].SetActive(false);
				}
			}
			for(int i = 0; i< ElfChar.Length; i++)
			{
				if(iElf != i)
				{
					ElfChar[i].SetActive(false);
				}
			}
			ElfPet[iPet].SetActive(true);
		}
		ElfChar[iElf].SetActive(true);
		Elf.GetComponent<Animation>().CrossFade(aniname[iani]);
	}
	//Wand
	private void prevWand()
	{
		iWand--;
		if(iWand < 0) iWand = Wand.Length - 1;
		for(int i = 0; i < Wand.Length; i++)
		{
			if(iWand != i)
			{
				Wand[i].SetActive(false);
			}
		}

		if(iani > 0)
		{
			for(int i = 0; i < Wand.Length; i++)
			{
				ElfWand[i].SetActive(true);
				ElfWand[i].GetComponent<SkinnedMeshRenderer>().material = maTWand[iWand];
				ElfWand[i].GetComponent<SkinnedMeshRenderer>().sharedMesh = mWand[iWand];
			}
			for(int i = 0; i < ElfPet.Length; i++)
			{
				if(iPet != i)
				{
					ElfPet[i].SetActive(false);
				}
			}
			for(int i = 0; i< ElfChar.Length; i++)
			{
				if(iElf != i)
				{
					ElfChar[i].SetActive(false);
				}
			}
			ElfPet[iPet].SetActive(true);
		}
		Wand [iWand].SetActive (true);
		Elf.GetComponent<Animation>().CrossFade(aniname[iani]);
	}
	private void nextWand()
	{
		iWand++;
		if(iWand >= Wand.Length) iWand = 0;
		for(int i = 0; i < Wand.Length; i++)
		{
			if(iWand != i)
			{
				Wand[i].SetActive(false);
			}
		}
		if(iani > 0)
		{
			for(int i = 0; i < ElfWand.Length; i++)
			{
				ElfWand[i].SetActive(true);
				ElfWand[i].GetComponent<SkinnedMeshRenderer>().material = maTWand[iWand];
				ElfWand[i].GetComponent<SkinnedMeshRenderer>().sharedMesh = mWand[iWand];
			}
			for(int i = 0; i < ElfPet.Length; i++)
			{
				if(iPet != i)
				{
					ElfPet[i].SetActive(false);
				}
			}
			for(int i = 0; i< ElfChar.Length; i++)
			{
				if(iElf != i)
				{
					ElfChar[i].SetActive(false);
				}
			}
			ElfPet[iPet].SetActive(true);
		}
		Wand [iWand].SetActive (true);
		Elf.GetComponent<Animation>().CrossFade(aniname[iani]);
	}
	
	//broom
	private void prevPet()
	{
		iPet--;
		if(iPet < 0) iPet = Pet.Length - 1;
		for(int i = 0; i < Pet.Length; i++)
		{
			if(iPet != i)
			{
				Pet[i].SetActive(false);
			}
		}
		if(iani > 0)
		{
			for(int i = 0; i < ElfWand.Length; i++)
			{
				ElfWand[i].SetActive(true);
				ElfWand[i].GetComponent<SkinnedMeshRenderer>().material = maTWand[iWand];
				ElfWand[i].GetComponent<SkinnedMeshRenderer>().sharedMesh = mWand[iWand];
			}
			for(int i = 0; i < ElfPet.Length; i++)
			{
				if(iPet != i)
				{
					ElfPet[i].SetActive(false);
				}
			}
			for(int i = 0; i< ElfChar.Length; i++)
			{
				if(iElf != i)
				{
					ElfChar[i].SetActive(false);
				}
			}
			ElfPet[iPet].SetActive(true);
		}
		Pet [iPet].SetActive (true);
	}
	private void nextPet()
	{
		iPet++;
		if(iPet >= Pet.Length) iPet = 0;
		for(int i = 0; i < Pet.Length; i++)
		{
			if(iPet != i)
			{
				Pet[i].SetActive(false);
			}
		}
		if(iani > 0)
		{
			for(int i = 0; i < ElfWand.Length; i++)
			{
				ElfWand[i].SetActive(true);
				ElfWand[i].GetComponent<SkinnedMeshRenderer>().material = maTWand[iWand];
				ElfWand[i].GetComponent<SkinnedMeshRenderer>().sharedMesh = mWand[iWand];
			}
			for(int i = 0; i < ElfPet.Length; i++)
			{
				if(iPet != i)
				{
					ElfPet[i].SetActive(false);
				}
			}
			for(int i = 0; i< ElfChar.Length; i++)
			{
				if(iElf != i)
				{
					ElfChar[i].SetActive(false);
				}
			}
			ElfPet[iPet].SetActive(true);
		}
		Pet [iPet].SetActive (true);
	}
}