using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using MultiCoreLibCSE;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    
    [Header("Player Settings")]
    public float MovementSpeed = 5f;
    public Rigidbody rb2D;
    public GameObject graphics;
    
    
    private Vector3 Movement;
    
    public async void Start() {
        var task = await loadPlayerModel();

        graphics.GetComponent<MeshFilter>().mesh = task;
    }

    public async Task<Mesh> loadPlayerModel() {
        return await Task.FromResult(PLYVoxelParser.parse(Resources.Load<TextAsset>(JASGMain.GameId + "/models/entity/player/PlayerModel").text).generateMesh());
    }

    void Update() {
        
        Movement.x = Input.GetAxisRaw("Horizontal");
        Movement.z = Input.GetAxisRaw("Vertical");
        Movement.y = 0;
        
        rb2D.MovePosition(rb2D.position + Movement.normalized * MovementSpeed * Time.deltaTime);
    }
}
