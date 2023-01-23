using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class LeaderboardHandler : MonoBehaviour
{
    public List<LeaderBoardPage> leaderBoardPages=new();
    [Tooltip("If it is checked, the leardboard can be refreshed when swiping down from the top.")]
    public bool LeaderboardRefreshable;
    private void Start()
    {
        GetandSetLBDataAsync();
    }

    private async void GetandSetLBDataAsync()
    {
        //Get Leaderboard Data
        string LBData = await APIHelper.GetLeaderBoard();
        //Parse Leaderboard Data to Class
        ParseLeaderboard(LBData);
        CreateLeaderboardMembers(leaderBoardPages);
    }
    public void OnLeaderboardScroll(RectTransform rectTransform)
    {
        if (!GameManager.Instance.leaderboardHandler.LeaderboardRefreshable){ return;}
        if (rectTransform.anchoredPosition.y < -100)
        {
            Debug.Log("Loading");
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, -100);
            GameManager.Instance.uiReferences.LeaderboardScroll.enabled = false;
            GameManager.Instance.uiReferences.LBLoading.SetActive(true);
        }
    }

    private void CreateLeaderboardMembers(List<LeaderBoardPage> leaderBoardPages)
    {
        if (leaderBoardPages.Count == 0) { return; }

#if UNITY_EDITOR //DEBUG
        //leaderBoardPages[0].data.AddRange(leaderBoardPages[0].data);
#endif //END DEBUG

        for (int i = 0; i < leaderBoardPages.Count; i++)
        {
            for (int k = 0; k < leaderBoardPages[i].data.Count; k++)
            {
                RectTransform lbMemberRect = Instantiate(GameManager.Instance.uiReferences.LBMemberPrefab, GameManager.Instance.uiReferences.LeaderboardScroll.content).GetComponent<RectTransform>();
                lbMemberRect.anchoredPosition = new Vector2(0, (lbMemberRect.rect.size.y) * -k);
                lbMemberRect.GetComponent<LeaderboardMemberSetter>().leaderboardMember = leaderBoardPages[i].data[k];
                lbMemberRect.GetComponent<LeaderboardMemberSetter>().ConfigureCell(leaderBoardPages[i].data[k]);
            }
        }
        GameManager.Instance.uiReferences.LeaderboardScroll.content.sizeDelta = new Vector2(GameManager.Instance.uiReferences.LeaderboardScroll.content.sizeDelta.x, GameManager.Instance.uiReferences.LeaderboardScroll.content.childCount*80);
        GameManager.Instance.uiReferences.LeaderboardScroll.GetComponent<UI_InfiniteScroll>().Init();
        GameManager.Instance.uiReferences.LeaderboardScroll.GetComponent<UI_ScrollRectOcclusion>().Init();
        GameManager.Instance.uiReferences.LeaderboardBackButton.gameObject.SetActive(true);
    }

    public void CloseLeaderboard()
    {
        GameManager.Instance.uiReferences.LeaderboardPopup.gameObject.SetActive(false);
        GameManager.Instance.uiReferences.LBLoading.SetActive(false);
        GameManager.Instance.uiReferences.LeaderboardScroll.content.anchoredPosition = Vector2.zero;
        return; //If needed to clear the list on close
        for (int i = 0; i < GameManager.Instance.uiReferences.LeaderboardScroll.content.transform.childCount; i++)
        {
            Destroy(GameManager.Instance.uiReferences.LeaderboardScroll.content.transform.GetChild(i).gameObject);
        }
    }

    private void ParseLeaderboard(string lbPageJson)
    {
        LeaderBoardPage lbPage = Newtonsoft.Json.JsonConvert.DeserializeObject<LeaderBoardPage>(lbPageJson);
        leaderBoardPages = new();
        leaderBoardPages.Add(lbPage);
        //Note: If there are more than 1 pages, this needs a loop to add all data to the list!
    }

    public void OnLeaderboardButton()
    {
        GameManager.Instance.uiReferences.LeaderboardBackButton.gameObject.SetActive(false);
        GameManager.Instance.uiReferences.LeaderboardPopup.gameObject.SetActive(true);
        GameManager.Instance.uiReferences.LeaderboardBackButton.gameObject.SetActive(true);
    }
}

[System.Serializable]
public class LeaderBoardPage
{
    public int page;
    public bool is_last;
    public List<LeaderboardMember> data = new();
}

[System.Serializable]
public class LeaderboardMember
{
    public int rank;
    public string nickname;
    public int score;
}

