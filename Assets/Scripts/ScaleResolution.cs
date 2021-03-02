using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleResolution : MonoBehaviour
{
	public ASCII ascii;
	public int characterHeight = 20;
	public int characterWidth = 20;
	public int rowMin;
	public int colMin;
	public int rowMax;
	public int colMax;
	private float lastHeight;

	void Awake()
	{
		if (ascii == null)
			ascii = GetComponent<ASCII>();

		lastHeight = Screen.height;
		int rows = Screen.height / characterHeight;
		int cols = Screen.width / characterWidth;
		ascii.rows = (uint)Mathf.Min(Mathf.Max(rows, rowMin), rowMax);
		ascii.columns = (uint)Mathf.Min(Mathf.Max(cols, colMin), colMax);
	}

	// Update is called once per frame
	void Update()
	{
		if (lastHeight != Screen.height)
		{
			lastHeight = Screen.height;
			int rows = Screen.height / characterHeight;
			int cols = Screen.width / characterWidth;
			ascii.rows = (uint)Mathf.Min(Mathf.Max(rows, rowMin), rowMax);
			ascii.columns = (uint)Mathf.Min(Mathf.Max(cols, colMin), colMax);
		}
	}
}
