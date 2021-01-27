using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ASCII : MonoBehaviour {
	public static ASCII Instance { private set; get; }
	public enum ColorMode { ORIGINAL, DECIMATED, ONE_BIT, OVERLAY_ONLY }

	private Material mat;
	private int charWidth, charHeight;
	private char[][] mappedChars;
	private Texture2D charsGradient;

	private bool isOverlayDirty = false;
	private OverlayChar[][] overlay;
	private Texture2D overlayTexture;
	private RenderTexture overlayRT;

	public bool pixelate = true;
	[Range(0, 1)]
	public float tranparency = 0;
	public Color background = Color.black;
	[Tooltip("Only in ONE_BIT mode!")]
	public Color foreground = Color.white;
	[Tooltip("Must be 10 chars or will be truncated/padded.\n(Sort by brightness ascending!)")]	
	public string lookupChars;
	public uint columns = 120, rows = 120;
	public ColorMode colorMode = ColorMode.ORIGINAL;
	[Tooltip("Must be white on black baground")]
	public Texture2D bitmapFont;
	[Tooltip("Must be an exact representation of the chars in the bitmap font textre (including blank lines).")]
	[TextArea]
	public string characterMapping;

	private void OnEnable(){
		Instance = this;
		Setup();
	}

	private void Setup() {
		mat = new Material(Shader.Find("Hidden/ASCII"));

		if(lookupChars == null || lookupChars.Length == 0){
			Debug.LogError("No lookup chars provided");
			enabled = false;
			return;
		}

		while(lookupChars.Length < 10){
			lookupChars += lookupChars.ToCharArray()[lookupChars.Length -1];
		}

		computeCharacterMapping();

		charsGradient = new Texture2D(charWidth * 10, charHeight);
		charsGradient.filterMode = FilterMode.Point;

		for(int i = 0; i < 10; i++){
			char c = lookupChars.ToCharArray()[i];
			Vector2 charPosition = lookupCharPosition(c);
			int startX = (int) charPosition.x * charWidth;
			int startY = (int) charPosition.y * charHeight;

			for(int x = 0; x < charWidth; x++){
				for(int y = 0; y < charHeight; y++){
					Color col = bitmapFont.GetPixel(startX + x, bitmapFont.height - (startY + y));
					charsGradient.SetPixel(i * charWidth + x, charHeight - y, col);
				}
			}
		}
		charsGradient.Apply();	
		mat.SetTexture("_BitmapFont", charsGradient);
		ClearOverlay();
	}

	private struct OverlayChar{
		public char c;
		public Color foreground, background;
		public OverlayChar(char c, Color foreground){
			this.c = c;
			this.foreground = foreground;
			background = ASCII.Instance.background;
		}
		public OverlayChar(char c, Color foreground, Color background){
			this.c = c;
			this.foreground = foreground;
			this.background = background;
		}
	}
	private OverlayChar Whitespace = new OverlayChar(' ', Color.white, new Color(0, 0, 0, 0));

	public void ClearOverlay(){
		overlay = new OverlayChar[rows][];
		for(int row = 0; row < rows; row++){
			overlay[row] = new OverlayChar[columns];
			for(int column = 0; column < columns; column++){
				overlay[row][column] = Whitespace;
			}
		}
		isOverlayDirty = true;
	}

	public void PutChar(char c, Color foreground, int x, int y){		
		PutChar(c, foreground, background, x, y);
	}

	public void PutChar(char c, Color foreground, Color background, int x, int y){
		overlay[y][x] = new OverlayChar(c, foreground, background);
		isOverlayDirty = true;
	}

	public void PutString(string s, Color foreground, int x, int y){
		PutString(s, foreground, background, x, y);
	}

	public void PutString(string s, Color foreground, Color background, int x, int y){
		char[] chars = s.ToCharArray();
		for(int i = 0; x + i < columns && i < chars.Length; i++){
			PutChar(chars[i], foreground, background, x + i, y);
		}
	}

	public void ClearArea(int x, int y, int width , int height){
		for(int i = 0; i + y < rows && i < height; i++){
			for(int j = 0; j + x < columns && j < width; j++){
				overlay[y + i][x + j] = Whitespace;
			}
		}
		isOverlayDirty = true;
	}

	public void DrawBox(char border, Color foreground, Color background, int x, int y, int width, int height){
		OverlayChar cBorder = new OverlayChar(border, foreground, background);
		OverlayChar cClear = new OverlayChar(' ', foreground, background);

		if(x + width > columns || y + height > rows){
			throw new UnityException("Box does not fit screen!");
		}

		for(int row = 0; row < height; row++){
			for(int column = 0; column < width; column++){
				overlay[y + row][x + column] = column == 0 || row == 0 || column == (width -1) || row == (height -1) ? cBorder : cClear;
			}
		}
		isOverlayDirty = true;
	}

	private void RenderOverlay(){
		overlayTexture = new Texture2D((int) (charWidth * columns), (int) (charHeight * rows));
		overlayTexture.filterMode = FilterMode.Point;
		overlayTexture.wrapModeV = TextureWrapMode.Clamp;

		for(int row = 0; row < rows; row++){
			for(int column = 0; column < columns; column++){		
				Vector2 charPosition = lookupCharPosition(overlay[row][column].c);

				for(int x = 0; x < charWidth; x++){
					for(int y = 0; y < charHeight; y++){
						Color bitmapFontColor = bitmapFont.GetPixel((int) charPosition.x * charWidth + x, bitmapFont.height - ((int) charPosition.y * charHeight + y));
						bool isCharPixel = bitmapFontColor.r > 0.5f;
						Color pixelColor;
						if(isCharPixel){
							pixelColor = overlay[row][column].foreground;
						}
						else{
							pixelColor = overlay[row][column].background;
						}
						overlayTexture.SetPixel(column * charWidth + x, overlayTexture.height - (row * charHeight + y), pixelColor);
					}
				}
			}
		}
		overlayTexture.Apply();

		mat.SetTexture("_Overlay", overlayTexture);
	}

	private Vector2 lookupCharPosition(char c){
		for(int row = 0; row < mappedChars.Length; row++){
			for(int column = 0; column < mappedChars[row].Length; column++){
				if(c == mappedChars[row][column]){
					return new Vector2(column, row);
				}
			}
		}
		throw new UnityException("Char \\" + (int) c + " not availiable in bitmapFont or not mapped!");
	}

	private void computeCharacterMapping(){
		string[] lines = characterMapping.Split('\n');
		mappedChars = new char[lines.Length][];
		for(int i = 0; i < lines.Length; i++){
			mappedChars[i] = lines[i].ToCharArray();
		}
		charHeight = bitmapFont.height / mappedChars.Length;
		charWidth = bitmapFont.width / mappedChars[0].Length;
	}

	private void Update(){
		mat.SetInt("colorMode", colorMode == ColorMode.ORIGINAL ? 0 : colorMode == ColorMode.DECIMATED ? 1 : 2);
		mat.SetInt("columns", (int) columns);
		mat.SetInt("rows", (int) rows);
		mat.SetColor("background", background);
		mat.SetColor("foreground", foreground);
		mat.SetFloat("mix", tranparency);
		mat.SetInt("overlayOnly", colorMode == ColorMode.OVERLAY_ONLY ? 1 : 0);
	}

	private void OnRenderImage(RenderTexture src, RenderTexture dest){
		if(isOverlayDirty){
			isOverlayDirty = false;
			RenderOverlay();
		}

		if(!pixelate){
			if(colorMode == ColorMode.OVERLAY_ONLY){
				mat.SetTexture("_MainTexFull", src);
			}
			Graphics.Blit(src, dest, mat);
		}
		else{
			RenderTexture ds = RenderTexture.GetTemporary((int) columns, (int) rows);
			ds.filterMode = FilterMode.Point;
			Graphics.Blit(src, ds);
			RenderTexture tmp = RenderTexture.GetTemporary(src.width, src.height);
			Graphics.Blit(ds, tmp);
			RenderTexture.ReleaseTemporary(ds);
			Graphics.Blit(tmp, dest, mat);
			if(colorMode == ColorMode.OVERLAY_ONLY){
				mat.SetTexture("_MainTexFull", tmp);
			}
			RenderTexture.ReleaseTemporary(tmp);
		}
	}
}
