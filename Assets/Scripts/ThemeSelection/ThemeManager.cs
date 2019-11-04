using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class ThemeManager : MonoBehaviour
{
    public List<int> themeList = new List<int>();
    public List<Sprite> themeListImages = new List<Sprite>();
    public static ThemeManager Instance;
    public int themeCounter = 0;

    public UnityEvent callNextScreen;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start(){
        List<int> randomPoolPickNumber = new List<int> {0,1,2,3,4,5,6,7,8};
        for (int i = 0; i < 6; i++)
        {
            int randomIndex = Random.Range(0,randomPoolPickNumber.Count-1);
            themeList.Add(randomPoolPickNumber[randomIndex]);
            randomPoolPickNumber.RemoveAt(randomIndex);
        }        
    }
    public Sprite PickSpriteFromTheme(){
        Sprite themeSprite = themeListImages[themeList[themeCounter]];
        //int themeId = themeList[themeCounter];
        //themeSprite = themeListImages[themeId-1];
        return themeSprite;
    }

    public string PickStringFromTheme(){
        return GameUtility.ThemeNameText(themeList[themeCounter]);
    }

    public void StartGame(){
        if(themeList.Count == 6)
            SceneManager.LoadScene("Game");
    }

    public void VerifyStartGame(){
        if(themeCounter > 5)
            callNextScreen.Invoke();
    }
}
