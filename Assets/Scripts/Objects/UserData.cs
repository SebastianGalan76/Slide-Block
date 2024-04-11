using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;

[System.Serializable]
[System.Xml.Serialization.XmlRoot("userData")]
public class UserData
{
    [XmlElement("fileVersion")]
    public int fileVersion;

    [XmlElement("adValue")]
    public int adValue;

    [XmlArray("levels")]
    [XmlArrayItem("stage")]
    public List<StageNode> stages;

    [XmlElement("settings")]
    public SettingsElement settings;

    [XmlElement("statistic")]
    public StatisticNode statistic;
}

[System.Serializable]
public class StageNode {
    [XmlAttribute("id")]
    public int id;

    [XmlElement("level")]
    public List<LevelStatus> levels;
}

[System.Serializable]
public class LevelStatus {
    [XmlAttribute("id")]
    public int id;

    [XmlElement("unlocked")]
    public bool unlocked;

    [XmlElement("finished")]
    public bool finished;
}

[System.Serializable]
public struct SettingsElement {
    [XmlElement("sound")]
    public int sound;

    [XmlElement("music")]
    public int music;
}

[System.Serializable]
public struct StatisticNode {
    [XmlElement("stage")]
    public List<StatisticStageElement> stages;
}

[System.Serializable]
public class StatisticStageElement {
    [XmlAttribute("id")]
    public int id;

    [XmlElement("starAmount")]
    public int starAmount;
}