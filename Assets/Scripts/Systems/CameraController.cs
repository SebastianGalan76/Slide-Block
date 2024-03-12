using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private new Camera camera;

    private void Awake() {
        camera = GetComponent<Camera>();  
    }

    public void LoadCamera(int stage, int level) {
        (float size, float posX, float posY) = LevelLoader.LoadCamera(stage, level);
        camera.transform.position = new Vector3(posX, posY, -10);
        camera.orthographicSize = size;
    }
}
