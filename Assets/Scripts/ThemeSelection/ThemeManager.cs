using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ThemeManager : MonoBehaviour
{
    public List<int> themeList = new List<int>();
    public List<Sprite> themeListImages = new List<Sprite>();
    public static ThemeManager Instance;
    private int themeCounter =0;
    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start(){
        List<int> randomPoolPickNumber = new List<int> {1,2,3,4,5,6,7,8,9};
        for (int i = 0; i < 6; i++)
        {
            int randomIndex = Random.Range(0,randomPoolPickNumber.Count-1);
            themeList.Add(randomPoolPickNumber[randomIndex]);
            randomPoolPickNumber.RemoveAt(randomIndex);
        }        
    }
    public Sprite PickSpriteFromTheme(){
        Sprite themeSprite = themeListImages[themeList[themeCounter]-1];
        //int themeId = themeList[themeCounter];
        //themeSprite = themeListImages[themeId-1];
        themeCounter++;
            if(themeCounter > 5)
                Invoke("StartGame", 1);
        return themeSprite;
    }

    public string PickStringFromTheme(){
        return GameUtility.ThemeNameText(themeList[themeCounter-1]);
    }

    public void StartGame(){
        if(themeList.Count == 6)
            SceneManager.LoadScene("Game");
    }
}
