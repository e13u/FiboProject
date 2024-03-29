﻿using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable()]
public struct UIManagerParameters
{
    [Header("Answers Options")]
    [SerializeField] float margins;
    public float Margins { get { return margins; } }

    [Header("Resolution Screen Options")]
    [SerializeField] Color correctBGColor;
    public Color CorrectBGColor { get { return correctBGColor; } }
    [SerializeField] Color incorrectBGColor;
    public Color IncorrectBGColor { get { return incorrectBGColor; } }
    [SerializeField] Color finalBGColor;
    public Color FinalBGColor { get { return finalBGColor; } }

    [Header("Theme Screen Options")]
    public List<GameObject> themePanelDisplay;

    public List<Sprite> themeStarsScreen;    
    public Sprite correctAnswersStar;
    public Sprite incorrectAnswersStar;
    public Sprite notAnsweredStar;


}
[Serializable()]
public struct UIElements
{
    [SerializeField] RectTransform answersContentArea;
    public RectTransform AnswersContentArea { get { return answersContentArea; } }

    [SerializeField] TextMeshProUGUI questionInfoTextObject;
    public TextMeshProUGUI QuestionInfoTextObject { get { return questionInfoTextObject; } }

    [SerializeField] Image questionImageObject;
    public Image QuestionImageObject { get { return questionImageObject; } }

    [SerializeField] TextMeshProUGUI scoreText;
    public TextMeshProUGUI ScoreText { get { return scoreText; } }

    [Space]

    [SerializeField] Animator resolutionScreenAnimator;
    public Animator ResolutionScreenAnimator { get { return resolutionScreenAnimator; } }

    [SerializeField] Image resolutionBG;
    public Image ResolutionBG { get { return resolutionBG; } }

    [SerializeField] Image resolutionTheme;
    public Image ResolutionTheme { get { return resolutionTheme; } }
    [SerializeField] TextMeshProUGUI resolutionStateInfoText;
    public TextMeshProUGUI ResolutionStateInfoText { get { return resolutionStateInfoText; } }

    [SerializeField] TextMeshProUGUI resolutionScoreText;
    public TextMeshProUGUI ResolutionScoreText { get { return resolutionScoreText; } }

    [Space]

    [SerializeField] TextMeshProUGUI highScoreText;
    public TextMeshProUGUI HighScoreText { get { return highScoreText; } }

    [SerializeField] CanvasGroup mainCanvasGroup;
    public CanvasGroup MainCanvasGroup { get { return mainCanvasGroup; } }

    [SerializeField] RectTransform finishUIElements;
    public RectTransform FinishUIElements { get { return finishUIElements; } }

    [Space]
    [SerializeField] RectTransform themeUIDisplay;
    public RectTransform ThemeUIDisplay {get {return themeUIDisplay;}}
    public List<Image> answersStars;


}
[Serializable()]
public struct EndGameElements{
    public Image themeImg;
    public List<Image> answersStarsEnd;
    public Text DKPText;
}

[Serializable()]
public struct ThemeInGameElements{
    public Sprite themeIconImg;
    public Color themeColor;
}

public class UIManager : MonoBehaviour {

    #region Variables

    public enum         ResolutionScreenType   { Correct, Incorrect, Finish }

    [Header("References")]
    [SerializeField]    GameEvents             events                       = null;

    [Header("UI Elements (Prefabs)")]
    [SerializeField]    AnswerData             answerPrefab                 = null;

    [SerializeField]    UIElements             uIElements                   = new UIElements();

    [Space]
    [SerializeField]    UIManagerParameters    parameters                   = new UIManagerParameters();

    private             List<AnswerData>       currentAnswers               = new List<AnswerData>();
    private             int                    resStateParaHash             = 0;

    private             IEnumerator            IE_DisplayTimedResolution    = null;

    public List<EndGameElements> endGameElements = new List<EndGameElements>();
    private  int endGameCounter = 0;

