using UnityEngine;
using System.Collections;

public class AttackButton : MonoBehaviour {

	private Vector2 centerPos;    //摇杆的中心点位置，屏幕坐标
	public GUITexture centerBall;    //球型摇杆
	public float joyStickRadius;    //摇杆移动范围的半径
	private Vector2 position;    //摇杆要传递出去的参数,就靠他控制移动旋转
//	private int lastFingerID = -1;    //最后一次触摸的手指id
	private bool pressed = false;    //球型摇杆移动开关

	public float screenRateX  = 1;
	public float screenRateY  = 1;
	private float rate  = 0.1f;

	// Use this for initialization
	void Start () {
		screenRateX = Screen.width * rate /  1280;
		screenRateY = Screen.width * rate /  720;
//		centerBall.transform.localScale = new Vector3( centerBall.pixelInset.size.x / Screen.width, centerBall.pixelInset.size.y / Screen.height,0);
		centerBall.transform.localScale = new Vector3( screenRateX, screenRateY,0);
		centerBall.transform.position = new Vector3 (centerBall.transform.position.x - centerBall.pixelInset.size.x * 3/ 2 / Screen.width,
		                                             centerBall.transform.position.y + centerBall.pixelInset.size.y / 2 / Screen.height, 0);		
		//将摇杆坐标赋给作为摇杆的中心,以后用来复位摇杆用
		centerPos = centerBall.transform.position;
	}
	
	// Update is called once per frame
	void Update () 
	{
		int count = Input.touchCount;    //获取触摸点的数量

		for (int i = 0; i < count; i++) {    //逐个分析触摸点的操作
			Touch touch = Input.GetTouch (i);    //获取当前处理的触摸点
			//将当前的触摸坐标转换为屏幕坐标
			Vector2 currentTouchPos = new Vector2 (touch.position.x / Screen.width - centerBall.pixelInset.size.x / 2 / Screen.width,
			                                       touch.position.y / Screen.height - centerBall.pixelInset.size.y / 2 / Screen.height);
			
			Vector2 temp = currentTouchPos - centerPos;    //得到方向向量temp（触摸的位置和摇杆的坐标差）
			
			if (centerBall.HitTest (touch.position)) {    //判断是否触摸点在要干范围之内   
				if (temp.magnitude < joyStickRadius) {    //如果方向向量temp的长度没有超出摇杆的半径,temp.magnitude为求坐标差的距离,及两点间的距离
//					lastFingerID = touch.fingerId;    //记录该触摸的id
					pressed = true;    //摇杆移动开关打开
					break;
				}
			} 
			else
			{
				pressed = false;
			}
		}

		if (count == 0) {
			pressed = false;		
		}
	}

	public bool IsPressed()
	{
		return pressed;
	}
}
