using UnityEngine;
using System.Collections;

public class PetSelectItem : MonoBehaviour {
	public GameObject readyObj;
	public PetType type;
	public GameObject lockObj;

	private PetDialog petDialog;
	// Use this for initialization
	void Start () {
		petDialog = GetComponentInParent<PetDialog>();
		PetSelectionChanged();
		EventService.Instance.GetEvent<PetSelectionEvent>().Subscribe(PetSelectionChanged);

		UpdateUI();
	}

	void UpdateUI()
	{
		int level = PetDB.Instance.GetPetLvById((int)type);

		if (level == 0)
			lockObj.SetActive(true);		
		else
			lockObj.SetActive(false);
	}

	// Update is called once per frame
	void Update () {
	
	}

	public void UnlockPet()
	{
//		Debug.Log("aaaaaaaaaaaa");
		PetInfo info = IOHelper.GetPetInfoByIdAndLevel((int)type,1);
		if (info != null)
			UnlockPetDialog.PopUp(info.cost, (int)type, UpdateUI);
	}

	void PetSelectionChanged()
	{
		int onBattle = PetDB.Instance.GetPetOnBattleById((int)type);
		if (onBattle == 1)
			readyObj.SetActive(true);
		else
			readyObj.SetActive(false);
	}

	public void onSelect()
	{
		int onBattle = PetDB.Instance.GetPetOnBattleById((int)type);
		if (onBattle == 0)
			petDialog.OnPetSelect(type);
	}

	void OnDestroy()
	{
		EventService.Instance.GetEvent<PetSelectionEvent>().Unsubscribe(PetSelectionChanged);
	}
}
