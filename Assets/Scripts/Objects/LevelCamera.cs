using UnityEngine;

public class LevelCamera : MonoBehaviour
{
    private new Camera camera;

    private void Awake() {
        camera = GetComponent<Camera>();  
    }

    public void LoadCamera(float size, float posX, float posY) {
        camera.transform.position = new Vector3(posX, posY, -10);
        camera.orthographicSize = size;

        // set the desired aspect ratio, I set it to fit every screen 
        float targetAspect = 1080f / 1920f;

        // determine the game window's current aspect ratio
        float windowAspect = (float)Screen.width / (float)Screen.height;

        // current viewport height should be scaled by this amount
        float scaleHeight = windowAspect / targetAspect;

        // if scaled height is less than current height, add letterbox
        if(scaleHeight < 1.0f) {
            Rect rect = camera.rect;

            rect.width = 1.0f;
            rect.height = scaleHeight;
            rect.x = 0;
            rect.y = (1.0f - scaleHeight) / 2.0f;

            camera.rect = rect;
        } else {
            float scalewidth = 1.0f / scaleHeight;

            Rect rect = camera.rect;

            rect.width = scalewidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scalewidth) / 2.0f;
            rect.y = 0;

            camera.rect = rect;
        }
    }
}
