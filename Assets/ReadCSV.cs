using System.Collections;
using UnityEngine;
using System.Linq; 
using System.Globalization;

public class ReadCSV : MonoBehaviour{

    public TextAsset csvFile; 

	public void Start(){
		string[,] grid = SplitData(csvFile.text);
		PlaceCubes(grid);
	}
	
	public void PlaceCubes(string[,] grid){

		int linSize = grid.GetLength(0);
		int colSize = grid.GetLength(1);

		for(int z = 1; z < colSize -1 ; z++){

			double porcodio = double.Parse(grid[2,z]);
			Debug.Log( z + " " + porcodio);
		}
	}
	
	static public string[,] SplitData(string csvText){
		string[] lines = csvText.Split("\n"[0]); 
		int width = 0; 
		for (int i = 0; i < lines.Length; i++){
			string[] row = SplitCsvLine( lines[i] ); 
			width = Mathf.Max(width, row.Length); 
		}
		string[,] outputGrid = new string[width + 1, lines.Length + 1]; 
		for (int y = 0; y < lines.Length; y++){
			string[] row = SplitCsvLine( lines[y] ); 
			for (int x = 0; x < row.Length; x++) {
				outputGrid[x,y] = row[x]; 
				outputGrid[x,y] = outputGrid[x,y].Replace("\"\"", "\"");
			}
		}
		return outputGrid; 
	}
 
	static public string[] SplitCsvLine(string line){
		return (from System.Text.RegularExpressions.Match m in System.Text.RegularExpressions.Regex.Matches(line,
		pattern: @"(((?<x>[^,\r\n]+)),?)", 
		System.Text.RegularExpressions.RegexOptions.ExplicitCapture)
		select m.Groups[1].Value).ToArray();
	}
}
