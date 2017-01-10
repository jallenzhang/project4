using UnityEngine;
using System.Collections;
//创建枚举类型,可在加载脚本的实体上选择是左摇杆还是右摇杆
public enum JoyStickType
{
	leftJoyStick,
	rightJoyStick
}

//脚本JoyStick类
public class JoyStick : MonoBehaviour
{
	public JoyStickType joyStickType;    //摇杆类型，左摇杆还是右摇杆
	private Vector2 centerPos;    //摇杆的中心点位置，屏幕坐标
	public GUITexture centerBall;    //球型摇杆
	public float joyStickRadius;    //摇杆移动范围的半径
	private Vector2 position;    //摇杆要传递出去的参数,就靠他控制移动旋转
	private int lastFingerID = -1;    //最后一次触摸的手指id
	private bool centerBallMoving = false;    //球型摇杆移动开关
	//加载时运行方法
	void Start ()
	{
		//为了让摇杆适配不同分辨率屏幕,设置大小和坐标
		centerBall.transform.localScale = new Vector3( centerBall.pixelInset.size.x / Screen.width, centerBall.pixelInset.size.y / Screen.height,0);
		//因为GUI纹理锚点都是左下角0,0 所以为了让两边摇杆对称摇杆坐标向-x方向移动半个纹理宽的屏幕距离(屏幕距离通过 纹理宽:屏幕分辨率宽 获得)
		if (joyStickType == JoyStickType.leftJoyStick) {
			centerBall.transform.position = new Vector3 (centerBall.transform.position.x + centerBall.pixelInset.size.x / 2 / Screen.width,
					     centerBall.transform.position.y + centerBall.pixelInset.size.y / 2 / Screen.height, 0);		

		}
		else if (joyStickType == JoyStickType.rightJoyStick) {
			centerBall.transform.position = new Vector3 (centerBall.transform.position.x - centerBall.pixelInset.size.x * 3/ 2 / Screen.width,
			             centerBall.transform.position.y + centerBall.pixelInset.size.y / 2 / Screen.height, 0);		
		}
		//将摇杆坐标赋给作为摇杆的中心,以后用来复位摇杆用
		centerPos = centerBall.transform.position;
	}
	//每帧运行方法
	void Update ()
	{
		//调用摇杆方法
		JoyStickController();
	}
	//摇杆方法
	void JoyStickController()
	{
		int count = Input.touchCount;    //获取触摸点的数量
		
		for (int i = 0; i < count; i++)    //逐个分析触摸点的操作
		{
			Touch touch = Input.GetTouch(i);    //获取当前处理的触摸点
			//将当前的触摸坐标转换为屏幕坐标
			Vector2 currentTouchPos = new Vector2(touch.position.x/Screen.width - centerBall.pixelInset.size.x/2/Screen.width,
			                                      touch.position.y/Screen.height -centerBall.pixelInset.size.y/2/Screen.height);

			Vector2 temp = currentTouchPos - centerPos;    //得到方向向量temp（触摸的位置和摇杆的坐标差）
			
			if (centerBall.HitTest(touch.position))    //判断是否触摸点在要干范围之内
			{   
				if (temp.magnitude < joyStickRadius)    //如果方向向量temp的长度没有超出摇杆的半径,temp.magnitude为求坐标差的距离,及两点间的距离　　
				{
					lastFingerID = touch.fingerId;    //记录该触摸的id
					centerBallMoving = true;    //摇杆移动开关打开
				}
			}   
			//若中心球移动开关打开，摇杆中心球就会跟随手指移动。但需要加以限制，当手指触摸没有超出摇杆的圆形区域时，中心球完全跟随手指触摸；
			//当手指触摸超出圆形区域时，中心球处于触摸位置和摇杆中心点所形成的方向上并且不能超出半径
			if (touch.fingerId == lastFingerID && centerBallMoving)
			{
				if (temp.magnitude < joyStickRadius)    //如果手指触摸没有超出摇杆的圆形区域，即摇杆半径，摇杆中心球的位置一直跟随手指
				{
					
					centerBall.transform.position = new Vector3 (currentTouchPos.x - centerBall.pixelInset.size.x/2/Screen.width
					                                             ,currentTouchPos.y -centerBall.pixelInset.size.y/2/Screen.height,0);    //设置摇杆的坐标等于触点的坐标
				}
				else    //超出半径
				{
					Vector2 temp2 = temp;    //定义临时变量temp2
					temp2.Normalize();    //将temp2标准化
					//设置摇杆坐标位置不超过半径
					centerBall.transform.position = new Vector3((joyStickRadius * temp2 + centerPos).x, (joyStickRadius * temp2 + centerPos).y, 0);
				}
				
				if (temp.x >= 0)
				{
					//一下为示例代码：控制旋转方向，主要利用Vector2.Angle(temp, new Vector2(0, 5))得到角度并利用
					//initialization_script.current_player_tank_script.BodyRotation(Vector2.Angle(temp, new Vector2(0, 5)));
				}
				if (temp.x< 0)
				{
					//一下为示例代码：控制旋转方向，主要利用Vector2.Angle(temp, new Vector2(0, 5))得到角度并利用
					//initialization_script.current_player_tank_script.BodyRotation(-1 * Vector2.Angle(temp1, new Vector2(0, 5)));
				}
				//控制移动的函数或者控制开火的函数，假设左摇杆控制移动，右摇旋转
				switch(joyStickType)
				{
				case JoyStickType.leftJoyStick:
					position = temp*100;    //移动需坐标
					break;
				case JoyStickType.rightJoyStick:
					position = temp*10;    //旋转需坐标
					break;
				}
				//当释放触摸的时候中心球位置重置
				if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
				{
					centerBall.transform.position = new Vector3(centerPos.x, centerPos.y, 0);    //摇杆复位
					temp = new Vector2(0,0);    //距离差为0
					position = temp;    //赋值0给需要坐标已停止动作
					centerBallMoving = false;    //设置不能移动摇杆
					lastFingerID = -1;    //清楚本次手指触摸id
				}
			}
		}
	}
	//获取传递坐标方法
	public Vector2 getPositions()
	{
		return position;
	}
}