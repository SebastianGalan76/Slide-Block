using System.Collections.Generic;
using UnityEngine;
using static LevelLoader;

public class PlatformManager : MonoBehaviour
{
    private const int PLATFORM_SIZE = 14;

    [SerializeField] private GameObject platformPrefab;
    [SerializeField] private GameObject movingBlockPrefab, destinationPlacePrefab;

    private FieldType[,] platform;
    private Block[,] movingBlocks;

    public ColorBlock[] colorBlocks;

    private void Awake() {
        LoadNewLevel(1, 7);
    }

    public void LoadNewLevel(int stage, int level) {
        platform = LoadPlatform(stage, level);

        for(int x = 0;x < PLATFORM_SIZE;x++) {
            for(int y = 0;y < PLATFORM_SIZE;y++) {
                if(platform[x, y] == 0) {
                    continue;
                }

                GameObject platformObj = Instantiate(platformPrefab, new Vector3(x, -y, 1), Quaternion.identity);
                platformObj.name = "Platform (" + x + ", " + y + ")";
                platformObj.transform.SetParent(transform, true);
            }
        }

        //Load moving blocks
        movingBlocks = new Block[PLATFORM_SIZE, PLATFORM_SIZE];
        Dictionary<int, List<BlockValues>> movingBlocksDictionary = LoadMovingBlocks(stage, level);
        foreach(int type in movingBlocksDictionary.Keys) {
            List<BlockValues> blockValues = movingBlocksDictionary[type];
            
            int blockIndex = 0;
            foreach(BlockValues blockValue in blockValues) {
                GameObject blockObj = Instantiate(movingBlockPrefab, new Vector3(blockValue.posX, blockValue.posY, 1), Quaternion.identity);
                blockObj.GetComponent<SpriteRenderer>().sprite = colorBlocks[type].movingBlockSprite;
                blockObj.transform.SetParent(transform, true);

                int x = blockValue.positionPlatform % PLATFORM_SIZE;
                int y = blockValue.positionPlatform / PLATFORM_SIZE;
                Block block = blockObj.GetComponent<Block>();
                blockObj.name = "MovingBlock#" + type + " (" + blockIndex + ") - ("+x+", "+y+")";

                movingBlocks[x, y] = block;
                block.Initialize(colorBlocks[type].color, x, y);
                blockIndex++;
            }
        }

        //Load destination places
        Dictionary<int, List<BlockValues>> destinationPlacesDictionary = LoadDestinationPlaces(stage, level);
        foreach(int type in destinationPlacesDictionary.Keys) {
            List<BlockValues> placeValues = destinationPlacesDictionary[type];

            int placeIndex = 0;
            foreach(BlockValues blockValue in placeValues) {
                GameObject placeObj = Instantiate(destinationPlacePrefab, new Vector3(blockValue.posX, blockValue.posY, 1), Quaternion.identity);
                placeObj.GetComponent<SpriteRenderer>().sprite = colorBlocks[type].destinationPlaceSprite;
                placeObj.transform.SetParent(transform, true);

                int x = blockValue.positionPlatform % PLATFORM_SIZE;
                int y = blockValue.positionPlatform / PLATFORM_SIZE;
                platform[x, y] = colorBlocks[type].color;
                placeObj.name = "DestinationPlace#" + type + " (" + placeIndex + ") - (" + x + ", " + y + ")";

                placeIndex++;
            }
        }
    }

