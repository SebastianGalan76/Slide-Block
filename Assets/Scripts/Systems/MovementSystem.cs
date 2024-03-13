using UnityEngine;

public class MovementSystem : MonoBehaviour
{
    [SerializeField] private PlatformManager platform;
    [SerializeField] private LevelManager level;

    private float startPositionX, deltaX, deltaAbsX;
    private float startPositionY, deltaY, deltaAbsY;
    private float moveSensitivity = 0.2f;

    private void Update() {
#if UNITY_EDITOR
        if(Input.GetKeyDown(KeyCode.D)) {
            Move(DirectionType.RIGHT);
        }
        if(Input.GetKeyDown(KeyCode.A)) {
            Move(DirectionType.LEFT);
        }
        if(Input.GetKeyDown(KeyCode.W)) {
            Move(DirectionType.UP);
        }
        if(Input.GetKeyDown(KeyCode.S)) {
            Move(DirectionType.DOWN);
        }
#endif

#if UNITY_ANDROID
        if(Input.touchCount == 1) {
            Touch touch = Input.GetTouch(0);

            switch(touch.phase) {
                case TouchPhase.Began:
                    startPositionX = touch.position.x;
                    startPositionY = touch.position.y;
                    deltaX = 0;
                    deltaY = 0;
                    break;
                case TouchPhase.Moved:
                    deltaX = startPositionX - touch.position.x;
                    deltaY = startPositionY - touch.position.y;

                    deltaX /= Screen.dpi;
                    deltaY /= Screen.dpi;

                    deltaAbsX = Mathf.Abs(deltaX);
                    deltaAbsY = Mathf.Abs(deltaY);

                    if(deltaAbsX > deltaAbsY) {
                        if(deltaX > moveSensitivity && startPositionX != 0) {
                            Move(DirectionType.LEFT);

                            startPositionX = 0;
                            deltaX = 0;
                        }
                        if(deltaX < -moveSensitivity && startPositionX != 0) {
                            Move(DirectionType.RIGHT);

                            startPositionX = 0;
                            deltaX = 0;
                        }
                    }

                    if(deltaAbsY > deltaAbsX) {
                        if(deltaY > moveSensitivity && startPositionY != 0) {
                            Move(DirectionType.DOWN);

                            startPositionY = 0;
                            deltaY = 0;
                        }
                        if(deltaY < -moveSensitivity && startPositionY != 0) {
                            Move(DirectionType.UP);

                            startPositionY = 0;
                            deltaY = 0;
                        }
                    }
                    break;
                case TouchPhase.Ended:
                    startPositionX = 0;
                    startPositionY = 0;
                    deltaX = 0;
                    deltaY = 0;
                    break;
            }
        }
#endif
    }

    private void Move(DirectionType direction) {
        switch (direction) {
            case DirectionType.UP:
                platform.MoveUp();
                break;
                case DirectionType.DOWN:
                platform.MoveDown();
                break;
                case DirectionType.LEFT:
                platform.MoveLeft();
                break;
                case DirectionType.RIGHT:
                platform.MoveRight();
                break;
        }

        if(platform.CheckFinish()) {
            level.FinishLevel();
        }
    }
}
