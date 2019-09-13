using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ThemeButton : MonoBehaviour
{
    public void TurnThemeOn(){
        GetComponent<Image>().sprite = ThemeManager.Instance.PickSpriteFromTheme();
        GetComponent<Button>().interactable = false;
        GetComponentInChildren<TextMeshProUGUI>().text = ThemeManager.Instance.PickStringFromTheme(); 
    }
}
