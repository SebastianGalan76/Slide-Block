using UnityEngine;

public class UIInfo : MonoBehaviour
{
    [SerializeField] private Info[] infoArray;

    public delegate void OnClickActionHandler();
    private OnClickActionHandler action;

    public void ShowInfo(InfoType infoType, OnClickActionHandler action = null) {
        if(infoArray == null) {
            return;
        }

        this.action = action;

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

    public void PerformAction() {
        if(action != null) {
            action();
        }
    }
}

public enum InfoType {
    STOPPABLE_BLOCK, LEVEL_LOCKED, FINISHED_ALL_LEVELS, AD_IS_NOT_LOADED
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
