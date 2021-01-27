using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonPlayerController : MonoBehaviour {
	public float stepLength = 1;

	// Update is called once per frame
	private void Update () {
		if(Input.GetKeyDown("w")){
			Walk();
		}
		else if(Input.GetKeyDown("a")){
			TurnLeft();
		}
		else if(Input.GetKeyDown("d")){
			TurnRight();
		}
		else if(Input.touchCount > 0){
			Touch touch = Input.GetTouch(0);
			if(touch.phase != TouchPhase.Began){
				return;
			}

			if(touch.position.x < Screen.width / 3){
				TurnLeft();
			}
			else if(touch.position.x > Screen.width / 3 * 2){
				TurnRight();
			}
			else{
				Walk();
			}
		}
	}

	private void Walk(){
		if(Physics.Raycast(transform.position, transform.forward, stepLength * 1.1f)){
			return;
		}
		Vector3 startPosition = transform.position;
		transform.position = startPosition + transform.forward * stepLength;
	}

	private void TurnLeft(){
		float startRotation = transform.eulerAngles.y;
		transform.eulerAngles = new Vector3(transform.eulerAngles.x, startRotation - 90, transform.eulerAngles.z);
	}

	private void TurnRight(){
		float startRotation = transform.eulerAngles.y;
		transform.eulerAngles = new Vector3(transform.eulerAngles.x, startRotation + 90, transform.eulerAngles.z);
	}
}
