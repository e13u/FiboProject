using System.IO;
using System.Xml.Serialization;
using UnityEngine;
using System.Collections.Generic;

public class GameUtility {
    public static List<int> sortedThemes = new List<int> {0,1,2,3,4,5 };

    public const float      ResolutionDelayTime     = 1;
    public const string     SavePKPKey             = "PKP_Value";

    public const string     FileName                = "Q";
    public static string    FileDir                 
    {
        get
        {
            return Application.dataPath + "/";
        }
    }

    public static string ThemeNameText(int themeId)
    {
        switch (themeId)
        {
            case 0:
                return "Biologia";
            case 1:
                return "Fisica";
            case 2:
                return "Geografia";
            case 3:
                return "Historia";
            case 4:
                return "Matematica";
            case 5:
                return "Portugues";
            default:
                return "Meme";
        }
    }

    public static int ThemeNameText(string themeId)
    {
        switch (themeId)
        {
            case "Biologia":
                return 0;
            case "Fisica":
                return 1;
            case "Geografia":
                return 2;
            case "Historia":
                return 3;
            case "Matematica":
                return 4;
            case "Portugues":
                return 5;
            default:
                return 6;
        }
    }
}


[System.Serializable()]
public class Data
{
    public Question[] Questions = new Question[0];

    public Data () { }

    public static void Write(Data data, string path)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(Data));
        using (Stream stream = new FileStream(path, FileMode.Create))
        {
            serializer.Serialize(stream, data);
        }
    }
    public static Data Fetch(string filePath)
    {
        return Fetch(out bool result, filePath);
    }
    public static Data Fetch(out bool result, string filePath)
    {
        if (!File.Exists(filePath)) { result = false; return new Data(); }

        XmlSerializer deserializer = new XmlSerializer(typeof(Data));
        using (Stream stream = new FileStream(filePath, FileMode.Open))
        {
            var data = (Data)deserializer.Deserialize(stream);

            result = true;
            return data;
        }
    }
}