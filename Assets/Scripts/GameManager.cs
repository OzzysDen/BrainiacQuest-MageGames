using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager m_instance;
    public static GameManager Instance => m_instance;

    [Header("Game & Player Data")]
    public PlayerProperties playerProperties;
    public QuestionHub questionHub;
    public int timeToAnswer=20;
    [Space]

    [Header("Class References")]
    public UIReferences uiReferences;
    public LeaderboardHandler leaderboardHandler;

    private void Awake()
    {
        if (m_instance != null)
            Destroy(gameObject);
        m_instance = this;
         DontDestroyOnLoad(gameObject);
        SceneManager.activeSceneChanged += OnActiveSceneChanged;
    }
    private void OnActiveSceneChanged(Scene arg0, Scene arg1)
    {
        if (arg1.buildIndex == 0)
        {
            uiReferences = GameObject.FindGameObjectWithTag("UIRef")?.GetComponent<UIReferences>();
            leaderboardHandler = GameObject.FindGameObjectWithTag("LBHandler").GetComponent<LeaderboardHandler>();
        }
    }

    private void Start()
    {
        GetandParseQuestions();
        SetPlayerName();
        AudioManager.Instance.PlayAudio("Intro");
        StartCoroutine(StartSequence());
    }

    private IEnumerator StartSequence()
    {
        for (int i = 0; i < 100; i++)
        {
            uiReferences.LogoImage.color = new Color(1,1,1,i*0.01f);
            yield return new WaitForSecondsRealtime(0.01f);
        }
        uiReferences.PlayButton.SetActive(true);
        uiReferences.LeaderboardButton.SetActive(true);
    }

    private async void GetandParseQuestions()
    {
        string QuestionData = await APIHelper.GetQuestionsandAnswers();
        string[] strings = QuestionData.Split("\n");
        QuestionData = "";
        for (int i = 0; i < strings.Length; i++)
        {
            QuestionData += strings[i];
        }
        questionHub = Newtonsoft.Json.JsonConvert.DeserializeObject<QuestionHub>(QuestionData);
        for (int i = 0; i < questionHub.questions.Count; i++)
        {
            questionHub.questions[i].answer =  (questionHub.questions[i].answer) switch
            {
                "A"=> "0",
                "B"=> "1",
                "C"=> "2",
                "D"=> "3",
            };
            /*
            switch (questionHub.questions[i].answer)
            {
                case "A":
                    questionHub.questions[i].answer = "0";
                    break;
                case "B":
                    questionHub.questions[i].answer = "1";
                    break;
                case "C":
                    questionHub.questions[i].answer = "2";
                    break;
                case "D":
                    questionHub.questions[i].answer = "3";
                    break;
            }*/
        }
    }

    private void SetPlayerName()
    {
        playerProperties.name = playerProperties.name == string.Empty ? "DummyPlayer" : "RealPlayername";
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }  
    }
#endif

}

[System.Serializable]
public struct PlayerProperties
{
    public string name;
    public PlayerResult Result;
}
[System.Serializable]
public class PlayerResult
{
    public int CorrectCount;
    public int WrongCount;
    public int NoAnswer;
    public int Points;
    public float CorrectPercent;
    public FamousWordsSO famousWords;
    public string GetFamousSentence()
    {
        return famousWords.FamousSentences[Random.Range(0, famousWords.FamousSentences.Count)].Sentence;
    }
}
[System.Serializable]
public class Question
{
    public string category;
    public string question;
    public List<string> choices;
    public string answer;
}
[System.Serializable]
public class QuestionHub
{
    public List<Question> questions;
}
