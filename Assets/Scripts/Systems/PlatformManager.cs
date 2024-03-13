using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

using static LevelLoader;

public class PlatformManager : MonoBehaviour
{
    private const int PLATFORM_SIZE = 14;

    [Header("Prefabs")]
    [SerializeField] private GameObject platformPrefab;
    [SerializeField] private GameObject movingBlockPrefab, destinationPlacePrefab;
    [SerializeField] private GameObject stoppableBlockPrefab;

    [SerializeField] private ColorBlock[] colorBlocks;

    private FieldType[,] platform;
    private Block[,] movingBlocks;

    private Dictionary<int, int> colorBlockType;
    private BorderManager borderManager;

    private void Awake() {
        borderManager = GetComponent<BorderManager>();
    }

    public void LoadLevel(int stage, int level) {
        foreach(Transform child in transform) {
            Destroy(child.gameObject);
        }

        platform = LoadPlatform(stage, level);
        borderManager.GenerateBorder(platform, PLATFORM_SIZE);

        for(int x = 0;x < PLATFORM_SIZE;x++) {
            for(int y = 0;y < PLATFORM_SIZE;y++) {
                if(platform[x, y] != FieldType.PLATFORM) {
                    continue;
                }

                GameObject platformObj = Instantiate(platformPrefab, new Vector3(x, -y, 1), Quaternion.identity);
                platformObj.name = "Platform (" + x + ", " + y + ")";
                platformObj.transform.SetParent(transform, true);
            }
        }

        //Load moving blocks
        colorBlockType = new Dictionary<int, int>();

        movingBlocks = new Block[PLATFORM_SIZE, PLATFORM_SIZE];
        Dictionary<int, List<BlockValues>> movingBlocksDictionary = LoadMovingBlocks(stage, level);
        foreach(int type in movingBlocksDictionary.Keys) {
            List<BlockValues> blockValues = movingBlocksDictionary[type];
            
            int blockIndex = 0;
            foreach(BlockValues blockValue in blockValues) {
                ColorBlock colorBlock = GetRandomColor(type);

                GameObject blockObj = Instantiate(movingBlockPrefab, new Vector3(blockValue.posX, blockValue.posY, 1), Quaternion.identity);
                blockObj.GetComponent<SpriteRenderer>().sprite = colorBlock.movingBlockSprite;
                blockObj.transform.SetParent(transform, true);

                int x = blockValue.positionPlatform % PLATFORM_SIZE;
                int y = blockValue.positionPlatform / PLATFORM_SIZE;
                blockObj.name = "MovingBlock#" + type + " (" + blockIndex + ") - ("+x+", "+y+")";
                
                Block block = blockObj.GetComponent<Block>();
                block.Initialize(colorBlock.color, x, y);
                movingBlocks[x, y] = block;
                
                blockIndex++;
            }
        }

        //Load destination places
        Dictionary<int, List<BlockValues>> destinationPlacesDictionary = LoadDestinationPlaces(stage, level);
        foreach(int type in destinationPlacesDictionary.Keys) {
            List<BlockValues> placeValues = destinationPlacesDictionary[type];

            int placeIndex = 0;
            foreach(BlockValues blockValue in placeValues) {
                ColorBlock colorBlock = GetRandomColor(type);

                GameObject placeObj = Instantiate(destinationPlacePrefab, new Vector3(blockValue.posX, blockValue.posY, 1), Quaternion.identity);
                placeObj.GetComponent<SpriteRenderer>().sprite = colorBlock.destinationPlaceSprite;
                placeObj.transform.SetParent(transform, true);

                int x = blockValue.positionPlatform % PLATFORM_SIZE;
                int y = blockValue.positionPlatform / PLATFORM_SIZE;
                platform[x, y] = colorBlock.color;
                placeObj.name = "DestinationPlace#" + type + " (" + placeIndex + ") - (" + x + ", " + y + ")";

                placeIndex++;
            }
        }

        //Load stoppable blocks
        List<BlockValues> stoppableBlocks = LoadStoppableBlocks(stage, level);
        int stoppableBlockIndex = 0;
        foreach(BlockValues blockValues in stoppableBlocks) {
            GameObject blockObj = Instantiate(stoppableBlockPrefab, new Vector3(blockValues.posX, blockValues.posY, 1), Quaternion.identity);
            blockObj.transform.SetParent(transform, true);

            int x = blockValues.positionPlatform % PLATFORM_SIZE;
            int y = blockValues.positionPlatform / PLATFORM_SIZE;
            blockObj.name = "StoppableBlock (" + stoppableBlockIndex + ") - (" + x + ", " + y + ")";

            StoppableBlock block = blockObj.GetComponent<StoppableBlock>();
            block.Initialize(FieldType.STOPPABLE, x, y);
            movingBlocks[x, y] = block;

            stoppableBlockIndex++;
        }
    }

