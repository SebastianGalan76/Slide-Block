using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMainMenu : MonoBehaviour
{
    [SerializeField] private GameObject levelListPanel;
    [SerializeField] private TMP_Text starAmountTotal;

    [SerializeField] private TMP_Text[] stageStarAmount;

    private Animator animator;

    private void Awake() {
        starAmountTotal.SetText(UserData.GetTotalStarAmount().ToString());
        animator = GetComponent<Animator>();

        Time.timeScale = 1;

        AdSystem.InitializeAdMob();
        BannerAdController.HideAd();

        Leaderboard.Authenticate();
    }

    public void PlayGame() {
        int stage = PlayerPrefs.GetInt("LastPlayedStage", 1);
        int level = PlayerPrefs.GetInt("LastPlayedLevel", 1);

        PlayerPrefs.SetInt("Stage", stage);
        PlayerPrefs.SetInt("Level", level);
        PlayerPrefs.Save();

        SceneManager.LoadScene("Game");
    }

    public void ShowLevelList() {
        animator.Play("ShowLevelList");

        stageStarAmount[0].SetText(UserData.GetStarAmountForStage(1).ToString());
    }

    public void HideLevelList() {
        animator.Play("HideLevelList");
    }

    public void ShowLeaderboard() {
        Leaderboard.Show();
    }
}
