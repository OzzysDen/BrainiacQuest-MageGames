using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class UIReferences : MonoBehaviour
{
    #region UI References
    //public Button LeaderboardButton;
    public Button LeaderboardBackButton;
    public GameObject LeaderboardPopup;
    public ScrollRect LeaderboardScroll;
    public GameObject LBMemberPrefab;
    public GameObject LBLoading;
    public Image LogoImage;
    public GameObject PlayButton;
    public GameObject LeaderboardButton;

    #endregion UI References

    /// <summary>
    /// Scene Dependent methods. Placed here, so if the Scene changes some button events would not be lost.
    /// </summary>

    public void ChangeToGameplayScene()
    {
        SceneManager.LoadSceneAsync(1);
    }





}
