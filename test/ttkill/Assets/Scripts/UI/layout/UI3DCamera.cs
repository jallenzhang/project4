using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class UI3DCamera : MonoBehaviour {
	private Camera m_camera;
	// Use this for initialization
	void Start () {
		m_camera = GetComponent<Camera>();
		m_camera.fieldOfView = ScreenHelper.getCameraFOV(m_camera.fieldOfView);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
