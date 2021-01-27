using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ASCII))]
public class SwitchRenderMode : MonoBehaviour {

	private ASCII effect;

	// Use this for initialization
	void Start () {
		effect = GetComponent<ASCII>();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKey("1")){
			effect.colorMode = ASCII.ColorMode.ORIGINAL;
		}
		else if(Input.GetKey("2")){
			effect.colorMode = ASCII.ColorMode.DECIMATED;
		}
		else if(Input.GetKey("3")){
			effect.colorMode = ASCII.ColorMode.ONE_BIT;
		}
		else if(Input.GetKey("4")){
			effect.colorMode = ASCII.ColorMode.OVERLAY_ONLY;
		}
		else if(Input.GetKeyDown("p")){
			effect.pixelate = !effect.pixelate;
		}
	}
}
