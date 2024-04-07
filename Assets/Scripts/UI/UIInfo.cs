using System.Collections.Generic;
using UnityEngine;

public class UIInfo : MonoBehaviour
{
    [SerializeField] private Info[] infoArray;

    public void ShowInfo(InfoType infoType) {
        if(infoArray == null) {
            return;
        }

        foreach(Info info in infoArray) {
            if(info.type == infoType) {
                info.Show();
                Time.timeScale = 0f;

                return;
            }
        }
    }

    public void HideAllInfo() {
        Time.timeScale = 1f;

        if(infoArray == null) {
            return;
        }

        foreach(Info info in infoArray) {
            info.Hide();
        }
    }
}

public enum InfoType {
    STOPPABLE_BLOCK
}
[System.Serializable]
public struct Info {
    public InfoType type;
    public GameObject info;

    public void Show() {
        info.SetActive(true);
    }

    public void Hide() {
        info.SetActive(false);
    }
}
