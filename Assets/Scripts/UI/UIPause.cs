using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIPause : MonoBehaviour
{
    private Animator panelAnimator;
    private bool isShowed;

    private void Awake() {
        panelAnimator = GetComponent<Animator>();
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

    public void GoToMainMenu() {
        SceneManager.LoadScene("MainMenu");
    }
}
