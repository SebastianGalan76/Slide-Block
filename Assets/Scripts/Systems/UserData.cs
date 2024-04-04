using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using UnityEngine;
using UnityEditor;
using GoogleMobileAds.Api;
using Unity.VisualScripting;

public class UserData
{
    private const string FILE_NAME = "UserData.xml";
    private const int FILE_VERSION = 1;

    private static string filePath;
    private static bool isLoaded;
    private static XmlDocument doc;

    public static void CompleteLevel(int stage, int level) {
        LoadDocument();

        if(!GetLevelStatus(stage, level).finished) {
            XmlNode stageNode = GetOrCreateStageNode(stage);
            XmlNode levelNode = GetOrCreateLevelNode(stageNode, level);

            XmlNode finishedNode = levelNode.SelectSingleNode("finished");
            if(finishedNode != null) {
                finishedNode.InnerText = "true";
            } else {
                XmlElement finishedElement = doc.CreateElement("finished");
                finishedElement.InnerText = "true";
                levelNode.AppendChild(finishedElement);
            }

            IncreaseStarAmountForStage(stage);
        }

        level++;
        if(level > 100) {
            stage++;
            level = 1;
        }
        UnlockLevel(stage, level);


        SaveDocument();
    }

    public static void SkipLevel(int stage, int level) {
        level++;
        if(level > 100) {
            stage++;
            level = 1;
        }
        UnlockLevel(stage, level);
    }

    public static void UnlockLevel(int stage, int level) {
        LoadDocument();

        XmlNode stageNode = GetOrCreateStageNode(stage);
        XmlNode levelNode = GetOrCreateLevelNode(stageNode, level);

        XmlNode unlockedNode = levelNode.SelectSingleNode("unlocked");
        if(unlockedNode != null) {
            unlockedNode.InnerText = "true";
        } else {
            XmlElement unlockedElement = doc.CreateElement("unlocked");
            unlockedElement.InnerText = "true";
            levelNode.AppendChild(unlockedElement);
        }

        SaveDocument();
    }

    public static LevelStatus GetLevelStatus(int stage, int level) {
        LoadDocument();

        XmlNode levelsNode = doc.SelectSingleNode("//levels");
        XmlNode stageNode = levelsNode.SelectSingleNode("stage[@id='" + stage + "']");
        if (stageNode==null)
        {
            return new LevelStatus(false, false);
        }

        XmlNode levelNode = stageNode.SelectSingleNode("level[@id='" + level + "']");
        if(levelNode == null) {
            return new LevelStatus(false, false);
        }

        bool unlocked, finished;
        XmlNode unlockedNode = levelNode.SelectSingleNode("unlocked");
        if(unlockedNode != null && unlockedNode.InnerText == "true") {
            unlocked = true;
        } else {
            unlocked = false;
        }

        XmlNode finishedNode = levelNode.SelectSingleNode("finished");
        if(finishedNode != null && finishedNode.InnerText == "true") {
            finished = true;
        } else {
            finished = false;
        }

        return new LevelStatus(unlocked, finished);
    }

    public static int GetTotalStarAmount() {
        LoadDocument();

        int amount = 0;

        XmlNode statisticNode = doc.SelectSingleNode("//statistic");
        if(statisticNode == null) {
            return 0;
        }

        XmlNodeList stageNodes = statisticNode.SelectNodes("stage");
        foreach(XmlNode stageNode in stageNodes) {
            XmlNode starAmountNode = stageNode.SelectSingleNode("starAmount");

            if(starAmountNode == null) {
                continue;
            }
            amount += int.Parse(starAmountNode.InnerText);
        }


        return amount;
    }

