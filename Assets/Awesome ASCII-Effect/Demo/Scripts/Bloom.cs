using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Bloom : MonoBehaviour {
	private Material mat;

	// Use this for initialization
	void Start () {
		mat = new Material(Shader.Find("Hidden/Bloom"));
	}
	
	// Update is called once per frame
	void Update () {
	}

	private void OnRenderImage(RenderTexture src, RenderTexture dest){
		mat.SetTexture("_MainTexFull", src);
		RenderTexture tmp = RenderTexture.GetTemporary(src.width / 4, src.height / 4);
		Graphics.Blit(src, tmp);
		Graphics.Blit(tmp, dest, mat);
		RenderTexture.ReleaseTemporary(tmp);
	}
}
