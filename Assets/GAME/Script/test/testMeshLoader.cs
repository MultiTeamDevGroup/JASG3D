using System.Collections;
using System.Collections.Generic;
using System.IO;
using MultiCoreLibCSE;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class testMeshLoader : MonoBehaviour {

    void Start() {
        PLYVoxelParser.PLYFile file = PLYVoxelParser.parse(File.ReadAllText(GameLocationUtil.gameSRCLoc + "/test.ply"));
        GetComponent<MeshFilter>().mesh = file.generateMesh();
    }

    
    void Update(){
        
    }
}
