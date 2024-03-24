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
    }

    public void ShowSkipButton() {
        RectTransform restartRect = restartButton.GetComponent<RectTransform>();
        restartRect.anchoredPosition = new Vector3(-180, -50);

        skipButton.gameObject.SetActive(true);
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
}
