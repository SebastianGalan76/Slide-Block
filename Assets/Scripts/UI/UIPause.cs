using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIPause : MonoBehaviour
{
    private Animator panelAnimator;

    private void Awake() {
        panelAnimator = GetComponent<Animator>();
    }

    public void ShowPanel() {
        panelAnimator.SetBool("isPaused", true);
    }

    public void HidePanel() {
        panelAnimator.SetBool("isPaused", false);
    }

    public void GoToMainMenu() {
        SceneManager.LoadScene("MainMenu");
    }
}
