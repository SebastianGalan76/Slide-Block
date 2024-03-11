using System.Numerics;
using System.Xml;

public class LevelLoader
{
    private const string FILE_PATH = "Assets/Resources/LevelsXML.xml";

    private static bool isLoaded;
    private static XmlDocument doc;

    public static void LoadDocument() {
        doc = new XmlDocument();
        doc.Load(FILE_PATH);
        isLoaded = true;
    }

    public static int[,] LoadPlatform(int stage, int level) {
        if(!isLoaded) {
            LoadDocument();
        }

        int[,] platform = new int[14, 14];

        XmlNode node = doc.SelectSingleNode("//stage[@id='" + stage + "']/level[@id='" + level + "']/platform");
        char[] platformValues = node.InnerText.ToCharArray();
        for(int x = 0;x < 14;x++) {
            for(int y = 0;y < 14;y++) {
                if(platformValues[y * 14 + x] == '0') {
                    platform[x, y] = 0;
                } else {
                    platform[x, y] = 1;
                }
            }
        }

        return platform;
    }

    public static (float, float, float) LoadCamera(int stage, int level) {
        if(!isLoaded) {
            LoadDocument();
        }

        XmlNode node = doc.SelectSingleNode("//stage[@id='" + stage + "']/level[@id='" + level + "']/camera");
        XmlNode posXNode = node.ChildNodes[0];
        XmlNode posYNode = node.ChildNodes[1];
        XmlNode sizeNode = node.ChildNodes[2];

        float.TryParse(sizeNode.InnerText, out float size);
        float.TryParse(posXNode.InnerText, out float posX);
        float.TryParse(posYNode.InnerText, out float posY);

        return (size, posX, posY);
    }
}
