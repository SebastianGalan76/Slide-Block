using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public int stage, level;

    [SerializeField] private PlatformManager platformManager;
    [SerializeField] private CameraController cameraController;

    private void Start() {
        LoadLevel(stage, level);
    }

    private void LoadLevel(int stage, int level) {
        platformManager.LoadLevel(stage, level);
        cameraController.LoadCamera(stage, level);
    }
}
