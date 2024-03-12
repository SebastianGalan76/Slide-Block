using UnityEngine;

public class StoppableBlock : Block
{
    [SerializeField] private Sprite stoppedBlockSprite;
    [SerializeField] private Sprite unstoppedBlockSprite;
    
    private SpriteRenderer spriteRenderer;

    public bool isStopped;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnMouseDown() {
        OnClick();
    }

    public void OnClick() {
        isStopped = !isStopped;

        if(isStopped) {
            spriteRenderer.sprite = stoppedBlockSprite;
        } else {
            spriteRenderer.sprite = unstoppedBlockSprite;
        }
    }
}
