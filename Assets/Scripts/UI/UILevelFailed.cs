using GoogleMobileAds.Api;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UILevelFailed : MonoBehaviour
{
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private UIInfo infoUI;

    [SerializeField] private TMP_Text countdownText;
    [SerializeField] private GameObject restartButton;

    private Animator panelAnimator;
    private int countdownValue;
    private bool isShowed;

    private void Awake() {
        panelAnimator = GetComponent<Animator>();
    }

    public void ShowPanel() {
        isShowed = true;

        gameObject.SetActive(true);

        countdownValue = 5;
        countdownText.SetText(countdownValue.ToString());

        restartButton.SetActive(false);
        panelAnimator.Play("Show");

        AudioManager.Instance.PlaySound(SoundType.LEVEL_FAILED);

        InvokeRepeating("Countdown", 1f, 1f);
    }

    public void HidePanel() {
        isShowed = false;

        panelAnimator.Play("Hide");
        CancelInvoke();
    }

    public void OnAnimationFinish() {
        if(!isShowed) {
            gameObject.SetActive(false);
        }
    }

    public void RestartLevel() {
        if(countdownValue > 0) {
            return;
        }

        levelManager.RestartLevel();

        HidePanel();
    }

    public void SkipLevel() {
        RewardedAdController.GetInstance().ShowAd((Reward reward) => {
            levelManager.SkipLevel();
        }, delegate() {
            infoUI.ShowInfo(InfoType.AD_IS_NOT_LOADED);
        });

        HidePanel();
    }

    private void Countdown() {
        countdownValue--;
        countdownText.SetText(countdownValue.ToString());

        if(countdownValue <= 0) {
            restartButton.SetActive(true);
            CancelInvoke();
        }
    }
}
