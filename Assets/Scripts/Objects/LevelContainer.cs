using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelContainer : MonoBehaviour
{
    private const float ANIMATION_DURATION = 0.5f;

    [SerializeField] private GameObject cameraPrefab;
    [SerializeField] private Transform platformParent;

    private Vector3 destination;
    private float xPos;

    public void Initialize(int stage, int level) {
        LevelData.CameraElement cameraData = LevelDataManager.GetInstance().LoadCamera(stage, level);
        GameObject camera = Instantiate(cameraPrefab);
        camera.GetComponent<LevelCamera>().LoadCamera(cameraData);
        camera.transform.SetParent(transform, false);

        xPos = cameraData.size + 2;
        platformParent.localPosition = new Vector3(xPos, 0);
    }

    public void ShowLevel() {
        destination = Vector3.zero;
        StartCoroutine(Move());
    }

    public void HideLevel() {
        destination = new Vector3(-xPos, 0, 0);
        StartCoroutine(Move(true));
    }

    public Transform GetPlatformParent() {
        return platformParent;
    }

    private IEnumerator Move(bool hide = false) {
        Vector3 startPosition = platformParent.localPosition;
        float startTime = Time.time;

        while(Time.time - startTime < ANIMATION_DURATION) {
            float progress = (Time.time - startTime) / ANIMATION_DURATION;

            platformParent.localPosition = Vector3.Lerp(startPosition, destination, progress);
            yield return null;
        }

        platformParent.localPosition = destination;

        if(hide) {
            Destroy(gameObject);
        }
    }
}
