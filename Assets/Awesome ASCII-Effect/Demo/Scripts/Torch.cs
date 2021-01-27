using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Light))]
public class Torch : MonoBehaviour {

	private Light source;

	public float minIntensity, maxIntensity, flickerSpeed;

	// Use this for initialization
	void Start () {
		source = GetComponent<Light>();
	}
	
	// Update is called once per frame
	void Update () {
		float time = Time.time;
	#if UNITY_EDITOR
			time = (float) UnityEditor.EditorApplication.timeSinceStartup;
	#endif
		source.intensity = Mathf.Lerp(minIntensity, maxIntensity, (Mathf.Sin(time * flickerSpeed) + 1) / 2);
	}
}
