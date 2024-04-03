using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public int stage, level;

    [SerializeField] private MovementSystem movementSystem;
    [SerializeField] private PlatformManager platformManager;

    [Header("User Interface")]
    [SerializeField] private UIGame gameUI;
    [SerializeField] private UILevelComplete levelCompleteUI;
    [SerializeField] private UILevelFailed levelFailedUI;

    private void Start() {
        //stage = PlayerPrefs.GetInt("Stage");
        //level = PlayerPrefs.GetInt("Level");

        BannerAdController.LoadAd();
        RewardedAdController.LoadAd();
        InterstitialAdController.LoadAds();

        StartLevel(stage, level);
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.M)) {
            StartNextLevel();
        }
    }

    public void CompleteLevel() {
        movementSystem.enabled = false;
        UserData.CompleteLevel(stage, level);

        levelCompleteUI.ShowPanel(level);
    }

    public void LostLevel() {
        movementSystem.enabled = false;
        levelFailedUI.ShowPanel();
    }

    public void PauseLevel() {
        movementSystem.enabled = false;

        Time.timeScale = 0;
    }

    public void ResumeLevel() {
        movementSystem.enabled = true;

        Time.timeScale = 1;
    }

    public void RestartLevel() {
        StartLevel(stage, level);
    }

    public void StartNextLevel() {
        level++;
        if(level > 100) {
            stage++;
            level = 1;
        }

        StartLevel(stage, level);
    }

    public void SkipLevel() {
        UserData.SkipLevel(stage, level);
        StartNextLevel();
    }

    private void StartLevel(int stage, int level) {
        platformManager.LoadLevel(stage, level);

        movementSystem.enabled = true;
        gameUI.ChangeLevel(level);

        Time.timeScale = 1;
        AdSystem.ChangeAdValue(1);

        if(stage > 1 || level > 10) {
            InterstitialAdController.CheckAdValue();
        }

        PlayerPrefs.SetInt("LastPlayedStage", stage);
        PlayerPrefs.SetInt("LastPlayedLevel", level);
        PlayerPrefs.Save();
    }
}
