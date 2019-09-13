using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    #region Variables

    private             Data                data                    = new Data();

    [SerializeField]    GameEvents          events                  = null;

    [SerializeField]    Animator            timerAnimtor            = null;
    [SerializeField]    TextMeshProUGUI     timerText               = null;
    [SerializeField]    Color               timerHalfWayOutColor    = Color.yellow;
    [SerializeField]    Color               timerAlmostOutColor     = Color.red;
    private             Color               timerDefaultColor       = Color.white;
    private             int                 timeLeft                = 0;

    private             List<AnswerData>    PickedAnswers           = new List<AnswerData>();
    public             List<int>           FinishedQuestions       = new List<int>();
    private             int                 currentQuestion         = 0;
    private int correctAnswerStreak = 0;

    private             int                 timerStateParaHash      = 0;

    private             IEnumerator         IE_WaitTillNextRound    = null;
    private             IEnumerator         IE_StartTimer           = null;

    public List<int> selectedThemes = new List<int>();
    private int currentTheme = 0;

    //temp  
    public Text themeText;
    public Text roundText;

    private             bool                IsFinished
    {
        get
        {
            //return (FinishedQuestions.Count < data.Questions.Length) ? false : true;
            return (events.round > events.maxRound) ? true : false;
        }
    }

    private int currentDPK;

    #endregion

    #region Default Unity methods

    /// <summary>
    /// Function that is called when the object becomes enabled and active
    /// </summary>
    private void OnEnable()
    {
        events.UpdateQuestionAnswer += UpdateAnswers;
        events.AcceptAnswer += Accept;
    }
    /// <summary>
    /// Function that is called when the behaviour becomes disabled
    /// </summary>
    private void OnDisable()
    {
        events.UpdateQuestionAnswer -= UpdateAnswers;
        events.AcceptAnswer -= Accept;
    }

    /// <summary>
    /// Function that is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        try
        {
            selectedThemes = ThemeManager.Instance.themeList;
        }
        catch (System.Exception)
        {
            
            selectedThemes = new List<int> {1,2,3,4,5,6};
        }
        selectedThemes.Add(0);
        selectedThemes.Sort();
    }

    /// <summary>
    /// Function that is called when the script instance is being loaded.
    /// </summary>
    private void Start()
    {
        InitializeDPK();
        events.StartupHighscore = PlayerPrefs.GetInt(GameUtility.SavePKPKey);
        events.round = 1;
        roundText.text = "Round: "+events.round.ToString();

        //timerDefaultColor = timerText.color;

        //timerStateParaHash = Animator.StringToHash("TimerState");

        //var seed = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
        //UnityEngine.Random.InitState(seed);
        // selectedThemes = new List<int>(GameUtility.sortedThemes);

        SortTheme();
    }
    #endregion

    //Sorteia um dos temas da lista
    void SortTheme()
    {
        correctAnswerStreak = 0;
        currentTheme = selectedThemes[Random.Range(1, selectedThemes.Count)];
        selectedThemes.Remove(currentTheme);
        currentDPK = events.DKP[GameUtility.ThemeNameText(currentTheme)];
        LoadData();
        ThemeDisplay();
    }
    
    void ThemeDisplay(){
        //events.UpdateQuestionUI(null);
        events.DisplayThemeScreen(currentTheme, 1);
        StartCoroutine("ThemeDisplayWait");
    }
    IEnumerator ThemeDisplayWait(){
        yield return new WaitForSeconds(2);
        events.DisplayThemeScreen(currentTheme, 0);
        StartCoroutine(IE_WaitTillNextRound);
    }
    /// <summary>
    /// Function that is called to load data from the xml file.
    /// </summary>
    void LoadData()
    {
        //var path = Path.Combine(GameUtility.FileDir, GameUtility.FileName + events.round + ".xml
        string dificulty = VerifyTierForThemeDificulty(currentDPK);

        string themeFile = GameUtility.ThemeNameText(currentTheme) +"_"+ dificulty.ToString() + ".xml";

        themeText.text = themeFile;
        Debug.Log("TEMA+DIFICULDADE:" + themeFile);
        var path = Path.Combine(GameUtility.FileDir, themeFile);
        data = Data.Fetch(path);
        IE_WaitTillNextRound = WaitTillNextRound();
        //Corrotina chama o Display
    }

    /// <summary>
    /// Function that is called to update new selected answer.
    /// </summary>
    public void UpdateAnswers(AnswerData newAnswer)
    {
        if (data.Questions[currentQuestion].Type == AnswerType.Single)
        {
            foreach (var answer in PickedAnswers)
            {
                if (answer != newAnswer)
                {
                    answer.Reset();
                }
            }
            PickedAnswers.Clear();
            PickedAnswers.Add(newAnswer);
        }
        else
        {
            bool alreadyPicked = PickedAnswers.Exists(x => x == newAnswer);
            if (alreadyPicked)
            {
                PickedAnswers.Remove(newAnswer);
            }
            else
            {
                PickedAnswers.Add(newAnswer);
            }
        }
    }

    /// <summary>
    /// Function that is called to clear PickedAnswers list.
    /// </summary>
    public void EraseAnswers()
    {
        PickedAnswers = new List<AnswerData>();
    }

    /// <summary>
    /// Function that is called to display new question.
    /// </summary>
    void Display()
    {
        EraseAnswers();
        var question = GetRandomQuestion();

        if (events.UpdateQuestionUI != null)
        {
            events.UpdateQuestionUI(question);
        } else { Debug.LogWarning("Ups! Something went wrong while trying to display new Question UI Data. GameEvents.UpdateQuestionUI is null. Issue occured in GameManager.Display() method."); }

        if (question.UseTimer)
        {
            UpdateTimer(question.UseTimer);
        }
    }

    /// <summary>
    /// Function that is called to accept picked answers and check/display the result.
    /// </summary>
    public void Accept()
    {
        UpdateTimer(false);
        bool isCorrect = CheckAnswers();
        FinishedQuestions.Add(currentQuestion);

        if (isCorrect)
        {
            correctAnswerStreak++;
            //UpdateScore(data.Questions[currentQuestion].AddScore);
            int totalTime = data.Questions[currentQuestion].Timer;
            int timeCoeficent = timeLeft / totalTime;

            int score = events.baseScore * correctAnswerStreak;
            //Debug.Log("BaseScore: " + events.baseScore + "  " + "TimeCoeficent: "
                //+ timeCoeficent + "  " + "CorrectAnswerStreak: " + correctAnswerStreak);
            UpdateScore(currentTheme, score);
        }
        else
        {
            if(correctAnswerStreak > 0)
                correctAnswerStreak = 0;    
            correctAnswerStreak--;

            //UpdateScore(-data.Questions[currentQuestion].AddScore);
            int score = (correctAnswerStreak-1) * 10;
            UpdateScore(currentTheme, score);
        }

        events.currentQuestionThemeNumber++;
        if(events.currentQuestionThemeNumber > events.maxQuestionForTheme)
        {
            FinishedQuestions.Clear();
            events.currentQuestionThemeNumber = 1;
            events.round ++;
            roundText.text = "Round: " + events.round.ToString();


            if (IsFinished)
            {
                SetHighscore();
            }
            else
            {
                SortTheme();
            }
        }
        else{
            if (IE_WaitTillNextRound != null)
            {
                StopCoroutine(IE_WaitTillNextRound);
            }
            IE_WaitTillNextRound = WaitTillNextRound();
            StartCoroutine(IE_WaitTillNextRound);
        }


        var type
            = (IsFinished)
            ? UIManager.ResolutionScreenType.Finish
            : (isCorrect) ? UIManager.ResolutionScreenType.Correct
            : UIManager.ResolutionScreenType.Incorrect;

        events.DisplayResolutionScreen?.Invoke(type, data.Questions[currentQuestion].AddScore);

        AudioManager.Instance.PlaySound((isCorrect) ? "CorrectSFX" : "IncorrectSFX");
    }

    #region Timer Methods

    void UpdateTimer(bool state)
    {
        switch (state)
        {
            case true:
                IE_StartTimer = StartTimer();
                StartCoroutine(IE_StartTimer);

                timerAnimtor.SetInteger(timerStateParaHash, 2);
                break;
            case false:
                if (IE_StartTimer != null)
                {
                    StopCoroutine(IE_StartTimer);
                }

                //timerAnimtor.SetInteger(timerStateParaHash, 1);
                break;
        }
    }
    IEnumerator StartTimer()
    {
        var totalTime = data.Questions[currentQuestion].Timer;
        timeLeft = totalTime;

        timerText.color = timerDefaultColor;
        while (timeLeft > 0)
        {
            timeLeft--;

            AudioManager.Instance.PlaySound("CountdownSFX");

            if (timeLeft < totalTime / 2 && timeLeft > totalTime / 4)
            {
                timerText.color = timerHalfWayOutColor;
            }
            if (timeLeft < totalTime / 4)
            {
                timerText.color = timerAlmostOutColor;
            }

            timerText.text = timeLeft.ToString();
            yield return new WaitForSeconds(1.0f);
        }
        Accept();
    }
    IEnumerator WaitTillNextRound()
    {
        yield return new WaitForSeconds(GameUtility.ResolutionDelayTime);
        Display();
    }

    #endregion

    /// <summary>
    /// Function that is called to check currently picked answers and return the result.
    /// </summary>
    bool CheckAnswers()
    {
        if (!CompareAnswers())
        {
            return false;
        }
        return true;
    }
    /// <summary>
    /// Function that is called to compare picked answers with question correct answers.
    /// </summary>
    bool CompareAnswers()
    {
        if (PickedAnswers.Count > 0)
        {
            List<int> c = data.Questions[currentQuestion].GetCorrectAnswers();
            List<int> p = PickedAnswers.Select(x => x.AnswerIndex).ToList();

            var f = c.Except(p).ToList();
            var s = p.Except(c).ToList();

            return !f.Any() && !s.Any();
        }
        return false;
    }

    /// <summary>
    /// Function that is called restart the game.
    /// </summary>
    public void RestartGame()
    {
        //If next level is the first level, meaning that we start playing a game again, reset the final score.
        if (events.round == 1) { events.CurrentFinalScore = 0; }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    /// <summary>
    /// Function that is called to quit the application.
    /// </summary>
    public void QuitGame()
    {
        //On quit reset the current level back to the first level.
        events.round = 1;

        Application.Quit();
    }

    /// <summary>
    /// Function that is called to set new highscore if game score is higher.
    /// </summary>
    private void SetHighscore()
    {
        UpdatePKP();
        var pkp = PlayerPrefs.GetInt(GameUtility.SavePKPKey);
        //if (pkp < events.CurrentFinalScore)
        //{
        PlayerPrefs.SetFloat(GameUtility.SavePKPKey, events.PKP);
        //}
    }
    /// <summary>
    /// Function that is called update the score and update the UI.
    /// </summary>
    private void UpdateScore(int themeiD, int add)
    {
        //events.DKP[GameUtility.ThemeNameText(themeiD)] += add;
        UpdateDKP(GameUtility.ThemeNameText(themeiD), add);
        events.ScoreUpdated?.Invoke();
    }

    #region Getters

    Question GetRandomQuestion()
    {
        var randomIndex = GetRandomQuestionIndex();
        currentQuestion = randomIndex;

        return data.Questions[currentQuestion];
    }
    int GetRandomQuestionIndex()
    {
        var random = 0;
        if (FinishedQuestions.Count < data.Questions.Length)
        {
            do
            {
                random = UnityEngine.Random.Range(0, data.Questions.Length);
            } while (FinishedQuestions.Contains(random) || random == currentQuestion);
        }
        return random;
    }

    #endregion
    #region DKP
    void InitializeDPK()
    {
        events.DKP.Clear();
        events.DPKList.Clear();
        events.DKP.Add ("Teste",0);
        events.DKP.Add("Portugues", 100);//1
        events.DKP.Add("Biologia", 100);//2
        events.DKP.Add("Geografia", 100);//3
        events.DKP.Add("Artes", 100);//4
        events.DKP.Add("Matematica", 100);//5
        events.DKP.Add("Filosofia", 100);//6
        events.DKP.Add("Fisica", 100);//7
        events.DKP.Add("Historia", 100);//8
        events.DKP.Add("Sociologia", 100);//9


        for (int i = 0; i < events.DKP.Count; i++)
        {
            events.DPKList.Add(100);
        }
    }

    void UpdateDKP(string theme, int scoreDKP)
    {
        events.DKP[theme] += scoreDKP;
        if (events.DKP[theme] < 0)
            events.DKP[theme] = 0;
        events.DPKList[GameUtility.ThemeNameText(theme)] += scoreDKP;
        if (events.DPKList[GameUtility.ThemeNameText(theme)] < 0)
            events.DPKList[GameUtility.ThemeNameText(theme)] = 0;
    }

    void UpdatePKP()
    {
        int total = 0;
        for (int i = 1; i < events.DKP.Count; i++)
        {
            Debug.Log(GameUtility.ThemeNameText(i));
            total += events.DKP[GameUtility.ThemeNameText(i)];
        }
        Debug.Log(total);
        float media = total / 9;
        Debug.Log(media);
        events.PKP = media;
    }

    string VerifyTierForThemeDificulty(int tier)
    {
        if(tier < 200)
            return "Easy";
        else if (tier < 500)
            return "Medium";
        else
            return "Hard";
    }
    #endregion

}