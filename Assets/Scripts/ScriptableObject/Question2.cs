using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable()]
public struct Answer2
{
    [SerializeField] private string _info;
    public string Info { get { return _info; } }

    [SerializeField] private bool _isCorrect;
    public bool IsCorrect { get { return _isCorrect; } }
}
[CreateAssetMenu(fileName = "New Question", menuName = "Quiz/new Question")]
public class Question2 : ScriptableObject {

    public enum                 AnswerType2                  { Multi, Single }

    [SerializeField] private    String      _info           = String.Empty;
    public                      String      Info            { get { return _info; } }

    [SerializeField]            Answer2[]    _answers        = null;
    public                      Answer2[]    Answers         { get { return _answers; } }

    //Parameters

    [SerializeField] private    bool        _useTimer       = true;
    public                      bool        UseTimer        { get { return _useTimer; } }

    [SerializeField] private    int         _timer          = 10;
    public                      int         Timer           { get { return _timer; } }

    [SerializeField] private    AnswerType2  _answerType     = AnswerType2.Single;
    public                      AnswerType2  GetAnswerType   { get { return _answerType; } }

    [SerializeField] private    int         _addScore       = 10;
    public                      int         AddScore        { get { return _addScore; } }

    /// <summary>
    /// Function that is called to collect and return correct answers indexes.
    /// </summary>
    public List<int> GetCorrectAnswers ()
    {
        List<int> CorrectAnswers = new List<int>();
        for (int i = 0; i < Answers.Length; i++)
        {
            if (Answers[i].IsCorrect)
            {
                CorrectAnswers.Add(i);
            }
        }
        return CorrectAnswers;
    }
}