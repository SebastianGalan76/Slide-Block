using System.Collections.Generic;
using UnityEngine;
using static PlatformManager;

public class MovementSystem : MonoBehaviour
{
    [SerializeField] private PlatformManager platformManager;
    [SerializeField] private LevelManager levelManager;

    private float startPositionX, deltaX, deltaAbsX;
    private float startPositionY, deltaY, deltaAbsY;
    private float moveSensitivity = 0.15f;

    private int totalBlocks;
    private int movingBlocksCount;

    private void Update() {
        if(movingBlocksCount>0) {
            return;
        }

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

    public void StartNewLevel(int totalBlocks) {
        this.totalBlocks = totalBlocks;
        this.movingBlocksCount = 0;
    }
    public void ChangeTotalBlocksCount(int value) {
        totalBlocks += value;
    }
    public void FinishMovement() {
        movingBlocksCount--;

        if(movingBlocksCount == 0) {
            platformManager.CheckPlatform();
        }
    }

    private void Move(DirectionType direction) {
        movingBlocksCount = totalBlocks;

        switch (direction) {
            case DirectionType.UP:
                MoveUp();
                break;
                case DirectionType.DOWN:
                MoveDown();
                break;
                case DirectionType.LEFT:
                MoveLeft();
                break;
                case DirectionType.RIGHT:
                MoveRight();
                break;
        }
    }

    private void MoveRight() {
        Dictionary<Block, Movement> movement = new Dictionary<Block, Movement>();

        for(int x = 0;x < PLATFORM_SIZE;x++) {
            for(int y = 0;y < PLATFORM_SIZE;y++) {
                Block block = platformManager.movingBlocks[x, y];
                if(block == null) {
                    continue;
                }

                if(block is StoppableBlock stoppableBlock && stoppableBlock.isStopped) {
                    movement.Add(platformManager.movingBlocks[x, y], new Movement(0, x, y));
                    continue;
                }

                int movementAmount = 0;
                int xNew = x + 1;
                while(xNew >= 0 && xNew < PLATFORM_SIZE) {
                    if(platformManager.platform[xNew, y] == FieldType.BORDER || platformManager.platform[xNew, y] == FieldType.NULL) {
                        break;
                    }

                    if(platformManager.movingBlocks[xNew, y] == null) {
                        movementAmount++;
                    }
                    if(platformManager.movingBlocks[xNew, y] is StoppableBlock stoppableBlock2 && stoppableBlock2.isStopped) {
                        break;
                    }

                    xNew++;
                }
                movement.Add(platformManager.movingBlocks[x, y], new Movement(movementAmount, x + movementAmount, y));
            }
        }

        Block[,] newMovingBlocks = new Block[PLATFORM_SIZE, PLATFORM_SIZE];
        foreach(var item in movement) {
            item.Key.Move(DirectionType.RIGHT, item.Value.value);

            newMovingBlocks[item.Value.xNew, item.Value.yNew] = item.Key;
        }
        platformManager.movingBlocks = newMovingBlocks;
    }
    private void MoveLeft() {
        Dictionary<Block, Movement> movement = new Dictionary<Block, Movement>();

        for(int x = 0;x < PLATFORM_SIZE;x++) {
            for(int y = 0;y < PLATFORM_SIZE;y++) {
                Block block = platformManager.movingBlocks[x, y];
                if(block == null) {
                    continue;
                }

                if(block is StoppableBlock stoppableBlock && stoppableBlock.isStopped) {
                    movement.Add(platformManager.movingBlocks[x, y], new Movement(0, x, y));
                    continue;
                }

                int movementAmount = 0;
                int xNew = x - 1;
                while(xNew >= 0 && xNew < PLATFORM_SIZE) {
                    if(platformManager.platform[xNew, y] == FieldType.BORDER || platformManager.platform[xNew, y] == FieldType.NULL) {
                        break;
                    }

                    if(platformManager.movingBlocks[xNew, y] == null) {
                        movementAmount++;
                    }
                    if(platformManager.movingBlocks[xNew, y] is StoppableBlock stoppableBlock2 && stoppableBlock2.isStopped) {
                        break;
                    }

                    xNew--;
                }

                movement.Add(platformManager.movingBlocks[x, y], new Movement(movementAmount, x - movementAmount, y));
            }
        }

        Block[,] newMovingBlocks = new Block[PLATFORM_SIZE, PLATFORM_SIZE];
        foreach(var item in movement) {
            item.Key.Move(DirectionType.LEFT, item.Value.value);

            newMovingBlocks[item.Value.xNew, item.Value.yNew] = item.Key;
        }
        platformManager.movingBlocks = newMovingBlocks;
    }
    private void MoveUp() {
        Dictionary<Block, Movement> movement = new Dictionary<Block, Movement>();

        for(int x = 0;x < PLATFORM_SIZE;x++) {
            for(int y = 0;y < PLATFORM_SIZE;y++) {
                Block block = platformManager.movingBlocks[x, y];
                if(block == null) {
                    continue;
                }

                if(block is StoppableBlock stoppableBlock && stoppableBlock.isStopped) {
                    movement.Add(platformManager.movingBlocks[x, y], new Movement(0, x, y));
                    continue;
                }

                int movementAmount = 0;
                int yNew = y - 1;
                while(yNew >= 0 && yNew < PLATFORM_SIZE) {
                    if(platformManager.platform[x, yNew] == FieldType.BORDER || platformManager.platform[x, yNew] == FieldType.NULL) {
                        break;
                    }

                    if(platformManager.movingBlocks[x, yNew] == null) {
                        movementAmount++;
                    }
                    if(platformManager.movingBlocks[x, yNew] is StoppableBlock stoppableBlock2 && stoppableBlock2.isStopped) {
                        break;
                    }

                    yNew--;
                }

                movement.Add(platformManager.movingBlocks[x, y], new Movement(movementAmount, x, y - movementAmount));
            }
        }

        Block[,] newMovingBlocks = new Block[PLATFORM_SIZE, PLATFORM_SIZE];
        foreach(var item in movement) {
            item.Key.Move(DirectionType.UP, item.Value.value);

            newMovingBlocks[item.Value.xNew, item.Value.yNew] = item.Key;
        }
        platformManager.movingBlocks = newMovingBlocks;
    }
    private void MoveDown() {
        Dictionary<Block, Movement> movement = new Dictionary<Block, Movement>();

        for(int x = 0;x < PLATFORM_SIZE;x++) {
            for(int y = 0;y < PLATFORM_SIZE;y++) {
                Block block = platformManager.movingBlocks[x, y];
                if(block == null) {
                    continue;
                }

                if(block is StoppableBlock stoppableBlock && stoppableBlock.isStopped) {
                    movement.Add(platformManager.movingBlocks[x, y], new Movement(0, x, y));
                    continue;
                }

                int movementAmount = 0;
                int yNew = y + 1;
                while(yNew >= 0 && yNew < PLATFORM_SIZE) {
                    if(platformManager.platform[x, yNew] == FieldType.BORDER || platformManager.platform[x, yNew] == FieldType.NULL) {
                        break;
                    }

                    if(platformManager.movingBlocks[x, yNew] == null) {
                        movementAmount++;
                    }
                    if(platformManager.movingBlocks[x, yNew] is StoppableBlock stoppableBlock2 && stoppableBlock2.isStopped) {
                        break;
                    }

                    yNew++;
                }

                movement.Add(platformManager.movingBlocks[x, y], new Movement(movementAmount, x, y + movementAmount));
            }
        }

        Block[,] newMovingBlocks = new Block[PLATFORM_SIZE, PLATFORM_SIZE];
        foreach(var item in movement) {
            item.Key.Move(DirectionType.DOWN, item.Value.value);

            newMovingBlocks[item.Value.xNew, item.Value.yNew] = item.Key;
        }
        platformManager.movingBlocks = newMovingBlocks;
    }
}
