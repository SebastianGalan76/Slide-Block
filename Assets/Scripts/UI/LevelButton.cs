using GoogleMobileAds.Api;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelButton : LocalButton
{
    [SerializeField] private Sprite lockedSprite;
    private Sprite unlockedSprite;

    private UIInfo infoUI;
    private int stageNumber;
    private int levelNumber;
    private UserData.LevelStatus levelStatus;

    public void Initialize(UIInfo infoUI, int stage, int level) {
        stageNumber = stage;
        levelNumber = level;
        this.infoUI = infoUI;

        unlockedSprite = GetComponent<Image>().sprite;
        gameObject.name = level.ToString();

        levelStatus = UserData.GetLevelStatus(stage, level);
        GetComponent<Button>().onClick.AddListener(() => { StartLevel(); });

        ReloadButton();
    }

    private void StartLevel() {
        if(!levelStatus.unlocked) {
            infoUI.ShowInfo(InfoType.LEVEL_LOCKED, Unlock);

            return;
        }

        PlayerPrefs.SetInt("Stage", stageNumber);
        PlayerPrefs.SetInt("Level", levelNumber);
        PlayerPrefs.Save();

        SceneManager.LoadScene("Game");
    }

    private void Unlock() {
        RewardedAdController.ShowAd((Reward reward) => {
            UserData.UnlockLevel(stageNumber, levelNumber);
            levelStatus.unlocked = true;
            infoUI.HideAllInfo();

            ReloadButton();
        }, delegate () {
            infoUI.ShowInfo(InfoType.AD_IS_NOT_LOADED);
        });
    }

    private void ReloadButton() {
        if(levelStatus.unlocked) {
            TMP_Text levelNumberText = transform.Find("TLevel").GetComponent<TMP_Text>();
            levelNumberText.SetText(levelNumber.ToString());
            levelNumberText.gameObject.SetActive(true);

            GetComponent<Image>().sprite = unlockedSprite;
        } else {
            GetComponent<Image>().sprite = lockedSprite;
        }

        if(levelStatus.finished) {
            GameObject starObject = transform.Find("IStar").gameObject;
            starObject.SetActive(true);
        }
    }
}