    public void MoveRight() {
        Dictionary<Block, Movement> movement = new Dictionary<Block, Movement>();

        for(int x = 0;x < PLATFORM_SIZE;x++) {
            for(int y = 0;y < PLATFORM_SIZE;y++) {
                Block block = movingBlocks[x, y];
                if(block == null) {
                    continue;
                }

                if(block is StoppableBlock stoppableBlock && stoppableBlock.isStopped) {
                    movement.Add(movingBlocks[x, y], new Movement(0, x, y));
                    continue;
                }

                int movementAmount = 0;
                int xNew = x + 1;
                while(xNew >= 0 && xNew < PLATFORM_SIZE) {
                    if(platform[xNew, y] == FieldType.BORDER || platform[xNew, y] == FieldType.NULL) {
                        break;
                    }

                    if(movingBlocks[xNew, y] == null) {
                        movementAmount++;
                    }
                    if(movingBlocks[xNew, y] is StoppableBlock stoppableBlock2 && stoppableBlock2.isStopped) {
                        break;
                    }

                    xNew++;
                }
                movement.Add(movingBlocks[x, y], new Movement(movementAmount, x + movementAmount, y));
            }
        }

        Block[,] newMovingBlocks = new Block[PLATFORM_SIZE, PLATFORM_SIZE];
        foreach (var item in movement)
        {
            item.Key.Move(DirectionType.RIGHT, item.Value.value);

            newMovingBlocks[item.Value.xNew, item.Value.yNew] = item.Key;
        }
        movingBlocks = newMovingBlocks;
    }
    public void MoveLeft() {
        Dictionary<Block, Movement> movement = new Dictionary<Block, Movement>();

        for(int x = 0;x < PLATFORM_SIZE;x++) {
            for(int y = 0;y < PLATFORM_SIZE;y++) {
                Block block = movingBlocks[x, y];
                if(block == null) {
                    continue;
                }

                if(block is StoppableBlock stoppableBlock && stoppableBlock.isStopped) {
                    movement.Add(movingBlocks[x, y], new Movement(0, x, y));
                    continue;
                }

                int movementAmount = 0;
                int xNew = x - 1;
                while(xNew >= 0 && xNew < PLATFORM_SIZE) {
                    if(platform[xNew, y] == FieldType.BORDER || platform[xNew, y] == FieldType.NULL) {
                        break;
                    }

                    if(movingBlocks[xNew, y] == null) {
                        movementAmount++;
                    }
                    if(movingBlocks[xNew, y] is StoppableBlock stoppableBlock2 && stoppableBlock2.isStopped) {
                        break;
                    }

                    xNew--;
                }

                movement.Add(movingBlocks[x, y], new Movement(movementAmount, x - movementAmount, y));
            }
        }

        Block[,] newMovingBlocks = new Block[PLATFORM_SIZE, PLATFORM_SIZE];
        foreach(var item in movement) {
            item.Key.Move(DirectionType.LEFT, item.Value.value);

            newMovingBlocks[item.Value.xNew, item.Value.yNew] = item.Key;
        }
        movingBlocks = newMovingBlocks;
    }
    public void MoveUp() {
        Dictionary<Block, Movement> movement = new Dictionary<Block, Movement>();

        for(int x = 0;x < PLATFORM_SIZE;x++) {
            for(int y = 0;y < PLATFORM_SIZE;y++) {
                Block block = movingBlocks[x, y];
                if(block == null) {
                    continue;
                }

                if(block is StoppableBlock stoppableBlock && stoppableBlock.isStopped) {
                    movement.Add(movingBlocks[x, y], new Movement(0, x, y));
                    continue;
                }

                int movementAmount = 0;
                int yNew = y - 1;
                while(yNew >= 0 && yNew < PLATFORM_SIZE) {
                    if(platform[x, yNew] == FieldType.BORDER || platform[x, yNew] == FieldType.NULL) {
                        break;
                    }

                    if(movingBlocks[x, yNew] == null) {
                        movementAmount++;
                    }
                    if(movingBlocks[x, yNew] is StoppableBlock stoppableBlock2 && stoppableBlock2.isStopped) {
                        break;
                    }

                    yNew--;
                }

                movement.Add(movingBlocks[x, y], new Movement(movementAmount, x, y - movementAmount));
            }
        }

        Block[,] newMovingBlocks = new Block[PLATFORM_SIZE, PLATFORM_SIZE];
        foreach(var item in movement) {
            item.Key.Move(DirectionType.UP, item.Value.value);

            newMovingBlocks[item.Value.xNew, item.Value.yNew] = item.Key;
        }
        movingBlocks = newMovingBlocks;
    }
    public void MoveDown() {
        Dictionary<Block, Movement> movement = new Dictionary<Block, Movement>();

        for(int x = 0;x < PLATFORM_SIZE;x++) {
            for(int y = 0;y < PLATFORM_SIZE;y++) {
                Block block = movingBlocks[x, y];
                if(block == null) {
                    continue;
                }

                if(block is StoppableBlock stoppableBlock && stoppableBlock.isStopped) {
                    movement.Add(movingBlocks[x, y], new Movement(0, x, y));
                    continue;
                }

                int movementAmount = 0;
                int yNew = y + 1;
                while(yNew >= 0 && yNew < PLATFORM_SIZE) {
                    if(platform[x, yNew] == FieldType.BORDER || platform[x, yNew] == FieldType.NULL) {
                        break;
                    }

                    if(movingBlocks[x, yNew] == null) {
                        movementAmount++;
                    }
                    if(movingBlocks[x, yNew] is StoppableBlock stoppableBlock2 && stoppableBlock2.isStopped) {
                        break;
                    }

                    yNew++;
                }

                movement.Add(movingBlocks[x, y], new Movement(movementAmount, x, y + movementAmount));
            }
        }

        Block[,] newMovingBlocks = new Block[PLATFORM_SIZE, PLATFORM_SIZE];
        foreach(var item in movement) {
            item.Key.Move(DirectionType.DOWN, item.Value.value);

            newMovingBlocks[item.Value.xNew, item.Value.yNew] = item.Key;
        }
        movingBlocks = newMovingBlocks;
    }

    public bool CheckFinish() {
        for(int x = 0;x < PLATFORM_SIZE;x++) {
            for(int y = 0;y < PLATFORM_SIZE;y++) {
                Block block = movingBlocks[x, y];
                if(block == null) {
                    continue;
                }
                if(block is StoppableBlock) {
                    continue;
                }

                if(block.GetBlockType() != platform[x, y]) {
                    return false;
                }

            }
        }

        return true;
    }

    private ColorBlock GetRandomColor(int type) {
        if(colorBlockType.ContainsKey(type)) {
            return colorBlocks[colorBlockType[type]];
        }

        int randomIndex;
        do {
            randomIndex = Random.Range(0, colorBlocks.Length);
        }while(colorBlockType.ContainsValue(randomIndex));

        colorBlockType.Add(type, randomIndex);
        return colorBlocks[randomIndex];
    }

    [System.Serializable]
    public struct ColorBlock {
        public FieldType color;
        public Sprite movingBlockSprite;
        public Sprite destinationPlaceSprite;
    }
    public struct Movement {
        public int value;
        public int xNew;
        public int yNew;

        public Movement(int value, int xNew, int yNew) {
            this.value = value;
            this.xNew = xNew;
            this.yNew = yNew;
        }
    }
}
