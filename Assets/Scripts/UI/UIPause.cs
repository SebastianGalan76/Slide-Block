using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIPause : MonoBehaviour
{
    private Animator panelAnimator;
    private bool isPaused;

    private void Awake() {
        panelAnimator = GetComponent<Animator>();
    }

    public void ShowPanel() {
        isPaused = true;

        gameObject.SetActive(true);
        panelAnimator.SetBool("isPaused", true);
    }

    public void HidePanel() {
        isPaused = false;

        panelAnimator.SetBool("isPaused", false);
    }

    public void OnAnimationFinish() {
        if(!isPaused) {
            gameObject.SetActive(false);
        }
    }

    public void GoToMainMenu() {
        SceneManager.LoadScene("MainMenu");
    }
}
