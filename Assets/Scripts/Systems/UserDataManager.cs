using System.IO;
using UnityEngine;
using System.Xml.Serialization;
using System.Collections.Generic;
using static UserData;

public class UserDataManager
{
    private static UserDataManager instance;

    private const string FILE_NAME = "UserData.xml";
    private const int FILE_VERSION = 1;

    private UserData userData;

    private string filePath;
    private int starAmount = -1;

    private UserDataManager() {
        LoadUserData();
    }

    public void CompleteLevel(int stage, int level) {
        StageNode stageNode = GetOrCreateStageNode(stage);
        LevelStatus levelStatus = GetOrCreateLevelStatus(stageNode, level);
        if(!levelStatus.finished) {
            IncreaseStarAmountForStage(stage);

            levelStatus.finished = true;
            UnlockNextLevel(stage, level);
        }
    }
    public void UnlockLevel(int stage, int level) {
        StageNode stageNode = GetOrCreateStageNode(stage);
        LevelStatus levelStatus = GetOrCreateLevelStatus(stageNode, level);
        levelStatus.unlocked = true;

        SaveUserData();
    }
    public void UnlockNextLevel(int currentStage, int currentLevel) {
        currentLevel++;
        if(currentLevel > 100) {
            currentStage++;
            currentLevel = 1;
        }
        UnlockLevel(currentStage, currentLevel);
    }

    public LevelStatus GetLevelStatus(int stage, int level) {
        StageNode stageNode = GetOrCreateStageNode(stage);
        LevelStatus levelStatus = GetOrCreateLevelStatus(stageNode, level);
        return levelStatus;
    }

    public int GetTotalStarAmount() {
        if(starAmount != -1) {
            return starAmount;
        }

        int amount = 0;
        foreach(StatisticStageElement stageElement in userData.statistic.stages) {
            amount += stageElement.starAmount;
        }

        starAmount = amount;
        return amount;
    }
    public int GetStarAmountForStage(int stage) {
        StatisticStageElement stageElement = userData.statistic.stages[stage - 1];
        if(stageElement != null) {
            return stageElement.starAmount;
        }
        return 0;
    }

    public int GetAdValue() {
        return userData.adValue;
    }
    public void ChangeAdValue(int adValue) {
        userData.adValue = adValue;

        SaveUserData();
    }

    public void LoadAudioSettings(ref AudioType sound, ref AudioType music) {
        sound.SetEnabled(userData.settings.sound == 1 ? true : false);
        music.SetEnabled(userData.settings.music == 1 ? true : false);
    }

    public void SaveAudioSettings(AudioType sound, AudioType music) {
        userData.settings.sound = sound.IsEnabled() ? 1 : 0;
        userData.settings.music = music.IsEnabled() ? 1 : 0;

        SaveUserData();
    }

    private void IncreaseStarAmountForStage(int stage) {
        StatisticStageElement statisticStageElement = GetOrCreateStatisticStageElement(stage);
        statisticStageElement.starAmount += 1;
        starAmount += 1;

        SaveUserData();
    }

    private StageNode GetOrCreateStageNode(int stage) {
        foreach(StageNode stageNode in userData.stages) {
            if(stageNode.id == stage) {
                return stageNode;
            }
        }

        StageNode newStageNode = new StageNode {
            id = stage,
            levels = new List<LevelStatus>()
        };
        userData.stages.Add(newStageNode);
        SaveUserData();
        return newStageNode;
    }
    private LevelStatus GetOrCreateLevelStatus(StageNode stage, int level) {
        foreach(LevelStatus levelStatus in stage.levels) {
            if(levelStatus.id == level) {
                return levelStatus;
            }
        }

        LevelStatus newLevelStatus = new LevelStatus {
            id = level,
            finished = false,
            unlocked = false,
        };
        stage.levels.Add(newLevelStatus);
        SaveUserData();
        return newLevelStatus;
    }
    private StatisticStageElement GetOrCreateStatisticStageElement(int stage) {
        foreach(StatisticStageElement statisticStageElement in userData.statistic.stages) {
            if(statisticStageElement.id == stage) {
                return statisticStageElement;
            }
        }

        StatisticStageElement newStatisticStageElement = new StatisticStageElement {
            id = stage,
            starAmount = 0
        };
        userData.statistic.stages.Add(newStatisticStageElement);
        SaveUserData();
        return newStatisticStageElement;
    }

    private void SaveUserData() {
        XmlSerializer serializer = new XmlSerializer(typeof(UserData));

        using(FileStream fileStream = new FileStream(filePath, FileMode.Create)) {
            serializer.Serialize(fileStream, userData);
        }
    }
    private void LoadUserData() {
        if(Application.platform == RuntimePlatform.Android) {
            filePath = Application.persistentDataPath + "/" + FILE_NAME;
        } else if(Application.platform == RuntimePlatform.WindowsEditor) {
            filePath = "Assets/Resources/" + FILE_NAME;
        }

        XmlSerializer serializer = new XmlSerializer(typeof(UserData));

        if(File.Exists(filePath)) {
            using(FileStream fileStream = new FileStream(filePath, FileMode.Open)) {
                userData = (UserData)serializer.Deserialize(fileStream);
            }
        } else {
            userData = GetDefaultUserData();
            SaveUserData();
        }
    }
    private UserData GetDefaultUserData() {
        UserData userData = new UserData();

        userData.fileVersion = FILE_VERSION;
        userData.adValue = 0;

        userData.settings = new SettingsElement() {
            sound = 1,
            music = 1,
        };

        userData.statistic = new StatisticNode() {
            stages = new List<StatisticStageElement> {
                new StatisticStageElement() {
                    id = 1,
                    starAmount = 0
                }
            }
        };

        userData.stages = new List<StageNode> {
            new StageNode() {
                id = 1,
                levels = new List<LevelStatus> {
                    new LevelStatus() {
                        id = 1,
                        unlocked = true,
                        finished = false
                    }
                }
            }
        };

        return userData;
    }

    public static UserDataManager GetInstance() {
        if(instance == null) {
            instance = new UserDataManager();
        }

        return instance;
    }
}
