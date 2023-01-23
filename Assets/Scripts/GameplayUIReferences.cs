using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.UI.Extensions;

public class GameplayUIReferences : MonoBehaviour
{
    public GameObject QuestionContent;
    public TextMeshProUGUI ReadyContentText;
    public TextMeshProUGUI TimerText;
    public TextMeshProUGUI PointsText;
    [Space]
    public TextMeshProUGUI CategoryText;
    public GameObject QuestionBody;
    public TextMeshProUGUI QuestionText;
    public TextWriter qTextWriter;
    public TextMeshProUGUI QuestionNumberText;
    public TextMeshProUGUI FailedText;
    [Space]
    public List<AnswerUI> AnswerUIElements;
    [Space]
    public Color32 ButtonIdleColor;
    public Color32 ButtonSelectedColor;
    public Color32 ButtonCorrectColor;
    public Color32 ButtonWrongColor;
    public Color32 ButtonDeactiveColor;
    [Space]
    public UIParticleSystem CorrectParticles;
    [Space]
    [Header("Wait Time in Seconds")]
    public float WaitAfterCorrect;
    public float WaitAfterWrong;

    [Space]
    [Header("Game End")]
    public GameObject EndScreen;
    public TextMeshProUGUI ResultText;
    public TextMeshProUGUI CorrectText;
    public TextMeshProUGUI WrongText;
    public TextMeshProUGUI NotAnswered;
    public TextMeshProUGUI PlayerPointsText;
    public TextMeshProUGUI ResultPercentText;
    public TextMeshProUGUI FamousSentenceText;
    [Space]
    public GameObject PointContent;
    public GameObject TimeContent;
    [Space]
    [Header("Bio")]
    public GameObject DevBioContent;
}

[System.Serializable]
public struct AnswerUI
{
    public Button AnswerButton;
    public TextMeshProUGUI AnswerButtonText;

}
