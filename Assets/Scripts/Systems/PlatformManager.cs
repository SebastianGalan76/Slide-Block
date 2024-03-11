using System.Collections.Generic;
using UnityEngine;
using static LevelLoader;

public class PlatformManager : MonoBehaviour
{
    [SerializeField] private GameObject platformPrefab;
    [SerializeField] private GameObject movingBlockPrefab, destinationPlacePrefab;

    private int[,] platform;
    private Block[,] movingBlocks;

    public ColorBlock[] colorBlocks;

    private void Awake() {
        LoadNewLevel(1, 1);
    }

    public void LoadNewLevel(int stage, int level) {
        platform = LoadPlatform(stage, level);

        for(int x = 0;x < 14;x++) {
            for(int y = 0;y < 14;y++) {
                if(platform[x, y] == 0) {
                    continue;
                }

                Instantiate(platformPrefab, new Vector3(x, -y, 1), Quaternion.identity);
            }
        }

        //Load moving blocks
        movingBlocks = new Block[14, 14];
        Dictionary<int, List<BlockValues>> movingBlocksDictionary = LoadMovingBlocks(stage, level);
        foreach(int type in movingBlocksDictionary.Keys) {
            List<BlockValues> blockValues = movingBlocksDictionary[type];
            
            int blockIndex = 0;
            foreach(BlockValues blockValue in blockValues) {
                GameObject blockObj = Instantiate(movingBlockPrefab, new Vector3(blockValue.posX, blockValue.posY, 1), Quaternion.identity);
                blockObj.GetComponent<SpriteRenderer>().sprite = colorBlocks[type].movingBlockSprite;
                blockObj.name = "MovingBlock#" + type + " (" + blockIndex+")";
                
                int x = blockValue.positionPlatform / 14;
                int y = blockValue.positionPlatform % 14;
                Block block = blockObj.GetComponent<Block>();

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
                GameObject place = Instantiate(destinationPlacePrefab, new Vector3(blockValue.posX, blockValue.posY, 1), Quaternion.identity);
                place.GetComponent<SpriteRenderer>().sprite = colorBlocks[type].destinationPlaceSprite;
                place.name = "DestinationPlace#" + type + " (" + placeIndex + ")";

                placeIndex++;
            }
        }
    }

    [System.Serializable]
    public struct ColorBlock {
        public BlockType color;
        public Sprite movingBlockSprite;
        public Sprite destinationPlaceSprite;
    }
}
