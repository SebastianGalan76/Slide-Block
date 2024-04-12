using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public int stage, level;

    [SerializeField] private MovementSystem movementSystem;
    [SerializeField] private PlatformManager platformManager;

    [Header("User Interface")]
    [SerializeField] private UIGame gameUI;
    [SerializeField] private UIInfo infoUI;

    [SerializeField] private UILevelComplete levelCompleteUI;
    [SerializeField] private UILevelFailed levelFailedUI;

    private UserDataManager userDataManager;

    private void Start() {
        stage = PlayerPrefs.GetInt("Stage");
        level = PlayerPrefs.GetInt("Level");

        BannerAdController.GetInstance().LoadAd();
        RewardedAdController.GetInstance().LoadAd();
        InterstitialAdController.GetInstance().LoadAds();

        userDataManager = UserDataManager.GetInstance();

        StartLevel(stage, level);
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.M)) {
            StartNextLevel();
        }
    }

    public void CompleteLevel() {
        movementSystem.enabled = false;
        userDataManager.CompleteLevel(stage, level);

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
        userDataManager.UnlockNextLevel(stage, level);
        StartNextLevel();
    }

    private void StartLevel(int stage, int level) {
        if(!LevelDataManager.GetInstance().LevelExist(stage, level)) {
            infoUI.ShowInfo(InfoType.FINISHED_ALL_LEVELS, delegate () {
                SceneManager.LoadScene("MainMenu");
            });

            return;
        }

        platformManager.LoadLevel(stage, level);

        movementSystem.enabled = true;
        gameUI.ChangeLevel(level);

        Time.timeScale = 1;
        AdSystem.GetInstance().ChangeAdValue(1);

        if(stage > 1 || level > 10) {
            InterstitialAdController.GetInstance().CheckAdValue();
        }

        PlayerPrefs.SetInt("LastPlayedStage", stage);
        PlayerPrefs.SetInt("LastPlayedLevel", level);
        PlayerPrefs.Save();
    }
}
