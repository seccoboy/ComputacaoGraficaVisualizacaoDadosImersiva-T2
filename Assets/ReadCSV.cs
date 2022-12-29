using System.Collections;
using UnityEngine;
using System.Linq; 
using System.Collections.Generic;

public class ReadCSV : MonoBehaviour{

	public float spread;
    public TextAsset csvFile; 
	public GameObject cubePrefab;

	public int eixo1;
	public int eixo2;
	public int eixo3;

	private List<GameObject> cubes = new List<GameObject>();


	public void UpdateCubes(){

		foreach(var cube in cubes) {
			DestroyImmediate(cube);
        }
		Start();
	}
		

	public void Start(){
		string[,] grid = SplitData(csvFile.text);
		PlaceCubes(grid);
	}
	

	public void PlaceCubes(string[,] grid){

		int linSize = grid.GetLength(0);
		int colSize = grid.GetLength(1);

		for(int z = 1; z < colSize -1 ; z++){
			float xCoord = float.Parse(grid[eixo1,z]);
			float yCoord = float.Parse(grid[eixo3,z]);
			float zCoord = float.Parse(grid[eixo2,z]);
        	cubes.Add(Instantiate(cubePrefab = GameObject.Instantiate(cubePrefab, new Vector3(xCoord/spread,yCoord/spread,zCoord/spread),transform.rotation  )));
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
