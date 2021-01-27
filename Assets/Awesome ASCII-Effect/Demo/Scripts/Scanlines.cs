using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Scanlines : MonoBehaviour {

	public int numberOfScanlines = 200;
	[Range(0, 1)]
	public float transparency;
	public Color scanlineColor;

	private Material mat;

	// Use this for initialization
	void Start () {
		mat = new Material(Shader.Find("Hidden/Scanlines"));
	}
	
	// Update is called once per frame
	void Update () {
		mat.SetColor("scanlineColor", scanlineColor);
		mat.SetFloat("transparency", transparency);
		mat.SetInt("numberOfScanlines", numberOfScanlines);
	}

	private void OnRenderImage(RenderTexture src, RenderTexture dest){
		Graphics.Blit(src, dest, mat);
	}
}
