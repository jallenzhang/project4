using UnityEngine;
using System.Collections;

public class BakeMeshTest : MonoBehaviour
{
	private bool bake = false;
	void Start()
	{
	}

	IEnumerator Generator()
	{
		GenerateBake();
		yield return new WaitForSeconds(0.05f);
		if (bake)
			StartCoroutine(Generator());
	}

	public void StartBake()
	{
		bake = true;
		GameData.Instance.Wudi = true;
		StartCoroutine(Generator());
	}

	public void StopBake()
	{
		bake = false;
		GameData.Instance.Wudi = false;
	}

	void GenerateBake()
	{
		Mesh frameMesh = new Mesh();
		frameMesh.name = "test";
		
		// Sample animation to get bones in the right place
		m_animation.Sample();
		
		// Bake the mesh
		m_skinnedMeshRenderer.BakeMesh(frameMesh);
		
		// Setup game object to show frame
		GameObject frameGO = new GameObject("test");
//			frameGO.name = frameName;
		frameGO.transform.position = transform.position;// + new Vector3(frameIndex, 0.0f, 0.0f);
		frameGO.transform.rotation = m_skinnedMeshRenderer.transform.rotation;
		
		// Setup mesh filter
		MeshFilter meshFilter = frameGO.AddComponent<MeshFilter>();
		meshFilter.mesh = frameMesh;
		
		// Setup mesh renderer
		MeshRenderer meshRenderer = frameGO.AddComponent<MeshRenderer>();
		meshRenderer.sharedMaterials = bakeMat;//m_skinnedMeshRenderer.sharedMaterials;
		TweenAlpha.Begin(frameGO, 0.5f, 0f);
		TweenScale.Begin(frameGO, 0.5f, Vector3.one * 0.8f);

		Destroy(frameGO, 0.3f);
	}
	
	[SerializeField]
	Animation m_animation; // Animation component used for baking
	
	[SerializeField]
	SkinnedMeshRenderer m_skinnedMeshRenderer; // Skinned mesh renderer used for baking
	
	[SerializeField]
	string m_clipToBake = "meleewalk"; // Name of the animation clip to bake
	
	[SerializeField]
	int m_numFramesToBake = 20; // Number of frames to bake

	[SerializeField]
	Material[] bakeMat;
}