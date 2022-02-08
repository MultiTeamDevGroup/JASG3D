using System;
using System.Collections;
using System.Collections.Generic;
using MultiCoreLibCSE;
using UnityEngine;

public class PLYVoxelParser {
    
    public class PLYFile {
        public List<Voxel> voxels;
        public Mesh model;
        
        public PLYFile(List<Voxel> voxels) {
            this.voxels = new List<Voxel>();
            this.voxels.AddRange(voxels);
        }

        public Mesh generateMesh() {

            List<Vector3> vertices_ = new List<Vector3>();
            List<Color> vertexColor = new List<Color>();
            List<int> tris = new List<int>();
            for (int i = 0; i < voxels.Count; i++) {
                vertices_.Add(new Vector3(voxels[i].x, voxels[i].y, voxels[i].z)); //0, 0, 0
                vertices_.Add(new Vector3(voxels[i].x, voxels[i].y, voxels[i].z+1)); //0, 0, 1
                vertices_.Add(new Vector3(voxels[i].x+1, voxels[i].y, voxels[i].z)); //1, 0, 0
                vertices_.Add(new Vector3(voxels[i].x+1, voxels[i].y, voxels[i].z+1)); //1, 0, 1
                
                vertices_.Add(new Vector3(voxels[i].x, voxels[i].y+1, voxels[i].z)); //0, 1, 0
                vertices_.Add(new Vector3(voxels[i].x, voxels[i].y+1, voxels[i].z+1)); //0, 1, 1
                vertices_.Add(new Vector3(voxels[i].x+1, voxels[i].y+1, voxels[i].z)); //1, 1, 0
                vertices_.Add(new Vector3(voxels[i].x+1, voxels[i].y+1, voxels[i].z+1)); //1, 1, 1

                Color vertexColor_ = new Color32((byte)voxels[i].r, (byte)voxels[i].g, (byte)voxels[i].b, 255);
                vertexColor.Add(vertexColor_);
                vertexColor.Add(vertexColor_);
                vertexColor.Add(vertexColor_);
                vertexColor.Add(vertexColor_);
                vertexColor.Add(vertexColor_);
                vertexColor.Add(vertexColor_);
                vertexColor.Add(vertexColor_);
                vertexColor.Add(vertexColor_);

                int d = i * 8;
                int[] triangles_ = new int[] {
                    //bottom
                    d + 0, d + 2, d + 1,
                    d + 1, d + 2, d + 3,
                    //top
                    d + 4, d + 5, d + 6,
                    d + 5, d + 7, d + 6,
                    //front
                    d + 0, d + 4, d + 2,
                    d + 4, d + 6, d + 2,
                    //left
                    d + 1, d + 5, d + 4,
                    d + 4, d + 0, d + 1,
                    //right
                    d + 2, d + 6, d + 7,
                    d + 7, d + 3, d + 2,
                    //back
                    d + 5, d + 1, d + 3,
                    d + 3, d + 7, d + 5
                };
                
                tris.AddRange(triangles_);
            }

            Mesh ret = new Mesh();
            ret.Clear();
            ret.vertices = vertices_.ToArray();
            ret.triangles = tris.ToArray();
            ret.colors = vertexColor.ToArray();
            
            return ret;
        }
    }
    
    public static PLYFile parse(string fileContent) {
        
        //Initial loading voxels from text file to PLYFile object
        String[] arr = fileContent.Split(new[] { "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
        bool isFileValid = arr[0] == "ply";
        int voxelArraySize = 0;
        int endHeaderLine = -1;
        List<Voxel> voxelArray = new List<Voxel>();
        for (int i = 0; i < arr.Length; i++) {
            if (arr[i].StartsWith("element vertex")) {
                String[] lineArr = arr[i].Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                if (lineArr.Length >= 3) {
                    voxelArraySize = Int32.Parse(lineArr[2]);
                }
            }else if (arr[i].StartsWith("end_header")) {
                endHeaderLine = i;
            }else if (endHeaderLine != -1 && i > endHeaderLine) {
                String[] vxLn = arr[i].Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                //Debug.Log("x: " + vxLn[0] + " y: " + vxLn[1] + " z: " + vxLn[2] + " r: " + vxLn[3] + " g: " + vxLn[4] + " b: " + vxLn[5]);
                voxelArray.Add(new Voxel(Int32.Parse(vxLn[0]),Int32.Parse(vxLn[1]),Int32.Parse(vxLn[2]),Int32.Parse(vxLn[3]),Int32.Parse(vxLn[4]),Int32.Parse(vxLn[5])));
            }
        }
        PLYFile plyFile = new PLYFile(voxelArray);
        
        
        //Removing all voxels that has all 6 sides covered by another one, so less faces get turned into mesh == less lag
        
        //TODO make this part of the ply file parsing actually do something
        
        return plyFile;
    }

    public class Voxel {
        public int x;
        public int y;
        public int z;
        
        public int r;
        public int g;
        public int b;

        public Voxel(int x, int y, int z, int r, int g, int b) {
            this.x = x;
            this.y = y;
            this.z = z;
            this.r = r;
            this.g = g;
            this.b = b;
        }

        public void print() {
            Debug.Log("x: " + x + " y: " + y + " z: " + z + " r: " + r + " g: " + g + " b: " + b);
        }

        public Vector3Int getAsVector() {
            return new Vector3Int(this.x, this.y, this.z);
        }
    }

}
