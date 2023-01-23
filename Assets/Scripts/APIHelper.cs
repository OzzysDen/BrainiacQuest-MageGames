using UnityEngine;
using UnityEngine.Networking;
using System.Threading.Tasks;
using System.Linq;

public static class APIHelper
{
    public static async Task<string> GetLeaderBoard()
    {
        var url = "https://magegamessite.web.app/case1/leaderboard_page_1.json";
        using var www = UnityWebRequest.Get(url);
        www.SetRequestHeader("Content-Type", "application/json");
        var operation = www.SendWebRequest();

        while (!operation.isDone)
            await Task.Yield();

        if (www.result == UnityWebRequest.Result.Success)
        {
            //Debug.Log($"Success: {www.downloadHandler.text}");
            return www.downloadHandler.text;
        }
        else
        {
            //Debug.LogError($"Failed: {www.error}");
            return www.downloadHandler.error;
        }
    }
    public static async Task<string> GetQuestionsandAnswers()
    {
        var url = "https://magegamessite.web.app/case1/questions.json";
        using var www = UnityWebRequest.Get(url);
        www.SetRequestHeader("Content-Type", "application/json");
        var operation = www.SendWebRequest();

        while (!operation.isDone)
        {
            await Task.Yield();
        }
        if (www.result==UnityWebRequest.Result.Success)
        {
            return www.downloadHandler.text;
        }
        else
        {
            return www.downloadHandler.error;
        }
    }

    public static Color32 RandomColor()
    {
        return new Color32((byte)Random.Range(0,256), (byte)Random.Range(0, 256), (byte)Random.Range(0, 256), 255);
    }
}