    public static int GetStarAmountForStage(int stage) {
        LoadDocument();

        XmlNode statisticNode = doc.SelectSingleNode("//statistic");

        if(statisticNode==null) {
            return 0;
        }

        XmlNode stageNode = statisticNode.SelectSingleNode("stage[@id='" + stage + "']");
        if(stageNode==null) {
            return 0;
        }

        XmlNode starAmount = stageNode.SelectSingleNode("starAmount");
        if(starAmount==null) {
            return 0;
        }

        int amount = int.Parse(starAmount.InnerText);
        return amount;
    }

    public static int GetAdValue() {
        LoadDocument();

        XmlNode adValueNode = doc.SelectSingleNode("//adValue");
        if(adValueNode == null) {
            return 0;
        }

        return int.Parse(adValueNode.InnerText);
    }

    public static void ChangeAdValue(int adValue) {
        LoadDocument();

        XmlNode adValueNode = doc.SelectSingleNode("//adValue");
        if(adValueNode == null) {
            XmlElement adValueElement = doc.CreateElement("adValue");
            adValueElement.InnerText = adValue.ToString();
            doc.SelectSingleNode("userData").AppendChild(adValueElement);
            return;
        }

        adValueNode.InnerText = adValue.ToString();
    }

    public static void LoadAudioSettings(ref AudioType sound, ref AudioType music) {
        LoadDocument();

        XmlNode settingsNode = GetOrCreateSettingsStageNode();

        XmlNode soundElement = settingsNode.SelectSingleNode("sound");
        sound.SetEnabled(soundElement.InnerText == "1" ? true : false);

        XmlNode musicElement = settingsNode.SelectSingleNode("music");
        music.SetEnabled(musicElement.InnerText == "1" ? true : false);
    }

    public static void SaveAudioSettings(AudioType sound, AudioType music) {
        LoadDocument();

        XmlNode settingsNode = GetOrCreateSettingsStageNode();
        XmlNode soundNode = settingsNode.SelectSingleNode("sound");
        if(soundNode == null) {
            XmlElement soundElement = doc.CreateElement("sound");
            settingsNode.AppendChild(soundElement);
            soundNode = soundElement;
        }
        soundNode.InnerText = sound.IsEnabled() ? "1" : "0";

        XmlNode musicNode = settingsNode.SelectSingleNode("music");
        if(musicNode == null) {
            XmlElement musicElement = doc.CreateElement("music");
            settingsNode.AppendChild(musicElement);
            musicNode = musicElement;
        }
        musicNode.InnerText = music.IsEnabled() ? "1" : "0";

        SaveDocument();
    }

    private static XmlNode GetOrCreateStageNode(int stage) {
        XmlNode levelsNode = doc.SelectSingleNode("//levels");
        XmlNode stageNode = levelsNode.SelectSingleNode("stage[@id='" + stage + "']");
        if(stageNode == null) {
            XmlElement stageElement = doc.CreateElement("stage");
            stageElement.SetAttribute("id", stage.ToString());
            levelsNode.AppendChild(stageElement);
            stageNode = stageElement;
        }
        return stageNode;
    }
    private static XmlNode GetOrCreateLevelNode(XmlNode stageNode, int level) {
        XmlNode levelNode = stageNode.SelectSingleNode("level[@id='" + level + "']");
        if(levelNode == null) {
            XmlElement levelElement = doc.CreateElement("level");
            levelElement.SetAttribute("id", level.ToString());
            stageNode.AppendChild(levelElement);
            levelNode = levelElement;
        }
        return levelNode;
    }

    private static void IncreaseStarAmountForStage(int stage) {
        LoadDocument();

        XmlNode stageNode = GetOrCreateStatisticStageNode(stage);

        XmlNode starAmount = stageNode.SelectSingleNode("starAmount");
        if(starAmount == null) {
            XmlElement starAmountElement = doc.CreateElement("starAmount");
            stageNode.AppendChild(starAmountElement);
            starAmountElement.InnerText = "1";
        } else {
            int currentValue = int.Parse(starAmount.InnerText);
            starAmount.InnerText = (currentValue + 1).ToString();
        }

        SaveDocument();
    }

