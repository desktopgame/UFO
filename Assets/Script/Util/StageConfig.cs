using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text.RegularExpressions;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class StageConfig : MonoBehaviour {
	[SerializeField]
	private GameObject[] tiles;

	[SerializeField, Multiline]
	private string background;

	[SerializeField, Multiline]
	private string foreground;

	[SerializeField]
	private bool interactive = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	#if UNITY_EDITOR
	public void SetForeground(string foreground) {
		var temp = this.foreground;
		this.foreground = foreground;
		if(temp != this.foreground) {
			AssetDatabase.Refresh();
		}
	}

	public string GetForeground() {
		return foreground;
	}

	public void SetBackground(string background) {
		var temp = this.background;
		this.background = background;
		if(temp != this.background) {
			AssetDatabase.Refresh();
		}
	}

	public string GetBackground() {
		return background;
	}

	public GameObject[] GetTiles() {
		return tiles;
	}

	public bool IsInteractive() {
		return interactive;
	}
	#endif
}
#if UNITY_EDITOR
[CustomEditor(typeof(StageConfig))]
public class StageConfigEditor : Editor {
	private StageConfig self;
	private string autoFillBG = "0";
	private int frameWidth;
	private int frameHeight;
	private string replOldBG = "0";
	private string replNewBG = "0";
	private string replOldFG = "0";
	private string replNewFG = "0";
	private string backgroundData = "";
	private string foregroundData = "";
	private bool interactiveActive;
	private int frameCount;
	private static readonly string ROOT_NAME = "AutoGen_Stage";

	void OnEnable () {
		this.self = target as StageConfig;
	}

	public override void OnInspectorGUI () {
		base.OnInspectorGUI ();
		//インタラクティブモードなら定期的に更新
		if(self.IsInteractive()) {
			UpdateInteractive();
		}
		EditorGUILayout.BeginVertical();
		if(GUILayout.Button("Create")) {
			CreateStage();
		}
		CheckTextBox();
		ShowTools();
		EditorGUILayout.EndVertical();
	}

	private void UpdateInteractive() {
		//最後にデータが更新されてから数フレーム経過でステージ更新
		if(frameCount++ >= 20 && interactiveActive) {
			AssetDatabase.SaveAssets();
			CreateStage();
			this.frameCount = 0;
			this.interactiveActive = false;
		}
		//前回のデータと比較
		var sbg = self.GetBackground();
		var sfg = self.GetForeground();
		if(backgroundData != sbg ||
		   foregroundData != sfg) {
			frameCount = 0;
			interactiveActive = true;
		}
		//データ更新
		this.backgroundData = sbg;
		this.foregroundData = sfg;
	}

	private void ShowTools() {
		GUILayout.Label("Tools");
		ShowAutoFillTool();
		ShowFrameTool();
		self.SetBackground(ShowReplaceTool(ref replOldBG, ref replNewBG, "Replace Background", self.GetBackground()));
		self.SetForeground(ShowReplaceTool(ref replOldFG, ref replNewFG, "Replace Foreground", self.GetForeground()));
	}

	private void ShowAutoFillTool() {
		EditorGUILayout.BeginHorizontal();
		this.autoFillBG = EditorGUILayout.TextField(autoFillBG);
		var c = autoFillBG.ToString()[0];
		if(GUILayout.Button("Auto Fill Foreground")) {
			var buff = new System.Text.StringBuilder();
			var bgLines = self.GetBackground().Split('\n');
			for(int i=0; i<bgLines.Length; i++) {
				var values = bgLines[i].Split(',');
				for(int j=0; j<values.Length; j++) {
					buff.Append(c);
					if(j < values.Length - 1) {
						buff.Append(',');
					}
				}
				if(i < bgLines.Length - 1) {
					buff.AppendLine();
				}
			}
			self.SetForeground(buff.ToString());
		}
		EditorGUILayout.EndHorizontal();
	}

	private void ShowFrameTool() {
		EditorGUILayout.BeginHorizontal();
		this.frameWidth = EditorGUILayout.IntField(frameWidth);
		this.frameHeight = EditorGUILayout.IntField(frameHeight);
		if(GUILayout.Button("Create Frame")) {
			var buff = new System.Text.StringBuilder();
			for(int i=0; i<frameHeight; i++) {
				if(i == 0) {
					buff.AppendLine(CreateLine('1', '1', '1', frameWidth));
					continue;
				}
				if(i == (frameHeight - 1)) {
					buff.Append(CreateLine('1', '1', '1', frameWidth));
					continue;
				}
				buff.AppendLine(CreateLine('1', '0', '1', frameWidth));
			}
			self.SetBackground(buff.ToString());
		}
		EditorGUILayout.EndHorizontal();
	}

	private string ShowReplaceTool(ref string oldValue, ref string newValue, string desc, string src) {
		EditorGUILayout.BeginHorizontal();
		oldValue = EditorGUILayout.TextField(oldValue);
		newValue = EditorGUILayout.TextField(newValue);
		if(GUILayout.Button(desc)) {
			var buf = new System.Text.StringBuilder(src);
			src = buf.Replace(oldValue, newValue).ToString();
		}
		EditorGUILayout.EndHorizontal();
		return src;
	}

