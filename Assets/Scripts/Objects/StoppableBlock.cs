using UnityEngine;

public class StoppableBlock : Block
{
    [SerializeField] private Sprite stoppedBlockSprite;
    [SerializeField] private Sprite unstoppedBlockSprite;
    
    private SpriteRenderer spriteRenderer;
    private PlatformManager platformManager;

    public bool isStopped;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnMouseDown() {
        OnClick();
    }

    public void Initialize(FieldType type, int posX, int posY, PlatformManager platformManager) {
        Initialize(type, posX, posY);

        this.platformManager = platformManager;
    }

    public void OnClick() {
        isStopped = !isStopped;

        if(isStopped) {
            platformManager.ChangePlatformFieldType(posX, posY, FieldType.NULL);

            spriteRenderer.sprite = stoppedBlockSprite;
        } else {
            platformManager.ChangePlatformFieldType(posX, posY, FieldType.PLATFORM);

            spriteRenderer.sprite = unstoppedBlockSprite;
        }
    }
}
