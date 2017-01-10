import System.Collections.Generic;


// TextureGUI Class: create a basic class for creating and placing GUI elements
// texture = the texture to display
// offset = pixel offset from top left corner, can be modified for easy positioning

class TextureGUI {
	var texture:Texture; //useful: texture.width, texture.height
	var offset:Vector2; // .x and .y
	private var originalOffset:Vector2; //store the original to correctly reset anchor point
	enum Point { TopLeft, TopRight, BottomLeft, BottomRight, Center} //what part of texture to position around?
	
	var anchorPoint = Point.TopLeft; // Unity default is from top left corner of texture
		
	function setAnchor() { // meant to be run ONCE at Start.
		originalOffset = offset;
		if (texture) { // check for null texture
			switch(anchorPoint) { //depending on where we want to center our offsets
				case anchorPoint.TopLeft: // Unity default, do nothing
					break;
				case anchorPoint.TopRight: // Take the offset and go to the top right corner
					offset.x = originalOffset.x - texture.width;
					break;
					
				case anchorPoint.BottomLeft: // bottom left corner of texture
					offset.y = originalOffset.y - texture.height;
					break;
					
				case anchorPoint.BottomRight: //bottom right corner of texture
					offset.x = originalOffset.x - texture.width;
					offset.y = originalOffset.y - texture.height;
					break;
					
				case anchorPoint.Center: //and the center of the texture (useful for screen center textures)
					offset.x = originalOffset.x - texture.width/2;
					offset.y = originalOffset.y - texture.height/2;
					break;
			}
		}
	}	
}

//Timer Class:
	
	
class TimerGUI extends TextureGUI { // Extend functionality from TextureGUI for a depreciating timer graphic
	var textureLEnd:Texture; // left side of full texture (non stretching part)
	var offsetLEnd:Vector2; // left side of full texture (non stretching part) start position
	var textureCenter:Texture; // center of timer (will be stretched across width)
	var offsetCenter:Vector2; 
	var textureREnd:Texture;
	var offsetREnd:Vector2;
	var timerPerct:float = 1; // percentage (0 to 1) this stretches the center
	var desiredWidth:float = 403; // max width of the timer in pixels
	
	function setTime(newTime:float) {
		timerPerct = newTime; // sets the percent based on value
	}
}


// SwitchGUI Class: Extends the TextureGUI to be able to load in multiple textures and switch between them
class SwitchGUI extends TextureGUI {
	var switchableTextures = new List.<Texture>();
	var currentTexture:int = 0;
	function Start() {
		if (switchableTextures.Count > 0) {
			texture = switchableTextures[currentTexture];
		}
	}
	function changeTexture(switchTo:int) {
		if (switchTo < switchableTextures.Count && switchTo >= 0) {
			texture = switchableTextures[switchTo];
			currentTexture = switchTo;
		} else {
			//Debug.Log( this + ": tried to call invalid part of switchTextures array!");
		}
	}
	
	function up() {
		if ((currentTexture+1) < switchableTextures.Count) {
			++currentTexture;
			texture = switchableTextures[currentTexture];
		} else {
			//Debug.Log( this + ": at the top!");
		}
	}
	
	function nextTexture() {
		if ((currentTexture+1) < switchableTextures.Count) { // if we are at the end of the array
			++currentTexture;
			texture = switchableTextures[currentTexture];
		} else {// loop to the beginning
			currentTexture = 0;
			texture = switchableTextures[currentTexture];
		}
	}
	
	function down() {
		if ((currentTexture-1) >= 0) {
			--currentTexture;
			texture = switchableTextures[currentTexture];
		} else {
			//Debug.Log( this + ": at the bottom!");
		}
	}

}

// Location class: 


class Location {
	enum Point { TopLeft, TopRight, BottomLeft, BottomRight, Center}
	
	var pointLocation = Point.TopLeft;
	var offset:Vector2;
	
		
	function updateLocation() {
		switch(pointLocation) {
			case pointLocation.TopLeft:
				offset = Vector2(0,0);
				break;
			case pointLocation.TopRight:
				offset = Vector2(Screen.width,0);
				break;
				
			case pointLocation.BottomLeft:
				offset = Vector2(0,Screen.height);
				break;
				
			case pointLocation.BottomRight:
				offset = Vector2(Screen.width,Screen.height);
				break;
				
			case pointLocation.Center:
				offset = Vector2(Screen.width/2,Screen.height/2);
				break;
		}		
	}
}

class TextureAnchor {
	enum Point { TopLeft, TopRight, BottomLeft, BottomRight, Center}
	
	var anchorPoint = Point.TopLeft;
	var offset:Vector2;
	
	function update() {
		switch(anchorPoint) {
			case anchorPoint.TopLeft:
				offset = Vector2(0,0);
				break;
			case anchorPoint.TopRight:
				offset = Vector2(Screen.width,0);
				break;
				
			case anchorPoint.BottomLeft:
				offset = Vector2(0,Screen.height);
				break;
				
			case anchorPoint.BottomRight:
				offset = Vector2(Screen.width,Screen.height);
				break;
				
			case anchorPoint.Center:
				offset = Vector2(Screen.width/2,Screen.height/2);
				break;
		}		
	}
}