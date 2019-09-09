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
        onOff = !onOff;
        //onOff == true ? buttonImage.sprite = selectedTheme: buttonImage.sprite = notSelectedTheme;
        if(onOff){
            buttonImage.sprite = selectedTheme;
        }
        else{
            buttonImage.sprite = notSelectedTheme;
        }
            ThemeManager.Instance.AddRemoveThemeList(themeId);
    }
}
