using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISettingsPanel : MonoBehaviour
{
    [SerializeField] private Animator soundToggleAnimator, musicToggleAnimator;

    private void Awake() {
        soundToggleAnimator.SetBool("enabled", AudioManager.Instance.sound.IsEnabled());
        musicToggleAnimator.SetBool("enabled", AudioManager.Instance.music.IsEnabled());
        soundToggleAnimator.keepAnimatorStateOnDisable = true;
        musicToggleAnimator.keepAnimatorStateOnDisable = true;
    }

    public void SwitchSound() {
        bool enabled = AudioManager.Instance.SwitchSound();

        soundToggleAnimator.SetBool("enabled", enabled);
    }

    public void SwitchMusic() {
        bool enabled = AudioManager.Instance.SwitchMusic();

        musicToggleAnimator.SetBool("enabled", enabled);
    }
}
