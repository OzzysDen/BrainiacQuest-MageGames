using System.Collections;
using UnityEngine;
using PlayerUIAnimator;
using System.Linq;

public class GameplayManager : MonoBehaviour
{
    //Reference
    public GameplayUIReferences GameplayUI;
    public int Timer = 0;
    private int CurrentQuestionIndex = 0;
    public Question CurrentQuestion=new();
    private Coroutine TimerRoutine;

    private void Start()
    {
        SetCurrentQuestion(true);
    }
    private void SetCurrentQuestion(bool isInitial=false)
    {
        if (CurrentQuestion==GameManager.Instance.questionHub.questions.Last())
        {
            //Play end screen;
            StartCoroutine(GameEndSequence());
            return;
        }

        if (isInitial)
        {
            CurrentQuestionIndex=0;
            CurrentQuestion = GetNewQuestion(CurrentQuestionIndex);
            StartCoroutine(SetUIForNextQuest(isInitial));
        }
        else
        {
            CurrentQuestionIndex++;
            CurrentQuestion = GetNewQuestion(CurrentQuestionIndex);
            StartCoroutine(SetUIForNextQuest(isInitial, 1));
        }
        //Debug.Log(CurrentQuestionIndex + " current Q Index");

    }
    private Question GetNewQuestion(int qIndex) { return GameManager.Instance.questionHub.questions[qIndex]; }
    private IEnumerator GameEndSequence()
    {
        //Set UI
        GameplayUI.CorrectText.SetText($"Correct: <color=green><size=120%>{GameManager.Instance.playerProperties.Result.CorrectCount}");
        GameplayUI.WrongText.SetText($"Wrong: <color=red><size=120%>{GameManager.Instance.playerProperties.Result.WrongCount}");
        GameplayUI.NotAnswered.SetText($"Not Answered: <color=red><size=120%>{GameManager.Instance.playerProperties.Result.NoAnswer}");
        GameplayUI.PlayerPointsText.SetText($"Points: <color=orange><size=120%>{GameManager.Instance.playerProperties.Result.Points}");
        GameplayUI.ResultPercentText.SetText($"You answered correctly\n <size=120%><color=orange>{GameManager.Instance.playerProperties.Result.CorrectCount*100/ GameManager.Instance.questionHub.questions.Count}%</color></size> of the questions.");
        GameplayUI.FamousSentenceText.SetText(GameManager.Instance.playerProperties.Result.GetFamousSentence());

        //Hide Unneccesary things
        GameplayUI.QuestionContent.SetActive(false);
        GameplayUI.PointContent.SetActive(false);
        GameplayUI.TimeContent.SetActive(false);
        yield return new WaitForSecondsRealtime(0.5f);

        //Enable Objects
        GameplayUI.EndScreen.SetActive(true);
        AudioManager.Instance.PlayAudio("Popup");
        yield return new WaitForSecondsRealtime(0.5f);

        GameplayUI.CorrectText.gameObject.SetActive(true);
        AudioManager.Instance.PlayAudio("ResultPopup");
        yield return new WaitForSecondsRealtime(0.5f);
        
        GameplayUI.WrongText.gameObject.SetActive(true);
        AudioManager.Instance.PlayAudio("ResultPopup");
        yield return new WaitForSecondsRealtime(0.5f);

        GameplayUI.NotAnswered.gameObject.SetActive(true);
        AudioManager.Instance.PlayAudio("ResultPopup");
        yield return new WaitForSecondsRealtime(0.5f);

        GameplayUI.PlayerPointsText.gameObject.SetActive(true);
        AudioManager.Instance.PlayAudio("ResultPopup");
        yield return new WaitForSecondsRealtime(0.5f);
        
        GameplayUI.ResultPercentText.gameObject.SetActive(true);
        AudioManager.Instance.PlayAudio("ResultPopup");
        yield return new WaitForSecondsRealtime(0.5f);
        
        GameplayUI.FamousSentenceText.gameObject.SetActive(true);
        AudioManager.Instance.PlayAudio("ResultPopup");
        yield return new WaitForSecondsRealtime(2f);

        //Show Bio
        GameplayUI.DevBioContent.SetActive(true);
    }
    private IEnumerator SetUIForNextQuest(bool isInitial=false, float waitBeforeStart=0)
    {
        ResetUI();
        yield return new WaitForSecondsRealtime(waitBeforeStart);
        if (isInitial)
        {
            GameplayUI.ReadyContentText.gameObject.SetActive(true);
            for (int i = 0; i < 3; i++)
            {
                GameplayUI.ReadyContentText.SetText($"Ready?\n<color=green><size=125%>{3-i}");
                yield return new WaitForSecondsRealtime(1);
            }
            GameplayUI.ReadyContentText.gameObject.SetActive(false);
        }

        GameplayUI.PointsText.SetText($"<color=orange><size=120%>{GameManager.Instance.playerProperties.Result.Points}</size></color>");
        GameplayUI.CategoryText.SetText(CurrentQuestion.category.ToUpper());
        GameplayUI.CategoryText.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(1);
        GameplayUI.QuestionText.SetText(CurrentQuestion.question);
        GameplayUI.QuestionBody.SetActive(true);
        GameplayUI.QuestionNumberText.SetText($"{CurrentQuestionIndex+1}/{GameManager.Instance.questionHub.questions.Count}");
        //yield return new WaitForSecondsRealtime(1f);
        //GameplayUI.qTextWriter.Write();
        yield return new WaitForSecondsRealtime(0.5f);
        for (int i = 0; i < 4; i++)
        {
            GameplayUI.AnswerUIElements[i].AnswerButtonText.SetText(CurrentQuestion.choices[i]);
            GameplayUI.AnswerUIElements[i].AnswerButton.gameObject.SetActive(true);
            AudioManager.Instance.PlayAudio("Popup");
            yield return new WaitForSecondsRealtime(0.35f);
        }
        SetAnswerButtonsInteractable(true);
        TimerRoutine = StartCoroutine(TimerController());
        yield return null;
    }
    private IEnumerator TimerController()
    {
        GameplayUI.TimerText.GetComponent<ScaleTween>().Play();
        for (int i = GameManager.Instance.timeToAnswer; i > -1; i--)
        {
            Timer = i;
            GameplayUI.TimerText.SetText($"<color=red><size=120%>{Timer}</size></color>");
            if (Timer<=5)
            {
                GameplayUI.TimerText.GetComponent<ScaleTween>().Play();
                AudioManager.Instance.PlayAudio("Clock");
            }
            yield return new WaitForSecondsRealtime(1);
            if (Timer==0)
            {
                SetAnswerButtonsInteractable(active:false, isTimeout:true);
                GameManager.Instance.playerProperties.Result.NoAnswer++;
                SetPlayerPoint(false);
                //show correct answer
                GameplayUI.AnswerUIElements[int.Parse(CurrentQuestion.answer)].AnswerButton.image.color = GameplayUI.ButtonCorrectColor;
                //Wait after timeout
                yield return new WaitForSecondsRealtime(3);
                SetCurrentQuestion(isInitial:false);
            }
        }
        yield return null;
    }
    private IEnumerator CheckAnswer(int answerIndex)
    {
        //set buttons passive
        SetAnswerButtonsInteractable(false);
        GameplayUI.AnswerUIElements[answerIndex].AnswerButton.image.color = GameplayUI.ButtonSelectedColor;
        yield return new WaitForSecondsRealtime(1);
        GameplayUI.AnswerUIElements[int.Parse(CurrentQuestion.answer)].AnswerButton.image.color = GameplayUI.ButtonCorrectColor;
        //Wrong answer handle:
        if (answerIndex != int.Parse(CurrentQuestion.answer))
        {
            GameManager.Instance.playerProperties.Result.WrongCount++;
            AudioManager.Instance.PlayAudio("Wrong");
            GameplayUI.AnswerUIElements[answerIndex].AnswerButton.image.color = GameplayUI.ButtonWrongColor;
            SetPlayerPoint(false);
            yield return new WaitForSecondsRealtime(GameplayUI.WaitAfterWrong);
        }
        //Correct answer handle:
        else
        {
            GameManager.Instance.playerProperties.Result.CorrectCount++;
            SetPlayerPoint(true);
            AudioManager.Instance.PlayAudio("Correct");

            yield return new WaitForSecondsRealtime(GameplayUI.WaitAfterCorrect);
        }
        SetCurrentQuestion();
        yield return null;
    }
    private IEnumerator SetPointText(int pointDiff, bool isCorrect)
    {
        if (isCorrect)
        {
            GameplayUI.CorrectParticles.StartParticleEmission();
        }
        for (int i = 1; i < pointDiff+1; i++)
        {
            GameplayUI.PointsText.GetComponent<ScaleTween>().Play();
            GameplayUI.PointsText.SetText(isCorrect==true?$"<color=orange><size=120%>{GameManager.Instance.playerProperties.Result.Points - pointDiff + i}</size></color>": $"<color=orange><size=120%>{GameManager.Instance.playerProperties.Result.Points-(i-pointDiff)}</size></color>");
            yield return new WaitForSecondsRealtime(0.15f);
        }
    }
    private void ResetUI()
    {
        SetAnswerButtonsInteractable(false);
        GameplayUI.ReadyContentText.gameObject.SetActive(false);
        GameplayUI.TimerText.SetText("<color=red><size=120%>--</size></color>");
        GameplayUI.PointsText.SetText($"<color=orange><size=120%>{GameManager.Instance.playerProperties.Result.Points}</size></color>");
        GameplayUI.CategoryText.gameObject.SetActive(false);
        GameplayUI.QuestionBody.SetActive(false);
        GameplayUI.QuestionText.SetText("");
        
        for (int i = 0; i < GameplayUI.AnswerUIElements.Count; i++)
        {
            GameplayUI.AnswerUIElements[i].AnswerButton.gameObject.SetActive(false);
            GameplayUI.AnswerUIElements[i].AnswerButton.image.color = GameplayUI.ButtonIdleColor;
        }
    }
    private void StopTimer()
    {
        StopCoroutine(TimerRoutine);
        TimerRoutine=null;
        Timer = 0;
        GameplayUI.TimerText.SetText("<color=red><size=120%>--</size></color>");
    }
    private void SetAnswerButtonsInteractable(bool active, bool isTimeout=false)
    {
        for (int i = 0; i < 4; i++)
        {
            GameplayUI.AnswerUIElements[i].AnswerButton.interactable = active;
            if (isTimeout)
            {
                GameplayUI.AnswerUIElements[i].AnswerButton.image.color = GameplayUI.ButtonDeactiveColor;
                if (i == 3) AudioManager.Instance.PlayAudio("Timeout");
            }
        }
    }
    private void SetPlayerPoint(bool answer)
    {
        int pointDiff = GameManager.Instance.playerProperties.Result.Points;
        GameManager.Instance.playerProperties.Result.Points = answer ? GameManager.Instance.playerProperties.Result.Points += 10 : GameManager.Instance.playerProperties.Result.Points -= 5;
        pointDiff = pointDiff - GameManager.Instance.playerProperties.Result.Points;
        if (answer==true)
        {
            StartCoroutine(SetPointText(Mathf.Abs(pointDiff), isCorrect: true));
        }
        else if (answer==false)
        {
            StartCoroutine(SetPointText(Mathf.Abs(pointDiff), isCorrect: false));
        }
    }
    public void AnswerButtonEvent(int answerIndex)
    {
        StartCoroutine(CheckAnswer(answerIndex));
        AudioManager.Instance.PlayAudio("SelectAnswer");
        StopTimer();
    }
}
