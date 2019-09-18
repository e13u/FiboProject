using System.IO;
using System.Xml.Serialization;
using UnityEngine;
using System.Collections.Generic;

public class GameUtility {
    public static List<int> sortedThemes = new List<int> {0,1,2,3,4,5 };

    public const float      ResolutionDelayTime     = 2;
    public const string     SavePKPKey             = "PKP_Value";

    public const string     FileName                = "Q";
    public static string    FileDir                 
    {
        get
        {
            return Application.persistentDataPath + "/";
        }
    }

    public static string ThemeNameText(int themeId)
    {
        switch (themeId)
        {
            case 1:
                return "Portugues";
            case 2:
                return "Biologia";
            case 3:
                return "Geografia";
            case 4:
                return "Artes";
            case 5:
                return "Matematica";
            case 6:
                return "Filosofia";
            case 7:
                return "Fisica";
            case 8:
                return "Historia";
            case 9:
                return "Sociologia";
            default:
                return "Meme";
        }
    }

    public static int ThemeNameText(string themeId)
    {
        switch (themeId)
        {
            case "Portugues":
                return 1;
            case "Biologia":
                return 2;
            case "Geografia":
                return 3;
            case "Artes":
                return 4;
            case "Matematica":
                return 5;
            case "Filosofia":
                return 6;
            case "Fisica":
                return 7;
            case "Historia":
                return 8;
            case "Sociologia":
                return 9;
            default:
                return 10;
        }
    }
}


[System.Serializable()]
public class Data
{
    public Question[] Questions = new Question[0];
    public Question2[] Questions2 = new Question2[0];

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