using UnityEngine;
using System.Collections;
using UnityEditor;
using System;

public class Colorify_create_mask : MaterialEditor {

	private UnityEngine.Object[] materials;

	private void SaveTextureToFile(Texture2D texture,string folder,string nameAdd,bool jpg)
	{
		byte[] bytes;
		if (jpg)
			bytes = texture.EncodeToJPG();
		else
			bytes = texture.EncodeToPNG();
		System.IO.File.WriteAllBytes(System.IO.Directory.GetCurrentDirectory() + "\\" + folder + nameAdd, bytes );
	}

	public void GenerateMask(bool jpg)
	{
		Texture2D mainTex = (Texture2D)((Material)target).GetTexture("_MainTex");
		Texture2D image = new Texture2D(mainTex.width, mainTex.height,TextureFormat.RGB24,false);

		string path = AssetDatabase.GetAssetPath(mainTex);

		string ext;
		if (jpg)
			ext = ".jpg";
		else
			ext = ".png";

		if (System.IO.File.Exists (System.IO.Directory.GetCurrentDirectory () + "\\" + path + "_mask" + ext)) 
		{
			if (!EditorUtility.DisplayDialog("Overwrite existing mask?",
			                            	 "Recolor mask for that texture already exists. Overwrite existing mask?",
			                                 "Overwrite", "Stop mask creation"))
			{
				return;
			}
		}


		if (UnityEditorInternal.InternalEditorUtility.HasPro())
		{
			RenderTexture tempRT = RenderTexture.GetTemporary(mainTex.width,mainTex.height);
			Material mat = new Material(Shader.Find("Hidden/Colorify_mask_creator"));
			mat.CopyPropertiesFromMaterial((Material)target);
			Graphics.Blit(mainTex,tempRT,mat);
			RenderTexture oldRT = RenderTexture.active;
			RenderTexture.active = tempRT;
			image.ReadPixels(new Rect(0, 0, tempRT.width, tempRT.height), 0, 0);
			RenderTexture.active = oldRT;
			RenderTexture.ReleaseTemporary(tempRT);
		}
		else
		{
			float _Range = ((Material)target).GetFloat("_Range");
			float _HueRange = ((Material)target).GetFloat("_HueRange");
			float _Range2 = ((Material)target).GetFloat("_Range2");
			float _HueRange2 = ((Material)target).GetFloat("_HueRange2");

			Color _Color = ((Material)target).GetColor("_Color");
			Color _PatCol = ((Material)target).GetColor("_PatCol");
			Color _PatCol2 = ((Material)target).GetColor("_PatCol2");


			float hue;
			float targetHue;
			float targetHue2;
			float coef1;
			float hueCoef1;
			float coef2;
			float hueCoef2;
			float brightness;
			Color col;
						
			TextureImporter ti =(TextureImporter)TextureImporter.GetAtPath(path);

			Color[] pixels;

			if (ti.isReadable)
			{
				pixels = mainTex.GetPixels();
			}
			else
			{
				ti.isReadable = true;
				AssetDatabase.ImportAsset(path);
				pixels = mainTex.GetPixels();
				ti.isReadable = false;
				AssetDatabase.ImportAsset(path);
			}



			for (int i = 0; i < pixels.Length; i++)
			{
				col = pixels[i] * _Color;
				hue = Mathf.Atan2(1.73205f * (col.g - col.b), 2 * col.r - col.g - col.b + 0.001f);
				targetHue = Mathf.Atan2(1.73205f * (_PatCol.g - _PatCol.b), 2 * _PatCol.r - _PatCol.g - _PatCol.b + 0.001f);
				targetHue2 = Mathf.Atan2(1.73205f * (_PatCol2.g - _PatCol2.b), 2 * _PatCol2.r - _PatCol2.g - _PatCol2.b + 0.001f);
				
				coef1 = Mathf.Clamp01(1 - ((col.r - _PatCol.r)*(col.r - _PatCol.r) + (col.g - _PatCol.g)*(col.g - _PatCol.g) + (col.b - _PatCol.b)*(col.b - _PatCol.b)) / (_Range * _Range));
				hueCoef1 = Mathf.Clamp01(1 - Mathf.Min(Mathf.Abs(hue-targetHue),6.28319f - Mathf.Abs(hue-targetHue))/(_HueRange * _HueRange));
				coef2 = Mathf.Clamp01(1 - ((col.r - _PatCol2.r)*(col.r - _PatCol2.r) + (col.g - _PatCol2.g)*(col.g - _PatCol2.g) + (col.b - _PatCol2.b)*(col.b - _PatCol2.b)) / (_Range2 * _Range2));
				hueCoef2 = Mathf.Clamp01(1 - Mathf.Min(Mathf.Abs(hue-targetHue2),6.28319f - Mathf.Abs(hue-targetHue2))/(_HueRange2 * _HueRange2));
				
				brightness = col.r * 0.21f + col.g * 0.72f + col.b * 0.07f;
				pixels[i].r = Mathf.Sqrt(coef1 * hueCoef1);
				pixels[i].g = Mathf.Sqrt(coef2 * hueCoef2);
				pixels[i].b = brightness;
				pixels[i].a = 0;
			}
			image.SetPixels(pixels);
			image.Apply();
		}

		SaveTextureToFile(image,path,"_mask"+ext,jpg);
		AssetDatabase.ImportAsset(path+"_mask"+ext, ImportAssetOptions.Default);
		Texture2D maskTex = (Texture2D)(AssetDatabase.LoadMainAssetAtPath(path+"_mask"+ext));
		if (maskTex != null)
		{
			((Material)target).SetTexture("_ColorifyMaskTex",maskTex);
			EditorUtility.DisplayDialog("Mask generation success.","Successfully generated recolor mask.","OK");
		}
		else
		{
			Debug.LogError("Error while generating recolor mask: could not load generated file.");
			EditorUtility.DisplayDialog("Error.","Could not load generated file.","OK");
		}
		GC.Collect();
	}

	public override void Awake ()
	{
		base.Awake();
		materials = new UnityEngine.Object[1];
		materials[0] = serializedObject.targetObject;
	}

	public override void OnInspectorGUI ()
	{
		serializedObject.Update ();
		materials[0] = serializedObject.targetObject;
		var theShader = serializedObject.FindProperty ("m_Shader"); 
		if (isVisible && !theShader.hasMultipleDifferentValues && theShader.objectReferenceValue != null)
		{
			EditorGUI.BeginChangeCheck();

			foreach(MaterialProperty mProp in GetMaterialProperties(materials))
			{
				ShaderProperty(mProp,mProp.displayName);
				GUILayout.Space(4);
				if (mProp.name == "_ColorifyMaskTex")
				{
					if (GUILayout.Button("Generate PNG mask"))
						GenerateMask(false);
					if (GUILayout.Button("Generate JPG mask"))
						GenerateMask(true);
				}
				GUILayout.Space(4);
			}
			
			if (EditorGUI.EndChangeCheck())
				PropertiesChanged ();
		}
	}
}