	private string CreateLine(char start, char body, char end, int len) {
		var buff = new System.Text.StringBuilder();
		len = len - 2;
		buff.Append(start);
		buff.Append(',');
		for(int i=0; i<len; i++) {
			buff.Append(body);
			buff.Append(',');
		}
		buff.Append(end);
		return buff.ToString();
	}

	private void CheckTextBox() {
		//
		//`foreground`, `background`の入力内容を検査して適切なエラーをだす
		//二つのテキストは
		//-同じ行数
		//-全ての行が同じ列数
		//である必要がある。
		//
		var bg = self.GetBackground();
		var fg = self.GetForeground();
		//入力されているか検査
		if(bg.Length == 0 || fg.Length == 0) {
			EditorGUILayout.HelpBox("must be input to `background` and `foreground`.", MessageType.Warning);
			return;
		}
		//行の長さを検査
		var bgLines = bg.Split('\n');
		var fgLines = fg.Split('\n');
		if(bgLines.Length != fgLines.Length) {
			EditorGUILayout.HelpBox("not same line length", MessageType.Error);
			return;
		}
		//入力されている全てのテキストの長さを検査
		CheckColumnLength(bgLines, fgLines);
	}

	private void CheckColumnLength(string[] bgLines, string[] fgLines) {
		int bgMin, bgMax, fgMin, fgMax;
		CheckColumnLength(bgLines, out bgMin, out bgMax);
		CheckColumnLength(fgLines, out fgMin, out fgMax);
		var arr = new int[]{bgMin, bgMax, fgMin, fgMax};
		var arrMin = arr.Min();
		var arrMax = arr.Max();
		if(arrMin != arrMax) {
			EditorGUILayout.HelpBox("not same column length by both", MessageType.Error);
			return;
		}
	}

	private void CheckColumnLength(string[] lines, out int outMin, out int outMax) {
		var minColumn = lines.Select((e) => e.Split(',').Length).Min();
		var maxColumn = lines.Select((e) => e.Split(',').Length).Max();
		outMin = minColumn;
		outMax = maxColumn;
		if(minColumn != maxColumn) {
			EditorGUILayout.HelpBox("not same column length", MessageType.Error);
			return;
		}
	}

	private void CreateStage() {
		var obj = GameObject.Find(ROOT_NAME);
		//既に存在するなら削除してから生成
		if(obj != null) {
			GameObject.DestroyImmediate(obj);
		}
		obj = new GameObject(ROOT_NAME);
		Undo.RegisterCreatedObjectUndo(obj, "Create New GameObject");
		//背景を作成
		CreateCell(self.GetBackground().Split('\n'), 0);
		CreateCell(self.GetForeground().Split('\n'), 5);
	}

	private void CreateCell(string[] lines, int layer) {
		var error = false;
		var size = GetSpriteSize(out error);
		var tiles = self.GetTiles();
		Debug.Log("create lines " + lines.Length);
		if(error) {
			Debug.LogError("not same sprite size");
			return;
		}
		for(int i=0; i<lines.Length; i++) {
			var line = new GameObject("_" + layer + "_Line[" + i + "]");
			line.transform.parent = GameObject.Find(ROOT_NAME).transform;
			Undo.RegisterCreatedObjectUndo(line, "Create New GameObject");
			var values = lines[i].Split(',');
			for(int j=0; j<values.Length; j++) {
				var posX = j * size.x;
				var posY = i * size.y;
				var index = int.Parse(values[j]);
				if(index < 0 || index >= tiles.Length) {
					Debug.LogError("invalid text, value=" + index + " location=" + i + "," + j);
					break;
				}
				var obj = (GameObject)PrefabUtility.InstantiatePrefab(tiles[index]);
				obj.transform.position = new Vector3(posX, posY, 0);
				obj.transform.parent = line.transform;
				obj.GetComponent<SpriteRenderer>().sortingOrder = layer;
				Undo.RegisterCreatedObjectUndo(obj, "Create New GameObject");
			}
		}
	}

	private Vector2 GetSpriteSize(out bool error) {
		var tiles = self.GetTiles();
		var v2 = new Vector2();
		var notInit = true;
		error = false;
		foreach(var tile in tiles) {
			var scale = tile.transform.localScale;
			var sprite = tile.GetComponent<SpriteRenderer>().sprite;
			if(sprite == null) {
				continue;
			}
			var spriteSize = sprite.bounds.size;
			var scaledSize = new Vector2(spriteSize.x * scale.x, spriteSize.y * scale.y);
			if(notInit) {
				v2 = scaledSize;
				notInit = false;
			} else {
				var diffX = Mathf.Abs(v2.x - scaledSize.x);
				var diffY = Mathf.Abs(v2.y - scaledSize.y);
				if(diffX > 0.5f || diffY > 0.5f) {
					error = true;
					break;
				}
			}
		}
		return v2;
	}
}
#endif