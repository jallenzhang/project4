using UnityEngine;
using System.Collections;

public class GuideCamera : MonoBehaviour {
	[HideInInspector]
	public Vector3 left_bottom;
	private Vector3 tmp_left_bottom1;
	private Vector3 tmp_left_bottom2;
	private Vector3 tmp_left_bottom_dir;
	[HideInInspector]
	public Vector3 left_top;
	private Vector3 tmp_left_top1;
	private Vector3 tmp_left_top2;
	private Vector3 tmp_left_top_dir;
	[HideInInspector]
	public Vector3 right_top;
	private Vector3 tmp_right_top1;
	private Vector3 tmp_right_top2;
	private Vector3 tmp_right_top_dir;
	[HideInInspector]
	public Vector3 right_bottom;
	private Vector3 tmp_right_bottom1;
	private Vector3 tmp_right_bottom2;
	private Vector3 tmp_right_bottom_dir;
	
	private float width;
	private float height;
	// Use this for initialization
	void Start () {
		width = Screen.width;
		height = Screen.height;
	}
	
	// Update is called once per frame
	void Update () {
		
		tmp_left_bottom1 = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
		tmp_left_bottom2 = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane + 1));
		
		tmp_left_bottom_dir = tmp_left_bottom2 - tmp_left_bottom1;
		Ray ray = new Ray(tmp_left_bottom1, tmp_left_bottom_dir);
		RaycastHit raycastHit;
		
		//进行光线投射操作,第一个参数为光线的开始点和方向，第二个参数为光线碰撞器碰到哪里的输出信息，第三个参数为光线的长度
		Physics.Raycast(ray, out raycastHit, Mathf.Infinity, 1<<LayerMask.NameToLayer("Ground"));
		left_bottom = raycastHit.point;
		
		tmp_left_top1 = Camera.main.ScreenToWorldPoint(new Vector3(0, height, Camera.main.nearClipPlane));
		tmp_left_top2 = Camera.main.ScreenToWorldPoint(new Vector3(0, height, Camera.main.nearClipPlane + 1));
		tmp_left_top_dir = tmp_left_top2 - tmp_left_top1;
		ray = new Ray(tmp_left_top1, tmp_left_top_dir);
		
		//进行光线投射操作,第一个参数为光线的开始点和方向，第二个参数为光线碰撞器碰到哪里的输出信息，第三个参数为光线的长度
		Physics.Raycast(ray, out raycastHit, Mathf.Infinity, 1<<LayerMask.NameToLayer("Ground"));
		left_top = raycastHit.point;
		
		tmp_right_top1 = Camera.main.ScreenToWorldPoint(new Vector3(width, height, Camera.main.nearClipPlane));
		tmp_right_top2 = Camera.main.ScreenToWorldPoint(new Vector3(width, height, Camera.main.nearClipPlane + 1));
		tmp_right_top_dir = tmp_right_top2 - tmp_right_top1;
		ray = new Ray(tmp_right_top1, tmp_right_top_dir);
		Physics.Raycast(ray, out raycastHit, Mathf.Infinity, 1<<LayerMask.NameToLayer("Ground"));
		right_top = raycastHit.point;
		
		tmp_right_bottom1 = Camera.main.ScreenToWorldPoint(new Vector3(width, 0, Camera.main.nearClipPlane));
		tmp_right_bottom2 = Camera.main.ScreenToWorldPoint(new Vector3(width, 0, Camera.main.nearClipPlane + 1));
		tmp_right_bottom_dir = tmp_right_bottom2 - tmp_right_bottom1;
		ray = new Ray(tmp_right_bottom1, tmp_right_bottom_dir);
		Physics.Raycast(ray, out raycastHit, Mathf.Infinity, 1<<LayerMask.NameToLayer("Ground"));
		right_bottom = raycastHit.point;
	}
	
	void OnDrawGizmosSelected() {
		Gizmos.color = Color.yellow;
		Gizmos.DrawSphere(left_bottom, .1F);
		
		Gizmos.color = Color.green;
		Gizmos.DrawSphere(left_top, .1F);
		
		Gizmos.color = Color.red;
		Gizmos.DrawSphere(right_top, .1F);
		
		Gizmos.color = Color.blue;
		Gizmos.DrawSphere(right_bottom, .1F);
		
		Debug.DrawLine(left_bottom, left_top, Color.red);
		Debug.DrawLine(left_bottom, right_bottom, Color.red);
		Debug.DrawLine(right_top, right_bottom, Color.red);
		Debug.DrawLine(right_top, left_top, Color.red);
	}
}
