using UnityEngine;

public class MovementSystem : MonoBehaviour
{
    [SerializeField] private PlatformManager platform;
    [SerializeField] private LevelManager level;

    private void Update() {
        if(Input.GetKeyDown(KeyCode.D)) {
            Move(DirectionEnum.RIGHT);
        }
        if(Input.GetKeyDown(KeyCode.A)) {
            Move(DirectionEnum.LEFT);
        }
        if(Input.GetKeyDown(KeyCode.W)) {
            Move(DirectionEnum.UP);
        }
        if(Input.GetKeyDown(KeyCode.S)) {
            Move(DirectionEnum.DOWN);
        }
    }

    private void Move(DirectionEnum direction) {
        switch (direction) {
            case DirectionEnum.UP:
                platform.MoveUp();
                break;
                case DirectionEnum.DOWN:
                platform.MoveDown();
                break;
                case DirectionEnum.LEFT:
                platform.MoveLeft();
                break;
                case DirectionEnum.RIGHT:
                platform.MoveRight();
                break;
        }

        if(platform.CheckFinish()) {
            level.FinishLevel();
        }
    }

    private enum DirectionEnum {
        UP, DOWN, LEFT, RIGHT
    }
}