    public void MoveRight() {
        Dictionary<Block, Movement> movement = new Dictionary<Block, Movement>();

        for(int x = 0;x < PLATFORM_SIZE;x++) {
            for(int y = 0;y < PLATFORM_SIZE;y++) {
                if(movingBlocks[x, y] == null) {
                    continue;
                }
                
                int movementAmount = 0;
                int xNew = x + 1;
                while(xNew >= 0 && xNew < PLATFORM_SIZE && platform[xNew, y] != FieldType.NULL) {
                    if(movingBlocks[xNew, y] == null) {
                        movementAmount++;
                    }
                    xNew++;
                }
                movement.Add(movingBlocks[x, y], new Movement(movementAmount, x + movementAmount, y));
            }
        }

        Block[,] newMovingBlocks = new Block[PLATFORM_SIZE, PLATFORM_SIZE];
        foreach (var item in movement)
        {
            item.Key.MoveRight(item.Value.value);

            newMovingBlocks[item.Value.xNew, item.Value.yNew] = item.Key;
        }
        movingBlocks = newMovingBlocks;
    }
    public void MoveLeft() {
        Dictionary<Block, Movement> movement = new Dictionary<Block, Movement>();

        for(int x = 0;x < PLATFORM_SIZE;x++) {
            for(int y = 0;y < PLATFORM_SIZE;y++) {
                if(movingBlocks[x, y] == null) {
                    continue;
                }

                int movementAmount = 0;
                int xNew = x - 1;
                while(xNew >= 0 && xNew < PLATFORM_SIZE && platform[xNew, y] != FieldType.NULL) {
                    if(movingBlocks[xNew, y] == null) {
                        movementAmount++;
                    }
                    xNew--;
                }

                movement.Add(movingBlocks[x, y], new Movement(movementAmount, x - movementAmount, y));
            }
        }

        Block[,] newMovingBlocks = new Block[PLATFORM_SIZE, PLATFORM_SIZE];
        foreach(var item in movement) {
            item.Key.MoveLeft(item.Value.value);

            newMovingBlocks[item.Value.xNew, item.Value.yNew] = item.Key;
        }
        movingBlocks = newMovingBlocks;
    }
    public void MoveUp() {
        Dictionary<Block, Movement> movement = new Dictionary<Block, Movement>();

        for(int x = 0;x < PLATFORM_SIZE;x++) {
            for(int y = 0;y < PLATFORM_SIZE;y++) {
                if(movingBlocks[x, y] == null) {
                    continue;
                }
                
                int movementAmount = 0;
                int yNew = y - 1;
                while(yNew >= 0 && yNew < PLATFORM_SIZE && platform[x, yNew] != FieldType.NULL) {
                    if(movingBlocks[x, yNew] == null) {
                        movementAmount++;
                    }
                    yNew--;
                }

                movement.Add(movingBlocks[x, y], new Movement(movementAmount, x, y - movementAmount));
            }
        }

        Block[,] newMovingBlocks = new Block[PLATFORM_SIZE, PLATFORM_SIZE];
        foreach(var item in movement) {
            item.Key.MoveUp(item.Value.value);

            newMovingBlocks[item.Value.xNew, item.Value.yNew] = item.Key;
        }
        movingBlocks = newMovingBlocks;
    }
    public void MoveDown() {
        Dictionary<Block, Movement> movement = new Dictionary<Block, Movement>();

        for(int x = 0;x < PLATFORM_SIZE;x++) {
            for(int y = 0;y < PLATFORM_SIZE;y++) {
                if(movingBlocks[x, y] == null) {
                    continue;
                }

                int movementAmount = 0;
                int yNew = y + 1;
                while(yNew >= 0 && yNew < PLATFORM_SIZE && platform[x, yNew] != FieldType.NULL) {
                    if(movingBlocks[x, yNew] == null) {
                        movementAmount++;
                    }
                    yNew++;
                }

                movement.Add(movingBlocks[x, y], new Movement(movementAmount, x, y + movementAmount));
            }
        }

        Block[,] newMovingBlocks = new Block[PLATFORM_SIZE, PLATFORM_SIZE];
        foreach(var item in movement) {
            item.Key.MoveDown(item.Value.value);

            newMovingBlocks[item.Value.xNew, item.Value.yNew] = item.Key;
        }
        movingBlocks = newMovingBlocks;
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
