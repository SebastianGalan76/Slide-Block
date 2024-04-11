using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMainMenu : MonoBehaviour
{
    [SerializeField] private GameObject levelListPanel;
    [SerializeField] private GameObject settingsPanel;

    [SerializeField] private TMP_Text starAmountTotal;

    [SerializeField] private TMP_Text[] stageStarAmount;

    private Animator animator;

    private void Awake() {
        starAmountTotal.SetText(UserDataManager.GetInstance().GetTotalStarAmount().ToString());
        animator = GetComponent<Animator>();

        Time.timeScale = 1;

        AdSystem.GetInstance().InitializeAdMob();
        BannerAdController.GetInstance().HideAd();
        RewardedAdController.GetInstance().LoadAd();

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

        stageStarAmount[0].SetText(UserDataManager.GetInstance().GetStarAmountForStage(1).ToString());
    }

    public void HideLevelList() {
        animator.Play("HideLevelList");
    }

    public void ShowSettings() {
        settingsPanel.SetActive(true);
    }

    public void HideSettings() {
        settingsPanel.SetActive(false);
    }

    public void ShowLeaderboard() {
        Leaderboard.Show();
    }
}
