using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class LevelLoader
{
    private static bool isLoaded;
    private static XmlDocument doc;

    public static FieldType[,] LoadPlatform(int stage, int level) {
        LoadDocument();

        FieldType[,] platform = new FieldType[14, 14];

        XmlNode node = doc.SelectSingleNode("//stage[@id='" + stage + "']/level[@id='" + level + "']/platform");
        char[] platformValues = node.InnerText.ToCharArray();
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

    public static (float, float, float) LoadCamera(int stage, int level) {
        LoadDocument();

        XmlNode node = doc.SelectSingleNode("//stage[@id='" + stage + "']/level[@id='" + level + "']/camera");
        XmlNode posXNode = node.ChildNodes[0];
        XmlNode posYNode = node.ChildNodes[1];
        XmlNode sizeNode = node.ChildNodes[2];

        float.TryParse(sizeNode.InnerText, out float size);
        float.TryParse(posXNode.InnerText, out float posX);
        float.TryParse(posYNode.InnerText, out float posY);

        return (size, posX, posY);
    }

    public static Dictionary<int, List<BlockValues>> LoadMovingBlocks(int stage, int level) {
        LoadDocument();

        Dictionary<int, List<BlockValues>> movingBlocks = new Dictionary<int, List<BlockValues>>();
        XmlNodeList nodeList = doc.SelectNodes("//stage[@id='" + stage + "']/level[@id='" + level + "']//movingBlock");
        
        for(int i = 0;i < nodeList.Count;i++) {
            XmlNode typeNode = nodeList[i].ChildNodes[0];
            XmlNode posPlatformNode = nodeList[i].ChildNodes[1];
            XmlNode posXNode = nodeList[i].ChildNodes[2];
            XmlNode posYNode = nodeList[i].ChildNodes[3];

            int.TryParse(typeNode.InnerText, out int type);
            int.TryParse(posPlatformNode.InnerText, out int posPlatform);
            int.TryParse(posXNode.InnerText, out int posX);
            int.TryParse(posYNode.InnerText, out int posY);

            if(!movingBlocks.ContainsKey(type)) {
                movingBlocks.Add(type, new List<BlockValues>());
            }

            movingBlocks[type].Add(new BlockValues(posPlatform, posX, posY));
        }

        return movingBlocks;
    }

    public static Dictionary<int, List<BlockValues>> LoadDestinationPlaces(int stage, int level) {
        LoadDocument();

        Dictionary<int, List<BlockValues>> destinationPlaces = new Dictionary<int, List<BlockValues>>();
        XmlNodeList nodeList = doc.SelectNodes("//stage[@id='" + stage + "']/level[@id='" + level + "']//destinationPlace");

        for(int i = 0;i < nodeList.Count;i++) {
            XmlNode typeNode = nodeList[i].ChildNodes[0];
            XmlNode posPlatformNode = nodeList[i].ChildNodes[1];
            XmlNode posXNode = nodeList[i].ChildNodes[2];
            XmlNode posYNode = nodeList[i].ChildNodes[3];

            int.TryParse(typeNode.InnerText, out int type);
            int.TryParse(posPlatformNode.InnerText, out int posPlatform);
            int.TryParse(posXNode.InnerText, out int posX);
            int.TryParse(posYNode.InnerText, out int posY);

            if(!destinationPlaces.ContainsKey(type)) {
                destinationPlaces.Add(type, new List<BlockValues>());
            }

            destinationPlaces[type].Add(new BlockValues(posPlatform, posX, posY));
        }

        return destinationPlaces;
    }

    public static List<BlockValues> LoadStoppableBlocks(int stage, int level) {
        LoadDocument();

        List<BlockValues> stoppableBlocks = new List<BlockValues>();
        XmlNodeList nodeList = doc.SelectNodes("//stage[@id='" + stage + "']/level[@id='" + level + "']//stoppableBlock");

        for(int i = 0;i < nodeList.Count;i++) {
            XmlNode posPlatformNode = nodeList[i].ChildNodes[0];
            XmlNode posXNode = nodeList[i].ChildNodes[1];
            XmlNode posYNode = nodeList[i].ChildNodes[2];

            int.TryParse(posPlatformNode.InnerText, out int posPlatform);
            int.TryParse(posXNode.InnerText, out int posX);
            int.TryParse(posYNode.InnerText, out int posY);

            stoppableBlocks.Add(new BlockValues(posPlatform, posX, posY));
        }

        return stoppableBlocks;
    }

    public static List<BlockValues> LoadDestructivePlaces(int stage, int level) {
        LoadDocument();

        List<BlockValues> destructivePlaces = new List<BlockValues>();
        XmlNodeList nodeList = doc.SelectNodes("//stage[@id='" + stage + "']/level[@id='" + level + "']//destructivePlace");

        for(int i = 0;i < nodeList.Count;i++) {
            XmlNode posPlatformNode = nodeList[i].ChildNodes[0];
            XmlNode posXNode = nodeList[i].ChildNodes[1];
            XmlNode posYNode = nodeList[i].ChildNodes[2];

            int.TryParse(posPlatformNode.InnerText, out int posPlatform);
            int.TryParse(posXNode.InnerText, out int posX);
            int.TryParse(posYNode.InnerText, out int posY);

            destructivePlaces.Add(new BlockValues(posPlatform, posX, posY));
        }

        return destructivePlaces;
    }

    private static void LoadDocument() {
        if(isLoaded)
            return;

        doc = new XmlDocument();
        if(Application.platform == RuntimePlatform.Android) {
            TextAsset levelAsset = (TextAsset)Resources.Load("LevelsXML");
            doc.LoadXml(levelAsset.text.Replace(".", ","));
        } else if(Application.platform == RuntimePlatform.WindowsEditor) {
            doc.Load("Assets/Resources/LevelsXML.xml");
        }

        isLoaded = true;
    }

    public struct BlockValues {
        public int positionPlatform;
        public int posX, posY;

        public BlockValues(int positionPlatform, int posX, int posY) {
            this.positionPlatform = positionPlatform;
            this.posX = posX;
            this.posY = posY;
        }
    }
}
