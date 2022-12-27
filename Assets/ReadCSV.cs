using System.Collections;
using UnityEngine;
using System.Linq; 

public class ReadCSV : MonoBehaviour{
    public TextAsset csvFile; 
	public void Start(){
		string[,] grid = SplitData(csvFile.text);
	}
	
	static public string[,] SplitData(string csvText){
		string[] lines = csvText.Split("\n"[0]); 
		int width = 0; 
		string debugaa = "";
		for (int i = 0; i < lines.Length; i++){
			string[] row = SplitCsvLine( lines[i] ); 
			width = Mathf.Max(width, row.Length); 
		}
		string[,] outputGrid = new string[width + 1, lines.Length + 1]; 
		for (int y = 0; y < lines.Length; y++){
			string[] row = SplitCsvLine( lines[y] ); 
			debugaa += outputGrid[0,y] + " ";
			for (int x = 0; x < row.Length; x++) {
				outputGrid[x,y] = row[x]; 
				outputGrid[x,y] = outputGrid[x,y].Replace("\"\"", "\"");
			}
		}
		Debug.Log(debugaa);
		return outputGrid; 
	}
 
	static public string[] SplitCsvLine(string line){
		return (from System.Text.RegularExpressions.Match m in System.Text.RegularExpressions.Regex.Matches(line,
		pattern: @"(((?<x>[^,\r\n]+)),?)", 
		System.Text.RegularExpressions.RegexOptions.ExplicitCapture)
		select m.Groups[1].Value).ToArray();
	}
}
