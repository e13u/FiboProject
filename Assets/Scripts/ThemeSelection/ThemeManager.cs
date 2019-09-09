using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThemeManager : MonoBehaviour
{
    public int selectedThemesQuantity = 0;
    public List<int> themeList = new List<int>();
    public static ThemeManager Instance;
    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
