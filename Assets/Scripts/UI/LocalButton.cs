using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LocalButton : MonoBehaviour
{
    [SerializeField] private float cooldown = 0f;

    private Button button;

    private void Awake() {
        button = GetComponent<Button>();

        button.onClick.AddListener(OnClick);
    }

    private void OnClick() {
        if(button.interactable) {
            if(cooldown > 0) {
                StartCoroutine(ResetCooldown());
                button.interactable = false;
            }

            AudioManager.Instance.PlaySound(SoundType.BUTTON);
        }

        IEnumerator ResetCooldown() {
            yield return new WaitForSecondsRealtime(cooldown);
            button.interactable = true;
        }
    }
}
