using GoogleMobileAds.Api;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIGame : MonoBehaviour
{
    [SerializeField] private TMP_Text[] levelNumberText;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button skipButton;

    [SerializeField] private LevelManager levelManager;
    [SerializeField] private UIPause pauseUI;

    private Animator pauseButtonAnim;

    private bool isPaused;

    private void Awake() {
        pauseButtonAnim = pauseButton.GetComponent<Animator>();

        RewardedAdController.OnAdLoaded += RewardedAdController_OnAdLoaded;
    }

    private void RewardedAdController_OnAdLoaded() {
        ShowSkipButton();
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.K)) {
            ShowSkipButton();
        }
    }

    public void ChangeLevel(int level) {
        foreach(TMP_Text text in levelNumberText) {
            text.SetText("Level " + level);
        }

        if(RewardedAdController.IsReady()) {
            ShowSkipButton();
        } else {
            HideSkipButton();
        }
    }

    public void ShowSkipButton() {
        RectTransform restartRect = restartButton.GetComponent<RectTransform>();
        restartRect.anchoredPosition = new Vector3(-180, -50);

        skipButton.gameObject.SetActive(true);
    }
    public void HideSkipButton() {
        RectTransform restartRect = restartButton.GetComponent<RectTransform>();
        restartRect.anchoredPosition = new Vector3(-50, -50);

        skipButton.gameObject.SetActive(false);
    }

    public void PauseLevel() {
        isPaused = !isPaused;

        if(isPaused) {
            pauseButtonAnim.Play("Pause");
            levelManager.PauseLevel();
            pauseUI.ShowPanel();
        } else {
            pauseButtonAnim.Play("Resume");
            levelManager.ResumeLevel();
            pauseUI.HidePanel();
        }
    }

    public void RestartLevel() {
        if(isPaused) {
            PauseLevel();
        }

        levelManager.RestartLevel();
    }

    public void SkipLevel() {
        RewardedAdController.ShowAd((Reward reward) => {
            if(!RewardedAdController.IsReady()) {
                HideSkipButton();
            }

            levelManager.SkipLevel();
        });
    }
}
