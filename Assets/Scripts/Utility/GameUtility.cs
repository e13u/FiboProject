using System.IO;
using System.Xml.Serialization;
using UnityEngine;
using System.Collections.Generic;

public class GameUtility {
    public static List<int> sortedThemes = new List<int> {0,1,2,3,4,5 };

    public const float      ResolutionDelayTime     = 1;
    public const string     SavePrefKey             = "Game_Highscore_Value";

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
                return "Biologia_Easy";
            case 1:
                return "Fisica_Easy";
            case 2:
                return "Geografia_Easy";
            case 3:
                return "Historia_Easy";
            case 4:
                return "Matematica_Easy";
            case 5:
                return "Portugues_Easy";
            default:
                return "Meme_Easy";
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