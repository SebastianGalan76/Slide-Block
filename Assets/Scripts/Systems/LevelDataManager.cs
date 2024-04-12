using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;
using static LevelData;
public class LevelDataManager
{
    private static LevelDataManager instance;
    
    private LevelData levelData;

    private LevelDataManager() { 
        LoadLevelData();
    }

    public FieldType[,] LoadPlatform(int stage, int level) {
        FieldType[,] platform = new FieldType[14, 14];
        LevelNode levelNode = GetLevelNode(stage, level);
        
        char[] platformValues = levelNode.platform.ToCharArray();
        for(int x = 0;x < 14;x++) {
            for(int y = 0;y < 14;y++) {
                if(platformValues[y * 14 + x] == '0') {
                    platform[x, y] = FieldType.NULL;
                } else {
                    platform[x, y] = FieldType.PLATFORM;
                }
            }
        }

        return platform;
    }

    public CameraElement LoadCamera(int stage, int level) {
        LevelNode levelNode = GetLevelNode(stage, level);
        return levelNode.camera;
    }

    public List<MovingBlockElement> LoadMovingBlocks(int stage, int level) {
        LevelNode levelNode = GetLevelNode(stage, level);
        return levelNode.movingBlocks;
    }

    public List<DestinationPlaceElement> LoadDestinationPlaces(int stage, int level) {
        LevelNode levelNode = GetLevelNode(stage, level);
        return levelNode.destinationPlaces;
    }

    public List<StoppableBlockElement> LoadStoppableBlocks(int stage, int level) {
        LevelNode levelNode = GetLevelNode(stage, level);
        return levelNode.stoppableBlocks;
    }

    public List<DestructivePlaceElement> LoadDestructivePlaces(int stage, int level) {
        LevelNode levelNode = GetLevelNode(stage, level);
        return levelNode.destructivePlaces;
    }

    public int GetLevelAmountForStage(int stage) {
        int levelAmount = 0;
        foreach(StageNode stageNode in levelData.stages) {
            if(stageNode.id == stage) {
                foreach(LevelNode levelNode in stageNode.levels) {
                    levelAmount++;
                }
            }
        }

        return levelAmount;
    }

    public bool LevelExist(int stage, int level) {
        return GetLevelNode(stage, level) != null;
    }

    public LevelNode GetLevelNode(int stage, int level) {
        foreach(StageNode stageNode in levelData.stages) {
            if(stageNode.id == stage) {
                foreach(LevelNode levelNode in stageNode.levels) {
                    if(levelNode.id == level) {
                        return levelNode;
                    }
                }

            }
        }

        return null;
    }

    private void LoadLevelData() {
        XmlSerializer serializer = new XmlSerializer(typeof(LevelData));
        if(Application.platform == RuntimePlatform.Android) {
            TextAsset levelAsset = (TextAsset)Resources.Load("LevelsXML");

            using(StringReader streamReader = new StringReader(levelAsset.text)) {
                levelData = (LevelData)serializer.Deserialize(streamReader);
            }
        } else if(Application.platform == RuntimePlatform.WindowsEditor) {
            TextAsset levelAsset = (TextAsset)Resources.Load("LevelsXML");

            using(StringReader streamReader = new StringReader(levelAsset.text)) {
                levelData = (LevelData)serializer.Deserialize(streamReader);
            }
        }
    }

    public static LevelDataManager GetInstance() {
        if(instance == null) {
            instance = new LevelDataManager();
        }

        return instance;
    }
}
