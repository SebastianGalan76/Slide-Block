using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ButtonCooldown : MonoBehaviour
{
    [SerializeField] private float cooldown = 1f;

    private Button button;

    private void Awake() {
        button = GetComponent<Button>();

        button.onClick.AddListener(OnClick);
    }

    private void OnClick() {
        if(button.interactable) {
            StartCoroutine(ResetCooldown());
            button.interactable = false;
        }

        IEnumerator ResetCooldown() {
            yield return new WaitForSecondsRealtime(cooldown);
            button.interactable = true;
        }
    }
}
