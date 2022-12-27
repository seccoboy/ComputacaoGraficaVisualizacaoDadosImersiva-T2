using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubePlotter : MonoBehaviour
{
    public CubeMover cubePrefab;
    void Start(){
        CubeMover cube = GameObject.Instantiate(cubePrefab);
        cube.multiplier = 1;
    }
    void Update(){
        
    }
}
