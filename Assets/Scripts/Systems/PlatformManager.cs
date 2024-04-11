using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    public const int PLATFORM_SIZE = 14;

    [SerializeField] private MovementSystem movementSystem;
    [SerializeField] private LevelManager levelManager;

    [Header("Prefabs")]
    [SerializeField] private GameObject levelContainerPrefab;
    [SerializeField] private GameObject platformPrefab;
    [SerializeField] private GameObject movingBlockPrefab, destinationPlacePrefab;
    [SerializeField] private GameObject stoppableBlockPrefab;
    [SerializeField] private GameObject destructivePlacePrefab;

    [SerializeField] private ColorBlock[] colorBlocks;

    public FieldType[,] platform;
    public Block[,] movingBlocks;

    private Dictionary<int, int> colorBlockType;
    private BorderManager borderManager;

    private LevelContainer levelContainer;
    private LevelLoader levelLoader;

    private void Awake() {
        borderManager = GetComponent<BorderManager>();

        levelLoader = LevelLoader.GetInstance();
    }

    public void LoadLevel(int stage, int level) {
        StopAllCoroutines();
        foreach(Transform child in transform) {
            Destroy(child.gameObject);
        }

        if(levelContainer != null) {
            levelContainer.HideLevel();
        }

        GameObject levelContainerObj = Instantiate(levelContainerPrefab);
        levelContainer = levelContainerObj.GetComponent<LevelContainer>();
        levelContainer.Initialize(stage, level);
        levelContainer.transform.position = new Vector3(level * 100f, stage * 100);

        platform = levelLoader.LoadPlatform(stage, level);
        borderManager.GenerateBorder(platform, PLATFORM_SIZE, levelContainer.GetPlatformParent());

        for(int x = 0;x < PLATFORM_SIZE;x++) {
            for(int y = 0;y < PLATFORM_SIZE;y++) {
                if(platform[x, y] != FieldType.PLATFORM) {
                    continue;
                }

                GameObject platformObj = Instantiate(platformPrefab);
                platformObj.transform.localPosition = new Vector3(x, -y, 1);
                platformObj.name = "Platform (" + x + ", " + y + ")";
                platformObj.transform.SetParent(levelContainer.GetPlatformParent(), false);
            }
        }

        //Load moving blocks
        int totalMovingBlocks = 0;

        colorBlockType = new Dictionary<int, int>();

        movingBlocks = new Block[PLATFORM_SIZE, PLATFORM_SIZE];
        Dictionary<int, List<LevelLoader.BlockValues>> movingBlocksDictionary = levelLoader.LoadMovingBlocks(stage, level);
        totalMovingBlocks += movingBlocksDictionary.Count;
        foreach(int type in movingBlocksDictionary.Keys) {
            List<LevelLoader.BlockValues> blockValues = movingBlocksDictionary[type];
            
            int blockIndex = 0;
            foreach(LevelLoader.BlockValues blockValue in blockValues) {
                ColorBlock colorBlock = GetRandomColor(type);

                GameObject blockObj = Instantiate(movingBlockPrefab, new Vector3(blockValue.posX, blockValue.posY, 1), Quaternion.identity);
                blockObj.GetComponent<SpriteRenderer>().sprite = colorBlock.movingBlockSprite;
                blockObj.transform.SetParent(levelContainer.GetPlatformParent(), false);

                int x = blockValue.positionPlatform % PLATFORM_SIZE;
                int y = blockValue.positionPlatform / PLATFORM_SIZE;
                blockObj.name = "MovingBlock#" + type + " (" + blockIndex + ") - ("+x+", "+y+")";
                
                Block block = blockObj.GetComponent<Block>();
                block.Initialize(colorBlock, x, y, movementSystem);
                movingBlocks[x, y] = block;
                
                blockIndex++;
            }
        }

        //Load destination places
        Dictionary<int, List<LevelLoader.BlockValues>> destinationPlacesDictionary = levelLoader.LoadDestinationPlaces(stage, level);
        foreach(int type in destinationPlacesDictionary.Keys) {
            List<LevelLoader.BlockValues> placeValues = destinationPlacesDictionary[type];

            int placeIndex = 0;
            foreach(LevelLoader.BlockValues blockValue in placeValues) {
                ColorBlock colorBlock = GetRandomColor(type);

                GameObject placeObj = Instantiate(destinationPlacePrefab, new Vector3(blockValue.posX, blockValue.posY, 1), Quaternion.identity);
                placeObj.GetComponent<SpriteRenderer>().sprite = colorBlock.destinationPlaceSprite;
                placeObj.transform.SetParent(levelContainer.GetPlatformParent(), false);

                int x = blockValue.positionPlatform % PLATFORM_SIZE;
                int y = blockValue.positionPlatform / PLATFORM_SIZE;
                platform[x, y] = colorBlock.color;
                placeObj.name = "DestinationPlace#" + type + " (" + placeIndex + ") - (" + x + ", " + y + ")";

                placeIndex++;
            }
        }

        //Load stoppable blocks
        List<LevelLoader.BlockValues> stoppableBlocks = levelLoader.LoadStoppableBlocks(stage, level);
        totalMovingBlocks += stoppableBlocks.Count;
        int stoppableBlockIndex = 0;
        foreach(LevelLoader.BlockValues blockValues in stoppableBlocks) {
            GameObject blockObj = Instantiate(stoppableBlockPrefab, new Vector3(blockValues.posX, blockValues.posY, 1), Quaternion.identity);
            blockObj.transform.SetParent(levelContainer.GetPlatformParent(), false);

            int x = blockValues.positionPlatform % PLATFORM_SIZE;
            int y = blockValues.positionPlatform / PLATFORM_SIZE;
            blockObj.name = "StoppableBlock (" + stoppableBlockIndex + ") - (" + x + ", " + y + ")";

            StoppableBlock block = blockObj.GetComponent<StoppableBlock>();
            block.Initialize(x, y, movementSystem);
            movingBlocks[x, y] = block;

            stoppableBlockIndex++;
        }

        //Load destructive places
        List<LevelLoader.BlockValues> destructivePlaces = levelLoader.LoadDestructivePlaces(stage, level);
        int destructivePlaceIndex = 0;
        foreach(LevelLoader.BlockValues blockValue in destructivePlaces) {
            GameObject placeObj = Instantiate(destructivePlacePrefab, new Vector3(blockValue.posX, blockValue.posY, 1), Quaternion.identity);
            placeObj.transform.SetParent(levelContainer.GetPlatformParent(), false);

            int x = blockValue.positionPlatform % PLATFORM_SIZE;
            int y = blockValue.positionPlatform / PLATFORM_SIZE;
            platform[x, y] = FieldType.DESTRUCTIVE;
            placeObj.name = "DestructivePlace (" + destructivePlaceIndex + ") - (" + x + ", " + y + ")";

            destructivePlaceIndex++;
        }

        levelContainer.ShowLevel();
        movementSystem.StartNewLevel(totalMovingBlocks);
    }

    public void CheckPlatform() {
        movementSystem.enabled = false;

        bool levelIsComplete = true;
        bool levelIsLost = false;

        for(int x = 0;x < PLATFORM_SIZE;x++) {
            for(int y = 0;y < PLATFORM_SIZE;y++) {
                Block block = movingBlocks[x, y];
                if(block == null) {
                    continue;
                }

                if(platform[x, y] == FieldType.DESTRUCTIVE) {
                    movementSystem.ChangeTotalBlocksCount(-1);
                    block.DestroyBlock();
                    movingBlocks[x, y] = null;

                    if(block is not StoppableBlock) {
                        levelIsLost = true;
                    }
                }

                if(block is StoppableBlock) {
                    continue;
                }

                if(block.GetBlockType() != platform[x, y]) {
                    levelIsComplete = false;
                }

            }
        }

        if(levelIsComplete) {
            StartCoroutine(wait());

            IEnumerator wait() {
                yield return new WaitForSeconds(0.5f);

                EndLevel();
                levelManager.CompleteLevel();
            }
            return;
        }

        if(levelIsLost) {
            StartCoroutine(wait());

            IEnumerator wait() {
                yield return new WaitForSeconds(1.5f);

                EndLevel();
                levelManager.LostLevel();
            }
        } else {
            movementSystem.enabled = true;
        }
    }

    private void EndLevel() {
        foreach(Block block in movingBlocks) {
            if(block != null) {
                block.HideAllTrails();
            }
        }
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
        public Gradient trailColor;
    }
}
