using System.Collections;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public int stage, level;

    [SerializeField] private MovementSystem movementSystem;
    [SerializeField] private PlatformManager platformManager;
    [SerializeField] private CameraController cameraController;

    private void Start() {
        LoadLevel(stage, level);
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.L)){
            LoadLevel(stage, level);
        }
    }

    public void FinishLevel() {
        movementSystem.enabled = false;
        UserData.FinishLevel(stage, level);

        StartCoroutine(wait());

        IEnumerator wait() {
            yield return new WaitForSeconds(1);

            level++;
            LoadLevel(stage, level);
        }
    }

    public void LostLevel() {
        movementSystem.enabled = false;
        StartCoroutine(wait());

        IEnumerator wait() {
            yield return new WaitForSeconds(1);

            LoadLevel(stage, level);
        }
    }

    private void LoadLevel(int stage, int level) {
        platformManager.LoadLevel(stage, level);
        cameraController.LoadCamera(stage, level);

        movementSystem.enabled = true;
    }
}
