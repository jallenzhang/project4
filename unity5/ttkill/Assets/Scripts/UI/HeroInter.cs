using UnityEngine;
using System.Collections;

public class HeroInter : MonoBehaviour {
	public GameObject petArea;
	private float speed = 1f;
	private float lastX = 0;
	private Vector3 pos1 = new Vector3(-1.53f, 0.35f, 0);
	private Vector3 pos2 = new Vector3(1.53f, 0.35f, 0);
	private Vector3 localScale = Vector3.one * 0.7f;
	private int petPos = 0;

	// Use this for initialization
	void Start () {

	}

	void OnEnable()
	{
		petPos = 0;
		int petCount = petArea.transform.childCount - 1;
		while(petCount>=0)
		{
			Destroy(petArea.transform.GetChild(petCount).gameObject);
			petCount-=1;
		}

		System.Array petArray = System.Enum.GetValues(typeof(PetType));
		foreach(var p in petArray)
		{
			int onBattle = PetDB.Instance.GetPetOnBattleById((int)p);
			GameObject pet = null;
			if (onBattle == 1)
			{
				switch((PetType)p)
				{
				case PetType.songshu:
					pet = (GameObject)Instantiate(Resources.Load("prefabs/pet/pet_songshu"));
					break;
				case PetType.tuzi:
					pet = (GameObject)Instantiate(Resources.Load("prefabs/pet/tuzi"));
					break;
				case PetType.pet3:
					pet = (GameObject)Instantiate(Resources.Load("prefabs/pet/pet03"));
					break;
				case PetType.pet4:
					pet = (GameObject)Instantiate(Resources.Load("prefabs/pet/pet04"));
					break;
				case PetType.pet5:
					pet = (GameObject)Instantiate(Resources.Load("prefabs/pet/pet05"));
					break;
				case PetType.pet6:
					pet = (GameObject)Instantiate(Resources.Load("prefabs/pet/pet06"));
					break;
				case PetType.pet7:
					pet = (GameObject)Instantiate(Resources.Load("prefabs/pet/pet07"));
					break;
				default:
					pet = (GameObject)Instantiate(Resources.Load("prefabs/pet/pet_songshu"));
					break;
				}
				
				pet.transform.parent = petArea.transform;
				pet.transform.localRotation = Quaternion.Euler(Vector3.zero);
				pet.transform.localScale = localScale;
				pet.GetComponent<Rigidbody>().isKinematic = true;
				pet.GetComponent<PinkPetSM>().enabled =false;
				if (petPos == 0)
				{
					pet.transform.localPosition = pos1;
					petPos += 1;
				}
				else if (petPos == 1)
				{
					pet.transform.localPosition = pos2;
					petPos += 1;
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDrag()
	{
		float currentX = Input.mousePosition.x;
		float deltaX = currentX - lastX;
		lastX = currentX;
		transform.rotation = transform.localRotation * Quaternion.Euler(new Vector3(0, -deltaX * speed, 0));
	}

	void OnMouseDown()
	{
		lastX = Input.mousePosition.x;
	}

	void OnMouseUp()
	{
//		lastX = 0;
	}
}
