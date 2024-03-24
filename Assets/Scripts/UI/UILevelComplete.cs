using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UILevelComplete : MonoBehaviour
{
    [SerializeField] private TMP_Text levelNumberText;
    [SerializeField] private LevelManager levelManager;
    
    private Animator panelAnimator;

    private void Awake() {
        panelAnimator = GetComponent<Animator>();
    }

    public void ShowPanel(int levelNumber) {
        gameObject.SetActive(true);

        panelAnimator.Play("Show");
        levelNumberText.text = "LEVEL " + levelNumber;
    }

    public void OnAnimationFinish() {
        gameObject.SetActive(false);

        levelManager.StartNextLevel();
    }
}
