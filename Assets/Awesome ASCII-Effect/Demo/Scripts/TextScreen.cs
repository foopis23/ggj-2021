using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextScreen : MonoBehaviour {
	void Start () {
		ASCII.Instance.DrawBox(' ', Color.black, Color.green, 0, 0, 60, 30);
		ASCII.Instance.ClearArea(5, 2, 50, 26);
		ASCII.Instance.PutString("> System loading... (Done.)", Color.green, 10, 7);
		ASCII.Instance.PutString("> Connecting to silo... (Established.)", Color.green, 10, 8);
		ASCII.Instance.PutString("> 5 rockets are operational", Color.green, 10, 9);
		ASCII.Instance.PutString("> Enter launch key: ****", Color.green, 10, 11);
		ASCII.Instance.DrawBox('+', Color.red, Color.black, 15, 18, 30, 7);
		ASCII.Instance.PutString("ERROR! INVALID LAUNCH KEY!", Color.red, 17, 21);
	}
}
