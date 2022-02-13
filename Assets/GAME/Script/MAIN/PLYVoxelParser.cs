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

        public Mesh generateMeshV1() {

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
            
            ret.Optimize();
            
            return ret;
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

                List<int> topTris = new List<int>();
                List<int> bottomTris = new List<int>();
                List<int> leftTris = new List<int>();
                List<int> rightTris = new List<int>();
                List<int> backTris = new List<int>();
                List<int> frontTris = new List<int>();
                
                List<int> triangles_ = new List<int>();
                
                int d = i * 8;
                if (!voxels[i].top.isCovered) {
                    topTris = new List<int> {
                        d + 4, d + 5, d + 6,
                        d + 5, d + 7, d + 6,
                    };
                    triangles_.AddRange(topTris);
                }
                
                if (!voxels[i].bottom.isCovered) {
                    bottomTris = new List<int> {
                        d + 0, d + 2, d + 1,
                        d + 1, d + 2, d + 3,
                    };
                    triangles_.AddRange(bottomTris);
                }
                
                if (!voxels[i].left.isCovered) {
                    leftTris = new List<int> {
                        d + 1, d + 5, d + 4,
                        d + 4, d + 0, d + 1,
                    };
                    triangles_.AddRange(leftTris);
                }
                
                if (!voxels[i].right.isCovered) {
                    rightTris = new List<int> {
                        d + 2, d + 6, d + 7,
                        d + 7, d + 3, d + 2,
                    };
                    triangles_.AddRange(rightTris);
                }
                
                if (!voxels[i].back.isCovered) {
                    backTris = new List<int> {
                        d + 5, d + 1, d + 3,
                        d + 3, d + 7, d + 5
                    };
                    triangles_.AddRange(backTris);
                }
                
                if (!voxels[i].front.isCovered) {
                    frontTris = new List<int> {
                        d + 0, d + 4, d + 2,
                        d + 4, d + 6, d + 2,
                    };
                    triangles_.AddRange(frontTris);
                }

                tris.AddRange(triangles_);
            }

            Mesh ret = new Mesh();
            ret.Clear();
            ret.vertices = vertices_.ToArray();
            ret.triangles = tris.ToArray();
            ret.colors = vertexColor.ToArray();
            
            ret.Optimize();
            
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
        
        //Removing all voxels that has all 6 sides covered by another one, so less faces get turned into mesh == less lag
        
        //TODO make this part of the ply file parsing actually do something

        for (int i = 0; i < voxelArray.Count; i++) {
            Vector3Int thisPos = voxelArray[i].getAsVector();
            for (int j = 0; j < voxelArray.Count; j++) {
                if (voxelArray[j].x == thisPos.x && voxelArray[j].z == thisPos.z) {
                    if (voxelArray[j].y == thisPos.y + 1) {
                        //above
                        voxelArray[i].top.setTrue();
                    }else if (voxelArray[j].y == thisPos.y - 1) {
                        //below
                        voxelArray[i].bottom.setTrue();
                    }
                }
                
                if (voxelArray[j].x == thisPos.x && voxelArray[j].y == thisPos.y) {
                    if (voxelArray[j].z == thisPos.z + 1) {
                        voxelArray[i].back.setTrue();
                    }else if (voxelArray[j].z == thisPos.z - 1) {
                        voxelArray[i].front.setTrue();
                    }
                }
                
                if (voxelArray[j].z == thisPos.z && voxelArray[j].y == thisPos.y) {
                    if (voxelArray[j].x == thisPos.x + 1) {
                        voxelArray[i].right.setTrue();
                    }else if (voxelArray[j].x == thisPos.x - 1) {
                        voxelArray[i].left.setTrue();
                    }
                }
            }
        }

        for (int i = 0; i < voxelArray.Count; i++) {
            if (voxelArray[i].isAllSidesCovered()) {
                voxelArray.Remove(voxelArray[i]);
            }
        }

        return new PLYFile(voxelArray);
    }

    public class Voxel {
        public int x;
        public int y;
        public int z;
        
        public int r;
        public int g;
        public int b;

        public VoxelFace top = new VoxelFace(false, Direction.TOP);
        public VoxelFace bottom = new VoxelFace(false, Direction.BOTTOM);
        public VoxelFace left = new VoxelFace(false, Direction.LEFT);
        public VoxelFace right = new VoxelFace(false, Direction.RIGHT);
        public VoxelFace back = new VoxelFace(false, Direction.BACK);
        public VoxelFace front = new VoxelFace(false, Direction.FRONT);

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

        public Color32 getAsColor() {
            return new Color32((byte)this.r, (byte)this.g, (byte)this.b, 255);
        }

        public bool isAllSidesCovered() {
            return top.isCovered && bottom.isCovered && left.isCovered && right.isCovered && back.isCovered && front.isCovered;
        }

        public enum Direction {
            TOP,
            BOTTOM,
            LEFT,
            RIGHT,
            BACK,
            FRONT
        }

        public class VoxelFace {
            public bool isCovered;
            public Direction direction;

            public VoxelFace(bool isCovered, Direction direction) {
                this.isCovered = isCovered;
                this.direction = direction;
            }

            public VoxelFace setTrue() {
                isCovered = true;
                return this;
            }
            
            public VoxelFace setFalse() {
                isCovered = false;
                return this;
            }
        }
    }

}
