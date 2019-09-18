using System.IO;
using System.Xml.Serialization;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class AplicationManager : MonoBehaviour
{
    public Text debugText;

    // Start is called before the first frame update
    void Awake()
    {
        // CopyQuestionToMobile("Artes_Easy.xml");
        // CopyQuestionToMobile("Biologia_Easy.xml");
        // CopyQuestionToMobile("Portugues_Easy.xml");
        // CopyQuestionToMobile("Fisica_Easy.xml");
        // CopyQuestionToMobile("Sociologia_Easy.xml");
        // CopyQuestionToMobile("Matematica_Easy.xml");
        // CopyQuestionToMobile("Filosofia_Easy.xml");
        // CopyQuestionToMobile("Geografia_Easy.xml");

        //File.Copy(Application.streamingAssetsPath+"/Artes_Easy.xml", Application.persistentDataPath+"/Artes_Easy.xml");
    }

    void CopyQuestionToMobile (string fileName) {
        string dataPath = Application.persistentDataPath + "/" + fileName;
        string assetPath = Application.dataPath+"Assets/Resources/" + fileName;  
        //debugText.text += "  "+assetPath;

         if(!File.Exists(dataPath)) {
             // File doesn't exist, move it from assets folder to data directory
             File.Copy(assetPath, dataPath);
         }else{
            debugText.text += dataPath;
         }

    }
     
}