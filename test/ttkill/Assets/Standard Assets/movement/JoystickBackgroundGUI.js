#pragma strict

@script RequireComponent(Joystick)
@script ExecuteInEditMode ()


var background = new SwitchGUI();
var location = new Location();
var screenRate : float  = 1;
var rate : float = 1.0f;
private var GUIalpha:float = 1;


private var joystick : Joystick;
joystick = GetComponent (Joystick);

var noGuiStyle : GUIStyle;

function Start()
{
	screenRate = Screen.width * rate /  1280;
//	screenRate *= rate;
}

function Update() {
	if (joystick.IsFingerDown()) {
		GUIalpha = 1f;
		background.up();
	} else {
		background.down();
		if (Input.touchCount > 0)
		{
			var count = Input.touchCount;
			for(var i : int= 0; i < count; i++)
			{
				if (Input.GetTouch(i).position.x > 120 
				&& Input.GetTouch(i).position.x < Screen.width / 3f 
				&& Input.GetTouch(i).position.y < Screen.height / 3f
				&& Input.GetTouch(i).position.y > 80)
				{
					GUIalpha = 1f;
					var touch : Touch = Input.GetTouch(i);
//					if (Input.GetMouseButtonDown(0))
					{
//						background.offset.x = touch.position.x;
//						background.offset.y = -touch.position.y;
						joystick.AdjustPosition(touch.position.x-48, touch.position.y-50);
					}
				}
			}
			
		}
		else
		{
			GUIalpha = 1;
		}
	}
	if (background.texture != null){
		location.updateLocation();
	}
}

function OnGUI () {
	GUI.color.a = GUIalpha;
	GUI.Box(Rect(location.offset.x + background.offset.x - background.texture.width/2,location.offset.y + background.offset.y - background.texture.height/2,background.texture.width * screenRate,background.texture.height * screenRate),background.texture,noGuiStyle);
}