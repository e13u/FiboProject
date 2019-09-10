using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ThemeManager : MonoBehaviour
{
    public int selectedThemesQuantity = 0;
    public List<int> themeList = new List<int>();
    public static ThemeManager Instance;
    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void AddRemoveThemeList(int themeId){
        if(!themeList.Contains(themeId)){
            themeList.Add(themeId);
        }
        else{
            themeList.Remove(themeId);
        }
        themeList.Sort();
    }

    public void StartGame(){
        if(themeList.Count == 6)
            SceneManager.LoadScene("Game");
    }

}
