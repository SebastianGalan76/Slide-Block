using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelButton : LocalButton
{
    [SerializeField] private Sprite lockedSprite;

    private int stageNumber;
    private int levelNumber;
    UserData.LevelStatus levelStatus;

    public void Initialize(int stage, int level) {
        stageNumber = stage;
        levelNumber = level;

        gameObject.name = level.ToString();

        levelStatus = UserData.GetLevelStatus(stage, level);
        GetComponent<Button>().onClick.AddListener(() => { StartLevel(); });

        if(levelStatus.unlocked) {
            TMP_Text levelNumberText = transform.Find("TLevel").GetComponent<TMP_Text>();
            levelNumberText.SetText(level.ToString());
            levelNumberText.gameObject.SetActive(true);
        }
        else{
            GetComponent<Image>().sprite = lockedSprite;
        }

        if(levelStatus.finished) {
            GameObject starObject = transform.Find("IStar").gameObject;
            starObject.SetActive(true);
        }
    }

    private void StartLevel() {
        PlayerPrefs.SetInt("Stage", stageNumber);
        PlayerPrefs.SetInt("Level", levelNumber);
        PlayerPrefs.Save();

        SceneManager.LoadScene("Game");
    }
}
