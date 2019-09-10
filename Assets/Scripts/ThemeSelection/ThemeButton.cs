using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ThemeButton : MonoBehaviour
{
    public Sprite notSelectedTheme;
    public Sprite selectedTheme;
    public int themeId = 0;
    public bool onOff = false;

    private Image buttonImage;
    void Start(){
        buttonImage = GetComponent<Image>();
    }
    public void TurnThemeOnOff(){

        if(ThemeManager.Instance.themeList.Count > 5){
            if(onOff){
                onOff = !onOff;
                ThemeManager.Instance.AddRemoveThemeList(themeId);
                buttonImage.sprite = notSelectedTheme;
            }
        }
        else{
             onOff = !onOff;
            ThemeManager.Instance.AddRemoveThemeList(themeId);
            if(onOff){
                buttonImage.sprite = selectedTheme;
            }
            else{
                buttonImage.sprite = notSelectedTheme;
            }
        }
    }
}
