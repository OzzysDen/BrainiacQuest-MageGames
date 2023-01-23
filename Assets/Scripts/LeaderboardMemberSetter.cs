using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardMemberSetter : MonoBehaviour
{
    public LeaderboardMember leaderboardMember=new();
    //UI References
    public TextMeshProUGUI RankText;
    public TextMeshProUGUI NicknameText;
    public TextMeshProUGUI ScoreText;
    public Image BGColorIMG;



    public void ConfigureCell(LeaderboardMember lbMember)
    {
        
        RankText.SetText(lbMember.rank.ToString());
        NicknameText.SetText(lbMember.nickname.ToString());
        ScoreText.SetText(lbMember.score.ToString());
        BGColorIMG.color = APIHelper.RandomColor();
    }
}