    public List<ThemeInGameElements> themeInGameElements = new List<ThemeInGameElements>();
    public Image themeIcon;
    public Image starIcon;
    public Image answerPrefabObj;
    public Image timerFeedbackImage;

    
    #endregion

    #region Default Unity methods

    /// <summary>
    /// Function that is called when the object becomes enabled and active
    /// </summary>
    void OnEnable()
    {
        events.UpdateQuestionUI         += UpdateQuestionUI;
        events.DisplayResolutionScreen  += DisplayResolution;
        events.ScoreUpdated             += UpdateScoreUI;
        events.DisplayThemeScreen       += DisplayThemeScreen;
        events.ResetResolutionUI        += ResetResolutionUI;
        events.EndGamePanelStats        += EndGamePanelStats;
        events.ThemeInGameElements      += ThemeInGameElementsDisplay;
    }
    /// <summary>
    /// Function that is called when the behaviour becomes disabled
    /// </summary>
    void OnDisable()
    {
        events.UpdateQuestionUI         -= UpdateQuestionUI;
        events.DisplayResolutionScreen  -= DisplayResolution;
        events.ScoreUpdated             -= UpdateScoreUI;
        events.DisplayThemeScreen       -= DisplayThemeScreen;
        events.ResetResolutionUI        -= ResetResolutionUI;
        events.EndGamePanelStats        -= EndGamePanelStats;
        events.ThemeInGameElements      -= ThemeInGameElementsDisplay;

    }

