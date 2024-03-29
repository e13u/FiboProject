﻿using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameEvents", menuName = "Quiz/new GameEvents")]
public class GameEvents : ScriptableObject {

    // public delegate void    UpdateQuestionUICallback            (Question question);
    public delegate void    UpdateQuestionUICallback            (Question2 question);

    public                  UpdateQuestionUICallback            UpdateQuestionUI                = null;

    public delegate void    UpdateQuestionAnswerCallback        (AnswerData pickedAnswer);
    public                  UpdateQuestionAnswerCallback        UpdateQuestionAnswer            = null;

    public delegate void    DisplayResolutionScreenCallback     (UIManager.ResolutionScreenType type, int score, int themeID, List<bool> answers);
    public                  DisplayResolutionScreenCallback     DisplayResolutionScreen         = null;

    public delegate void    ScoreUpdatedCallback();
    public                  ScoreUpdatedCallback                ScoreUpdated                    = null;

    public delegate void AcceptAnswerCallback();
    public AcceptAnswerCallback AcceptAnswer = null;

    public delegate void    DisplayThemeScreenCallback(int themeId, float alphaPanel);
    public                  DisplayThemeScreenCallback     DisplayThemeScreen =null;

    public delegate void DisplayThemeAfterResolutionCallback();
    public DisplayThemeAfterResolutionCallback DisplayAfterResolution = null;

    public delegate void ResetResolutionUICallBack();
    public ResetResolutionUICallBack ResetResolutionUI = null;
    public int baseScore = 50;
    [Space]
    public                  int                                 round                           = 1;
    public                  int                                 maxRound                        = 6;
    [Space]
    public                  int                                 currentQuestionThemeNumber      = 0;
    public                  int                                 maxQuestionForTheme             = 3;
    [Space]
    public                  int                                 CurrentFinalScore               = 0;
    public                  int                                 StartupHighscore                = 0;
    [Space]
    [SerializeField]
    public          Dictionary<string, int>                     DKP = new Dictionary<string, int>();
    public                  float                                 PKP                             = 0;
    public List<int> DPKList = new List<int>();

    public delegate void    EndGamePanelStatsCallBack(int themeId, List<bool> themeAnswersList, int DKPScore);
    public                  EndGamePanelStatsCallBack     EndGamePanelStats = null; 

    public delegate void   ThemeInGameElementsCallBack(int themeId);
    public                 ThemeInGameElementsCallBack     ThemeInGameElements = null; 
}