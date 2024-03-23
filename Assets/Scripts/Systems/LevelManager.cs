using System.Collections;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public int stage, level;

    [SerializeField] private MovementSystem movementSystem;
    [SerializeField] private PlatformManager platformManager;
    [SerializeField] private CameraController cameraController;

    private void Start() {
        StartLevel(stage, level);
    }

    public void FinishLevel() {
        movementSystem.enabled = false;
        UserData.FinishLevel(stage, level);

        StartCoroutine(wait());
        IEnumerator wait() {
            yield return new WaitForSeconds(1);

            level++;
            StartLevel(stage, level);
        }
    }

    public void LostLevel() {
        movementSystem.enabled = false;
        StartCoroutine(wait());

        IEnumerator wait() {
            yield return new WaitForSeconds(1);

            StartLevel(stage, level);
        }
    }

    public void PauseLevel() {
        movementSystem.enabled = false;

        Time.timeScale = 0;
    }

    public void ResumeLevel() {
        movementSystem.enabled = true;

        Time.timeScale = 1;
    }

    public void RestartLevel() {
        StartLevel(stage, level);
    }

    private void StartLevel(int stage, int level) {
        platformManager.LoadLevel(stage, level);
        cameraController.LoadCamera(stage, level);

        movementSystem.enabled = true;
    }
}