    /// <summary>
    /// Function that is called when the script instance is being loaded.
    /// </summary>
    void Start()
    {
        UpdateScoreUI();
        resStateParaHash = Animator.StringToHash("ScreenState");
        uIElements.ResolutionBG.transform.parent.GetComponent<CanvasGroup>().interactable = false;
        uIElements.ResolutionBG.transform.parent.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    #endregion
     void DisplayThemeScreen(int themeId, float alphaPanel){
        uIElements.ThemeUIDisplay.GetComponent<CanvasGroup>().alpha = alphaPanel;
        if(alphaPanel > 0){
            parameters.themePanelDisplay[themeId].gameObject.SetActive(true);
            uIElements.ThemeUIDisplay.GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
            
        else
        {
            parameters.themePanelDisplay[themeId].gameObject.SetActive(false);
            uIElements.ThemeUIDisplay.GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
    }

    void ThemeInGameElementsDisplay(int theme){
        themeIcon.color = themeInGameElements[theme].themeColor;
        starIcon.color = themeInGameElements[theme].themeColor;
        timerFeedbackImage.color = themeInGameElements[theme].themeColor;
        answerPrefabObj.color = themeInGameElements[theme].themeColor;
    }

    /// <summary>
    /// Function that is used to update new question UI information.
    /// </summary>
    void UpdateQuestionUI(Question2 question)
    {
        uIElements.QuestionInfoTextObject.text = question.Info;
        if(question.QuestionImage != null){
            uIElements.QuestionImageObject.enabled = true;
            uIElements.QuestionImageObject.sprite = question.QuestionImage;
            uIElements.QuestionInfoTextObject.rectTransform.sizeDelta = 
            new Vector3(uIElements.QuestionInfoTextObject.rectTransform.sizeDelta.x, 500,0);
        }   
        else{
            uIElements.QuestionImageObject.enabled = false;
            uIElements.QuestionInfoTextObject.rectTransform.sizeDelta = 
            new Vector3(uIElements.QuestionInfoTextObject.rectTransform.sizeDelta.x, 1000,0);
        }
        CreateAnswers(question);
    }
    /// <summary>
    /// Function that is used to display resolution screen.
    /// </summary>
    void DisplayResolution(ResolutionScreenType type, int score, int themeID, List<bool> answers)
    {
        UpdateResUI(type, score, themeID, answers);
        //uIElements.ResolutionScreenAnimator.SetInteger(resStateParaHash, 2);
        uIElements.MainCanvasGroup.blocksRaycasts = false;
        uIElements.ResolutionBG.transform.parent.GetComponent<CanvasGroup>().alpha = 1;

        if (type != ResolutionScreenType.Finish)
        {
            if (IE_DisplayTimedResolution != null)
            {
                StopCoroutine(IE_DisplayTimedResolution);
            }
            IE_DisplayTimedResolution = DisplayTimedResolution();
            StartCoroutine(IE_DisplayTimedResolution);
        }
    }
    IEnumerator DisplayTimedResolution()
    {
        yield return new WaitForSeconds(GameUtility.ResolutionDelayTime);
        //uIElements.ResolutionScreenAnimator.SetInteger(resStateParaHash, 1);
        uIElements.MainCanvasGroup.blocksRaycasts = true;
        uIElements.ResolutionBG.transform.parent.GetComponent<CanvasGroup>().alpha = 0;

        //
    }

    /// <summary>
    /// Function that is used to display resolution UI information.
    /// </summary>
    void UpdateResUI(ResolutionScreenType type, int score, int themeID, List<bool> answers)
    {
        //var highscore = PlayerPrefs.GetInt(GameUtility.SavePKPKey);
        //var highscore = events.PKP;
        uIElements.ResolutionTheme.sprite = parameters.themeStarsScreen[themeID];

        switch (type)
        {
            case ResolutionScreenType.Correct:
                uIElements.ResolutionBG.color = parameters.CorrectBGColor;
                uIElements.ResolutionStateInfoText.text = "Resposta certa!";
                //uIElements.ResolutionScoreText.text = "+" + score;
                break;
            case ResolutionScreenType.Incorrect:
                uIElements.ResolutionBG.color = parameters.IncorrectBGColor;
                uIElements.ResolutionStateInfoText.text = "Resposta errada :(";
                //uIElements.ResolutionScoreText.text = "-" + score;
                break;
            case ResolutionScreenType.Finish:
                //uIElements.ResolutionBG.color = parameters.FinalBGColor;
                //uIElements.ResolutionStateInfoText.text = "FINAL SCORE";
                //uIElements.ResolutionBG.transform.parent.GetComponent<CanvasGroup>().interactable = true;
                //uIElements.ResolutionBG.transform.parent.GetComponent<CanvasGroup>().blocksRaycasts = true;
                //StartCoroutine(CalculateScore());
                //uIElements.FinishUIElements.gameObject.SetActive(true);
                //uIElements.HighScoreText.gameObject.SetActive(true);
                //uIElements.HighScoreText.text = "PKP: " + highscore;
                StartCoroutine("DelayFinishResolution");
                break;
        }
        for (int i = 0; i < answers.Count; i++)
        {
            if(answers[i] == true) {
                uIElements.answersStars[i].sprite = parameters.correctAnswersStar;
                uIElements.answersStars[i].color = parameters.CorrectBGColor;
            }               
            else if(answers[i] == false){
                uIElements.answersStars[i].sprite = parameters.incorrectAnswersStar;
                uIElements.answersStars[i].color = parameters.IncorrectBGColor;
            }
            else{
                uIElements.answersStars[i].sprite = parameters.notAnsweredStar;
            }
        }
    }
    void ResetResolutionUI(){
        uIElements.answersStars[0].sprite = parameters.notAnsweredStar;
        uIElements.answersStars[0].color = Color.white;

        uIElements.answersStars[1].sprite = parameters.notAnsweredStar;
        uIElements.answersStars[1].color = Color.white;

        uIElements.answersStars[2].sprite = parameters.notAnsweredStar;
        uIElements.answersStars[2].color = Color.white;

    }
    IEnumerator DelayFinishResolution()
    {
        yield return new WaitForSeconds(GameUtility.ResolutionDelayTime);
        //uIElements.ResolutionBG.color = parameters.FinalBGColor;
        uIElements.ResolutionBG.transform.parent.GetComponent<CanvasGroup>().alpha = 0;
        //uIElements.ResolutionStateInfoText.text = "FINAL SCORE";
        uIElements.ResolutionBG.transform.parent.GetComponent<CanvasGroup>().interactable = false;
        uIElements.ResolutionBG.transform.parent.GetComponent<CanvasGroup>().blocksRaycasts = false;
        StartCoroutine(CalculateScore());
        uIElements.FinishUIElements.gameObject.SetActive(true);
        uIElements.HighScoreText.gameObject.SetActive(true);
        uIElements.HighScoreText.text = "PKP: " + events.PKP;
    }
    /// <summary>
    /// Function that is used to calculate and display the score.
    /// </summary>
    IEnumerator CalculateScore()
    {
        if (events.CurrentFinalScore == 0) { uIElements.ResolutionScoreText.text = 0.ToString(); yield break; }

        var scoreValue = 0;
        var scoreMoreThanZero = events.CurrentFinalScore > 0; 
        while (scoreMoreThanZero ? scoreValue < events.CurrentFinalScore : scoreValue > events.CurrentFinalScore)
        {
            scoreValue += scoreMoreThanZero ? 1 : -1;
            uIElements.ResolutionScoreText.text = scoreValue.ToString();

            yield return null;
        }
    }

    /// <summary>
    /// Function that is used to create new question answers.
    /// </summary>
    void CreateAnswers(Question2 question)
    {
        EraseAnswers();

        float offset = 0 - parameters.Margins;
        for (int i = 0; i < question.Answers.Length; i++)
        {
            AnswerData newAnswer = (AnswerData)Instantiate(answerPrefab, uIElements.AnswersContentArea);
            newAnswer.UpdateData(question.Answers[i].Info, i);

            newAnswer.Rect.anchoredPosition = new Vector2(0, offset);

            offset -= (newAnswer.Rect.sizeDelta.y + parameters.Margins);
            uIElements.AnswersContentArea.sizeDelta = new Vector2(uIElements.AnswersContentArea.sizeDelta.x, offset * -1);

            currentAnswers.Add(newAnswer);
        }
    }
    /// <summary>
    /// Function that is used to erase current created answers.
    /// </summary>
    void EraseAnswers()
    {
        foreach (var answer in currentAnswers)
        {
            Destroy(answer.gameObject);
        }
        currentAnswers.Clear();
    }

    /// <summary>
    /// Function that is used to update score text UI.
    /// </summary>
    void UpdateScoreUI()
    {
        uIElements.ScoreText.text = "Score: " + events.CurrentFinalScore;
    }

    void EndGamePanelStats(int themeId, List<bool> themeAnswersList, int DKPScore){
        endGameElements[endGameCounter].themeImg.sprite = parameters.themeStarsScreen[themeId];

        if(themeAnswersList[0] == true) 
            endGameElements[endGameCounter].answersStarsEnd[0].color = parameters.CorrectBGColor;
        else
             endGameElements[endGameCounter].answersStarsEnd[0].color = parameters.IncorrectBGColor;

        if(themeAnswersList[1] == true) 
            endGameElements[endGameCounter].answersStarsEnd[1].color = parameters.CorrectBGColor;
        else
             endGameElements[endGameCounter].answersStarsEnd[1].color = parameters.IncorrectBGColor;

        if(themeAnswersList[2] == true) 
            endGameElements[endGameCounter].answersStarsEnd[2].color = parameters.CorrectBGColor;
        else
             endGameElements[endGameCounter].answersStarsEnd[2].color = parameters.IncorrectBGColor;     

        endGameElements[endGameCounter].DKPText.text = DKPScore.ToString();
        endGameElements[endGameCounter].themeImg.color = themeInGameElements[endGameCounter].themeColor;
        endGameCounter++;
    }
}