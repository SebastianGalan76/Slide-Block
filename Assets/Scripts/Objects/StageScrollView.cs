using UnityEngine;

public class StageScrollView : MonoBehaviour
{
    [SerializeField] private int stage;

    [SerializeField] private Transform content;
    [SerializeField] private GameObject levelButtonPrefab;
    [SerializeField] private UIInfo infoUI;

    private const int PADDING_TOP = 60;

    private void Awake() {
        int levelAmount = LevelDataManager.GetInstance().GetLevelAmountForStage(stage);

        for(int i=0;i<levelAmount; i++) {
            GameObject levelButton = Instantiate(levelButtonPrefab);
            levelButton.GetComponent<LevelButton>().Initialize(infoUI, stage, i + 1);

            levelButton.transform.SetParent(content.transform, false);

            int col = i % 5;
            int row = i / 5;

            col -= 2;
            col *= 200;

            row = (-row * 200) - PADDING_TOP;
            levelButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(col, row);
        }

        int totalRow = levelAmount / 5;
        float contentHeight = 2 * PADDING_TOP + totalRow * 200f;

        content.GetComponent<RectTransform>().sizeDelta = new Vector2(0, contentHeight);
    }
}
