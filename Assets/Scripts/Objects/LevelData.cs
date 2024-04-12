using System.Collections.Generic;
using System.Xml.Serialization;

[System.Serializable]
[XmlRoot("levels")]
public class LevelData {
    [XmlElement("stage")]
    public List<StageNode> stages;

    [System.Serializable]
    public class StageNode {
        [XmlAttribute("id")]
        public int id;

        [XmlElement("level")]
        public List<LevelNode> levels;
    }

    [System.Serializable]
    public class LevelNode {
        [XmlAttribute("id")]
        public int id;

        [XmlElement("platform")]
        public string platform;

        [XmlElement("camera")]
        public CameraElement camera;

        [XmlElement("movingBlock")]
        public List<MovingBlockElement> movingBlocks;

        [XmlElement("destinationPlace")]
        public List<DestinationPlaceElement> destinationPlaces;

        [XmlElement("stoppableBlock")]
        public List<StoppableBlockElement> stoppableBlocks;

        [XmlElement("declassivePlace")]
        public List<DestructivePlaceElement> destructivePlaces;
    }

    [System.Serializable]
    public class CameraElement {
        [XmlElement("posX")]
        public float posX;

        [XmlElement("posY")]
        public float posY;

        [XmlElement("size")]
        public float size;
    }

    [System.Serializable]
    public class MovingBlockElement {
        [XmlElement("type")]
        public int type;

        [XmlElement("positionPlatform")]
        public int positionPlatform;

        [XmlElement("posX")]
        public float posX;

        [XmlElement("posY")]
        public float posY;
    }

    [System.Serializable]
    public class DestinationPlaceElement {
        [XmlElement("type")]
        public int type;

        [XmlElement("positionPlatform")]
        public int positionPlatform;

        [XmlElement("posX")]
        public float posX;

        [XmlElement("posY")]
        public float posY;
    }

    [System.Serializable]
    public class StoppableBlockElement {
        [XmlElement("positionPlatform")]
        public int positionPlatform;

        [XmlElement("posX")]
        public float posX;

        [XmlElement("posY")]
        public float posY;
    }

    [System.Serializable]
    public class DestructivePlaceElement {
        [XmlElement("positionPlatform")]
        public int positionPlatform;

        [XmlElement("posX")]
        public float posX;

        [XmlElement("posY")]
        public float posY;
    }
}