    private static XmlNode GetOrCreateStatisticStageNode(int stage) {
        XmlNode statisticNode = doc.SelectSingleNode("//statistic");

        if(statisticNode == null) {
            XmlElement statisticElement = doc.CreateElement("statistic");
            doc.SelectSingleNode("userData").AppendChild(statisticElement);
            statisticNode = statisticElement;
        }

        XmlNode stageNode = statisticNode.SelectSingleNode("stage[@id='" + stage + "']");
        if(stageNode == null) {
            XmlElement stageElement = doc.CreateElement("stage");
            stageElement.SetAttribute("id", stage.ToString());
            statisticNode.AppendChild(stageElement);
            stageNode = stageElement;
        }

        return stageNode;
    }

    private static XmlNode GetOrCreateSettingsStageNode() {
        XmlNode settingsNode = doc.SelectSingleNode("//settings");

        if(settingsNode == null) {
            XmlElement settingsElement = doc.CreateElement("settings");
            doc.SelectSingleNode("userData").AppendChild(settingsElement);
            settingsNode = settingsElement;
        }

        return settingsNode;
    }


    private static void LoadDocument() {
        if(isLoaded)
            return;

        doc = new XmlDocument();
        if(Application.platform == RuntimePlatform.Android) {
            filePath = Application.persistentDataPath + "/" + FILE_NAME;
            if(!File.Exists(filePath)) {
                CreateFileStructure();
                doc.Save(filePath);
            }

            doc.Load(filePath);
        }

#if UNITY_EDITOR
        if(Application.platform == RuntimePlatform.WindowsEditor) {
            filePath = "Assets/Resources/" + FILE_NAME;
            if(!File.Exists(filePath)) {
                CreateFileStructure();
                doc.Save(filePath);

                AssetDatabase.Refresh();
            }

            doc.Load(filePath);
        }
#endif

        isLoaded = true;
    }
    private static void SaveDocument() {
        if(Application.platform == RuntimePlatform.Android) {
            doc.Save(filePath);
        } else if(Application.platform == RuntimePlatform.WindowsEditor) {
            doc.Save(filePath);
        }
    }
    private static void CreateFileStructure() {
        XmlElement rootElement = doc.CreateElement("userData");
        doc.AppendChild(rootElement);

        XmlElement fileVersionElement = doc.CreateElement("fileVersion");
        fileVersionElement.InnerText = FILE_VERSION.ToString();
        rootElement.AppendChild(fileVersionElement);

        XmlElement levelsElement = doc.CreateElement("levels");
        rootElement.AppendChild(levelsElement);

        XmlElement stageElement = doc.CreateElement("stage");
        stageElement.SetAttribute("id", "1");
        levelsElement.AppendChild(stageElement);

        XmlElement levelElement = doc.CreateElement("level");
        levelElement.SetAttribute("id", "1");
        stageElement.AppendChild(levelElement);

        XmlElement unlockedElement = doc.CreateElement("unlocked");
        unlockedElement.InnerText = "true";
        levelElement.AppendChild(unlockedElement);

        XmlElement adValueElement = doc.CreateElement("adValue");
        adValueElement.InnerText = "0";
        rootElement.AppendChild(adValueElement);

        XmlElement settingsElement = doc.CreateElement("settings");
        rootElement.AppendChild(settingsElement);

        XmlElement soundElement = doc.CreateElement("sound");
        soundElement.InnerText = "1";
        settingsElement.AppendChild(soundElement);

        XmlElement musicElement = doc.CreateElement("music");
        musicElement.InnerText = "1";
        settingsElement.AppendChild(musicElement);
    }

    public struct LevelStatus {
        public bool unlocked;
        public bool finished;

        public LevelStatus(bool unlocked, bool finished) {
            this.unlocked = unlocked;
            this.finished = finished;
        }
    }
}
