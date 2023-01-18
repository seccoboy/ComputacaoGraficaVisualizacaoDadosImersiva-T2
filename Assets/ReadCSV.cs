using System.Collections;
using UnityEngine;
using System.Linq; 
using System.Collections.Generic;
using UnityEngine.UI;

public class ReadCSV : MonoBehaviour{

	public float spread;
	float tempSpread;
    public TextAsset csvFile; 
	public GameObject cubePrefab;
	public GameObject axisPrefab;
	private List<GameObject> cubes = new List<GameObject>();
	private List<TextMesh> textMeshes = new List<TextMesh>();
	public int eixo1, eixo2, eixo3;
	int tempEixo1;
	int tempEixo2;
	int tempEixo3;
	string[,] grid;
	GameObject Xaxis, Yaxis, Zaxis;

	public void Start(){
		tempSpread = spread;
		tempEixo1 = eixo1;
    	tempEixo2 = eixo2;
    	tempEixo3 = eixo3;
		grid = SplitData(csvFile.text);
		Xaxis = Instantiate(axisPrefab, new Vector3(0,0,0), Quaternion.Euler(0,0,0));
		Yaxis = Instantiate(axisPrefab, new Vector3(0,0,0), Quaternion.Euler(0,-90,0));
		Zaxis = Instantiate(axisPrefab, new Vector3(0,0,0), Quaternion.Euler(0,0,90));
		PlaceCubes(grid);
		UpdateTexts(grid);
	}

	void UpdateTexts(string[,] grid){
		var textosDoEixoX = Xaxis.GetComponentsInChildren<TextMesh>();
		foreach (var textodoeixo in textosDoEixoX){
			if (textodoeixo.name == "TextoDoEixo"){
				RotateTexts(textodoeixo.transform.gameObject);
				textodoeixo.text = grid[eixo1,0];
			}
		}

		var textosDoEixoY = Yaxis.GetComponentsInChildren<TextMesh>();
		foreach (var textodoeixoY in textosDoEixoY){
			if (textodoeixoY.name == "TextoDoEixo"){
				RotateTexts(textodoeixoY.transform.gameObject);
				textodoeixoY.text = grid[eixo3,0];
			}
		}
		var textosDoEixoZ = Zaxis.GetComponentsInChildren<TextMesh>();
		foreach (var textodoeixoZ in textosDoEixoZ){
			if (textodoeixoZ.name == "TextoDoEixo"){
				RotateTexts(textodoeixoZ.transform.gameObject);
				textodoeixoZ.text = grid[eixo2,0];
			}
		}
	}

	public void Update(){
		ValidateEixos(grid);
		if (tempEixo1 != eixo1 || tempEixo2 != eixo2 || tempEixo3 != eixo3 || tempSpread != spread) {
			tempSpread = spread;
        	tempEixo1 = eixo1;
        	tempEixo2 = eixo2;
        	tempEixo3 = eixo3;
        	PlaceCubes(grid);
			UpdateTexts(grid);
    	}
		foreach (TextMesh textMesh in textMeshes) {
		    if(textMesh != null){
    	   	 	RotateTexts(textMesh.gameObject);
				UpdateTexts(grid);
    		}
		}
	}	

	void ValidateEixos(string[,] grid) {
		int numColunas = grid.GetLength(1);
		eixo1 = Mathf.Clamp(eixo1, 0, numColunas - 1);
		eixo2 = Mathf.Clamp(eixo2, 0, numColunas - 1);
		eixo3 = Mathf.Clamp(eixo3, 0, numColunas - 1);
	}

	public void PlaceCubes(string[,] grid){
		foreach(var cube in cubes)
			DestroyImmediate(cube);
		foreach(var text in textMeshes) 
			DestroyImmediate(text);

		int linSize = grid.GetLength(0);
		int colSize = grid.GetLength(1);

		for(int z = 1; z < colSize -1 ; z++){

			float xCoord = float.Parse(grid[eixo1,z]);
			float yCoord = float.Parse(grid[eixo3,z]);
			float zCoord = float.Parse(grid[eixo2,z]);
	
			GameObject cube = GameObject.Instantiate(cubePrefab, new Vector3(xCoord/spread,yCoord/spread,zCoord/spread),transform.rotation);
			cubes.Add(cube);
		
			string textoDocubo = grid[eixo1,0] + ':' + grid[eixo1,z] + '\n' + grid[eixo2,0] + ':' + grid[eixo2,z] + '\n' + grid[eixo3,0] + ':' + grid[eixo3,z];
			GameObject textObject = cube.transform.Find("Text").gameObject;
			textObject.SetActive(false);
			TextMesh textMesh = textObject.GetComponent<TextMesh>();
		
			textMesh.text = textoDocubo;
			textMeshes.Add(textMesh);
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
			string[] row = SplitCsvLine(lines[i]); 
			width = Mathf.Max(width, row.Length); 
		}

		string[,] outputGrid = new string[width + 1, lines.Length + 1];

		for (int y = 0; y < lines.Length; y++){
			string[] row = SplitCsvLine(lines[y]); 
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
