using TMPro;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    [SerializeField] private Sprite lockedSprite;

    private int stageNumber;
    private int levelNumber;

    public void Initialize(int stage, int level) {
        stageNumber = stage;
        levelNumber = level;

        gameObject.name = level.ToString();

        UserData.LevelStatus levelStatus = UserData.GetLevelStatus(stage, level);
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
