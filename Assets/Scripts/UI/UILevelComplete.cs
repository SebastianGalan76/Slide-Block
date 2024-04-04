using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UILevelComplete : MonoBehaviour
{
    [SerializeField] private LevelManager levelManager;

    [SerializeField] private TMP_Text levelNumberText;
    [SerializeField] private ParticleSystem[] confetti;

    private Animator panelAnimator;

    private void Awake() {
        panelAnimator = GetComponent<Animator>();
    }

    public void ShowPanel(int levelNumber) {
        gameObject.SetActive(true);

        panelAnimator.Play("Show");
        levelNumberText.text = "LEVEL " + levelNumber;

        StartCoroutine(WaitForConfetti());
        IEnumerator WaitForConfetti() {
            yield return new WaitForSeconds(0.3f);

            AudioManager.Instance.PlaySound(SoundType.LEVEL_COMPLETE);
            foreach(ParticleSystem ps in confetti) {
                ps.Play();
            }
        }
    }

    public void OnAnimationFinish() {
        gameObject.SetActive(false);

        levelManager.StartNextLevel();
    }
}
