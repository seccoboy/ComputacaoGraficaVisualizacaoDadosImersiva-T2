using System.Collections;
using UnityEngine;
using System.Linq; 
using System.Collections.Generic;
using UnityEngine.UI;

public class ReadCSV : MonoBehaviour{

	public float spread;
    public TextAsset csvFile; 
	public GameObject cubePrefab;

	public int eixo1;
	public int eixo2;
	public int eixo3;

	private List<GameObject> cubes = new List<GameObject>();
	private List<GameObject> textObjects = new List<GameObject>();



	public void Start(){
		string[,] grid = SplitData(csvFile.text);
		PlaceCubes(grid);
	}

	public void Update(){
		foreach (GameObject textObject in textObjects) {
			RotateTexts(textObject);
		}
	}	

	public void PlaceCubes(string[,] grid){
		
			foreach(var cube in cubes) {
				DestroyImmediate(cube);
    	    }

			int linSize = grid.GetLength(0);
			int colSize = grid.GetLength(1);

			for(int z = 1; z < colSize -1 ; z++){

				float xCoord = float.Parse(grid[eixo1,z]);
				float yCoord = float.Parse(grid[eixo3,z]);
				float zCoord = float.Parse(grid[eixo2,z]);
				cubes.Add(GameObject.Instantiate(cubePrefab, new Vector3(xCoord/spread,yCoord/spread,zCoord/spread),transform.rotation  ));
				
				string textoDocubo = grid[eixo1,0] + ':' + grid[eixo1,z] + '\n' + grid[eixo2,0] + ':' + grid[eixo2,z] + '\n' + grid[eixo3,0] + ':' + grid[eixo3,z] ;

				GameObject textObject = new GameObject("Text");
				textObject.AddComponent<TextMesh>();
				textObject.GetComponent<TextMesh>().text = textoDocubo;
				textObject.transform.SetParent(cubes[cubes.Count-1].transform);
				textObjects.Add(textObject);
				textObject.transform.position = new Vector3(
					cubes[cubes.Count-1].transform.position.x - 2.0f,
					cubes[cubes.Count-1].transform.position.y + 5.0f,
					cubes[cubes.Count-1].transform.position.z);
				RotateTexts(textObject);
			}
		}
	private void RotateTexts (GameObject textObject){
			textObject.transform.LookAt(Camera.main.transform);
			textObject.transform.rotation = Quaternion.LookRotation(textObject.transform.position - Camera.main.transform.position);
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
