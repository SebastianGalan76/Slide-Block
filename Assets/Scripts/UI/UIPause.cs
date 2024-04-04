using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIPause : MonoBehaviour
{
    [SerializeField] private Animator soundToggleAnimator, musicToggleAnimator;

    private Animator panelAnimator;
    private bool isShowed;

    private void Awake() {
        panelAnimator = GetComponent<Animator>();

        soundToggleAnimator.SetBool("enabled", AudioManager.Instance.sound.IsEnabled());
        musicToggleAnimator.SetBool("enabled", AudioManager.Instance.music.IsEnabled());
        soundToggleAnimator.keepAnimatorStateOnDisable = true;
        musicToggleAnimator.keepAnimatorStateOnDisable = true;
    }

    public void ShowPanel() {
        isShowed = true;

        gameObject.SetActive(true);
        panelAnimator.SetBool("isPaused", true);
    }

    public void HidePanel() {
        isShowed = false;

        panelAnimator.SetBool("isPaused", false);
    }

    public void OnAnimationFinish() {
        if(!isShowed) {
            gameObject.SetActive(false);
        }
    }

    public void SwitchSound() {
        bool enabled = AudioManager.Instance.SwitchSound();

        soundToggleAnimator.SetBool("enabled", enabled);
    }

    public void SwitchMusic() {
        bool enabled = AudioManager.Instance.SwitchMusic();

        musicToggleAnimator.SetBool("enabled", enabled);
    }

    public void GoToMainMenu() {
        SceneManager.LoadScene("MainMenu");
    }
}
