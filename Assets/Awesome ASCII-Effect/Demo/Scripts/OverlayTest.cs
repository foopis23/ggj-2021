using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlayTest : MonoBehaviour {
	void Start () {
		ASCII.Instance.PutString("Hello World! 123", Color.white, 0, 0);	
		ASCII.Instance.PutString("Hello World! 123", Color.red, 0, 1);	
		ASCII.Instance.PutString("Hello World! 123", Color.green, 0, 2);
		ASCII.Instance.PutString("Hello World! 123", Color.blue, 0, 3);	
		ASCII.Instance.PutString("Hello World! 123", Color.cyan, 0, 4);
		ASCII.Instance.PutString("Hello World! 123", Color.magenta, 0, 5);
		ASCII.Instance.PutString("Hello World! 123", Color.gray, 0, 6);
		ASCII.Instance.PutString("Hello World! 123", Color.black, Color.white, 0, 7);
		ASCII.Instance.PutString("Hello World! 123", Color.red, Color.clear, 0, 8);
		ASCII.Instance.PutString("Hello World! 123", Color.yellow, 0, 9);

		ASCII.Instance.ClearArea(2, 1, 5, 2);

		ASCII.Instance.DrawBox('#', Color.red, Color.blue, 10, 22, 40, 5);

		ASCII.Instance.PutString("W,A,D - Move,  1,2,3,4,P - Effect", Color.red, Color.blue, 12, 24);
	}
}
